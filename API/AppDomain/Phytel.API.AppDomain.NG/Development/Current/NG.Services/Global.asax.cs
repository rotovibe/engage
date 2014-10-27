using AutoMapper;
using Lucene.Net.Documents;
using Phytel.API.AppDomain.NG.Allergy;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.MiniProfiler;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.ServiceInterface.Cors;
using ServiceStack.WebHost.Endpoints;
using System;
using ServiceStack;
using ServiceStack.WebHost.Endpoints.Support;
using ServiceStack.ServiceHost;
using System.Web;
using ServiceStack.WebHost.Endpoints.Extensions;
using ServiceStack.Common.Web;
using ServiceStack.Text;
using Phytel.API.DataAudit;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.AppDomain.NG.Programs;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Phytel.API.AppDomain.NG.Observation;

namespace Phytel.API.AppDomain.NG.Service
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly bool Profile = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings.Get("Profiler"));

        public class NGAppHost : AppHostBase
        {

            //Tell Service Stack the name of your application and where to find your web services
            public NGAppHost()
                : base("Phytel NG Services", typeof(NGService).Assembly)
            {
            }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                container.RegisterAutoWiredAs<SecurityManager, ISecurityManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<EndpointUtils, IEndpointUtils>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<AllergyEndpointUtil, IAllergyEndpointUtil>().ReusedWithin(Funq.ReuseScope.Request);                
                container.RegisterAutoWiredAs<PlanElementUtils, IPlanElementUtils>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<NGManager, INGManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<AuditUtil, IAuditUtil>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<ObservationEndpointUtil, IObservationEndpointUtil>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<ObservationsManager, IObservationsManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<AllergyManager, IAllergyManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<SearchManager, ISearchManager>().ReusedWithin(Funq.ReuseScope.Request);

                // automapper configuration
                Mapper.CreateMap<DdAllergy, DTO.Allergy>();
                
                Mapper.CreateMap<Document, IdNamePair>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Get("Id")))
                    .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Get("Name")));

                Mapper.CreateMap<DTO.Allergy, IdNamePair>().ForMember(d => d.Name, opt => opt.MapFrom(src => src.Description));

                Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });

                var emitGlobalHeadersHandler = new CustomActionHandler((httpReq, httpRes) => httpRes.EndRequest());
                SetConfig(new EndpointHostConfig
                {
                    RawHttpHandlers = { (httpReq) => httpReq.HttpMethod == HttpMethods.Options ? emitGlobalHeadersHandler : null },
                    GlobalResponseHeaders = { 
                    //{"Access-Control-Allow-Origin", "*"},
                    { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" }, 
                    { "Access-Control-Allow-Headers", "Content-Type" }, },
                    AllowJsonpRequests = true
                });

                // initialize datetime format
                JsConfig.DateHandler = JsonDateHandler.ISO8601;
            }
        }

        public class CustomActionHandler : IServiceStackHttpHandler, IHttpHandler
        {
            public Action<IHttpRequest, IHttpResponse> Action { get; set; }

            public CustomActionHandler(Action<IHttpRequest, IHttpResponse> action)
            {
                if (action == null)
                    throw new Exception("Action was not supplied to ActionHandler");

                Action = action;
            }

            public void ProcessRequest(IHttpRequest httpReq, IHttpResponse httpRes, string operationName)
            {
                Action(httpReq, httpRes);
            }

            public void ProcessRequest(HttpContext context)
            {
                ProcessRequest(context.Request.ToRequest(GetType().Name),
                    context.Response.ToResponse(),
                    GetType().Name);
            }

            public bool IsReusable
            {
                get { return false; }
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new NGAppHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Profile)
                Profiler.Start();
        }

        protected void Application_EndRequest(object src, EventArgs e)
        {
            if (Profile)
                Profiler.Stop();
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