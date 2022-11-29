using AutoMapper;
using FluentValidation.Results;
using InnoGotchiGame.Application.Managers;
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

			CreateMap<ValidationResult, ManagerRezult>()
				.ForMember(dest => dest.Errors, opt => opt.MapFrom(src => src.Errors.Select(x => x.ErrorMessage)));
		}
	}
}
