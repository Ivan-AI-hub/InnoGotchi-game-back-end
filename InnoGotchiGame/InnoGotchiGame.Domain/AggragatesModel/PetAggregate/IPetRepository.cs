using InnoGotchiGame.Domain.BaseModels;

namespace InnoGotchiGame.Domain.AggragatesModel.PetAggregate
{
    public interface IPetRepository : IRepository<IPet>
    {
        public IQueryable<IPet> GetFullData(bool trackChanges);
    }
}
