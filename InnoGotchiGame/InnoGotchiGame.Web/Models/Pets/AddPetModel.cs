using InnoGotchiGame.Application.Models;

namespace InnoGotchiGame.Web.Models.Pets
{
    public class AddPetModel
    {
        public int FarmId { get; set; }
        public string Name { get; set; }
        public PetViewDTO View { get; set; }
    }
}
