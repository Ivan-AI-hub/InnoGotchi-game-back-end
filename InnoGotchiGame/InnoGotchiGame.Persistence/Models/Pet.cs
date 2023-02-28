using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;

namespace InnoGotchiGame.Persistence.Models
{
    public class Pet : IPet
    {
        public int Id { get; private set; }

        public IPetStatistic Statistic { get; private set; }
        public IPetView View { get; private set; }

        public int FarmId { get; private set; }
        public IPetFarm? Farm { get; private set; }

        private Pet() { }
        private Pet(IPetStatistic statistic, IPetView view)
        {
            Statistic = statistic;
            View = view;
        }
        public Pet(IPetStatistic statistic, IPetView view, IPetFarm farm) : this(statistic, view)
        {
            Farm = farm;
            FarmId = farm.Id;
        }

        public Pet(IPetStatistic statistic, IPetView view, int farmId) : this(statistic, view)
        {
            FarmId = farmId;
        }
    }
}
