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
			CreateMap<FriendlyRelation, FriendlyRelationDTO>().ReverseMap();
			CreateMap<FriendlyRelationStatus, FriendlyRelationStatusDTO>().ReverseMap();

			CreateMap<Pet, PetDTO>().ReverseMap();
			CreateMap<PetStatistic, PetStatisticDTO>().ReverseMap();
			CreateMap<PetView, PetViewDTO>().ReverseMap();
		}
	}
}
