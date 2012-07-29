using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.nHibernate;
using Jumbleblocks.Domain.Security;
using NHibernate;

namespace Jumbleblocks.DAL.Security
{
    /// <summary>
    /// Repository for user
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        public User LoadForUsernameAndPassword(string username, string hashedPassword)
        {
            var vQuery = Session.QueryOver<User>()
                                .Where(u => u.Username == username && u.Password == hashedPassword);

            return Transact<User>(() => vQuery.SingleOrDefault());
        }


        public User LoadForUsername(string username)
        {
            var vQuery = Session.QueryOver<User>().Where(u => u.Username == username);

            return Transact<User>(() => vQuery.SingleOrDefault());
        }
    }
}
