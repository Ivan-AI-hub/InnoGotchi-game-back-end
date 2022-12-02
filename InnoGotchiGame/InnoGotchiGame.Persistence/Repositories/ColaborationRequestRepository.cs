using InnoGotchiGame.Domain;
using InnoGotchiGame.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchiGame.Persistence.Repositories
{
	public class ColaborationRequestRepository : IRepository<ColaborationRequest>
	{
		private InnoGotchiGameContext _context;
		public ColaborationRequestRepository(InnoGotchiGameContext context)
		{
			_context = context;
		}

		public int Add(ColaborationRequest item)
		{
			return _context.ColaborationRequests.Add(item).Entity.Id;
		}

		public bool Delete(int id)
		{
			var request = GetItemById(id);
			if (request == null)
				return false;

			_context.ColaborationRequests.Remove(request);
			return true;
		}

		public ColaborationRequest? GetItem(Func<ColaborationRequest, bool> predicate)
		{
			return _context.ColaborationRequests.FirstOrDefault(predicate);
		}

		public ColaborationRequest? GetItemById(int id)
		{
			return _context.ColaborationRequests.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<ColaborationRequest> GetItems()
		{
			return _context.ColaborationRequests.Include(x => x.RequestReceiver).Include(x => x.RequestSender);
		}

		public bool IsItemExist(int id)
		{
			if (_context.ColaborationRequests.Any(x => x.Id == id))
				return true;
			return false;
		}

		public void Save()
		{
			_context.SaveChanges();
		}

		public void Update(int updatedId, ColaborationRequest item)
		{
			item.Id = updatedId;
			_context.ColaborationRequests.Update(item);
		}
	}
}
