using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Security;
using Jumbleblocks.Core.Collections;

namespace Jumbleblocks.Domain.Security
{
    /// <summary>
    /// Jumbleblocks principal
    /// </summary>
    public class JumbleblocksPrincipal : IJumbleblocksPrincipal
    {
        public JumbleblocksPrincipal(IJumbleblocksIdentity identity, User user = null)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");


            Identity = identity;
            User = user;
        }

        public IJumbleblocksIdentity Identity
        {
            get;
            private set;
        }

        protected User User
        {
            get;
            private set;
        }

        System.Security.Principal.IIdentity System.Security.Principal.IPrincipal.Identity
        {
            get { return Identity; }
        }

        /// <summary>
        /// Checks to see if user is in role with given name
        /// </summary>
        /// <param name="role">name of role to check</param>
        /// <returns>true if is in role, otherwise false</returns>
        public bool IsInRole(string role)
        {
            return User != null && User.HasRole(role);
        }

        /// <summary>
        /// Checks to see if user has operation with given name
        /// </summary>
        /// <param name="operationName">name of operation</param>
        /// <returns>true if can perform operation, otherwise false</returns>
        public bool HasOperation(string operationName)
        {
            return  User != null && User.HasOperation(operationName);
        }

        /// <summary>
        /// Checks to see if a user is allowed to perform (and doesnt just have)  a given role
        /// </summary>
        /// <param name="roleName">name of role to check</param>
        /// <returns>true if can, otherwise false</returns>
        public bool CanPerformRole(string roleName)
        {
            return UserIsAuthenticated && IsInRole(roleName);
        }

        /// <summary>
        /// Checks to see if a user is allowed to perform (and doesnt just have) a given operation
        /// </summary>
        /// <param name="operationName">name of operation to check</param>
        /// <returns>true if can, otherwise false</returns>
        public bool CanPerformOperation(string operationName)
        {
            return UserIsAuthenticated && HasOperation(operationName);
        }

        /// <summary>
        /// Determines if user is authenticated
        /// </summary>
        private bool UserIsAuthenticated
        {
            get { return Identity.IsAuthenticated; }
        }
      
    }
}
