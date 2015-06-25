using Phytel.API.Common;
using Phytel.API.Common.Format;
using ServiceStack.MiniProfiler;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;
using System;
using Phytel.API.Common.Audit;
using Phytel.API.DataAudit;
namespace Phytel.API.DataDomain.PatientNote.Service
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new PatientNoteAppHost().Init();

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