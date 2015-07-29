using Phytel.API.DataDomain.Contract.DTO;
using Phytel.Services.IOC;

namespace Phytel.Services.AppSettings.Contract
{
    public class ContractPropertyAppSettingsProviderContainer : ContainerDecorator
    {
        protected readonly bool _raiseExecptions;

        public ContractPropertyAppSettingsProviderContainer(ContainerBuilder containerBuilder, bool raiseExceptions = false)
            : base(containerBuilder)
        {
            _raiseExecptions = raiseExceptions;
        }

        protected override void OnBuild(Funq.Container container)
        {


            container.Register<IAppSettingsProvider>(Constants.ContractPropertyAppSettingsProviderScopeName, c =>
                {
                    if(_raiseExecptions)
                    {
                        return new ContractExceptionAppSettingsProvider(
                            c.TryResolve<IContractClient>(),
                            c.TryResolveNamed<string>(Phytel.Services.API.Filter.Constants.NamedStringContractNumber),
                            c.TryResolveNamed<string>(Phytel.Services.API.Filter.Constants.NamedStringContext)
                            );
                    }
                    else
                    {
                        return new ContractAppSettingsProvider(
                            c.TryResolve<IContractClient>(),
                            c.TryResolveNamed<string>(Phytel.Services.API.Filter.Constants.NamedStringContractNumber),
                            c.TryResolveNamed<string>(Phytel.Services.API.Filter.Constants.NamedStringContext)
                            );
                    }
                }).ReusedWithin(Funq.ReuseScope.Request);
        }
    }
}