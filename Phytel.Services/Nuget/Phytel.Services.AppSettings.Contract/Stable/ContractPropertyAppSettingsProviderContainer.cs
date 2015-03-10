using Phytel.API.DataDomain.Contract.DTO;
using Phytel.Services.IOC;
using System.Collections.Generic;

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
            container.Register<IAppSettingsProvider>(Constants.ContractPropertyAppSettingsProviderScopeName, OnResolveContractPropertyAppSettingsProvider).ReusedWithin(Funq.ReuseScope.Request);
        }

        protected virtual IAppSettingsProvider OnResolveContractPropertyAppSettingsProvider(Funq.Container container)
        {
            IAppSettingsProvider rvalue = null;

            IContractClient contractClient = container.Resolve<IContractClient>();
            string contractNumber = container.ResolveNamed<string>(Phytel.Services.API.Container.Constants.NamedStringContractNumber);
            string context = container.ResolveNamed<string>(Phytel.Services.API.Container.Constants.NamedStringContext);

            if (string.IsNullOrEmpty(context))
            {
                context = "Unknown";
            }

            IDictionary<string, string> appSettings = new Dictionary<string, string>();

            if (contractClient != null && !string.IsNullOrEmpty(contractNumber))
            {
                List<ContractProperty> contractProperties = contractClient.GetAllContractProperties(contractNumber, context);
                foreach (ContractProperty contractProperty in contractProperties)
                {
                    if (!string.IsNullOrEmpty(contractProperty.Key))
                    {
                        KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(contractProperty.Key, contractProperty.Value);
                        appSettings.Add(kvp);
                    }
                }
            }

            rvalue = new AppSettingsProvider(appSettings);

            return rvalue;
        }
    }
}