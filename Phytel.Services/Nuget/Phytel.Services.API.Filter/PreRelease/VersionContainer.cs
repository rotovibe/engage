using Phytel.Services.API.Provider;
using Phytel.Services.IOC;
using ServiceStack.Common;

namespace Phytel.Services.API.Filter
{
    public class VersionContainer : ContainerDecorator
    {
        public VersionContainer(ContainerBuilder containerBuilder)
            : base(containerBuilder)
        {
        }

        protected override void OnBuild(Funq.Container container)
        {
            container.Register<HostContext>(c => HostContext.Instance).ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<HostContextProxy, IHostContextProxy>().ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IVersionProvider>(new VersionProvider());

            container.Register<double>(Constants.NamedStringVersion, c =>
            {
                IHostContextProxy hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.GetItemAsDouble(API.Provider.Constants.HostContextKeyVersion);
            }).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}