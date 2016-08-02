using DataDomain.Search.Repo.Containers;
using Phytel.API.DataDomain.Search.DTO;

namespace Phytel.API.DataDomain.Search.Service.Containers
{
    public static class HttpServiceContainer
    {
        private static string Proxy = "SearchProxy";

        public static Funq.Container Build(Funq.Container container)
        {
            //container.Register<IServiceProxy>(Proxy, new ServiceProxy());

            //container.Register<string>(Constants.NamedString, c =>
            //{
            //    var serviceProxy = c.ResolveNamed<IServiceProxy>(Proxy);
            //    return serviceProxy.ContractNumber;
            //}).ReusedWithin(Funq.ReuseScope.Request);

            container = SearchContainer.Configure(container);

            return container;
        }
    }
}