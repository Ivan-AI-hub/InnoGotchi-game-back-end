namespace InnoGotchiGame.Domain.Interfaces
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