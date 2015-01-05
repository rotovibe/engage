using Phytel.Services.IOC;
using Phytel.Services.ServiceStack.Provider;

namespace Phytel.Services.ServiceStack
{
    public class ServiceStackConfigProxyContainer : ContainerDecorator
    {
        public ServiceStackConfigProxyContainer(ContainerBuilder containerBuilder)
            : base(containerBuilder)
        {
        }

        protected override void OnBuild(Funq.Container container)
        {
            _container.RegisterAutoWiredAs<ServiceStackServiceConfigProxy, IServiceConfigProxy>().ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}