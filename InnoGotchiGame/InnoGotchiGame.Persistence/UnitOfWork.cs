using InnoGotchiGame.Persistence.Repositories;

namespace InnoGotchiGame.Persistence
{
	public class UnitOfWork : IDisposable
	{
		private bool _disposed = false;
		private InnoGotchiGameContext _context = new InnoGotchiGameContext();
		private UserRepository _userRepository;

		public UserRepository Users { get => _userRepository; }
		
		public UnitOfWork(InnoGotchiGameContext context)
		{
			_context = context;
			_userRepository = new UserRepository(_context);
		}

		public int Save()
		{
			return _context.SaveChanges();
		}

		public virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}
}
