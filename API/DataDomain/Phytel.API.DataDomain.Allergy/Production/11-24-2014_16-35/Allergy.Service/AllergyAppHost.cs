using System.Reflection;
using Phytel.API.DataDomain.Allergy.Service.Containers;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.Allergy.Service
{
    public class AllergyAppHost : AppHostBase
    {
        public AllergyAppHost() : base("Phytel Allergy Data Domain Services",
            Assembly.GetExecutingAssembly() 
            )
        {
        }

        public override void Configure(Funq.Container container)
        {
            Plugins.Add(new RequestLogsFeature() {RequiredRoles = new string[] {}});

            HttpServiceContainer.Build(container);
        }
    }
}