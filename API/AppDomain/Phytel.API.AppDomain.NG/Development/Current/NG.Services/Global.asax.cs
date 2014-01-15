using ServiceStack.MiniProfiler;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;
using System;

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
                //container.Register<ICacheClient>(new MemoryCacheClient());
                Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });
                SetConfig(new EndpointHostConfig { AllowJsonpRequests = true });

                //Permit modern browsers to allow sending of any REST HTTP Method
                //SetConfig(new EndpointHostConfig
                //{
                //    GlobalResponseHeaders = { 
                //    //{"Access-Control-Allow-Origin", "*"},
                //    //{ "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" }, 
                //    //{ "Access-Control-Allow-Headers", "Content-Type" }, },
                //});
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
            if (Request.IsLocal)
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