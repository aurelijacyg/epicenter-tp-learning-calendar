﻿using System.Threading.Tasks;
using Epicenter.Persistence.Interface.Repository.Authentication;
using Epicenter.Service.Interface.Exceptions.Authentication;
using Epicenter.Service.Interface.Operations.Authentication.User;
using Epicenter.Service.Interface.Operations.Employee;

namespace Epicenter.Service.Operations.Authentication
{
    public class RegisterUserOperation : IRegisterUserOperation
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly ICreateEmployeeOperation _createEmployeeOperation;

        public RegisterUserOperation(IInvitationRepository invitationRepository, ICreateEmployeeOperation employeeOperation)
        {
            _invitationRepository = invitationRepository;
            _createEmployeeOperation = employeeOperation;
        }

        public async Task<RegisterUserOperationResponse> Execute(RegisterUserOperationRequest request)
        {
            var existingInvitation = await _invitationRepository.GetWithInviterAsync(request.InviteId);
            if (existingInvitation == null)
            {
                throw new InvitationDoesNotExistException(request.InviteId);
            }

            var response = await _createEmployeeOperation.Execute(new CreateEmployeeOperationRequest
            {
                Email = existingInvitation.InvitationTo,
                ManagerEmail = existingInvitation.InvitationFrom.Email,
                FirstName = existingInvitation.FirstName,
                LastName = existingInvitation.LastName,
                Role = existingInvitation.Role,
                Password = request.Password,
                ImageData = request.ImageData
            });

            await _invitationRepository.DeleteAsync(existingInvitation);

            return new RegisterUserOperationResponse();
        }
    }
}