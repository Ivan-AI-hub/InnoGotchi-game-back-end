using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.BaseModels;

namespace InnoGotchiGame.Domain.AggragatesModel.PetAggregate
{
    public interface IPetRepository : IRepository<IPet>
    {
        public IQueryable<IPet> GetItemsWithFullData(bool trackChanges);
    }
}
