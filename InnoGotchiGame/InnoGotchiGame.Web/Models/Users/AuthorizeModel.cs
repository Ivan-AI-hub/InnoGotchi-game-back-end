using InnoGotchiGame.Application.Models;

namespace InnoGotchiGame.Web.Models.Users
{
	public class AuthorizeModel
	{
		public string AccessToken { get; set; }
		public UserDTO User { get; set; }
	}
}
