using AutoMapper;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Model;

namespace Phytel.API.DataDomain.Cohort.MongoDB.Map
{
    public class PatientReferralsListDefinitionResultMap : Profile
    {
        protected override void Configure()
        {
            CreateMap<MEPatientReferral, PatientReferralsListEntityData>()
                .ForMember(result => result.CreatedBy, opt => opt.MapFrom(source => source.RecordCreatedBy));
        }
    }
}