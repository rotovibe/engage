using DataDomain.Template.Repo.Containers;
using Phytel.API.DataDomain.Template.DTO;

namespace Phytel.API.DataDomain.Template.Service.Containers
{
    public static class HttpServiceContainer
    {
        private static string Proxy = "TemplateProxy";

        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<IServiceProxy>(Proxy, new ServiceProxy());

            container.Register<string>(Constants.NamedString, c =>
            {
                var serviceProxy = c.ResolveNamed<IServiceProxy>(Proxy);
                return serviceProxy.ContractNumber;
            }).ReusedWithin(Funq.ReuseScope.Request);

            container = TemplateContainer.Configure(container);

            return container;
        }
    }
}