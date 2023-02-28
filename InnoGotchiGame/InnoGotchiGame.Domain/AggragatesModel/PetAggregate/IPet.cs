using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;

namespace InnoGotchiGame.Domain.AggragatesModel.PetAggregate
{
    public interface IPet
    {
        int Id { get; }
        IPetStatistic Statistic { get; }
        IPetView View { get; }

        int FarmId { get; }
        IPetFarm Farm { get; }
    }
}