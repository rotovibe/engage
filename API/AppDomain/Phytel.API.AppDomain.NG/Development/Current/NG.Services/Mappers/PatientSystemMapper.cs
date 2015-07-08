using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public static class PatientSystemMapper
    {
        public static void Build()
        {
            Mapper.CreateMap<PatientSystemData, PatientSystem>();
            Mapper.CreateMap<PatientSystem, PatientSystemData>();
            Mapper.CreateMap<SystemSourceData, SystemSource>();
            Mapper.CreateMap<SystemSource, SystemSourceData>();
        }
    }
}