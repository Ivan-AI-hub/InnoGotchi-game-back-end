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
			return _context.Users.FirstOrDefault(x => x.Id == id);
		}

		public User? GetItem(Func<User, bool> predicate)
		{
			return _context.Users.FirstOrDefault(predicate);
		}

		public void Add(User item)
		{
			_context.Users.Add(item);
		}

		public void Update(int updatedId, User item)
		{
			item.Id = updatedId;
			_context.Users.Update(item);
		}

		public void Delete(int id)
		{
			var user = GetItemById(id);
			if (user != null)
			{
				_context.Users.Remove(user);
			}
		}

		public IQueryable<User> GetItems(Func<User, bool>? whereRule = null, Func<User, dynamic>? orderByRule = null, bool isDescendingOrder = false)
		{
			var users = _context.Users
				.Include(x => x.OwnPetFarm)
				.Include(x => x.SentFriendships)
				.Include(x => x.AcceptedFriendships);

			if (whereRule != null)
				users.Where(whereRule);

			if (orderByRule != null)
			{
				if (isDescendingOrder)
					users.OrderByDescending(orderByRule);
				else
					users.OrderBy(orderByRule);
			}


			return users;
		}

		public void Save()
		{
			_context.SaveChanges();
		}
	}
}
