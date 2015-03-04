using Phytel.Services.API.Cache.Mongo;
using Phytel.Services.AppSettings;
using Phytel.Services.Dates;
using Phytel.Services.IOC;
using Phytel.Services.Mongo.Repository;
using ServiceStack.CacheAccess;

namespace Phytel.Services.API.Cache
{
    public class MongoCacheClientContainer : ContainerDecorator
    {
        public const string ScopeName = "Phytel.Services.API.Cache.Mongo";

        public MongoCacheClientContainer(ContainerBuilder containerBuilder)
            : base(containerBuilder)
        {
        }

        protected override void OnBuild(Funq.Container container)
        {
            container.Register<IUnitOfWorkMongo<CacheContext>>(ScopeName, c =>
                new UnitOfWorkMongo<CacheContext>(
                    c.Resolve<IAppSettingsProvider>().Get(Services.API.Cache.Constants.AppSettingsKeys.ConnectionName, Services.API.Cache.Constants.AppSettingDefaultValues.ConnectionName),
                    false
                    )
                ).ReusedWithin(Funq.ReuseScope.Default);

            container.Register<IRepositoryMongo>(ScopeName, c =>
                new RepositoryMongo<CacheContext>(
                    c.ResolveNamed<IUnitOfWorkMongo<CacheContext>>(ScopeName)
                    )
                ).ReusedWithin(Funq.ReuseScope.Default);

            container.Register<IPhytelCache>(c =>
                new CacheRepository(
                    c.ResolveNamed<IRepositoryMongo>(ScopeName),
                    c.Resolve<IDateTimeProxy>()
                    )
                ).ReusedWithin(Funq.ReuseScope.Default);

            container.Register<ICacheClient>(c =>
                new MongoCacheClient(
                    c.Resolve<IPhytelCache>(),
                    c.Resolve<IDateTimeProxy>()
                    )
                );
        }
    }
}