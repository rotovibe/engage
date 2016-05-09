using ServiceStack.WebHost.Endpoints;
using System;
using System.Reflection;
using AutoMapper;
using Phytel.API.Interface;
using ServiceStack.Api.Swagger;
using ServiceStack.Common;
using Phytel.API.DataDomain.Cohort.MongoDB.Map;

namespace Phytel.API.DataDomain.Cohort.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class CohortAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public CohortAppHost() : base("Cohort Data Domain Services", Assembly.GetExecutingAssembly())
            {
            }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());

                HttpServiceContainer.Build(container);

                Mapper.Initialize(mapper =>
                {
                    mapper.AddProfile<ObjectIdMap>();
                    mapper.AddProfile<ReferralDefinitionRequestMap>();
                    mapper.AddProfile<ReferralDefinitionResultMap>();
                    mapper.AddProfile<PatientReferralDefinitionRequestMap>();
                    mapper.AddProfile<PatientReferralDefinitionResultMap>();
                    mapper.AddProfile<PatientReferralsListDefinitionRequestMap>();
                    mapper.AddProfile<PatientReferralsListDefinitionResultMap>();
                });

                // request filtering for setting global vals.
                RequestFilters.Add((req, res, requestDto) =>
                {
                    if (req.OperationName != "Resources" && req.OperationName != "ResourceRequest" )
                    { 
                        HostContext.Instance.Items.Add("Contract", ((IDataDomainRequest) requestDto).ContractNumber);
                        HostContext.Instance.Items.Add("Version", ((IDataDomainRequest) requestDto).Version);
                        HostContext.Instance.Items.Add("Context", ((IDataDomainRequest)requestDto).Context);
                        HostContext.Instance.Items.Add("UserId", ((IDataDomainRequest)requestDto).UserId);
                    }
                });
                Plugins.Add(new SwaggerFeature());
            }
        }


        protected void Application_Start(object sender, EventArgs e)
        {
            new CohortAppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

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