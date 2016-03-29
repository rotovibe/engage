using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public class ContactMapper
    {
        public static void Build()
        {
            Mapper.CreateMap<ContactTypeData, ContactType>().ReverseMap();
            Mapper.CreateMap<LanguageData, Language>().ReverseMap();
            Mapper.CreateMap<AddressData, Address>().ReverseMap();
            Mapper.CreateMap<EmailData, Email>().ReverseMap();
            Mapper.CreateMap<PhoneData, Phone>().ReverseMap();

            Mapper.CreateMap<CommModeData, CommMode>()
                .ForMember(dest => dest.LookUpModeId, opt => opt.MapFrom(src => src.ModeId));

            Mapper.CreateMap<CommMode, CommModeData>()
               .ForMember(dest => dest.ModeId, opt => opt.MapFrom(src => src.LookUpModeId));



            Mapper.CreateMap<ContactData, Contact>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeData))
                .ForMember(dest => dest.IsPatient, opt => opt.Ignore())
                .ForMember(dest => dest.IsUser, opt => opt.Ignore());

            Mapper.CreateMap<Contact, ContactData>()
                .ForMember(dest => dest.TypeData, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.RecentsList, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedOn, opt => opt.Ignore());

        }
    }
}