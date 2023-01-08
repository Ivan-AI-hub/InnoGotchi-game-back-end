﻿using InnoGotchiGame.Application.Managers;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
    [Route("/api/colaborators")]
    public class ColaborationRequestController : BaseController
    {
        private ColaborationRequestManager _requestManager;

        public ColaborationRequestController(ColaborationRequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        [HttpPost]
        public IActionResult AddCollaborator(int senderId, int recipientId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rezult = _requestManager.SendColaborationRequest(senderId, recipientId);
            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }
        [HttpPut("{requestId}/confirm")]
        public IActionResult ConfirmRequest(int requestId, int recipientId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rezult = _requestManager.ConfirmRequest(requestId, recipientId);

            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        [HttpPut("{requestId}/reject")]
        public IActionResult RejectRequest(int requestId, int participantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rezult = _requestManager.RejectRequest(requestId, participantId);
            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }

        [HttpDelete("{requestId}")]
        public IActionResult DeleteRequest(int requestId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var rezult = _requestManager.DeleteRequest(requestId);
            if (!rezult.IsComplete)
                return BadRequest(rezult.Errors);

            return Ok();
        }
    }
}
