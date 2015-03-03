using Phytel.Services.API.Cache.Mongo;
using Phytel.Services.AppSettings;
using Phytel.Services.IOC;
using Phytel.Services.Mongo.Repository;

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
                    c.Resolve<IAppSettingsProvider>().Get(Constants.AppSettingsKeys.ConnectionName, Constants.AppSettingDefaultValues.ConnectionName),
                    false
                    )
                ).ReusedWithin(Funq.ReuseScope.Default);
        }
    }
}