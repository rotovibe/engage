using DataDomain.Allergy.Repo.Containers;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.Common;

namespace Phytel.API.DataDomain.Allergy.Service.Containers
{
    public static class HttpServiceContainer
    {
        private static string Proxy = "AllergyProxy";

        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<HostContext>(c => HostContext.Instance).ReusedWithin(Funq.ReuseScope.Request);
            //container.Register<IServiceProxy>(Proxy, new ServiceProxy());

            //container.Register<string>(Constants.NamedString, c =>
            //{
            //    var serviceProxy = c.ResolveNamed<IServiceProxy>(Proxy);
            //    return serviceProxy.ContractNumber;
            //    //return "";
            //}).ReusedWithin(Funq.ReuseScope.Request);

            container = AllergyContainer.Configure(container);

            return container;
        }
    }
}