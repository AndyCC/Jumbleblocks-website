using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Jumbleblocks.Domain
{
    /// <summary>
    /// interface for a repository that is defined in the domain
    /// </summary>
    public interface IRepository<T>
        where T : class
    {
        /// <summary>
        /// returns number of T in repository
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Loads an item based on it's id 
        /// </summary>
        /// <param name="id">id of item to load</param>
        /// <returns>T</returns>
        T Load(object id);

        /// <summary>
        /// Loads all the T items 
        /// </summary>
        /// <returns>IEnumerable of T</returns>
        IEnumerable<T> LoadAll();

        /// <summary>
        /// Loads a range of T
        /// </summary>
        /// <param name="skip">number of items to skip</param>
        /// <param name="take">number of items to take</param>
        /// <returns>IEnumerable of T</returns>
        IEnumerable<T> LoadRange(int skip, int take);

        /// <summary>
        /// Loads a range of T
        /// </summary>
        /// <param name="skip">number of items to skip</param>
        /// <param name="take">number of items to take</param>
        /// <returns>IEnumerable of T</returns>
        /// <param name="orderByProperty">determines how to order the results</param>
        /// <param name="isAscending">Determines if order should be ascending. default is true</param>
        IEnumerable<T> LoadRange(int skip, int take, Expression<Func<T, object>> orderByProperty, bool isAscending = true);

        /// <summary>
        /// Saves or updates an item in the db
        /// </summary>
        /// <param name="item">item to save or load</param>       
        void SaveOrUpdate(T item);
    }
}
