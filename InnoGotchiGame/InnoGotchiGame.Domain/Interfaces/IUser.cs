namespace InnoGotchiGame.Domain.Interfaces
{
    public interface IUser
    {
        int Id { get; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string PasswordHach { get; set; }

        IPicture? Picture { get; set; }
        int OwnPetFarmId { get; set; }
        IPetFarm? OwnPetFarm { get; set; }

        IList<IColaborationRequest> SentColaborations { get; }
        IList<IColaborationRequest> AcceptedColaborations { get; }

        IEnumerable<IUser> GetColaborators();
    }
}