using Phytel.Services.API.Provider;
using Phytel.Services.IOC;
using ServiceStack.Common;

namespace Phytel.Services.API.Container
{
    public class ContractNumberContainer : ContainerDecorator
    {
        public ContractNumberContainer(ContainerBuilder containerBuilder)
            : base(containerBuilder)
        {
        }

        protected override void OnBuild(Funq.Container container)
        {
            container.Register<HostContext>(c => HostContext.Instance).ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<HostContextProxy, IHostContextProxy>().ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IContractNumberProvider>(new ContractNumberProvider());

            container.Register<string>(API.Container.Constants.NamedStringContractNumber, c =>
            {
                IHostContextProxy hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.GetItemAsString(API.Provider.Constants.HostContextKeyContractNumber);
            }).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}