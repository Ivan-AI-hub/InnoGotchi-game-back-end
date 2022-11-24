
namespace InnoGotchiGame.Application.Models
{
	public class FriendlyRelationDTO
	{
		public int Id { get; set; }
		public FriendlyRelationStatusDTO Status { get; set; }

		public int FirstFriendId { get; set; }
		public UserDTO FirstFriend { get; set; }

		public int SecondFriendId { get; set; }
		public UserDTO SecondFriend { get; set; }
	}
}
