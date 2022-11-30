namespace InnoGotchiGame.Application.Filtrators.Base
{
    public abstract class Filtrator<T>
    {
        internal abstract IQueryable<T> Filter(IQueryable<T> query);
    }
}