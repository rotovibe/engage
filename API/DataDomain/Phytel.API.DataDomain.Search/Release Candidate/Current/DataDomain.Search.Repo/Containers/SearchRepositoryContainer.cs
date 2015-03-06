using AutoMapper;
using DataDomain.Search.Repo.LuceneStrategy;
using Lucene.Net.Documents;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Search.DTO;

namespace DataDomain.Search.Repo.Containers
{
    public static class SearchRepositoryContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            //container.Register<IUOWMongo<SearchMongoContext>>(Constants.Domain, c =>
            //    new UOWMongo<SearchMongoContext>(
            //        c.ResolveNamed<string>(Constants.NamedString)
            //        )
            //    ).ReusedWithin(Funq.ReuseScope.Request);

            //container.Register<IMongoSearchRepository>(Constants.Domain, c =>
            //    new MongoSearchRepository<SearchMongoContext>(
            //        c.ResolveNamed<IUOWMongo<SearchMongoContext>>(Constants.Domain)
            //        )
            //    ).ReusedWithin(Funq.ReuseScope.Request);

            /*
             * Uncomment this and register MongoSearchRepository without a UOW wrapper.
             * Delete the one above.
             * 
            container.Register<IMongoSearchRepository>(Constants.Domain, c =>
                new MongoSearchRepository<SearchMongoContext>(
                    new SearchMongoContext(c.ResolveNamed<string>(Constants.NamedString)))
                ).ReusedWithin(Funq.ReuseScope.Request);
            */

            Mapper.CreateMap<DTO.Allergy, IdNamePair>().ForMember(d => d.Name, opt => opt.MapFrom(src => src.Name.Trim().ToUpper().Replace("\"", "").Replace(",", "")));
            Mapper.CreateMap<Document, IdNamePair>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Get("Id")))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Get("Name")));

            Mapper.CreateMap<Document, TextValuePair>()
                                .ForMember(d => d.Value, opt => opt.MapFrom(src => src.Get("CompositeName").Trim()))
                                .ForMember(d => d.Text, opt => opt.MapFrom(src => src.Get("CompositeName").Trim()));

            Mapper.CreateMap<Document, MedNameSearchDocData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Get("MongoId")))
                .ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.Get("PackageId")))
                .ForMember(d => d.DosageFormname, opt => opt.MapFrom(src => src.Get("DosageFormName")))
                .ForMember(d => d.CompositeName, opt => opt.MapFrom(src => src.Get("CompositeName")))
                .ForMember(d => d.ProprietaryName, opt => opt.MapFrom(src => src.Get("ProprietaryName")))
                .ForMember(d => d.RouteName, opt => opt.MapFrom(src => src.Get("RouteName")))
                .ForMember(d => d.SubstanceName, opt => opt.MapFrom(src => src.Get("SubstanceName")))
                .ForMember(d => d.Strength, opt => opt.MapFrom(src => src.Get("Strength")))
                .ForMember(d => d.Unit, opt => opt.MapFrom(src => src.Get("Unit")));

            Mapper.CreateMap<DTO.Medication, MedNameSearchDocData>()
                .ForMember(d => d.ProductNDC, opt => opt.MapFrom(src => src.NDC))
                .ForMember(d => d.CompositeName, opt => opt.MapFrom(src => src.ProprietaryName + " " + src.ProprietaryNameSuffix));

            // search index initialization
            container.RegisterAutoWiredAs<MedNameLuceneStrategy<MedNameSearchDocData, TextValuePair>, IMedNameLuceneStrategy<MedNameSearchDocData, TextValuePair>>().ReusedWithin(Funq.ReuseScope.Container);
            container.RegisterAutoWiredAs<AllergyLuceneStrategy<IdNamePair, IdNamePair>, IAllergyLuceneStrategy<IdNamePair, IdNamePair>>().ReusedWithin(Funq.ReuseScope.Container);
            return container;
        }
    }
}
