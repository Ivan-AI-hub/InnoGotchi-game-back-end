using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.BaseModels;

namespace InnoGotchiGame.Domain.AggragatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<IUser>
    {
        public IQueryable<IUser> GetFullData(bool trackChanges);
    }
}
