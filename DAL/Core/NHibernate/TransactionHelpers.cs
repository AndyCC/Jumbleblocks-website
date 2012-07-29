using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Jumbleblocks.nHibernate
{
    /// <summary>
    /// Transaction helpers for session
    /// </summary>
    public static class TransactionHelpers
    {
        /// <summary>
        /// Wraps a read query into a transaction
        /// </summary>
        /// <typeparam name="TResult">expected result type</typeparam>
        /// <param name="session">session to perform query against</param>
        /// <param name="function">function containing query</param>
        /// <returns>TResult</returns>
        public static TResult Transact<TResult>(this ISession session, Func<TResult> function)
        {
            if (!session.Transaction.IsActive)
            {
                //wrap transaction
                TResult result;

                using (var transaction = session.BeginTransaction())
                {
                    result = function.Invoke();
                    transaction.Commit();
                }

                return result;
            }

            //already in transaction (so don't wrap)
            return function.Invoke();
        }

        public static void Transact(this ISession session, Action action)
        {
            session.Transact<bool>(() =>
                  {
                      action.Invoke();
                      return false;
                  });
        }
    }
}
