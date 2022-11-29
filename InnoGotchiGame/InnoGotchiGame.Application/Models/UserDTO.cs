using InnoGotchiGame.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnoGotchiGame.Application.Models
{
	public class UserDTO
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string? PhotoFileLink { get; set; }

		public int OwnPetFarmId { get; set; }
		public PetFarmDTO? OwnPetFarm { get; set; }

		public List<FriendlyRelationDTO> SentFriendships { get; set; }
		public List<FriendlyRelationDTO> AcceptedFriendships { get; set; }
		public List<PetFarm> CollaboratedFarms { get; set; }

		/// <returns>All friends of user</returns>
		public IEnumerable<UserDTO> GetUserFriends()
		{
			Func<FriendlyRelationDTO, bool> whereFunc = x => x.Status == FriendlyRelationStatusDTO.Friends;

			List<UserDTO> friends = new List<UserDTO>();
			friends.AddRange(AcceptedFriendships.Where(whereFunc).Select(x => x.FirstFriend));
			friends.AddRange(SentFriendships.Where(whereFunc).Select(x => x.SecondFriend));
			return friends;
		}

		/// <returns>All unconfirmed invitations to be friends</returns>
		public IEnumerable<FriendlyRelationDTO> GetUnconfirmedInvite()
		{
			var invites = AcceptedFriendships.Where(x => x.Status == FriendlyRelationStatusDTO.Undefined);
			return invites;
		}
	}
}
