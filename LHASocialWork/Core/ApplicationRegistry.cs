using System;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using LHASocialWork.Entities;
using LHASocialWork.Models.Shared;
using LHASocialWork.Repositories;
using LHASocialWork.Services.Account;
using NHibernate;
using StructureMap.Configuration.DSL;

namespace LHASocialWork.Core
{
    public class ApplicationRegistry : Registry
    {
        public ApplicationRegistry()
        {
            For<IBaseRepository>().Use<BaseRepository>();
            For<ISessionFactory>().Singleton().Use(x => SessionFactory);
            For<ISession>().HttpContextScoped().Use(context => context.GetInstance<ISessionFactory>().OpenSession());
            For<GridOptionsModel>().Use(new GridOptionsModel { Column = "Id", Direction = MvcContrib.Sorting.SortDirection.Ascending, Page = 0 });

            Scan(x =>
            {
                x.TheCallingAssembly();
                x.Assembly(Assembly.GetAssembly(typeof(IAccountService)));
                x.WithDefaultConventions();
            });
        }

        private static string DatabaseConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["LHASocialWorkSite"].ConnectionString;
            }
        }

        private static ISessionFactory SessionFactory
        {
            get
            {
                return Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(DatabaseConnectionString))
                    .Mappings(m => m.FluentMappings
                                    .AddFromAssemblyOf<User>()
                                    .Conventions.Add(PrimaryKey.Name.Is(x =>
                                   {
                                       var fqn = x.EntityType.ToString();
                                       var lastPeriondIndex = fqn.LastIndexOf('.') + 1;
                                       if (lastPeriondIndex > fqn.Length) lastPeriondIndex = 0;
                                       return String.Format("{0}Id", fqn.Substring(lastPeriondIndex));
                                   })))
                    .BuildSessionFactory();
            }
        }
    }
}