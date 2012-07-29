using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Web.Wane
{
    /// <summary>
    /// The type of token
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// The token is for text
        /// </summary>
        Text,

        /// <summary>
        /// A new line
        /// </summary>
        NewLine,

        /// <summary>
        /// The token forms a delimeter
        /// </summary>
        Delimiter
    }
}
