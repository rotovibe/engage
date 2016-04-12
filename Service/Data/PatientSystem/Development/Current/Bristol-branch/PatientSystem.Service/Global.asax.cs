using ServiceStack.MiniProfiler;
using System;
using System.Reflection;
using Phytel.API.Interface;
using ServiceStack.Api.Swagger;
using ServiceStack.Common;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.PatientSystem.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class PatientSystemAppHost : AppHostBase
        {
            public PatientSystemAppHost() : base("Phytel PatientSystem Data Domain Services",
                Assembly.GetExecutingAssembly())
            {
            }

            public override void Configure(Funq.Container container)
            {
                Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });
                Plugins.Add(new SwaggerFeature());

                // get path variables and make them accessible through the context.
                RequestFilters.Add((req, res, requestDto) =>
                {
                //{ServiceStack.Api.Swagger.Resources}
                    if (req.OperationName != "Resources" && req.OperationName != "ResourceRequest")
                    {
                        if (requestDto.GetType() == typeof (Resources) ||
                            requestDto.GetType() == typeof (ResourceRequest)) return;
                        HostContext.Instance.Items.Add("Contract",
                            ((IDataDomainRequest) requestDto).ContractNumber.ToLower());
                        HostContext.Instance.Items.Add("UserId", ((IDataDomainRequest) requestDto).UserId);
                    }
                });

                HttpServiceContainer.Build(container);
                Plugins.Add(new SwaggerFeature());
            }
        }
        protected void Application_Start(object sender, EventArgs e)
        {
            new PatientSystemAppHost().Init();

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.IsLocal)
                Profiler.Start();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}