using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Jumbleblocks.Core.Configuration;
using Jumbleblocks.Website.Configuration;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Jumbleblocks.Web.Core;
using System.Reflection;

using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Domain.Security;
using Jumbleblocks.Domain.Blog.Deletion;
using Jumbleblocks.Core.Logging;
using Jumbleblocks.nHibernate;

using Jumbleblocks.DAL.Blog;
using Jumbleblocks.Web.Wane;
using Jumbleblocks.Core.Cryptography;
using Jumbleblocks.Core.Security;
using Jumbleblocks.Web.Security;
using Jumbleblocks.Web.Wane.ParseRules.Delimeters;
using Jumbleblocks.Web.Wane.ParseRules.Properties;
using Jumbleblocks.DAL.Security;
using Jumbleblocks.Website.TempLogger;
using System.Data;
using Jumbleblocks.Domain;
using NHibernate;
using NHibernate.Context;

namespace Jumbleblocks.Website
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        public IConfigurationReader ConfigurationReader { get; private set; }

        protected BlogConfigurationSection BlogConfiguration
        {
            get
            {
                return ConfigurationReader.GetSection<BlogConfigurationSection>(BlogContants.BlogSettingsConfigName);
            }
        }

        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();


            AreaRegistration.RegisterAllAreas();

            IWindsorContainer container = CreateWindsorContainter();
            IocContext.Container = container;

            ConfigurationReader = container.Resolve<IConfigurationReader>();
            DatabaseSetup.CreateSessionFactoryAndRegisterRepositories(ConfigurationReader, container);
                  
            RegisterControllerFactory(container);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            RegisterWaneTextParser(container);
            RegisterSecurity(container);

            RegisterLogging(container);
        }


        private static IWindsorContainer CreateWindsorContainter()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<IConfigurationReader>()
                                        .ImplementedBy<ConfigurationManagerWrapper>()
                                        .LifeStyle.Singleton);


            return container;
        }


        private static void RegisterControllerFactory(IWindsorContainer windsorContainer)
        {
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(windsorContainer, Assembly.GetExecutingAssembly()));
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("*/Scripts/*");

            routes.MapRoute(
             "ImageList",
             "Image/ImageList/",
             new { controller = "Image", action = "ImageList" }
             );

            routes.MapRoute(
                "BlogSummary",
                "Blog/Index/{page}",
                new { controller = "Blog", action = "Index", page = UrlParameter.Optional },
                new { page = @"\d+" }
            );

            routes.MapRoute(
                "ShowArticle",
                "Article/{year}/{month}/{day}/{title}",
                new { controller = "article", action = "Show" },
                new { year = @"\d+", month = @"\d+", day = @"\d+" }
                );

            routes.MapRoute(
                 "LoginForm",
                 "Authentication/{action}/{redirectUrl}",
                 new { controller = "Authentication", action = "LoginForm" },
                 new { redirectUrl = BlogConfiguration.AuthenticationDefaultRedirectUrl }
             );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/", // URL with parameters
                new { controller = BlogConfiguration.DefaultController, action = BlogConfiguration.DefaultAction } // Parameter defaults
            );

        }

        private void RegisterWaneTextParser(IWindsorContainer container)
        {
            container.Register(Component.For<IDelimeterSet>()
                                        .ImplementedBy<DelimeterSet>()
                                        .OnCreate((ds) =>
                                        {
                                            ds.AddDelimeterParseRule(new BoldParseRule());
                                            ds.AddDelimeterParseRule(new ItalicsParseRule());
                                            ds.AddDelimeterParseRule(new CodeParseRule());
                                            ds.AddDelimeterParseRule(new ImageParseRule());
                                            ds.AddDelimeterParseRule(new AnchorParseRule());
                                            ds.AddGlobalPropertyParseRule(new StyleClassParseRule());                                          
                                        }).LifestylePerWebRequest());

            container.Register(Component.For<IWaneTransform>()
                                        .ImplementedBy<WaneTransform>()
                                        .LifestylePerWebRequest());
        }


        private void RegisterSecurity(IWindsorContainer container)
        {

            container.Register(Component.For<IStringHasher>()
                                        .ImplementedBy<MD5StringHasher>()
                                        .LifestyleSingleton());

            container.Register(Component.For<IJumbleblocksSecurityService>()
                                        .ImplementedBy<SecurityService>()
                                        .LifestylePerWebRequest());

            container.Register(Component.For<IFormsAuthentication>()
                                        .ImplementedBy<FormsAuthenticationWrapper>()
                                        .LifestylePerWebRequest());

            container.Register(Component.For<IUserCache>()
                                         .ImplementedBy<HttpContextUserCache>()
                                         .LifestylePerWebRequest());

            container.Register(Component.For<IWebAuthenticator>()
                                        .ImplementedBy<WebAuthenticator>()
                                        .LifestylePerWebRequest());
        }

        private static void RegisterLogging(IWindsorContainer container)
        {
            container.Register(Component.For<IJumbleblocksLogger>().ImplementedBy<Logger>().LifeStyle.Transient);
        }

        protected void Application_BeginRequest()
        {
            StartSession();
        }

        protected void Application_EndRequest()
        {
            DisposeSession();
        }

        #region NHibernate

        //TODO: possibly move to own HTTPModule
        protected static ISessionFactory SessionFactory { get { return IocContext.Resolve<ISessionFactory>(); } }            

        protected void StartSession()
        {
            var session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
        }

        protected void DisposeSession()
        {
            var session = CurrentSessionContext.Unbind(SessionFactory);

            if (session.IsOpen)
                session.Close();

            session.Dispose();
        }

        #endregion
    }
}