using AutoMapper;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.MiniProfiler;
using System;

namespace Phytel.API.DataDomain.Allergy.Service
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new AllergyAppHost().Init();

            Mapper.CreateMap<MEAllergy, DdAllergy>().ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(d => d.SubType, opt => opt.MapFrom(src => src.SubType.ConvertAll<string>(c => c.ToString()) ))
                .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.ToString()))
                .ForMember(d => d.RecordCreatedBy, opt => opt.MapFrom(src => src.RecordCreatedBy.ToString()));
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