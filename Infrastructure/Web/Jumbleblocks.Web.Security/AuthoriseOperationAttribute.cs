using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using Jumbleblocks.Core.Security;
using Jumbleblocks.Web.Core;

namespace Jumbleblocks.Web.Security
{
    /// <summary>
    /// Attribute to authorise based on an operation
    /// 
    /// user must have 1 or more operation names in order to be authorised
    /// </summary>
    /// <remarks>Ignores roles</remarks>
    public class AuthorizeOperationAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// .Ctor
        /// </summary>
        /// <param name="operationNames">operations that user must have in order to perform operation/task</param>
        /// <exception cref="ArgumentNullException">Thrown if operationNames is null</exception>
        /// <exception cref="ArgumentException">Thrown if operationsNames' length is 0</exception>
        public AuthorizeOperationAttribute(params string[] operationNames)
        {
            if (operationNames == null)
                throw new ArgumentNullException("operationNames");
            else if (operationNames.Length == 0)
                throw new ArgumentException("operationNames", "Must have at least 1 operation");

            OperationNames = operationNames;
        }

        /// <summary>
        /// The name of the operations required. Must have 1 or more of provided operations
        /// </summary>
        public IEnumerable<string> OperationNames { get; private set; }

        /// <summary>
        /// Gets web authenticator
        /// </summary>
        public IWebAuthenticator WebAuthenticator { get { return IocContext.Container.Resolve<IWebAuthenticator>(); } }

        /// <summary>
        /// Core authorisation method
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>true if user has operation in list of operation, if not faslse</returns>
        /// <remarks>Ignores roles</remarks>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        { 
            IJumbleblocksPrincipal principal = WebAuthenticator.EnsureAuthenticatedAsJumbleblocksPrincipal(httpContext.User);

            if (principal == null)
                return false;

            foreach (string operation in OperationNames)
            {
                if (principal.CanPerformOperation(operation.Trim()))
                    return true;
            }

            return false;
        }

    }
}
