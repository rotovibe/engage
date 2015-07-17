using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Internal;
using Phytel.API.AppDomain.NG.DTO.Internal.PatientSystem;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public static class PatientSystemMapper
    {
        public static void Build()
        {
            Mapper.CreateMap<PatientSystemData, UtilPatientSystem>()
                .ForMember(dest => dest.SystemID, opt => opt.MapFrom(src => src.SystemID));
            Mapper.CreateMap<PatientSystemData, PatientSystem>();
            Mapper.CreateMap<PatientSystem, PatientSystemData>();
            Mapper.CreateMap<SystemData, DTO.System>();
            Mapper.CreateMap<DTO.System, SystemData>();
            Mapper.CreateMap<UpdatePatientsAndSystemsRequest, GetActiveSystemsRequest>();
        }
    }
}