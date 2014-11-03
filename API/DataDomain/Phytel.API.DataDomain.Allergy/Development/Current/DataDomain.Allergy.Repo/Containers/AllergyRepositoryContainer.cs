using Phytel.API.DataDomain.Allergy.DTO;

namespace DataDomain.Allergy.Repo.Containers
{
    public static class AllergyRepositoryContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
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
