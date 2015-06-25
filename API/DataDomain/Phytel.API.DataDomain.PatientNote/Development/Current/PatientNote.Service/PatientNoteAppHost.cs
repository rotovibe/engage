using System.Reflection;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.PatientNote.Service
{
    public class PatientNoteAppHost : AppHostBase
    {
        public PatientNoteAppHost() : base("Phytel PatientNote Data Domain Services",
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