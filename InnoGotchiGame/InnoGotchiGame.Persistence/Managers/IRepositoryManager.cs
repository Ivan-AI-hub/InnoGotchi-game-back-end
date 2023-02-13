using InnoGotchiGame.Persistence.Interfaces;

namespace InnoGotchiGame.Persistence.Managers
{
    public interface IRepositoryManager
    {
        IColaborationRequestRepository ColaborationRequest { get; }
        IPetFarmRepository PetFarm { get; }
        IPetRepository Pet { get; }
        IPictureRepository Picture { get; }
        IUserRepository User { get; }
        Task SaveAsync();
        void Detach(object item);
    }
}
