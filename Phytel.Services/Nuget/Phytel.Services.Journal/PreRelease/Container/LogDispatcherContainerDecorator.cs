using AutoMapper;
using Phytel.Services.API.Provider;
using Phytel.Services.AppSettings;
using Phytel.Services.Dates;
using Phytel.Services.Dispatch.Ase;
using Phytel.Services.IOC;
using Phytel.Services.Journal.Dispatch;
using Phytel.Services.Serializer;
using System;

namespace Phytel.Services.Journal
{
    public class LogDispatcherContainerDecorator : ContainerDecorator
    {
        public LogDispatcherContainerDecorator(ContainerBuilder containerBuilder)
            : base(containerBuilder)
        {
        }

        protected override void OnBuild(Funq.Container container)
        {
            container.Register<IMappingEngine>(Mapper.Engine);

            if (container.TryResolve<IAppSettingsProvider>() == null)
            {
                container.Register<IAppSettingsProvider>(new ConfigAppSettingsProvider());
            }
            if (container.TryResolve<IActionIdProvider>() == null)
            {
                container.Register<IActionIdProvider>(new ActionIdAsMongoObjectIdProvider());
            }
            if (container.TryResolve<IServiceConfigProxy>() == null)
            {
                container.Register<IServiceConfigProxy>(new ServiceStackServiceConfigProxy());
            }

            container.Register<ILogDispatcher>(c =>
                new LogDispatcher(
                    new AseBusDispatcher(
                        c.Resolve<IAppSettingsProvider>().Get(Constants.AppSettingKeys.PutEventPublishKey, Constants.AppSettingDefaultValues.PutEventPublishKey),
                        new SerializerJson()
                        ),
                        c.Resolve<IActionIdProvider>(),
                        new DateTimeUtcProxy(),
                        c.Resolve<IServiceConfigProxy>(),
                        c.Resolve<IMappingEngine>()
                    )
            ).ReusedWithin(Funq.ReuseScope.Request); ;
        }
    }
}