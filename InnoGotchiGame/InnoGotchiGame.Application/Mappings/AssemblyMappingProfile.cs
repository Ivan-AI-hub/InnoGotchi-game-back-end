using AutoMapper;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Domain.AggragatesModel.ColaborationRequestAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PictureAggregate;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;
using InnoGotchiGame.Persistence.Models;

namespace InnoGotchiGame.Application.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, IUser>().As<User>();

            CreateMap<Picture, PictureDTO>().ReverseMap();
            CreateMap<PictureDTO, IPicture>().As<Picture>();

            CreateMap<PetFarm, PetFarmDTO>().ReverseMap();
            CreateMap<PetFarmDTO, IPetFarm>().As<PetFarm>();

            CreateMap<ColaborationRequest, ColaborationRequestDTO>().ReverseMap();
            CreateMap<ColaborationRequestDTO, IColaborationRequest>().As<ColaborationRequest>();

            CreateMap<ColaborationRequestStatus, ColaborationRequestStatusDTO>().ReverseMap();

            CreateMap<Pet, PetDTO>().ReverseMap();
            CreateMap<PetDTO, IPet>().As<Pet>();

            CreateMap<PetStatistic, PetStatisticDTO>().ReverseMap();
            CreateMap<PetStatisticDTO, IPetStatistic>().As<PetStatistic>();

            CreateMap<PetView, PetViewDTO>().ReverseMap();
            CreateMap<PetViewDTO, IPetView>().As<PetView>();
        }
    }
}
