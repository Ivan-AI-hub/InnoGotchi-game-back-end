namespace InnoGotchiGame.Domain.Interfaces
{
    public interface IPetFarm
    {
        int Id { get; }
        string Name { get; set; }
        DateTime CreateDate { get; }

        int OwnerId { get; }
        IUser Owner { get; }

        IEnumerable<IPet> Pets { get; }
    }
}