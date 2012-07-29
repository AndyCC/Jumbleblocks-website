using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Domain;
using NHibernate;
using System.Linq.Expressions;

namespace Jumbleblocks.nHibernate
{
    /// <summary>
    /// A repository to perform lookups
    /// </summary>
    public class LookupRepository : ILookupRepository
    {
        public LookupRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Gets the session factory
        /// </summary>
        protected ISessionFactory SessionFactory { get { return _sessionFactory; } }

        /// <summary>
        /// Gets the current session
        /// </summary>
        protected virtual ISession Session
        {
            get { return SessionFactory.GetCurrentSession(); }
        }

        /// <summary>
        /// Loads an entity for it's Id
        /// </summary>
        /// <typeparam name="T">type of object to load</typeparam>
        /// <param name="id">id of object to load</param>
        /// <returns>Entity T</returns>
        public T LoadForId<T>(object id)
            where T : class
        {
            return Session.Transact<T>(() => Session.Load<T>(id));
        }

        /// <summary>
        /// Loads an entity based on its description (must be unique result)
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="descriptionPropertyExpression">description property to check against</param>
        /// <param name="descriptionToFind">description to find</param>
        /// <returns>Entity T, or null if not found</returns>
        public T LoadForDescription<T>(Expression<Func<T, string>> descriptionPropertyExpression, string descriptionToFind)
             where T : class
        {
            BinaryExpression comparison = Expression.Equal(descriptionPropertyExpression.Body, Expression.Constant(descriptionToFind));
            Expression<Func<T, bool>> condition = Expression.Lambda<Func<T, bool>>(comparison, descriptionPropertyExpression.Parameters);
            
            var query = Session.QueryOver<T>().Where(condition);

            return Session.Transact<T>(() => query.SingleOrDefault<T>());
        }

        /// <summary>
        /// Loads all entities of given type
        /// </summary>
        /// <returns>IEnumerable of T</returns>
        public IEnumerable<T> LoadAll<T>()
             where T : class
        {
            return Session.Transact<IEnumerable<T>>(() => Session.QueryOver<T>().List());
        }

        /// <summary>
        /// Loads a range of T
        /// </summary>
        /// <param name="skip">number of items to skip</param>
        /// <param name="take">number of items to take</param>
        /// <returns>IEnumerable of T</returns>
        public IEnumerable<T> LoadRange<T>(int skip, int take)
             where T : class
        {
            return Session.Transact<IEnumerable<T>>(() => Session.QueryOver<T>().Skip(skip).Take(take).List());
        }


        /// <summary>
        /// Loads a range of T with ordering
        /// </summary>
        /// <param name="skip">number of items to skip</param>
        /// <param name="take">number of items to take</param>
        /// <param name="orderByProperty">property to order on</param>
        /// <param name="isAscending">if true orders ascending, otherwise descending</param>
        /// <returns>IEnumerable of T</returns>
        public IEnumerable<T> LoadRange<T>(int skip, int take, Expression<Func<T, object>> orderByProperty, bool isAscending = true) where T : class
        {
            IQueryOver<T, T> query = Session.QueryOver<T>();

            if (isAscending)
                query = query.OrderBy(orderByProperty).Asc;
            else
                query = query.OrderBy(orderByProperty).Desc;

            IQueryOver<T> finishedQuery = query.Skip(skip).Take(take);

            return Session.Transact<IEnumerable<T>>(() => finishedQuery.List());
        }

        /// <summary>
        /// Gets a count of all the entites available
        /// </summary>
        /// <typeparam name="T">IEnumerable of T</typeparam>
        /// <returns>int count</returns>
        public int Count<T>()
             where T : class
        {
            return Session.Transact<int>(() => Session.QueryOver<T>().RowCount());
        }
    }
}
