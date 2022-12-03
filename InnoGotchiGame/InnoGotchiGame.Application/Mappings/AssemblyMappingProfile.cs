using AutoMapper;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Mappings
{
	public class AssemblyMappingProfile : Profile
	{
		public AssemblyMappingProfile()
		{
			CreateMap<User, UserDTO>().ReverseMap();
			CreateMap<PetFarm, PetFarmDTO>().ReverseMap();
			CreateMap<ColaborationRequest, ColaborationRequestDTO>().ReverseMap();
			CreateMap<ColaborationRequestStatus, ColaborationRequestStatusDTO>().ReverseMap();

			CreateMap<Pet, PetDTO>().ReverseMap();
			CreateMap<PetStatistic, PetStatisticDTO>().ReverseMap();
			CreateMap<PetView, PetViewDTO>().ReverseMap();
		}

		private IEnumerable<User> GetUserColaborators(User user)
		{
			Func<ColaborationRequest, bool> whereFunc = x => x.Status == ColaborationRequestStatus.Colaborators;

			List<User> friends = new List<User>();
			friends.AddRange(user.AcceptedColaborations.Where(whereFunc).Select(x => x.RequestSender));
			friends.AddRange(user.SentColaborations.Where(whereFunc).Select(x => x.RequestReceiver));
			return friends;
		}
	}
}
