using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;

namespace InnoGotchiGame.Persistence.Models
{
    public class PetFarm : IPetFarm
    {
        public int Id { get; private set; }
        public string Name { get; set; }

        public DateTime CreateDate { get; private set; }

        public int OwnerId { get; private set; }
        public IUser? Owner { get; private set; }

        public IEnumerable<IPet> Pets { get; private set; }

        private PetFarm() { }
        public PetFarm(string name)
        {
            Name = name;
            CreateDate = DateTime.UtcNow;
            Pets = new List<IPet>();
        }
        public PetFarm(string name, IUser owner) : this(name)
        {
            OwnerId = owner.Id;
            Owner = owner;
        }

        public PetFarm(string name, int ownerId) : this(name)
        {
            OwnerId = ownerId;
        }
    }
}
