namespace InnoGotchiGame.Application.Filtrators.Base
{
    /// <summary>
    /// An abstract class representing an implementation of IQuariable filtering
    /// </summary>
    public abstract class Filtrator<T>
    {
        /// <summary>
        /// Performs IQueryable filtering according to the specified rules
        /// </summary>
        /// <returns> Filtered IQueryable </returns>
        internal abstract IQueryable<T> Filter(IQueryable<T> query);
    }
}