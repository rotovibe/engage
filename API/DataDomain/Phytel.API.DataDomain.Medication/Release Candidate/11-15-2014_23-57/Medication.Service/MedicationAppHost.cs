using System.Reflection;
using Phytel.API.DataDomain.Medication.Service.Containers;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.Medication.Service
{
    public class MedicationAppHost : AppHostBase
    {
        public MedicationAppHost() : base("Phytel Medication Data Domain Services",
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