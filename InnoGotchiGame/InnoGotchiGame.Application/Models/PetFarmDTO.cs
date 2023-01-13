using System.Text.Json.Serialization;

namespace InnoGotchiGame.Application.Models
{
    public class PetFarmDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public int OwnerId { get; set; }

        public List<PetDTO> Pets { get; }

        public PetFarmDTO()
        {
            Pets = new List<PetDTO>();
        }
    }
}
