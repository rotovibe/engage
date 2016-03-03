using Phytel.API.Common;
using Phytel.API.Common.Audit;
using Phytel.API.Common.Format;
using Phytel.API.DataAudit;
using ServiceStack.MiniProfiler;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;
using System;

namespace Phytel.API.DataDomain.Contact.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class ContactAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public ContactAppHost() : base("Phytel Contact Data Domain Services", typeof(ContactService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
                Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });

                container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>();
                container.RegisterAutoWiredAs<ContactRepositoryFactory, IContactRepositoryFactory>();
                container.RegisterAutoWiredAs<Helpers, IHelpers>();
                container.RegisterAutoWiredAs<ContactDataManager, IContactDataManager>();
                container.RegisterAutoWiredAs<AuditHelpers, IAuditHelpers>();
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new ContactAppHost().Init();

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