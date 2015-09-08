using System.Reflection;
using Phytel.API.Interface;
using ServiceStack.Common;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.PatientSystem.Service
{
    public class PatientSystemAppHost : AppHostBase
    {
        public PatientSystemAppHost(): base("Phytel PatientSystem Data Domain Services",
            Assembly.GetExecutingAssembly() )
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