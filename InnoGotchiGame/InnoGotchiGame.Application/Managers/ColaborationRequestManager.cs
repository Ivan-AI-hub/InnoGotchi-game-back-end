
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
			var request = new ColaborationRequest() { RequestSenderId = senderId, RequestReceiverId = recipientId, Status = ColaborationRequestStatus.Undefined};
			_repository.Add(request);
			_repository.Save();
			return new ManagerRezult();
		}
	}
}
