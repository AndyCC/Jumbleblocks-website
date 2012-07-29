using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Jumbleblocks.Core.Security
{
    /// <summary>
    /// interface for jumbleblocks principal
    /// </summary>
    public interface IJumbleblocksPrincipal : IPrincipal
    {
        /// <summary>
        /// Identity of user
        /// </summary>
        new IJumbleblocksIdentity Identity { get; }

        /// <summary>
        /// Checks to see if user has operation with given name
        /// </summary>
        /// <param name="operationName">name of operation</param>
        /// <returns>true if can perform operation, otherwise false</returns>
        bool HasOperation(string operationName);

        /// <summary>
        /// Checks to see if a user is allowed to perform (and doesnt just have)  a given role
        /// </summary>
        /// <param name="roleName">name of role to check</param>
        /// <returns>true if can, otherwise false</returns>
        bool CanPerformRole(string roleName);

        /// <summary>
        /// Checks to see if a user is allowed to perform (and doesnt just have) a given operation
        /// </summary>
        /// <param name="operationName">name of operation to check</param>
        /// <returns>true if can, otherwise false</returns>
        bool CanPerformOperation(string operationName);

    }
}
