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
            Mapper.CreateMap<ContactData, Contact>().ReverseMap();            
            Mapper.CreateMap<LanguageData, Language>();
            Mapper.CreateMap<AddressData, Address>();
            Mapper.CreateMap<EmailData, Email>();
            Mapper.CreateMap<PhoneData, Phone>();
            Mapper.CreateMap<CommModeData, CommMode>();
        }
    }
}