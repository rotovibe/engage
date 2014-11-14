using AutoMapper;
using Phytel.API.DataDomain.PatientGoal.DTO;
using ServiceStack.MiniProfiler;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;
using System;

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
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            new PatientGoalAppHost().Init();

            Mapper.CreateMap<MEGoal, GoalData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(d => d.SourceId, opt => opt.MapFrom(src => src.SourceId.ToString()))
                .ForMember(d => d.TypeId, opt => opt.MapFrom(src => Convert.ChangeType(src.Type, src.Status.GetTypeCode())))
                .ForMember(d => d.StatusId, opt => opt.MapFrom(src => Convert.ChangeType(src.Status, src.Status.GetTypeCode())))
                .ForMember(d => d.CustomAttributes,
                    opt => opt.MapFrom(src => src.Attributes.ConvertAll<CustomAttributeData>(
                        c => new CustomAttributeData {Id = c.AttributeId.ToString(), Values = c.Values})));
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