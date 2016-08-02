using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public static class AllergyMapper
    {
        public static void Build()
        {
            Mapper.CreateMap<AllergyData, DTO.Allergy>();
            Mapper.CreateMap<DTO.Allergy, AllergyData>();
            Mapper.CreateMap<PatientAllergyData, PatientAllergy>();
            Mapper.CreateMap<PatientAllergy, PatientAllergyData>();
        }
    }
}