using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;

namespace Jumbleblocks.Web.Core
{
    /// <summary>
    /// Static container for IOC
    /// </summary>
    public static class IocContext
    {
        /// <summary>
        /// DI Container
        /// </summary>
        public static IWindsorContainer Container { get; set; }

        /// <summary>
        /// Resolves a service
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>T</returns>
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
