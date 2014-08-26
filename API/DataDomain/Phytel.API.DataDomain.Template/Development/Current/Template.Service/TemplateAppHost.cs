using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Template.Service.Containers;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.Template.Service
{
    public class TemplateAppHost : AppHostBase
    {
        public TemplateAppHost() : base("Phytel Template Data Domain Services", typeof(TemplateService).Assembly) { }

        public override void Configure(Funq.Container container)
        {
            Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });
            HttpServiceContainer.Build(container: container);
        }
    }
}