using InnoGotchiGame.Application.Managers;
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

        /// <summary>
        /// Create a collaborating request from <paramref name="senderId"/> to <paramref name="recipientId"/>
        /// </summary>
        /// <param name="senderId">id of the sending user</param>
        /// <param name="recipientId">id of the recipient user</param>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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

        /// <summary>
        /// Confirms the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <param name="recipientId">id of the recipient user</param>
        [HttpPut("{requestId}/confirm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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

        /// <summary>
        /// Rejects the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <param name="participantId">id of the participant</param>
        [HttpPut("{requestId}/reject")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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

        /// <summary>
        /// Deletes the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        [HttpDelete("{requestId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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
