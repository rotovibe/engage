using DataDomain.Template.Repo;
using DataDomain.Template.Repo.Containers;
using Phytel.API.DataDomain.Template.DTO;
using Phytel.Service.Proxy;

namespace Phytel.API.DataDomain.Template.Service.Containers
{
    public static class HttpServiceContainer
    {
        private static string Proxy = "TemplateProxy";

        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<IHostContextProxy>(Proxy, new HostContextProxy());

            container.Register<string>(Constants.NamedString, c =>
            {
                IHostContextProxy hostContextProxy = c.ResolveNamed<IHostContextProxy>(Proxy);
                return hostContextProxy.ContractNumber;
            }).ReusedWithin(Funq.ReuseScope.Request);

            container = TemplateContainer.Configure(container);
            return container;
        }
    }
}