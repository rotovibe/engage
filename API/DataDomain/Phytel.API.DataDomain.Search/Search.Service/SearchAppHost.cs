using System.Reflection;
using Phytel.API.DataDomain.Search.Service.Containers;
using Phytel.API.Interface;
using ServiceStack.Common;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.Search.Service
{
    public class SearchAppHost : AppHostBase
    {
        public SearchAppHost() : base("Phytel Search Data Domain Services",
            Assembly.GetExecutingAssembly() 
            )
        {
        }

        public override void Configure(Funq.Container container)
        {
            Plugins.Add(new RequestLogsFeature() {RequiredRoles = new string[] {}});

            HttpServiceContainer.Build(container);
            // request filtering for setting global vals.
            
            RequestFilters.Add((req, res, requestDto) =>
            {
                HostContext.Instance.Items.Add("Contract", ((IDataDomainRequest)requestDto).ContractNumber.ToLower());
            });

        }
    }
}