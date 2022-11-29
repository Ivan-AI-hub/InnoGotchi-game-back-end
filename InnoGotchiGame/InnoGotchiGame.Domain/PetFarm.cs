
namespace InnoGotchiGame.Domain
{
    public class PetFarm
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

		public int OwnerId { get; set; }
        public User Owner { get; set; }

		public List<User> Colaborators { get; set; }
		public List<Pet> Pets { get; }
    }
}
