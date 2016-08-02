using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public class ContactMapper
    {
        public static void Build()
        {
            Mapper.CreateMap<ContactSubTypeData, ContactSubType>().ReverseMap();
            Mapper.CreateMap<LanguageData, Language>().ReverseMap();
            Mapper.CreateMap<AddressData, Address>().ReverseMap();
            Mapper.CreateMap<EmailData, Email>().ReverseMap();
            Mapper.CreateMap<PhoneData, Phone>().ReverseMap();

            Mapper.CreateMap<CommModeData, CommMode>()
                .ForMember(dest => dest.LookUpModeId, opt => opt.MapFrom(src => src.ModeId));

            Mapper.CreateMap<CommMode, CommModeData>()
               .ForMember(dest => dest.ModeId, opt => opt.MapFrom(src => src.LookUpModeId));



            Mapper.CreateMap<ContactData, Contact>()
               .ForMember(dest => dest.ContactSubTypes, opt => opt.MapFrom(src => src.ContactSubTypesData))
                .ForMember(dest => dest.IsPatient, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.PatientId)))
                .ForMember(dest => dest.IsUser, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.UserId)));

            Mapper.CreateMap<Contact, ContactData>()
                .ForMember(dest => dest.ContactSubTypesData, opt => opt.MapFrom(src => src.ContactSubTypes))
                .ForMember(dest => dest.RecentsList, opt => opt.Ignore());


            Mapper.CreateMap<ContactData, ContactSearchInfo>();

            Mapper.CreateMap<ContactData, SyncPatientInfoData>();
            Mapper.CreateMap<PatientData, SyncContactInfoData>();

            Mapper.CreateMap<Member, CareTeamMemberData>().ReverseMap();
            Mapper.CreateMap<CareTeam, CareTeamData>().ReverseMap();
        }
    }
}