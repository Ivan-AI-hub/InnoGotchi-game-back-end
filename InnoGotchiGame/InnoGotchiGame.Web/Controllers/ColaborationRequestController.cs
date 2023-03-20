using InnoGotchiGame.Application.Managers;
using InnoGotchiGame.Web.Extensions;
using InnoGotchiGame.Web.Models.ErrorModels;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchiGame.Web.Controllers
{
    [Route("/api/colaborators")]
    public class ColaborationRequestController : BaseController
    {
        private readonly ColaborationRequestManager _requestManager;

        public ColaborationRequestController(ColaborationRequestManager requestManager)
        {
            _requestManager = requestManager;
        }

        /// <summary>
        /// Create a collaborating request from authorize user to <paramref name="recipientId"/>
        /// </summary>
        /// <param name="recipientId">id of the recipient user</param>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> AddCollaboratorAsync(int recipientId, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.GetUserId()!);
            var result = await _requestManager.SendColaborationRequestAsync(userId, recipientId, cancellationToken);
            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Ok();
        }

        /// <summary>
        /// Confirms the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>d
        [HttpPut("{requestId}/confirm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> ConfirmRequestAsync(int requestId, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.GetUserId()!);
            var result = await _requestManager.ConfirmRequestAsync(requestId, userId, cancellationToken);

            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Ok();
        }

        /// <summary>
        /// Rejects the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        [HttpPut("{requestId}/reject")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> RejectRequestAsync(int requestId, CancellationToken cancellationToken)
        {
            int userId = int.Parse(User.GetUserId()!);
            var result = await _requestManager.RejectRequestAsync(requestId, userId, cancellationToken);
            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Ok();
        }

        /// <summary>
        /// Deletes the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        [HttpDelete("{requestId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorDetails), 400)]
        public async Task<IActionResult> DeleteRequestAsync(int requestId, CancellationToken cancellationToken)
        {
            var result = await _requestManager.DeleteRequestAsync(requestId, cancellationToken);
            if (!result.IsComplete)
                return BadRequest(new ErrorDetails(400, result.Errors));

            return Ok();
        }
    }
}
