namespace InnoGotchiGame.Web.Models.Users
{
    public class UpdateUserPasswordModel
    {
        public int UpdatedId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
