using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.PatientSystem.MongoDB.DataManagement;
using ServiceStack.WebHost.Endpoints;
using System;

namespace Phytel.API.DataDomain.PatientSystem.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class PatientSystemAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public PatientSystemAppHost() : base("Phytel PatientSystem Data Domain Services", typeof(PatientSystemService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
                container.RegisterAutoWiredAs<PatientSystemDataManager, IPatientSystemDataManager>();
                container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>();
                container.RegisterAutoWiredAs<Helpers, IHelpers>();
                container.RegisterAutoWiredAs<ProceduresManager, IProceduresManager>();
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