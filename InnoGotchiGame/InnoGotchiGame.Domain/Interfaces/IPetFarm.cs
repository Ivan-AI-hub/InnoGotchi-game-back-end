namespace InnoGotchiGame.Domain.Interfaces
{
    public interface IPetFarm
    {
        int Id { get; }
        string Name { get; }
        DateTime CreateDate { get; }

        int OwnerId { get; }
        IUser Owner { get; }

        IList<IPet> Pets { get; }
    }
}