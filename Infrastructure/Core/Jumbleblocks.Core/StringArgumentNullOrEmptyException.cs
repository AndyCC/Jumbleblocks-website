using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Core
{
    /// <summary>
    /// Exception for when a string argument is null or empty
    /// </summary>
    [Serializable]
    public class StringArgumentNullOrEmptyException : ArgumentException
    {
        public StringArgumentNullOrEmptyException() { }
        public StringArgumentNullOrEmptyException(string message) : base(message) { }

        public StringArgumentNullOrEmptyException(string message, string paramName){}

        public StringArgumentNullOrEmptyException(string message, Exception inner) : base(message, inner) { }
        protected StringArgumentNullOrEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
