using System;
using System.Web;
using System.Web.Configuration;
using Funq;
using Phytel.API.AppDomain.NG.Service.Containers;
using Phytel.API.AppDomain.NG.Service.Mappers;
using Phytel.API.Interface;
using ServiceStack;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.MiniProfiler;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.WebHost.Endpoints.Extensions;
using ServiceStack.WebHost.Endpoints.Support;

namespace Phytel.API.AppDomain.NG.Service
{
    public class Global : HttpApplication
    {
        private static readonly bool Profile = Convert.ToBoolean(WebConfigurationManager.AppSettings.Get("Profiler"));
        private NGAppHost _ngAppHost;
        public class NGAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public NGAppHost() : base("Phytel NG Services", typeof (NGService).Assembly)
            {
            }

            public override void Configure(Container container)
            {
                //containers
                HttpServiceContainer.Build(container);
                SearchContainer.Build(container);

                // mappings
                AllergyMapper.Build();
                GoalsMapper.Build();
                AllergyMedSearchMapper.Build();
                MedSuppMapper.Build();
                PatientNoteMapper.Build();
                PatientSystemMapper.Build();
                PatientContactMapper.Build();
                ContactTypeLookUpsMappers.Build();
                ContactMapper.Build();
                Plugins.Add(new RequestLogsFeature() {RequiredRoles = new string[] {}});

                // request filtering for setting global vals.
                RequestFilters.Add((req, res, requestDto) =>
                {
                    HostContext.Instance.Items.Add("Contract", ((IAppDomainRequest) requestDto).ContractNumber);
                    HostContext.Instance.Items.Add("Version", ((IAppDomainRequest)requestDto).Version);
                });

                RequestFilters.Add((req, res, requestDto) =>
                {
                    var obj = req.ResponseContentType;
                });

                var emitGlobalHeadersHandler = new CustomActionHandler((httpReq, httpRes) => httpRes.EndRequest());
                SetConfig(new EndpointHostConfig
                {
                    RawHttpHandlers =
                    {
                        (httpReq) => httpReq.HttpMethod == HttpMethods.Options ? emitGlobalHeadersHandler : null
                    },
                    GlobalResponseHeaders =
                    {
                        //{"Access-Control-Allow-Origin", "*"},
                        {"Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS"},
                        {"Access-Control-Allow-Headers", "Content-Type"},
                    },
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
            _ngAppHost = new NGAppHost();
            _ngAppHost.Init();
            var cohortRulesProcessor = _ngAppHost.TryResolve<ICohortRulesProcessor>();
            if (cohortRulesProcessor != null)
                cohortRulesProcessor.Start();
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
            var cohortRulesProcessor = _ngAppHost.TryResolve<ICohortRulesProcessor>();
            if (cohortRulesProcessor != null)
                cohortRulesProcessor.Stop();
        }
    }
}
