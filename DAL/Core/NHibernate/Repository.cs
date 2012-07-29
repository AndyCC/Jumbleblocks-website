using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Linq.Expressions;
using Jumbleblocks.Domain;

namespace Jumbleblocks.nHibernate
{
    /// <summary>
    /// A generic repository for accessing nHibernate
    /// </summary>
    /// <typeparam name="T">Type of parameter</typeparam>
    public class Repository<T> : IRepository<T>
        where T : class
    {
        public Repository(ISessionFactory sessionFactory)
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
        /// returns number of T in repository
        /// </summary>
        public int Count
        {
            get { return Transact(() => Session.QueryOver<T>().RowCount()); }
        }

        /// <summary>
        /// Loads from db
        /// </summary>
        /// <param name="id">id to load with</param>
        /// <returns>T</returns>
        public T Load(object id)
        {
            return Transact<T>(() => Session.Load<T>(id));
        }

        /// <summary>
        /// Loads all 
        /// </summary>
        /// <returns>T</returns>
        public IEnumerable<T> LoadAll()
        {
            return Transact<IEnumerable<T>>(() => Session.QueryOver<T>().List());
        }

        /// <summary>
        /// Loads a range 
        /// </summary>
        /// <param name="skip">number of items to skip</param>
        /// <param name="take">number of items to take (select)</param>
        /// <returns>IEnumerable of T</returns>
        public IEnumerable<T> LoadRange(int skip, int take)
        {
            return Transact<IEnumerable<T>>(() => Session.QueryOver<T>().Skip(skip).Take(take).List());
        }

        /// <summary>
        /// Loads an ordered range of objects
        /// </summary>
        /// <param name="skip">number of items to skip</param>
        /// <param name="take">number of items to take (select)</param>
        /// <param name="orderByProperty">property to order on</param>
        /// <param name="isAscending">if true orders ascending, otherwise descending</param>
        /// <returns>IEnumerable of T</returns>
        public IEnumerable<T> LoadRange(int skip, int take, Expression<Func<T, object>> orderByProperty, bool isAscending = true)
        {
            IQueryOver<T, T> query = Session.QueryOver<T>();

            if (isAscending)
                query = query.OrderBy(orderByProperty).Asc;
            else
                query = query.OrderBy(orderByProperty).Desc;

            IQueryOver<T> finishedQuery = query.Skip(skip).Take(take);

            return Transact<IEnumerable<T>>(() => finishedQuery.List());
        }

        /// <summary>
        /// Saves an item 
        /// </summary>
        /// <param name="item">item to save</param>
        public void SaveOrUpdate(T item)
        {
            Transact(() => Session.SaveOrUpdate(item));
        }
        
        /// <summary>
        /// Wraps a read query into a transaction
        /// </summary>
        /// <typeparam name="TResult">type of result expected</typeparam>
        /// <param name="function">method to wrap in transaction</param>
        /// <returns>TResult</returns>
        protected virtual TResult Transact<TResult>(Func<TResult> function)
        {
            return Session.Transact<TResult>(function);
        }

        /// <summary>
        /// wraps a write (or no response expected) query in a transaction
        /// </summary>
        /// <param name="action">method to wrap in transaction</param>
        protected virtual void Transact(Action action)
        {
            Session.Transact(action);
        }
    }
}
