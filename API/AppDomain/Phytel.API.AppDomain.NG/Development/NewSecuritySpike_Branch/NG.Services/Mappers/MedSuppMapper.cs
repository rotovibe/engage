using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public static class MedSuppMapper
    {
        public static void Build()
        {
            Mapper.CreateMap<MedicationMapData, MedicationMap>();
            Mapper.CreateMap<MedicationMapData, MedicationMap>();
            Mapper.CreateMap<MedicationMap, MedicationMapData>();
            Mapper.CreateMap<PatientMedSuppData, PatientMedSupp>();
            Mapper.CreateMap<PatientMedSupp, PatientMedSuppData>();
        }
    }
}