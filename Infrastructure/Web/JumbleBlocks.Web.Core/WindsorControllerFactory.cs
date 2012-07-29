using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using System.Web.Routing;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace Jumbleblocks.Web.Core
{
    /// <summary>
    /// Controller factory using castle windsor
    /// </summary>
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        public WindsorControllerFactory(IWindsorContainer diContainer, params Assembly[] assembliesWithControllers)
        {
            if (diContainer == null)
                throw new ArgumentNullException("diContainer");

            DiContainer = diContainer;

            RegisterControllersInAssemblies(assembliesWithControllers);
        }

        /// <summary>
        /// Registers the controllers in the given assemblies
        /// </summary>
        /// <param name="assembliesWithControllers">assenblies to look in</param>
        private void RegisterControllersInAssemblies(Assembly[] assembliesWithControllers)
        {
            foreach (var asm in assembliesWithControllers)
            {
                DiContainer.Register(AllTypes.FromAssembly(asm)
                                              .BasedOn<IController>()
                                              .Configure((c) => c.Named(c.Implementation.FullName).LifestyleTransient())
                                              );

            }
        }


        /// <summary>
        /// Gets/Sets windsor container to use
        /// </summary>
        protected IWindsorContainer DiContainer { get; set; }

        /// <summary>
        /// Gets controller instance 
        /// </summary>
        /// <param name="requestContext">request context </param>
        /// <param name="controllerType">controloler that has been requested</param>
        /// <returns>IController</returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return base.GetControllerInstance(requestContext, controllerType);

            return DiContainer.Resolve<IController>(controllerType.FullName);
        }
    }
}
