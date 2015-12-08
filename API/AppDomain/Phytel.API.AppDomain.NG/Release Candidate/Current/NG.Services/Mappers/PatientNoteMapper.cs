using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Utilization;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public static class PatientNoteMapper
    {
        public static void Build()
        {

            //Notes in general.
            Mapper.CreateMap<PatientNoteData, PatientNote>();
            Mapper.CreateMap<PatientNote, PatientNoteData>();
            
            Mapper.CreateMap<PatientUtilizationData, PatientUtilization>()
                .ForMember(dest => dest.UtilizationSourceId, opt => opt.MapFrom(src => src.SourceId))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Reason));
            Mapper.CreateMap<PatientUtilization, PatientUtilizationData>()
                .ForMember(dest => dest.SourceId, opt => opt.MapFrom(src => src.UtilizationSourceId))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Text));
            // map to patientnote collection
            Mapper.CreateMap<PatientUtilization, PatientNote>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.TypeId))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(dest => dest.CreatedById, opt => opt.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
                .ForMember(dest => dest.ProgramIds, opt => opt.Ignore())
                .ForMember(dest => dest.MethodId, opt => opt.Ignore())
                .ForMember(dest => dest.OutcomeId, opt => opt.Ignore())
                .ForMember(dest => dest.WhoId, opt => opt.Ignore())
                .ForMember(dest => dest.SourceId, opt => opt.Ignore())
                .ForMember(dest => dest.Duration, opt => opt.Ignore())
                .ForMember(dest => dest.ContactedOn, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatedIdentity, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedById, opt => opt.Ignore());
        }
    }
}