using System.Text.Json.Serialization;

namespace InnoGotchiGame.Application.Models
{
    public class PetDTO
    {
        public int Id { get; set; }

        public PetStatisticDTO Statistic { get; set; }
        public PetViewDTO View { get; set; }
        public int FarmId { get; set; }
        [JsonIgnore]
        public PetFarmDTO Farm { get; set; }

        public PetDTO()
        {
            Statistic = new PetStatisticDTO();
            View = new PetViewDTO();
        }
    }
}
