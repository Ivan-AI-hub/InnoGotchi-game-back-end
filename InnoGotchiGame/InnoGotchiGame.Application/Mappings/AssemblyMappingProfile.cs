using AutoMapper;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Enums;
using InnoGotchiGame.Domain.Interfaces;

namespace InnoGotchiGame.Application.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile()
        {
            CreateMap<UserDTO, IUser>().As<User>();
            CreateMap<User, UserDTO>();

            CreateMap<PictureDTO, IPicture>().As<Picture>();
            CreateMap<Picture, PictureDTO>();

            CreateMap<PetFarmDTO, IPetFarm>().As<PetFarm>();
            CreateMap<PetFarm, PetFarmDTO>();

            CreateMap<ColaborationRequestDTO, IColaborationRequest>().As<ColaborationRequest>();
            CreateMap<ColaborationRequest, ColaborationRequestDTO>();

            CreateMap<ColaborationRequestStatus, ColaborationRequestStatusDTO>().ReverseMap();

            CreateMap<PetDTO, IPet>().As<Pet>();
            CreateMap<Pet, PetDTO>();

            CreateMap<PetStatisticDTO, IPetStatistic>().As<PetStatistic>();
            CreateMap<PetStatistic, PetStatisticDTO>();

            CreateMap<PetViewDTO, IPetView>().As<PetView>();
            CreateMap<PetView, PetViewDTO>();
        }
    }
}
