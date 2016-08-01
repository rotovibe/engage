using AutoMapper;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.MiniProfiler;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;
using System;
using Phytel.API.DataDomain.PatientGoal.Container;

namespace Phytel.API.DataDomain.PatientGoal.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class PatientGoalAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public PatientGoalAppHost() : base("Phytel PatientGoal Data Domain Services", typeof(PatientGoalService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
                Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });
                container.RegisterAutoWiredAs<AttributeRepositoryFactory, IAttributeRepositoryFactory>();
                container.RegisterAutoWiredAs<PatientGoalRepositoryFactory, IPatientGoalRepositoryFactory>();
                container.RegisterAutoWiredAs<PatientGoalDataManager, IPatientGoalDataManager>();

                PatientGoalContainer.Configure(container);
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new PatientGoalAppHost().Init();
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