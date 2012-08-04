using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Jumbleblocks.Core.Security
{
    public static class PrincipalHelpers
    {
        /// <summary>
        /// checks to see if principal can perform operation
        /// </summary>
        /// <param name="princial">principal to check</param>
        /// <param name="operationName">operation name to check</param>
        /// <returns>true if can perform operations, false if can not or not an IJumbleblocksPrincipal</returns>
        public static bool CanPerformOperation(this IPrincipal princial, string operationName)
        {
            var jumbleblocksPrincipal = princial as IJumbleblocksPrincipal;

            if (jumbleblocksPrincipal == null)
                return false;

            return jumbleblocksPrincipal.CanPerformOperation(operationName);
        }
    }
}
