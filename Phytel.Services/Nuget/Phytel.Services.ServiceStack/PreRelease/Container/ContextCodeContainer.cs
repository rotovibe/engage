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
            _container.RegisterIfNotAlready<HostContext>(c => HostContext.Instance, Funq.ReuseScope.Request);
            _container.RegisterAutoWiredAsIfNotAlready<HostContextProxy, IHostContextProxy>(Funq.ReuseScope.Request);

            _container.Register<string>(Constants.NamedStringContextCode, c =>
            {
                IHostContextProxy hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.GetItem<string>(Constants.HostContextKeyContextCode);
            }).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}