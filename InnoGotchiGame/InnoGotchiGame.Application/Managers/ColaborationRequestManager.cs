using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.AggragatesModel.ColaborationRequestAggregate;
using InnoGotchiGame.Domain.BaseModels;
using InnoGotchiGame.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Application.Managers
{
    /// <summary>
    /// Manager for working on collaborating requests
    /// </summary>
    public class ColaborationRequestManager
    {
        private IRepositoryManager _repositoryManager;
        private IColaborationRequestRepository _requestRepository;

        public ColaborationRequestManager(IRepositoryManager repositoryManager)
        {
            _requestRepository = repositoryManager.ColaborationRequest;
            _repositoryManager = repositoryManager;
        }

        /// <summary>
        /// Create a collaborating request from <paramref name="senderId"/> to <paramref name="recipientId"/>
        /// </summary>
        /// <param name="senderId">id of the sending user</param>
        /// <param name="recipientId">id of the recipient user</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> SendColaborationRequestAsync(int senderId, int recipientId, CancellationToken cancellationToken = default)
        {
            var result = new ManagerResult();

            var isSecondRequest = await _requestRepository.IsItemExistAsync(x => x.RequestSenderId == senderId && x.RequestReceiverId == recipientId ||
                                                           x.RequestReceiverId == senderId && x.RequestSenderId == recipientId, cancellationToken);
            if (isSecondRequest)
            {
                result.Errors.Add("The collaborating request for these users already exists");
                return result;
            }

            var request = new ColaborationRequest(senderId, recipientId, ColaborationRequestStatus.Undefined);

            _requestRepository.Create(request);
            await _repositoryManager.SaveAsync(cancellationToken);
            _repositoryManager.Detach(request);
            

            return result;
        }

        /// <summary>
        /// Confirms the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <param name="recipientId">id of the recipient user</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> ConfirmRequestAsync(int requestId, int recipientId, CancellationToken cancellationToken = default)
        {
            var result = new ManagerResult();
            if (!await IsRequestIdExistAsync(requestId, result, cancellationToken))
            {
                return result;
            }

            var request = await _requestRepository.GetItems(true).FirstAsync(x => x.Id == requestId, cancellationToken);

            if (request.Status == ColaborationRequestStatus.Colaborators)
                result.Errors.Add("Request already confirmed");
            if (request.RequestReceiverId != recipientId)
                result.Errors.Add("Only the recipient of the request can confirm the request. The recipient's ID does not match");

            if (!result.IsComplete)
            {
                return result;
            }

            request.Status = ColaborationRequestStatus.Colaborators;
            await _repositoryManager.SaveAsync(cancellationToken);

            _repositoryManager.Detach(request);

            return result;
        }

        /// <summary>
        /// Rejects the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <param name="participantId">id of the participant</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> RejectRequestAsync(int requestId, int participantId, CancellationToken cancellationToken = default)
        {
            var result = new ManagerResult();
            if (!await IsRequestIdExistAsync(requestId, result, cancellationToken))
            {
                return result;
            }

            var request = await _requestRepository.GetItems(true).FirstAsync(x => x.Id == requestId, cancellationToken);

            if (request.Status == ColaborationRequestStatus.NotColaborators)
                result.Errors.Add("Request already rejected");
            if (request.RequestReceiverId != participantId && request.RequestSenderId != participantId)
                result.Errors.Add("Only the participant of the request can reject the request. The recipient's ID does not match");

            if (!result.IsComplete)
            {
                return result;
            }

            request.Status = ColaborationRequestStatus.NotColaborators;
            await _repositoryManager.SaveAsync(cancellationToken);

            _repositoryManager.Detach(request);

            return result;
        }

        /// <summary>
        /// Deletes the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteRequestAsync(int requestId, CancellationToken cancellationToken = default)
        {
            var result = new ManagerResult();
            if (!await IsRequestIdExistAsync(requestId, result,cancellationToken))
            {
                return result;
            }

            var request = await _requestRepository.GetItems(false).FirstAsync(x => x.Id == requestId, cancellationToken);

            _requestRepository.Delete(request);
            await _repositoryManager.SaveAsync(cancellationToken);
            

            return result;
        }

        private async Task<bool> IsRequestIdExistAsync(int id, ManagerResult result, CancellationToken cancellationToken)
        {
            if (!(await _requestRepository.IsItemExistAsync(x => x.Id == id, cancellationToken)))
            {
                result.Errors.Add("The request ID is not in the database");
                return false;
            }
            return true;
        }
    }
}
