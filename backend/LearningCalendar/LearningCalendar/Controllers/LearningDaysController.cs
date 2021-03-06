﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using Epicenter.Api.Model.LearningDay;
using Epicenter.Service.Interface.Operations.LearningDay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Epicenter.Api.Model;
using Epicenter.Service.Interface.Exceptions.LearningDay;
using Epicenter.Service.Interface.Exceptions.Limit;

namespace Epicenter.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/learning-days")]
    public class LearningDaysController : ControllerBase
    {
        private readonly IGetLearningDaysOperation _getLearningDaysOperation;
        private readonly IGetSubordinateLearningDaysOperation _getSubordinateLearningDaysOperation;
        private readonly ICreateLearningDayOperation _createLearningDayOperation;
        private readonly IUpdateLearningDayOperation _updateLearningDayOperation;
        private readonly IDeleteLearningDayOperation _deleteLearningDayOperation;

        public LearningDaysController(IGetLearningDaysOperation getLearningDaysOperation,
            IGetSubordinateLearningDaysOperation getSubordinateLearningDaysOperation,
            ICreateLearningDayOperation createLearningDayOperation, 
            IUpdateLearningDayOperation updateLearningDayOperation, 
            IDeleteLearningDayOperation deleteLearningDayOperation)
        {
            _getLearningDaysOperation = getLearningDaysOperation;
            _getSubordinateLearningDaysOperation = getSubordinateLearningDaysOperation;
            _createLearningDayOperation = createLearningDayOperation;
            _updateLearningDayOperation = updateLearningDayOperation;
            _deleteLearningDayOperation = deleteLearningDayOperation;
        }

        [HttpGet]
        public async Task<ActionResult<LearningDayListModel>> GetLearningDays()
        {
            var response = await _getLearningDaysOperation.Execute();
            return Ok(new LearningDayListModel(response));
        }

        [HttpGet]
        [Route("subordinates")]
        public async Task<ActionResult<LearningDayListModel>> GetSubordinateLearningDays()
        {
            var response = await _getSubordinateLearningDaysOperation.Execute();
            return Ok(new LearningDayListModel(response));
        }

        [HttpDelete]
        [Route("learning-day/{id}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteLearningDay([Required]Guid id)
        {
            var request = new DeleteLearningDayOperationRequest
            {
                LearningDayId = id
            };
            await _deleteLearningDayOperation.Execute(request);
            return Ok();
        }

        [HttpPost]
        [Route("learning-day")]
        public async Task<ActionResult<LearningDayModel>> CreateLearningDay(CreateLearningDayModel model)
        {
            var request = new CreateLearningDayOperationRequest
            {
                Date = model.Date.Date,
                Comments = model.Comments,
                TopicIds = model.TopicIds
            };

            CreateLearningDayOperationResponse response;
            try
            {
                response = await _createLearningDayOperation.Execute(request);
            }
            catch (LearningDayAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (LimitExceededException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new LearningDayModel { Id = response.Id } );
        }

        [HttpPut]
        [Route("learning-day")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorModel),(int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateLearningDay(UpdateLearningDayModel model)
        {
            UpdateLearningDayOperationRequest request = new UpdateLearningDayOperationRequest
            {
                Comments = model.Comments,
                LearningDayId = model.LearningDayId,
                LearningDayTopics = model.LearningDayTopics
                    .Select(learningDayTopic => new UpdateLearningDayOperationRequest.LearningDayTopic
                    {
                        ProgressStatus = MapProgressStatus(learningDayTopic.ProgressStatus),
                        TopicId = learningDayTopic.TopicId
                    })
                    .ToList()
            };

            try
            {
                await _updateLearningDayOperation.Execute(request);
            }
            catch (LimitExceededException ex)
            {
                return BadRequest(new ErrorModel(ex.Message));
            }

            return Ok();
        }

        private UpdateLearningDayOperationRequest.ProgressStatus MapProgressStatus(UpdateLearningDayModel.ProgressStatus progressStatus)
        {
            return progressStatus switch
            {
                UpdateLearningDayModel.ProgressStatus.InProgress => UpdateLearningDayOperationRequest.ProgressStatus.InProgress,
                UpdateLearningDayModel.ProgressStatus.Done => UpdateLearningDayOperationRequest.ProgressStatus.Done,
                _ => throw new ArgumentOutOfRangeException(nameof(progressStatus), progressStatus, null)
            };
        }
    }
}