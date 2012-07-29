using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Jumbleblocks.Core.Expressions
{
    /// <summary>
    /// Extension methods which help with expressions
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Gets property name
        /// </summary>
        /// <typeparam name="TClass">Type of class</typeparam>
        /// <typeparam name="TProperty">Type underlying property</typeparam>
        /// <param name="property">expression to property</param>
        /// <returns>name of specified property</returns>
        public static string GetPropertyName<TClass, TProperty>(this TClass @class, Expression<Func<TClass, TProperty>> property)
            where TClass : class
        {
            var expression = (MemberExpression)property.Body;
            return expression.Member.Name;
        }


        /// <summary>
        /// Gets property name
        /// </summary>
        /// <typeparam name="TClass">Type of class</typeparam>
        /// <typeparam name="TProperty">Type underlying property</typeparam>
        /// <param name="property">expression to property</param>
        /// <returns>name of specified property</returns>
        public static string GetPropertyName<TClass, TProperty>(this Expression<Func<TClass, TProperty>> property)
        {
            var expression = (MemberExpression)property.Body;
            return expression.Member.Name;
        }

        /// <summary>
        /// Gets a value from a property
        /// </summary>
        /// <typeparam name="TClass">Type of class</typeparam>
        /// <typeparam name="TProperty">type underling property</typeparam>
        /// <param name="fromClass">class to get vakye from</param>
        /// <param name="property">path to property</param>
        /// <returns>value in property</returns>
        /// <remarks>useful when you are passing a lamda to identify a property to use in some calculation</remarks>
        public static TProperty GetPropertyValue<TClass, TProperty>(this TClass fromClass, Expression<Func<TClass, TProperty>> property)
        {
            PropertyInfo propertyInfo = fromClass.GetPropertyInfo(property); 
            return (TProperty)propertyInfo.GetValue(fromClass, null);
        }

        /// <summary>
        /// Gets property info for property
        /// </summary>
        /// <typeparam name="TClass">type of class property on</typeparam>
        /// <typeparam name="TProperty">type of property</typeparam>
        /// <param name="fromClass">class to retrieve property from</param>
        /// <param name="property">Property to get propertu info for</param>
        /// <param name="pBindingFlags">Binding flags to find property info with</param>
        /// <returns>PropertyInfo</returns>
        public static PropertyInfo GetPropertyInfo<TClass, TProperty>(this TClass fromClass, Expression<Func<TClass, TProperty>> property, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            return typeof(TClass).GetProperty(property.GetPropertyName(), bindingFlags);
        }
    }
}
