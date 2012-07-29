using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jumbleblocks.Core.Collections
{
    /// <summary>
    /// Equality Comparer based on a lamda expression
    /// </summary>
    public class LlamdaEqualityComparer<T> : IEqualityComparer<T>
    {
        public LlamdaEqualityComparer(Func<T, T, bool> equalityMethod, Func<T, int> hashCodeMethod)
        {
            if (equalityMethod == null)
                throw new ArgumentNullException("equalityMethod");

            if (hashCodeMethod == null)
                throw new ArgumentNullException("hashCodeMethod");

            EqualityMethod = equalityMethod;
            HashcodeMethod = hashCodeMethod;
        }

        protected Func<T,T, bool> EqualityMethod { get; private set; }
        protected Func<T, int> HashcodeMethod { get; private set; }

        public bool Equals(T x, T y)
        {
            return EqualityMethod(x, y);
        }

        public int GetHashCode(T obj)
        {
            return HashcodeMethod(obj);
        }
    }
}
