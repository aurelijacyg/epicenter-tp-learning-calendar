﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Epicenter.Service.Interface.Operations.Goal
{
    public interface IAssignGoalToTeamOperation
    {
        Task Execute(AssignGoalToTeamOperationRequest request);
    }

    public class AssignGoalToTeamOperationRequest
    {
        public Guid ManagerId { get; set; }
        public List<Guid> TopicIds { get; set; }
    }
}