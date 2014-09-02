using DataDomain.Template.Repo.Containers;
using Phytel.API.DataDomain.Template.DTO;
using Phytel.Service.Proxy;

namespace Phytel.API.DataDomain.Template.Service.Containers
{
    public static class HttpServiceContainer
    {
        public static Funq.Container Build(Funq.Container container)
        {
            container.Register<IHostContextProxy>(new HostContextProxy());
            container.Register<string>(Constants.NamedString, c =>
            {
                var hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.ContractNumber;
            }).ReusedWithin(Funq.ReuseScope.Request);

            container = TemplateContainer.Configure(container);
            return container;
        }
    }
}