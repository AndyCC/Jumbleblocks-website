using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jumbleblocks.Core.Collections;

namespace Jumbleblocks.Domain.Security
{
    /// <summary>
    /// Security Role
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Id of Role
        /// </summary>
        public virtual int? Id { get; protected set; }

        /// <summary>
        /// Name of role
        /// </summary>
        public virtual string Name { get; set; }


        private IList<Operation> _operations = new List<Operation>(0);

        /// <summary>
        /// Operations within role
        /// </summary>
        public virtual IEnumerable<Operation> Operations
        {
            get { return _operations; }
        }

        /// <summary>
        /// Adds an operation to the list
        /// </summary>
        /// <param name="operation">operation to save</param>
        public virtual void AddOperation(Operation operation)
        {
            if (operation == null)
                throw new ArgumentNullException("operation");

            if(!_operations.Contains(operation))
               _operations.Add(operation);
        }

        /// <summary>
        /// Checks to see if a role has an operation
        /// </summary>
        /// <param name="operationName">name of operation to check</param>
        /// <returns>true if has operation, otherwise false</returns>
        public virtual bool HasOperation(string operationName)
        {
            return _operations.Contains(o => o.NameEquals(operationName));
        }

        /// <summary>
        /// Checks to see if the role name equals the provided name
        /// </summary>
        /// <param name="roleName">name of role to check</param>
        /// <returns>true if name equals, otherwise false</returns>
        public virtual bool NameEquals(string roleName)
        {
            return Name.Equals(roleName);
        }
    }
}
