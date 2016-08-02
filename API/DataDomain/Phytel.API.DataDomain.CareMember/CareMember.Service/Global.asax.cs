using Phytel.API.Common;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.MiniProfiler;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;
using System;

namespace Phytel.API.DataDomain.CareMember.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class CareMemberAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public CareMemberAppHost() : base("Phytel CareMember Data Domain Services", typeof(CareMemberService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
                Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });

                container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>();
                container.RegisterAutoWiredAs<CareMemberRepositoryFactory, ICareMemberRepositoryFactory>();
                container.RegisterAutoWiredAs<Helpers, IHelpers>();
                container.RegisterAutoWiredAs<CareMemberDataManager, ICareMemberDataManager>();
                container.RegisterAutoWiredAs<AuditHelpers, IAuditHelpers>();
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new CareMemberAppHost().Init();

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