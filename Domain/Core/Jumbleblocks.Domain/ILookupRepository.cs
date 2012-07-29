using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Jumbleblocks.Domain
{
    /// <summary>
    /// Interface for a repository to look items up
    /// </summary>
    public interface ILookupRepository
    {
        /// <summary>
        /// Loads an entity for it's Id
        /// </summary>
        /// <typeparam name="T">type of object to load</typeparam>
        /// <param name="id">id of object to load</param>
        /// <returns>Entity T</returns>
        T LoadForId<T>(object id) where T : class;

        /// <summary>
        /// Loads an entity based on its description (must be unique result)
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="descriptionPropertyExpression">description property to check against</param>
        /// <param name="descriptionToFind">description to find</param>
        /// <returns>Entity T, or null if not found</returns>
        T LoadForDescription<T>(Expression<Func<T, string>> descriptionPropertyExpression, string descriptionToFind) where T : class;

        /// <summary>
        /// Loads all entities of given type
        /// </summary>
        /// <returns>IEnumerable of T</returns>
        IEnumerable<T> LoadAll<T>() where T : class;

        /// <summary>
        /// Loads a range of T
        /// </summary>
        /// <param name="skip">number of items to skip</param>
        /// <param name="take">number of items to take</param>
        /// <returns>IEnumerable of T</returns>
        IEnumerable<T> LoadRange<T>(int skip, int take) where T : class;

        /// <summary>
        /// Loads a range of T with ordering
        /// </summary>
        /// <param name="skip">number of items to skip</param>
        /// <param name="take">number of items to take</param>
        /// <param name="orderByProperty">property to order on</param>
        /// <param name="isAscending">if true orders ascending, otherwise descending</param>
        /// <returns>IEnumerable of T</returns>
        IEnumerable<T> LoadRange<T>(int skip, int take, Expression<Func<T, object>> orderByProperty, bool isAscending = true) where T : class;

        /// <summary>
        /// Gets a count of all the entites available
        /// </summary>
        /// <typeparam name="T">IEnumerable of T</typeparam>
        /// <returns>int count</returns>
        int Count<T>() where T : class;
    }
}
