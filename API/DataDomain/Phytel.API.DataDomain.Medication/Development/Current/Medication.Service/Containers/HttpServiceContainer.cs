using DataDomain.Medication.Repo.Containers;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Service.Containers
{
    public static class HttpServiceContainer
    {
        private static string Proxy = "MedicationProxy";

        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<IServiceProxy>(Proxy, new ServiceProxy());

            container.Register<string>(Constants.NamedString, c =>
            {
                //var serviceProxy = c.ResolveNamed<IServiceProxy>(Proxy);
                //return serviceProxy.ContractNumber;
                return "InHealth001";
            }).ReusedWithin(Funq.ReuseScope.Request);

            container = MedicationContainer.Configure(container);

            return container;
        }
    }
}