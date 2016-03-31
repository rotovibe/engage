using AppDomain.Engage.Population.DTO.Referrals;
using AutoMapper;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Model;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public class ReferralDefinitionRequestMap : Profile
    {
        protected override void Configure()
        {
            CreateMap<ReferralData, ReferralDefinitionData>()
                .ForMember(dest => dest.ExternalId, opts => opts.MapFrom(src => src.CohortId))
                .ForMember(dest => dest.ReferralName, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.ReferralReason, opts => opts.MapFrom(src => src.Reason));
        }
    }
}
