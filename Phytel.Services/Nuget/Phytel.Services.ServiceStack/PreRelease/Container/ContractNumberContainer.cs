using Phytel.Services.IOC;
using Phytel.Services.ServiceStack.Provider;
using Phytel.Services.ServiceStack.Proxy;
using ServiceStack.Common;

namespace Phytel.Services.ServiceStack
{
    public class ContractNumberContainer : ContainerDecorator
    {
        public ContractNumberContainer(ContainerBuilder containerBuilder)
            : base(containerBuilder)
        {
        }

        protected override void OnBuild(Funq.Container container)
        {
            _container.RegisterIfNotAlready<HostContext>(c => HostContext.Instance, Funq.ReuseScope.Request);
            _container.RegisterAutoWiredAsIfNotAlready<HostContextProxy, IHostContextProxy>(Funq.ReuseScope.Request);

            _container.RegisterAutoWiredAsIfNotAlready<ContractNumberProvider, IContractNumberProvider>(Funq.ReuseScope.Request);

            _container.Register<string>(Constants.NamedStringContractNumber, c =>
            {
                IHostContextProxy hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.GetItemAsString(Constants.HostContextKeyContractNumber);
            }).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}