using AutoMapper;
using Phytel.Services.AppSettings;
using Phytel.Services.Dates;
using Phytel.Services.Dispatch.Ase;
using Phytel.Services.IOC;
using Phytel.Services.Journal.Dispatch;
using Phytel.Services.Serializer;
using Phytel.Services.ServiceStack.Provider;

namespace Phytel.Services.Journal
{
    public class JournalDispatcherContainerDecorator : ContainerDecorator
    {
        public const string AppSettingKeyPublishKey = "Ase.PublishKey.AuditActionFailed";
        public const string PublishKey = "journalentryadd";

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
            container.Register<IJournalDispatcher>(c =>
                new JournalDispatcher(
                    new AseBusDispatcher(
                        OnBuildGetPublishKey(c, _appSettingKeyPublishKey, _publishKey),
                        new SerializerJson()
                        ),
                        c.Resolve<IActionIdProvider>(),
                        c.Resolve<IDateTimeProxy>(),
                        c.Resolve<IServiceConfigProxy>(),
                        c.Resolve<IMappingEngine>()
                    )
                );
        }

        protected virtual string OnBuildGetPublishKey(Funq.Container container, string appSettingKeyPublishKey, string publishKey)
        {
            string rvalue = PublishKey;

            if (string.IsNullOrEmpty(publishKey))
            {
                IAppSettingsProvider appSettingsProvider = container.Resolve<IAppSettingsProvider>();
                string appSettingValue = appSettingsProvider.Get(appSettingKeyPublishKey);
                if (!string.IsNullOrEmpty(appSettingValue))
                {
                    rvalue = appSettingValue;
                }
            }

            return rvalue;
        }
    }
}