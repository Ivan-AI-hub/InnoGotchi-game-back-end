using InnoGotchiGame.Domain;
using InnoGotchiGame.Domain.Interfaces;
using InnoGotchiGame.Persistence.Abstracts;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<IUser, User>, IUserRepository
    {
        public UserRepository(InnoGotchiGameContext context) : base(context)
        {
        }

        public override IQueryable<IUser> GetItems(bool trackChanges)
        {
            return GetOnlyDiscribeData(trackChanges);
        }

        public override Task<IUser?> FirstOrDefaultAsync(Expression<Func<IUser, bool>> predicate, bool trackChanges)
        {
            return GetFullData(trackChanges).Where(predicate).FirstOrDefaultAsync(predicate);
        }
        public override Task<IUser> FirstAsync(Expression<Func<IUser, bool>> predicate, bool trackChanges)
        {
            return GetFullData(trackChanges).Where(predicate).FirstAsync(predicate);
        }
        private IQueryable<IUser> GetFullData(bool trackChanges)
        {
            var users = Context.Users
                .Include(x => x.Picture)
                .Include(x => x.OwnPetFarm)
                    .ThenInclude(x => x.Pets)
                .Include(x => x.SentColaborations)
                    .ThenInclude(x => x.RequestReceiver)
                .Include(x => x.AcceptedColaborations)
                    .ThenInclude(x => x.RequestSender);


            return trackChanges ? users : users.AsNoTracking();
        }

        private IQueryable<IUser> GetOnlyDiscribeData(bool trackChanges)
        {
            var users = Context.Users.Include(x => x.Picture);


            return trackChanges ? users : users.AsNoTracking();
        }
    }
}
