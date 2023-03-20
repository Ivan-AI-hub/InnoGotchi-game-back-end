using InnoGotchiGame.Application.Models;

namespace InnoGotchiGame.Web.Models.Users
{
    public class UpdateUserDataModel
    {
        public int UpdatedId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PictureDTO? Picture { get; set; }
    }
}
