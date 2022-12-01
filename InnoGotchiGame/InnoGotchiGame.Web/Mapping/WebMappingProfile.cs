using AutoMapper;
using InnoGotchiGame.Application.Models;
using InnoGotchiGame.Web.Models.Users;

namespace InnoGotchiGame.Web.Mapping
{
	public class WebMappingProfile : Profile
	{
		public WebMappingProfile()
		{
			CreateMap<AddUserModel, UserDTO>();
			CreateMap<UpdateUserModel, UserDTO>();
		}
	}
}
