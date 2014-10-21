using DataDomain.Allergy.Repo.Containers;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy.Service.Containers
{
    public static class HttpServiceContainer
    {
        private static string Proxy = "AllergyProxy";

        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<IServiceProxy>(Proxy, new ServiceProxy());

            container.Register<string>(Constants.NamedString, c =>
            {
                var serviceProxy = c.ResolveNamed<IServiceProxy>(Proxy);
                return serviceProxy.ContractNumber;
            }).ReusedWithin(Funq.ReuseScope.Request);

            container = AllergyContainer.Configure(container);

            return container;
        }
    }
}