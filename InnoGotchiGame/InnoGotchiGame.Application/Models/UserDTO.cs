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

		public List<ColaborationRequestDTO> SentFriendships { get; set; }
		public List<ColaborationRequestDTO> AcceptedFriendships { get; set; }
		public List<PetFarm> CollaboratedFarms { get; set; }

		/// <returns>All friends of user</returns>
		public IEnumerable<UserDTO> GetUserFriends()
		{
			Func<ColaborationRequestDTO, bool> whereFunc = x => x.Status == ColaborationRequestStatusDTO.Friends;

			List<UserDTO> friends = new List<UserDTO>();
			friends.AddRange(AcceptedFriendships.Where(whereFunc).Select(x => x.FirstFriend));
			friends.AddRange(SentFriendships.Where(whereFunc).Select(x => x.SecondFriend));
			return friends;
		}

		/// <returns>All unconfirmed invitations to be friends</returns>
		public IEnumerable<ColaborationRequestDTO> GetUnconfirmedInvite()
		{
			var invites = AcceptedFriendships.Where(x => x.Status == ColaborationRequestStatusDTO.Undefined);
			return invites;
		}
	}
}
