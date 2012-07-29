using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Jumbleblocks.Core.Expressions;
using System.Reflection;

namespace Jumbleblocks.Testing
{
    /// <summary>
    /// allows setting of properties and calling of methods which are private or protected to allow them to be set
    /// </summary>
    public static class PrivateProtectedInternalHelpers
    {
        public static void SetProperty<TClass, TProperty>(this TClass o, Expression<Func<TClass, TProperty>> propertyGetter, TProperty valueToSet)
        {
            PropertyInfo propertyInfo = o.GetPropertyInfo(propertyGetter, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            propertyInfo.SetValue(o, valueToSet, null);
        }

        public static void SetProperty<TClass, TProperty>(this TClass o, string propertyName, TProperty valueToSet)
        {
            PropertyInfo propertyInfo = typeof(TClass).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            propertyInfo.SetValue(o, valueToSet, null);
        }

        public static void CallMethod<TClass>(this TClass o, string methodName, params object[] parameters)
        {
            Type type = typeof(TClass);
            Type[] parameterTypes = parameters.Select(p => p.GetType()).ToArray();

            MethodInfo methodInfo =  type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, parameterTypes, null);

            if (methodInfo == null)
                throw new InvalidOperationException(String.Format("Can not find method '{0}' on type '{1}' with provided parameters", methodName, type.FullName));

            methodInfo.Invoke(o, parameters);
        }
    }
}
