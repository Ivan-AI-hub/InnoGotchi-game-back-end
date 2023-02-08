using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Abstracts;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(InnoGotchiGameContext context) : base(context)
        {
        }

        public override IQueryable<User> GetItems(bool trackChanges)
        {
            return GetOnlyDiscribeData(trackChanges);
        }

        public override Task<User?> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate, bool trackChanges)
        {
            return GetFullData(trackChanges).FirstOrDefaultAsync(predicate);
        }

        private IQueryable<User> GetFullData(bool trackChanges)
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

        private IQueryable<User> GetOnlyDiscribeData(bool trackChanges)
        {
            var users = Context.Users.Include(x => x.Picture);


            return trackChanges ? users : users.AsNoTracking();
        }
    }
}
