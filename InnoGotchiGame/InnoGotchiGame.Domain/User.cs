namespace InnoGotchiGame.Domain
{
	public class User
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int PasswordHach { get; set; }

		public string? PhotoFileLink { get; set; }
		public int? OwnPetFarmId { get; set; }
		public PetFarm? OwnPetFarm { get; set; }

		public List<ColaborationRequest> SentColaborations { get; set; }
		public List<ColaborationRequest> AcceptedColaborations { get; set; }
		public List<PetFarm> CollaboratedFarms { get; set; }
	}
}
