using InnoGotchiGame.Application.Models;

namespace InnoGotchiGame.Web.Models.Users
{
    public class AddUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public PictureDTO? Picture { get; set; }
    }
}
