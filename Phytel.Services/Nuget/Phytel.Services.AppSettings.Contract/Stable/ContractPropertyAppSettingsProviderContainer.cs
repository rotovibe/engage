using Phytel.API.DataDomain.Contract.DTO;
using Phytel.Services.IOC;

namespace Phytel.Services.AppSettings.Contract
{
    public class ContractPropertyAppSettingsProviderContainer : ContainerDecorator
    {
        public ContractPropertyAppSettingsProviderContainer(ContainerBuilder containerBuilder)
            : base(containerBuilder)
        {
        }

        protected override void OnBuild(Funq.Container container)
        {
            container.Register<IAppSettingsProvider>(Constants.ContractPropertyAppSettingsProviderScopeName, c =>
                new ContractAppSettingsProvider(
                    c.TryResolve<IContractClient>(),
                    c.TryResolveNamed<string>(Phytel.Services.API.Filter.Constants.NamedStringContractNumber),
                    c.TryResolveNamed<string>(Phytel.Services.API.Filter.Constants.NamedStringContext)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}