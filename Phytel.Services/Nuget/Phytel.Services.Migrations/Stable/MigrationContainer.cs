using AutoMapper;
using Phytel.Services.AppSettings;
using Phytel.Services.Dates;
using Phytel.Services.IOC;
using Phytel.Services.Migrations.Mongo;
using Phytel.Services.Mongo.Repository;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Migrations
{
    public class MigrationContainer : ContainerDecorator
    {
        public const string ScopeName = "Migrations";

        protected readonly Assembly[] _assembliesWithMigrations;

        public MigrationContainer(ContainerBuilder containerBuilder, params Assembly[] assembliesWithMigrations)
            :base(containerBuilder)
        {
            _assembliesWithMigrations = assembliesWithMigrations;
        }

        protected override void OnBuild(Funq.Container container)
        {
            Mapper.CreateMap<MigrationLogEntity, MigrationLog>().ReverseMap();

            if(container.TryResolve<IMappingEngine>() == null)
            {
                container.Register<IMappingEngine>(Mapper.Engine);
            }
            if(container.TryResolve<IDateTimeProxy>() == null)
            {
                container.Register<IDateTimeProxy>(new DateTimeLocalProxy());
            }
            if(container.TryResolve<IAppSettingsProvider>() == null)
            {
                container.Register<IAppSettingsProvider>(
                    new EagerLoadedAppSettingsProvider(new Dictionary<string, string>()));
            }

            container.Register<IRepositoryMongo>(ScopeName, c =>
                new RepositoryMongo<MigrationMongoContext>(
                    new UnitOfWorkMongo<MigrationMongoContext>(
                        c.Resolve<IAppSettingsProvider>().Get(Constants.AppSettingKeys.DatabaseConnectionName, Constants.AppSettingDefaultValues.DatabaseConnectionName),
                        false
                        )
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IMigrationRepository>(c =>
                new MigrationRepository(
                    c.ResolveNamed<IRepositoryMongo>(ScopeName),
                    c.Resolve<IMappingEngine>(),
                    c.Resolve<IDateTimeProxy>()
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IMigrationHost>(c =>
                new MigrationHost(
                    c,
                    c.Resolve<IMigrationRepository>(),
                    _assembliesWithMigrations
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}
