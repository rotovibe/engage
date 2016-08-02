using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using ServiceStack.MiniProfiler;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using System;

namespace Phytel.API.DataDomain.Program.Service
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly bool Profile = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings.Get("Profiler"));

        public class ProgramAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public ProgramAppHost() : base("Phytel Program Data Domain Services", typeof(ProgramService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
                container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<Helpers, IHelpers>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<ProgramDataManager, IProgramDataManager>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<ProgramRepositoryFactory, IProgramRepositoryFactory>().ReusedWithin(Funq.ReuseScope.Request);
                container.RegisterAutoWiredAs<DTOUtility, IDTOUtility>().ReusedWithin(Funq.ReuseScope.Request);

                Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });

                // initialize datetime format
                JsConfig.DateHandler = JsonDateHandler.ISO8601;
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new ProgramAppHost().Init();

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