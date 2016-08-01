using ServiceStack.WebHost.Endpoints;
using System;
using Phytel.API.DataDomain.LookUp;

namespace Phytel.API.DataDomain.Patient.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class LookUpAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public LookUpAppHost() : base("Phytel LookUp Data Domain Services", typeof(LookUpService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                container.RegisterAutoWiredAs<LookUpDataManager, ILookUpDataManager>();
                container.RegisterAutoWiredAs<LookUpRepositoryFactory, ILookUpRepositoryFactory>();
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new LookUpAppHost().Init();

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