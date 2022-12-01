using System.ComponentModel.DataAnnotations;

namespace InnoGotchiGame.Web.Models.Users
{
	public class UpdateUserModel
	{
		[Required]
		public int UpdatedId { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string Email { get; set; }
		public string Password { get; set; }
		public string? PhotoFileLink { get; set; }
	}
}
