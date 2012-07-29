using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Domain.Security
{
    /// <summary>
    /// Security Operation
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// Id of operation
        /// </summary>
        public virtual int? Id { get; protected set; }

        /// <summary>
        /// Name of operation
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Checks to see if the name of the operation matches the one provided
        /// </summary>
        /// <param name="operationName">name of operation</param>
        /// <returns>true if equal, otherwise false</returns>
        public virtual bool NameEquals(string operationName)
        {
            return Name.Equals(operationName);
        }
    }
}
