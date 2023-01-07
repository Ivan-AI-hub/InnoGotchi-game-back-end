using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private InnoGotchiGameContext _context;
        public UserRepository(InnoGotchiGameContext context)
        {
            _context = context;
        }

        public User? GetItemById(int id)
        {
            return GetFullData().FirstOrDefault(x => x.Id == id);
        }

        public bool IsItemExist(int id)
        {
            return _context.Users.Any(x => x.Id == id);
        }

        public User? GetItem(Func<User, bool> predicate)
        {
            return GetFullData().FirstOrDefault(predicate);
        }

        public int Add(User item)
        {
            return _context.Users.Add(item).Entity.Id;
        }

        public void Update(int updatedId, User item)
        {
            item.Id = updatedId;
            _context.ChangeTracker.Clear();
            _context.Users.Update(item);
        }

        public bool Delete(int id)
        {
            var user = GetItemById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                return true;
            }
            return false;
        }

        public IQueryable<User> GetItems()
        {
            var users = GetOnlyDiscribeData();


            return users;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private IQueryable<User> GetFullData()
        {
            var users = _context.Users
                .Include(x => x.Picture)
                .Include(x => x.OwnPetFarm)
                    .ThenInclude(x => x.Pets)
                .Include(x => x.SentColaborations)
                    .ThenInclude(x => x.RequestReceiver)
                .Include(x => x.AcceptedColaborations)
                    .ThenInclude(x => x.RequestSender);


            return users;
        }

        private IQueryable<User> GetOnlyDiscribeData()
        {
            var users = _context.Users.Include(x => x.Picture);


            return users;
        }
    }
}
