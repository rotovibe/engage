using AutoMapper;
using Phytel.Services.API.Provider;
using Phytel.Services.AppSettings;
using Phytel.Services.Dates;
using Phytel.Services.Dispatch.Ase;
using Phytel.Services.IOC;
using Phytel.Services.Journal.Dispatch;
using Phytel.Services.Serializer;

namespace Phytel.Services.Journal
{
    public class JournalDispatcherContainerDecorator : ContainerDecorator
    {
        public const string AppSettingKeyPublishKey = "Ase.PublishKey.Journal.PostEntries";
        public const string PublishKey = "journalpostentries";

        protected readonly string _appSettingKeyPublishKey;
        protected readonly string _publishKey;

        public JournalDispatcherContainerDecorator(ContainerBuilder containerBuilder, string publishKey = null, string appSettingKeyPublishKey = AppSettingKeyPublishKey)
            : base(containerBuilder)
        {
            _appSettingKeyPublishKey = appSettingKeyPublishKey;
            _publishKey = publishKey;
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

            container.Register<IJournalDispatcher>(c =>
                new JournalDispatcher(
                    new AseBusDispatcher(
                        c.Resolve<IAppSettingsProvider>().Get(_appSettingKeyPublishKey, _publishKey, PublishKey),
                        new SerializerJson()
                        ),
                        c.Resolve<IActionIdProvider>(),
                        new DateTimeUtcProxy(),
                        c.Resolve<IServiceConfigProxy>(),
                        c.Resolve<IMappingEngine>()
                    )
                );
        }
    }
}