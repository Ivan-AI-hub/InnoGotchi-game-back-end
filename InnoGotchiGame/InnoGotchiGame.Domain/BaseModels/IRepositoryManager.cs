using InnoGotchiGame.Domain.AggragatesModel.ColaborationRequestAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PetFarmAggregate;
using InnoGotchiGame.Domain.AggragatesModel.PictureAggregate;
using InnoGotchiGame.Domain.AggragatesModel.UserAggregate;

namespace InnoGotchiGame.Domain.BaseModels
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
