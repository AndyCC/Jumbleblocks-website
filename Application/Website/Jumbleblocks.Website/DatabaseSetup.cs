using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor;
using NHibernate;
using NHibernate.Cfg;
using Jumbleblocks.nHibernate;
using System.Data;
using Jumbleblocks.Core.Configuration;
using NHibernate.Mapping.ByCode;
using Jumbleblocks.DAL.Blog;
using Jumbleblocks.DAL.Security;
using NHibernate.Context;
using Castle.MicroKernel.Registration;
using Jumbleblocks.Domain.Blog;
using Jumbleblocks.Domain;
using Jumbleblocks.Domain.Security;

namespace Jumbleblocks.Website
{
    public static class DatabaseSetup
    {
        /// <summary>
        /// Sets up db registering session factory and repositories
        /// </summary>
        /// <param name="configurationReader">configuration reader</param>
        /// <param name="container">windsor container</param>
        public static void CreateSessionFactoryAndRegisterRepositories(IConfigurationReader configurationReader,  IWindsorContainer container)
        {
            ISessionFactory sessionFactory = CreateSessionFactory(configurationReader);
            container.Register(Component.For<ISessionFactory>().Instance(sessionFactory).LifeStyle.Singleton);
            RegisterRepositories(container);
        }

        /// <summary>
        /// Creates session factory
        /// </summary>
        /// <param name="configurationReader">configuration reader</param>
        /// <returns></returns>
        private static ISessionFactory CreateSessionFactory(IConfigurationReader configurationReader)
        {
            var configuration = new NHibernate.Cfg.Configuration();
            configuration.SessionFactoryName("Jumblocks Blog");

            configuration.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2008FixedDialect>();
                db.IsolationLevel = IsolationLevel.ReadCommitted;
                db.ConnectionString = configurationReader.ConnectionStrings["BlogDb"].ConnectionString;
                db.Timeout = 10;
                db.BatchSize = 100;

                //for testing
                db.LogFormattedSql = true;
                db.LogSqlInConsole = true;
                db.AutoCommentSql = true;
            });

            var mapper = new ModelMapper();
            mapper.AddMapping<BlogPostMap>();
            mapper.AddMapping<BlogUserMap>();
            mapper.AddMapping<ImageReferenceMap>();
            mapper.AddMapping<TagMap>();
            mapper.AddMapping<SeriesMap>();

            mapper.AddMapping<UserMap>();
            mapper.AddMapping<RoleMap>();
            mapper.AddMapping<OperationMap>();

            configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());
            configuration.CurrentSessionContext<WebSessionContext>();

            return configuration.BuildSessionFactory();
        }

        /// <summary>
        /// Creates repositories
        /// </summary>
        /// <param name="container">container to add to</param>
        /// <remarks>may replace with castle installera at a later date</remarks>
        private static void RegisterRepositories(IWindsorContainer container)
        {
            container.Register(Component.For<IBlogPostRepository>().ImplementedBy<BlogPostRepository>().LifeStyle.Transient);
            container.Register(Component.For<IImageReferenceRepository>().ImplementedBy<ImageReferenceRepository>().LifeStyle.Transient);
            container.Register(Component.For<ILookupRepository>().ImplementedBy<LookupRepository>().LifeStyle.Transient);
            container.Register(Component.For<IUserRepository>().ImplementedBy<UserRepository>().LifeStyle.Transient);
        }
    }
}