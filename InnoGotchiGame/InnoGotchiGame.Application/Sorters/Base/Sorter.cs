namespace InnoGotchiGame.Application.Sorters.Base
{
    public abstract class Sorter<T>
    {
        public bool IsDescendingSort { get; set; } = false;
        internal abstract IQueryable<T> Sort(IQueryable<T> query);
    }
}