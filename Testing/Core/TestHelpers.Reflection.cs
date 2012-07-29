using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Jumbleblocks.Testing
{
    public static partial class TestHelpers
    {
        public static TClass CreateClassFromCtor<TClass>(params object[] parameters)
        {
            var t = typeof(TClass);

            IEnumerable<ConstructorInfo> ctors = t.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(ci => ci.GetParameters().Count() == parameters.Count());

            if (ctors.Count() == 0)
                throw new InvalidOperationException("Can not find constructors with same number of parameters as provided");

            ConstructorInfo found = null;

            foreach (var ci in ctors)
            {
                bool parametersMatch = true;

                ParameterInfo[] ciParameters = ci.GetParameters();

                for(int i=0; i<ciParameters.Length; i++)
                {
                    Type ciParamType = ciParameters[i].ParameterType;
                    Type paramType = parameters[i].GetType();

                    if (paramType != ciParamType && !paramType.IsSubclassOf(ciParamType) && !ciParamType.IsAssignableFrom(paramType))
                    {
                        parametersMatch = false;
                        break;
                    }                        
                }

                if (parametersMatch)
                {
                    found = ci;
                    break;
                }
            }

            if (found == null)
                throw new InvalidOperationException("Can not find a constructor with same parameter types in the exact same order");

            return (TClass)found.Invoke(parameters);
        }
    }
}
