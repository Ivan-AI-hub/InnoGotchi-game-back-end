
using AutoMapper;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;

namespace InnoGotchiGame.Application.Managers
{
	public class ColaborationRequestManager
	{
		private IRepository<ColaborationRequest> _repository;
		private IMapper _mapper;

		public ColaborationRequestManager(IRepository<ColaborationRequest> repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public ManagerRezult SendColaborationRequest(int senderId, int recipientId)
		{
			var rezult = new ManagerRezult();
			var request = new ColaborationRequest() { RequestSenderId = senderId, RequestReceiverId = recipientId, Status = ColaborationRequestStatus.Undefined};
			
			try
			{
				_repository.Add(request);
				_repository.Save();
			}
			catch(Exception ex)
			{
				if(ex.InnerException != null)
					rezult.Errors.Add(ex.InnerException.Message);
				rezult.Errors.Add(ex.Message);
			}
			return rezult;
		}

		public ManagerRezult ConfirmRequest(int requestId, int recipientId)
		{
			var rezult = new ManagerRezult();
			var request = _repository.GetItemById(requestId);
			if (request != null)
			{
				if (request.Status == ColaborationRequestStatus.Colaborators)
					rezult.Errors.Add("Request already confirmed");
				if (request.RequestReceiverId != recipientId)
					rezult.Errors.Add("Only the recipient of the request can confirm the request. The recipient's ID does not match");
				if(rezult.IsComplete)
				{
					request.Status = ColaborationRequestStatus.Colaborators;
					_repository.Update(requestId, request);
				}
			}
			else
			{
				rezult.Errors.Add("The request ID is not in the database");
			}

			return rezult;
		}

		public ManagerRezult RejectRequest(int requestId, int participantId)
		{
			var rezult = new ManagerRezult();
			var request = _repository.GetItemById(requestId);
			if (request != null)
			{
				if (request.Status == ColaborationRequestStatus.NotColaborators)
					rezult.Errors.Add("Request already rejected");
				if (request.RequestReceiverId != participantId && request.RequestSenderId != participantId)
					rezult.Errors.Add("Only the participant of the request can reject the request. The recipient's ID does not match");
				if (rezult.IsComplete)
				{
					request.Status = ColaborationRequestStatus.NotColaborators;
					_repository.Update(requestId, request);
				}
			}
			else
			{
				rezult.Errors.Add("The request ID is not in the database");
			}

			return rezult;
		}

		public ManagerRezult DeleteRequest(int requestId)
		{
			var rezult = new ManagerRezult();
			var request = _repository.GetItemById(requestId);
			if (request != null)
			{
				_repository.Delete(requestId);
			}
			else
			{
				rezult.Errors.Add("The request ID is not in the database");
			}

			return rezult;
		}
	}
}
