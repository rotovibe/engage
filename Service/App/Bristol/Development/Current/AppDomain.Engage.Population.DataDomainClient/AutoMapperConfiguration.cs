using AppDomain.Engage.Population.DTO.Referrals;
using AutoMapper;
using Phytel.API.DataDomain.Cohort.DTO.Model;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(mapper =>
            {
               mapper.AddProfile<ReferralDefinitionRequestMap>();
               mapper.AddProfile<ReferralDefinitionResultMap>();
            });
        }
    }
}
