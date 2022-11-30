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

		public int? OwnPetFarmId { get; set; }
		public PetFarmDTO? OwnPetFarm { get; set; }

		public List<ColaborationRequestDTO> SentColaborations { get; set; }
		public List<ColaborationRequestDTO> AcceptedColaborations { get; set; }
		public List<PetFarmDTO> CollaboratedFarms { get; set; }

		/// <returns>All colaborators of user</returns>
		public IEnumerable<UserDTO> GetUserColaborators()
		{
			Func<ColaborationRequestDTO, bool> whereFunc = x => x.Status == ColaborationRequestStatusDTO.Colaborators;

			List<UserDTO> friends = new List<UserDTO>();
			friends.AddRange(AcceptedColaborations.Where(whereFunc).Select(x => x.RequestSender));
			friends.AddRange(SentColaborations.Where(whereFunc).Select(x => x.RequestReceiver));
			return friends;
		}

		/// <returns>All unconfirmed invitations to be colaborators</returns>
		public IEnumerable<ColaborationRequestDTO> GetUnconfirmedInvite()
		{
			var invites = AcceptedColaborations.Where(x => x.Status == ColaborationRequestStatusDTO.Undefined);
			return invites;
		}
	}
}
