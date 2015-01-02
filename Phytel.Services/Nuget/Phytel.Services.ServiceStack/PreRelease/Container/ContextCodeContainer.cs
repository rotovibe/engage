using Phytel.Services.IOC;
using Phytel.Services.ServiceStack.Proxy;
using ServiceStack.Common;

namespace Phytel.Services.ServiceStack
{
    public class ContextCodeContainer : ContainerDecorator
    {
        public ContextCodeContainer(ContainerBuilder containerBuilder)
            : base(containerBuilder)
        {
        }

        protected override void OnBuild(Funq.Container container)
        {
            container.Register<HostContext>(c => HostContext.Instance).ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<HostContextProxy, IHostContextProxy>().ReusedWithin(Funq.ReuseScope.Request);

            container.Register<string>(Constants.NamedStringContextCode, c =>
            {
                IHostContextProxy hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.GetItem<string>(Constants.HostContextKeyContextCode);
            }).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}