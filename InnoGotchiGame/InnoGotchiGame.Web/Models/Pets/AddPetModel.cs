using InnoGotchiGame.Application.Models;

namespace InnoGotchiGame.Web.Models.Pets
{
	public class AddPetModel
	{
		public PetStatisticDTO Statistic { get; set; }
		public PetViewDTO View { get; set; }
		public int FarmId { get; set; }
	}
}
