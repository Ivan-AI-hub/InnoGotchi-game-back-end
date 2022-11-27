using InnoGotchiGame.Domain;

namespace InnoGotchiGame.Application.Filtrators
{
	public abstract class Filtrator<T>
	{
		internal abstract IQueryable<T> Filter(IQueryable<T> query);
	}
}