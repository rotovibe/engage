using AppDomain.Engage.Population.DTO.Referrals;
using AutoMapper;
using Phytel.API.DataDomain.Patient.DTO;
using AppDomain.Engage.Population.DTO.Demographics;


namespace AppDomain.Engage.Population.DataDomainClient
{
    public class ReferralWithPatientsListRequestMap : Profile
    {
        protected override void Configure()
        {
            CreateMap<Patient, PatientData>();
           //     .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
                

        }
    }
}
