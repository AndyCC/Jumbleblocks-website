using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Blog
{
    /// <summary>
    /// Exception throw when a user can not be found
    /// </summary>
    [Serializable]
    public class UnknownUserException : ApplicationException
    {
        public UnknownUserException(int? userId) 
        {
            UserId = userId;
        }

        public UnknownUserException(int? userId, string message) 
            : base(message) 
        {
            UserId = userId;
        }

        public UnknownUserException(int? userId, string message, Exception inner)
            : base(message, inner) 
        {
            UserId = userId;
        }

        protected UnknownUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }


        /// <summary>
        /// Id of user that could not be found
        /// </summary>
        public int? UserId { get; private set; }
    }
}
