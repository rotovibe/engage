using AutoMapper;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Cohort.DTO.Model;

namespace Phytel.API.DataDomain.Cohort
{
    public class ReferralDefinitionRequestMap : Profile
    {
        protected override void Configure()
        {
            CreateMap<ReferralData, MEReferral>();
        }
    }
}
