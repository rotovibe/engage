using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public class ContactTypeLookUpsMappers
    {
        public static void Build()
        {
            Mapper.CreateMap<ContactTypeLookUpData, ContactTypeLookUp>()
                .ForMember(dest => dest.Group, opt => opt.MapFrom(src => (int) src.Group))
                .ForMember(dest => dest.ParentId,
                    opt => opt.MapFrom(src => src.ParentId == "000000000000000000000000" ? null : src.ParentId));

        }
    }
}