using AutoMapper;
using Phytel.API.DataDomain.Allergy.DTO;

namespace DataDomain.Allergy.Repo.Containers
{
    public static class AllergyRepositoryContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            Mapper.CreateMap<MEAllergy, AllergyData>().ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(d => d.TypeIds, opt => opt.MapFrom(src => src.TypeIds.ConvertAll<string>(c => c.ToString())))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn));

            Mapper.CreateMap<MEPatientAllergy, PatientAllergyData>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn));

            container.Register<IUOWMongo<AllergyMongoContext>>(Constants.Domain, c =>
                new UOWMongo<AllergyMongoContext>(
                    c.ResolveNamed<string>(Constants.NamedString)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IMongoAllergyRepository>(Constants.Domain, c =>
                new MongoAllergyRepository<AllergyMongoContext>(
                    c.ResolveNamed<IUOWMongo<AllergyMongoContext>>(Constants.Domain)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IMongoPatientAllergyRepository>(Constants.Domain, c =>
                new MongoPatientAllergyRepository<AllergyMongoContext>(
                    c.ResolveNamed<IUOWMongo<AllergyMongoContext>>(Constants.Domain)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            return container;
        }
    }
}
