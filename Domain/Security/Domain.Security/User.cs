using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Collections;

namespace Jumbleblocks.Domain.Security
{
    /// <summary>
    /// Represents a user
    /// </summary>
    public class User
    {
        public virtual int? Id { get; protected set; }

        public virtual string Username { get; set; }
        public virtual string Password { get; protected set; }


        public virtual string Forenames { get; set; }
        public virtual string Surname { get; set; }
        
        private IList<Role> _roles = new List<Role>(0);
        public virtual IEnumerable<Role> Roles { get { return _roles; } }

        /// <summary>
        /// Adds a role to a user
        /// </summary>
        /// <param name="role">role to add</param>
        public virtual void AddRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException();

            if(!_roles.Contains(role))
                _roles.Add(role);
        }

        /// <summary>
        /// Checks to see if a user has a role
        /// </summary>
        /// <param name="roleName">name of role to check for</param>
        /// <returns>true if has, otherwise false</returns>
        public virtual bool HasRole(string roleName)
        {
            return _roles.Contains(r => r.NameEquals(roleName));
        }
        
        /// <summary>
        /// Checks to see if user has the requested operation
        /// </summary>
        /// <param name="operationName">name of operation to check for</param>
        /// <returns>true if has operation, otherwise false</returns>
        public virtual bool HasOperation(string operationName)
        {
            foreach (Role role in _roles)
            {
                if (role.HasOperation(operationName))
                    return true;
            }

            return false;
        }
    }
}
