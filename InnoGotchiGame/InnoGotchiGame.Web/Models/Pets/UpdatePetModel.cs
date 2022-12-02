using InnoGotchiGame.Application.Models;

namespace InnoGotchiGame.Web.Models.Pets
{
	public class UpdatePetModel
	{
		public int UpdatedId { get; set; }
		public PetStatisticDTO Statistic { get; set; }
	}
}
