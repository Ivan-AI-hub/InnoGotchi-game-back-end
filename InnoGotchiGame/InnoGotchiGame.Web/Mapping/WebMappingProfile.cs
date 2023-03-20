using AutoMapper;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Web.Models.PetFarms;
using InnoGotchiGame.Web.Models.Pets;
using InnoGotchiGame.Web.Models.Users;

namespace InnoGotchiGame.Web.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<AddUserModel, UserDTO>();
            CreateMap<UpdateUserDataModel, UserDTO>();

            CreateMap<UpdatePetFarmModel, PetFarmDTO>();
            CreateMap<AddPetFarmModel, PetFarmDTO>();

            CreateMap<UpdatePetModel, PetDTO>();
            CreateMap<AddPetModel, PetDTO>();
        }
    }
}
