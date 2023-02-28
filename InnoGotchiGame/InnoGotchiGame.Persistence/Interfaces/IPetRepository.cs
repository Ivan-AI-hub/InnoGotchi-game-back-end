using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Interfaces;

namespace InnoGotchiGame.Persistence.Interfaces
{
    public interface IPetRepository : IRepository<IPet>
    {
        public IQueryable<IPet> GetItemsWithFullData(bool trackChanges);
    }
}
