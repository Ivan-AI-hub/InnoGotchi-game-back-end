using AutoMapper;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Enums;

namespace InnoGotchiGame.Application.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Picture, PictureDTO>().ReverseMap();
            CreateMap<PetFarm, PetFarmDTO>().ReverseMap();
            CreateMap<ColaborationRequest, ColaborationRequestDTO>().ReverseMap();
            CreateMap<ColaborationRequestStatus, ColaborationRequestStatusDTO>().ReverseMap();

            CreateMap<Pet, PetDTO>().ReverseMap();
            CreateMap<PetStatistic, PetStatisticDTO>().ReverseMap();
            CreateMap<PetView, PetViewDTO>().ReverseMap();
        }
    }
}
