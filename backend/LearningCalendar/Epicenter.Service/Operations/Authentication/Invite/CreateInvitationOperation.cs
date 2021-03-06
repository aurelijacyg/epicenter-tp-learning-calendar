﻿using System;
using System.Threading.Tasks;
using Epicenter.Domain.Entity.LearningCalendar;
using Epicenter.Persistence.Interface.Repository.Authentication;
using Epicenter.Persistence.Interface.Repository.Generic;
using Epicenter.Service.Context.Interface.Authorization;
using Epicenter.Service.Interface.Exceptions.Authentication;
using Epicenter.Service.Interface.Operations.Authentication.Invite;
using Epicenter.Service.Interface.Services.Mail;
using Microsoft.AspNetCore.Identity;

namespace Epicenter.Service.Operations.Authentication.Invite
{
    public class CreateInvitationOperation : Operation, ICreateInvitationOperation
    {
        private readonly IRepository<IdentityUser> _userRepository;
        private readonly IEmailService _emailService;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IAuthorizationContext _authorizationContext;
        
        public CreateInvitationOperation(IRepository<IdentityUser> userRepository, IEmailService emailService, IInvitationRepository invitationRepository, IAuthorizationContext authorizationContext)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _invitationRepository = invitationRepository;
            _authorizationContext = authorizationContext;
        }

        public async Task<CreateInvitationOperationResponse> Execute(CreateInvitationOperationRequest request)
        {
            var inviter = await _authorizationContext.CurrentIdentity();
            var existingInvitee = await _userRepository.QuerySingleOrDefaultAsync(user => user.Email == request.Email);

            if (existingInvitee != null)
            {
                throw new EmailAlreadyUseException();
            }

            var invite = new Domain.Entity.Infrastructure.Authentication.Invite
            {
                Id = Guid.NewGuid(),
                InvitationTo = request.Email,
                InvitationFromId = inviter.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role ?? Constants.Employee.DefaultRole,
                Created = DateTime.Now
            };

            var createInviteTask = _invitationRepository.CreateAsync(invite);

            string message = BuildInviteEmailMessage(invite.Id);

            var sendEmailTask =
                _emailService.SendEmailAsync("Invitation to Epicenter Calendar", message, invite.InvitationTo);

            await createInviteTask;
            await sendEmailTask;

            return new CreateInvitationOperationResponse();
        }

        private const string Protocol = "http://";
        private const string InviteUrlBase = "www.sasyska.lt/invite";

        private string BuildInviteEmailMessage(Guid inviteId)
        {
            var link = $"Hey, you have been invited to join Epicenter - The Learning Center. You can join using this link: {Protocol}{InviteUrlBase}/{inviteId}";

            string message = link;

            return message;
        } 
    }
}