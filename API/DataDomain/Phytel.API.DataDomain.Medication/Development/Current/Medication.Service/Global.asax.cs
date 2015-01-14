using AutoMapper;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Medication.DTO;
using ServiceStack.MiniProfiler;
using ServiceStack.ServiceInterface.Admin;
using ServiceStack.WebHost.Endpoints;
using System;

namespace Phytel.API.DataDomain.Medication.Service
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new MedicationAppHost().Init();

            Mapper.CreateMap<MEMedication, DTO.MedicationData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => src.RecordCreatedBy.ToString()))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.ToString()));

            Mapper.CreateMap<DTO.MedicationData, MEMedication>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => ObjectId.Parse(src.Id)))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => ObjectId.Parse(src.RecordCreatedBy)))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => ObjectId.Parse(src.UpdatedBy)));


            Mapper.CreateMap<MEMedicationMapping, DTO.MedicationMappingData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => src.RecordCreatedBy.ToString()))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.ToString()));

            Mapper.CreateMap<DTO.MedicationMappingData, MEMedicationMapping>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => ObjectId.Parse(src.Id)))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => ObjectId.Parse(src.RecordCreatedBy)))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => ObjectId.Parse(src.UpdatedBy)));

            Mapper.CreateMap<MEPatientMedSupp, PatientMedSuppData>()
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