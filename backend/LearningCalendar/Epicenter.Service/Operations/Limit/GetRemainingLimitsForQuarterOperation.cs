﻿using System.Linq;
using System.Threading.Tasks;
using Epicenter.Persistence.Interface.Repository.LearningCalendar;
using Epicenter.Service.Context.Interface.Authorization;
using Epicenter.Service.Interface.Operations.Limit;

namespace Epicenter.Service.Operations.Limit
{
    public class GetRemainingLimitsForQuarterOperation : Operation, IGetRemainingLimitsForQuarterOperation
    {
        private readonly IAuthorizationContext _authorizationContext;
        private readonly ILimitRepository _limitRepository;
        private readonly ILearningDayRepository _learningDayRepository;

        public GetRemainingLimitsForQuarterOperation(IAuthorizationContext authorizationContext,
            ILimitRepository limitRepository,
            ILearningDayRepository learningDayRepository)
        {
            _authorizationContext = authorizationContext;
            _limitRepository = limitRepository;
            _learningDayRepository = learningDayRepository;
        }

        public async Task<GetRemainingLimitsForQuarterOperationResponse> Execute(GetRemainingLimitsForQuarterOperationRequest request)
        {
            var currentEmployee = await _authorizationContext.CurrentEmployee();

            var assignedLimit = await _limitRepository.QuerySingleOrDefaultAsync(limit =>
                limit.Employees.Any(employee => employee.Id == currentEmployee.Id));

            var learningDaysInQuarter = (await _learningDayRepository.GetByEmployeeIdForQuarterAsync(currentEmployee.Id, request.Quarter)).Count;

            var remainingDaysPerQuarter = assignedLimit.DaysPerQuarter - learningDaysInQuarter;

            return new GetRemainingLimitsForQuarterOperationResponse
            {
                DaysPerQuarter = remainingDaysPerQuarter
            };
        }
    }
}