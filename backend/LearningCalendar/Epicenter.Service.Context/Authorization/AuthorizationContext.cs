﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Epicenter.Domain.Entity.LearningCalendar;
using Epicenter.Infrastructure.Extensions;
using Epicenter.Persistence.Interface.Repository.Generic;
using Epicenter.Persistence.Interface.Repository.LearningCalendar;
using Epicenter.Service.Context.Interface.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Epicenter.Service.Context.Authorization
{
    public class AuthorizationContext : IAuthorizationContext
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IRepository<IdentityUser> _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationContext(IEmployeeRepository employeeRepository, 
            IRepository<IdentityUser> userRepository,
            IHttpContextAccessor httpContextAccessor, 
            ITeamRepository teamRepository)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _teamRepository = teamRepository;
        }

        public string IdentityName => _httpContextAccessor.HttpContext.User.Identity.Name;

        public async Task<Employee> CurrentEmployee()
        {
            string email = IdentityName;

            return await _employeeRepository.GetDetailsAsync(email);
        }

        public async Task<IdentityUser> CurrentIdentity()
        {
            string email = IdentityName;

            return await _userRepository.QuerySingleOrDefaultAsync(user => user.Email == email);
        }

        public async Task<Team> GetTeamTreeIfAuthorizedForEmployee(Guid employeeId)
        {
            var team = await GetTeamTree();

            var selectedTeam = team.FindAnyOrDefault(
                root => root.Employees.Select(employee => employee.ManagedTeam),
                child => child.Manager.Id == employeeId);

            if (selectedTeam == null)
            { 
                throw new ApplicationException($"Employee ({team.Manager}) is not authorized to view ({employeeId}).");
            }

            return selectedTeam;
        }

        public async Task<Team> GetTeamTree()
        {
            var employee = await CurrentEmployee();
            var team = await _teamRepository.GetTeamTreeAsync(employee.Id);

            return team;
        }
    }
}