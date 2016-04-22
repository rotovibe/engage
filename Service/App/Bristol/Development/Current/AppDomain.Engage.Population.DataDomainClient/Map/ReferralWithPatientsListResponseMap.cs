using AppDomain.Engage.Population.DTO.Referrals;
using AutoMapper;
using Phytel.API.DataDomain.Patient.DTO;
using AppDomain.Engage.Population.DTO.Demographics;
using Phytel.API.Common;


namespace AppDomain.Engage.Population.DataDomainClient
{
    public class ReferralWithPatientsListResponseMap : Profile
    {
        protected override void Configure()
        {
            CreateMap<AppData, ProcessedData>();
                
                

        }
    }
}