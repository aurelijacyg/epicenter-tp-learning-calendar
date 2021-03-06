﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Epicenter.Domain.Entity.LearningCalendar;

namespace Epicenter.Service.Interface.Operations.LearningDay
{
    public interface IGetLearningDaysOperation
    {
        public Task<GetLearningDaysOperationResponse> Execute();
    }

    public class GetLearningDaysOperationResponse
    {
        public List<LearningDay> LearningDays { get; set; }

        public class LearningDay
        {
            public Guid Id { get; set; }
            public Employee Employee { get; set; }
            public DateTime Date { get; set; }
            public string Comments { get; set; }
            public List<LearningDayTopic> Topics { get; set; }

            public class LearningDayTopic
            {
                public Guid Id { get; set; }
                public string Subject { get; set; }
                public ProgressStatus ProgressStatus { get; set; }
            }
        }

        public class Employee
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}