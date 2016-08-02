using System.Reflection;
using Phytel.API.Interface;
using ServiceStack.Common;
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

            // get path variables and make them accessible through the context.
            RequestFilters.Add((req, res, requestDto) =>
            {
                HostContext.Instance.Items.Add("Contract", ((IDataDomainRequest)requestDto).ContractNumber.ToLower());
                HostContext.Instance.Items.Add("UserId", ((IDataDomainRequest)requestDto).UserId);
            });

            HttpServiceContainer.Build(container);
        }
    }
}