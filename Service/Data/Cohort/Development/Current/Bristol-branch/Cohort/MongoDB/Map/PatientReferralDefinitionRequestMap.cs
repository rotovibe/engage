using AutoMapper;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Model;

namespace Phytel.API.DataDomain.Cohort
{
    public class PatientReferralDefinitionRequestMap : Profile
    {
        protected override void Configure()
        {
            CreateMap<PatientReferralData, MEPatientReferral>();
        }
    }
}
