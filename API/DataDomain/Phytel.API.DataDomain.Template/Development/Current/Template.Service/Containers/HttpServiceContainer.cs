using Phytel.API.Common;
using Phytel.API.Common.Format;
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
                IHostContextProxy hostContextProxy = c.Resolve<IHostContextProxy>();
                return hostContextProxy.ContractNumber;
            }).ReusedWithin(Funq.ReuseScope.Request);

            container.RegisterAutoWiredAs<TemplateDataManager, ITemplateDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<Helpers, IHelpers>().ReusedWithin(Funq.ReuseScope.Request);
            
            return container;
        }
    }
}