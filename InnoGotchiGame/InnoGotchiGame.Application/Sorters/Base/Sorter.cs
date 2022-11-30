namespace InnoGotchiGame.Application.Sorters.Base
{
    public abstract class Sorter<T>
    {
        internal abstract IQueryable<T> Sort(IQueryable<T> query);
    }
}