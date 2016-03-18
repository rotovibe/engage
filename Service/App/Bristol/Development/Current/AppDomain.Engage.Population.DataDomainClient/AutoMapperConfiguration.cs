using AppDomain.Engage.Population.DTO.Referrals;
using AutoMapper;
using Phytel.API.DataDomain.Cohort.DTO.Model;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<ReferralDefinitionData, ReferralData>()
                .ForMember(dest => dest.CohortId, opts => opts.MapFrom(src => src.ExternalId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.ReferralName));
        }
    }
}
