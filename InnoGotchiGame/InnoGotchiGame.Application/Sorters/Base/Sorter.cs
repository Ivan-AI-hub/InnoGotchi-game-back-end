namespace InnoGotchiGame.Application.Sorters.Base
{
    /// <summary>
    /// An abstract class representing an implementation of IQuariable sorting
    /// </summary>
    public abstract class Sorter<T>
    {
        public bool IsDescendingSort { get; set; } = false;
        /// <summary>
        /// Performs IQueryable sorting according to the specified rules
        /// </summary>
        /// <returns> Sorted IQueryable </returns>
        internal abstract IQueryable<T> Sort(IQueryable<T> query);
    }
}