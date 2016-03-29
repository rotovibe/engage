using System;
using AutoMapper;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.DataDomain.Contact
{
    public static class ContactContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            Mapper.CreateMap<ContactTypeData, MEContactType>().ReverseMap();
            return container;
        }
    }
}
