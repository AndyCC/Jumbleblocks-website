using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Security
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Loads user for username and password combo
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="hashedPassword">hashed password</param>
        /// <returns>USer</returns>
        User LoadForUsernameAndPassword(string username, string hashedPassword);

        /// <summary>
        /// Loads for user name
        /// </summary>
        /// <param name="username">name of user</param>
        /// <returns>User</returns>
        User LoadForUsername(string username);
    }
}
