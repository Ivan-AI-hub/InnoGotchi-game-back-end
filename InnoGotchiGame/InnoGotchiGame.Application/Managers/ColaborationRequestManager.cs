using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Enums;
using InnoGotchiGame.Persistence.Interfaces;
using InnoGotchiGame.Persistence.Managers;

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
        public async Task<ManagerResult> SendColaborationRequestAsync(int senderId, int recipientId)
        {
            var result = new ManagerResult();
            var request = new ColaborationRequest(senderId, recipientId, ColaborationRequestStatus.Undefined);

            var isSecondRequest = await _requestRepository.IsItemExistAsync(x => x.RequestSenderId == senderId && x.RequestReceiverId == recipientId ||
                                                           x.RequestReceiverId == senderId && x.RequestSenderId == recipientId);
            if (isSecondRequest)
            {
                result.Errors.Add("The collaborating request for these users already exists");
                return result;
            }

            _requestRepository.Create(request);
            await _repositoryManager.SaveAsync();
            _repositoryManager.Detach(request);
            

            return result;
        }

        /// <summary>
        /// Confirms the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <param name="recipientId">id of the recipient user</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> ConfirmRequestAsync(int requestId, int recipientId)
        {
            var result = new ManagerResult();
            var request = await _requestRepository.FirstOrDefaultAsync(x => x.Id == requestId, false);
            if (request == null)
            {
                result.Errors.Add("The request ID is not in the database");
                return result;
            }

            if (request.Status == ColaborationRequestStatus.Colaborators)
                result.Errors.Add("Request already confirmed");

            if (request.RequestReceiverId != recipientId)
                result.Errors.Add("Only the recipient of the request can confirm the request. The recipient's ID does not match");

            if (result.IsComplete)
            {
                request.Status = ColaborationRequestStatus.Colaborators;
                _repositoryManager.ColaborationRequest.Update(request);
                await _repositoryManager.SaveAsync();
                _repositoryManager.Detach(request);
            }
            

            return result;
        }

        /// <summary>
        /// Rejects the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <param name="participantId">id of the participant</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> RejectRequestAsync(int requestId, int participantId)
        {
            var result = new ManagerResult();
            var request = await _requestRepository.FirstOrDefaultAsync(x => x.Id == requestId, false);
            if (request == null)
            {
                result.Errors.Add("The request ID is not in the database");
                return result;
            }

            if (request.Status == ColaborationRequestStatus.NotColaborators)
                result.Errors.Add("Request already rejected");
            if (request.RequestReceiverId != participantId && request.RequestSenderId != participantId)
                result.Errors.Add("Only the participant of the request can reject the request. The recipient's ID does not match");

            if (result.IsComplete)
            {
                request.Status = ColaborationRequestStatus.NotColaborators;
                _repositoryManager.ColaborationRequest.Update(request);
                await _repositoryManager.SaveAsync();
                _repositoryManager.Detach(request);
            }
            

            return result;
        }

        /// <summary>
        /// Deletes the request for colaboration
        /// </summary>
        /// <param name="requestId">id of the request</param>
        /// <returns>Result of method execution</returns>
        public async Task<ManagerResult> DeleteRequestAsync(int requestId)
        {
            var result = new ManagerResult();
            var request = await _requestRepository.FirstOrDefaultAsync(x => x.Id == requestId, false);

            if (request == null)
            {
                result.Errors.Add("The request ID is not in the database");
                return result;
            }

            _requestRepository.Delete(request);
            await _repositoryManager.SaveAsync();
            

            return result;
        }
    }
}
