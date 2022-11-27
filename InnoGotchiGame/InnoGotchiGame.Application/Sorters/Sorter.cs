using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Sorters
{
	public abstract class Sorter<T>
	{
		internal abstract IQueryable<T> Sort(IQueryable<T> query);
	}
}