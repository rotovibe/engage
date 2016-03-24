using AutoMapper;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Model;

namespace Phytel.API.DataDomain.Cohort
{
    public class ReferralDefinitionResultMap : Profile
    {
        protected override void Configure()
        {
            CreateMap<MEReferral, ReferralData>()
                .ForMember(result => result.CreatedBy, opt => opt.MapFrom(source => source.RecordCreatedBy));

        }
    }
}
