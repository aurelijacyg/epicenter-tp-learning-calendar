﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Epicenter.Api.Model.Topic
{
    public class LearnTopicModel
    {
        [Required]
        public Guid TopicId { get; set; }
    }
}