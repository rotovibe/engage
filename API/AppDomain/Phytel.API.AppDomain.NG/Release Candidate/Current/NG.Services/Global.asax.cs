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

namespace Phytel.API.AppDomain.NG.Service
{
    public class Global : System.Web.HttpApplication
    {
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
                container.RegisterAutoWiredAs<PlanElementUtils, IPlanElementUtils>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<NGManager, INGManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<AuditUtil, IAuditUtil>().ReusedWithin(Funq.ReuseScope.Request);


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
            //if (Request.IsLocal)
                Profiler.Start();
        }

        protected void Application_EndRequest(object src, EventArgs e)
        {
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