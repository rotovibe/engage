using AutoMapper;
using Lucene.Net.Documents;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.Service.Mappers
{
    public static class AllergyMedSearchMapper
    {
        public static void Build()
        {
            Mapper.CreateMap<DTO.Allergy, IdNamePair>().ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name.Trim().ToUpper().Replace("\"", "").Replace(",", "")));
            Mapper.CreateMap<Document, IdNamePair>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Get("Id")))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Get("Name")));

            Mapper.CreateMap<Document, TextValuePair>()
                                .ForMember(d => d.Value, opt => opt.MapFrom(src => src.Get("CompositeName").Trim()))
                                .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Get("CompositeName").Trim()));

            Mapper.CreateMap<Document, MedNameSearchDoc>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Get("MongoId")))
                .ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.Get("PackageId")))
                .ForMember(d => d.DosageFormname, opt => opt.MapFrom(src => src.Get("DosageFormName")))
                .ForMember(d => d.CompositeName, opt => opt.MapFrom(src => src.Get("CompositeName")))
                .ForMember(d => d.ProprietaryName, opt => opt.MapFrom(src => src.Get("ProprietaryName")))
                .ForMember(d => d.RouteName, opt => opt.MapFrom(src => src.Get("RouteName")))
                .ForMember(d => d.SubstanceName, opt => opt.MapFrom(src => src.Get("SubstanceName")))
                .ForMember(d => d.Strength, opt => opt.MapFrom(src => src.Get("Strength")))
                .ForMember(d => d.Unit, opt => opt.MapFrom(src => src.Get("Unit")));

            //Mapper.CreateMap<Document, MedFieldsSearchDoc>()
            //    .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Get("MongoId")))
            //    .ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.Get("PackageId")))
            //    .ForMember(d => d.DosageFormname, opt => opt.MapFrom(src => src.Get("DosageFormname")))
            //    .ForMember(d => d.CompositeName, opt => opt.MapFrom(src => src.Get("CompositeName")))
            //    .ForMember(d => d.ProprietaryName, opt => opt.MapFrom(src => src.Get("ProprietaryName")))
            //    .ForMember(d => d.RouteName, opt => opt.MapFrom(src => src.Get("RouteName")))
            //    .ForMember(d => d.SubstanceName, opt => opt.MapFrom(src => src.Get("SubstanceName")))
            //    .ForMember(d => d.Strength, opt => opt.MapFrom(src => src.Get("Strength")))
            //    .ForMember(d => d.Unit, opt => opt.MapFrom(src => src.Get("Unit")));

            //Mapper.CreateMap<DTO.Medication, MedFieldsSearchDoc>()
            //    .ForMember(d => d.CompositeName, opt => opt.MapFrom(src => src.ProprietaryName + " " + src.ProprietaryNameSuffix))
            //.ForMember(d => d.DosageFormname, opt => opt.MapFrom(src => src.DosageFormName))
            //.ForMember(d => d.RouteName, opt => opt.MapFrom(src => src.RouteName))
            //.ForMember(d => d.Strength, opt => opt.MapFrom(src => src.Strength))
            //.ForMember(d => d.SubstanceName, opt => opt.MapFrom(src => src.SubstanceName))
            //.ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            //.ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.ProductId))
            //.ForMember(d => d.ProprietaryName, opt => opt.MapFrom(src => src.ProprietaryName))
            //.ForMember(d => d.Unit, opt => opt.MapFrom(src => string.Empty));

            Mapper.CreateMap<DTO.Medication, MedNameSearchDoc>()
                .ForMember(d => d.ProductNDC, opt => opt.MapFrom(src => src.NDC))
                .ForMember(d => d.CompositeName, opt => opt.MapFrom(src => src.ProprietaryName + " " + src.ProprietaryNameSuffix));
        }
    }
}