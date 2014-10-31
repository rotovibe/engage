using AutoMapper;
using MongoDB.Bson;
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

            Mapper.CreateMap<MEAllergy, AllergyData>().ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(d => d.Type, opt => opt.MapFrom(src => src.Type.ConvertAll<string>(c => c.ToString()) ))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn));

            Mapper.CreateMap<MEPatientAllergy, PatientAllergyData>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn));
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