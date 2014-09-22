using ServiceStack.MiniProfiler;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;
using System;

namespace Phytel.API.DataDomain.PatientObservation.Service
{
    public class Global : System.Web.HttpApplication
    {
        public class PatientObservationAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public PatientObservationAppHost() : base("Phytel PatientObservation Data Domain Services", typeof(PatientObservationService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
                container.RegisterAutoWiredAs<PatientObservationDataManager, IPatientObservationDataManager>();
                container.RegisterAutoWiredAs<PatientObservationRepositoryFactory, IPatientObservationRepositoryFactory>();
                container.RegisterAutoWiredAs<ObservationUtil, IObservationUtil>();

                Plugins.Add(new RequestLogsFeature() { RequiredRoles = new string[] { } });
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new PatientObservationAppHost().Init();

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