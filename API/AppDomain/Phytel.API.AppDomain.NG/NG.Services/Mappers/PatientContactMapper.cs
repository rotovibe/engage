using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Internal;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public static class PatientContactMapper
    {
        public static void Build()
        {
            Mapper.CreateMap<Phone, PhoneData>();
        }
    }
}