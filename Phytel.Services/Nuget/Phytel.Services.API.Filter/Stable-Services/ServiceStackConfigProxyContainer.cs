using Phytel.Services.API.Provider;
using Phytel.Services.IOC;

namespace Phytel.Services.API.Filter
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