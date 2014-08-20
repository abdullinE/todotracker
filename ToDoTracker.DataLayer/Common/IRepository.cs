using System;
using System.Linq;
using System.Linq.Expressions;
using ToDoTracker.DataModel.Common;

namespace ToDoTracker.DataLayer.Common
{
    public interface IRepository<T> : IDisposable where T : EntityBase
    {
        /// <summary>
        /// Вытащить все объекты с базы
        /// </summary>
        IQueryable<T> All(bool includeDeleted = false);

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <param name="includeDeleted"> </param>
        IQueryable<T> Filter(Expression<Func<T, bool>> predicate, bool includeDeleted = false);

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        /// <param name="includeDeleted"> </param>
        IQueryable<TKey> Filter<TKey>(Expression<Func<TKey, bool>> predicate, bool includeDeleted = false) where TKey : T;

        /// <summary>
        /// Gets objects from database with filting and paging.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="includeDeleted">Specified atribute for deleted items </param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        IQueryable<TKey> Filter<TKey>(Expression<Func<TKey, bool>> filter,
            out int total, bool includeDeleted = false, int index = 0, int size = 50) where TKey : T;

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        /// <param name="includeDeleted"> </param>
        bool Contains(Expression<Func<T, bool>> predicate, bool includeDeleted = false);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        T Find(params object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeDeleted"> </param>
        T Find(Expression<Func<T, bool>> predicate, bool includeDeleted = false);

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="t">Specified a new object to create.</param>
        T Create(T t);

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="t">Specified a existing object to delete.</param>        
        int Delete(T t);

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includeDeleted"> </param>
        int Delete(Expression<Func<T, bool>> predicate, bool includeDeleted = false);

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="t">Specified the object to save.</param>
        int Update(T t);

        /// <summary>
        /// Get the total objects count.
        /// </summary>
        int Count(bool includeDeleted = false);
    }
}
