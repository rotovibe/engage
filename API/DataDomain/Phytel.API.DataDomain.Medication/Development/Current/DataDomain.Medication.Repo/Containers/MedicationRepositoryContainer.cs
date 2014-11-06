using Phytel.API.DataDomain.Medication.DTO;

namespace DataDomain.Medication.Repo.Containers
{
    public static class MedicationRepositoryContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            container.Register<IUOWMongo<MedicationMongoContext>>(Constants.Domain, c =>
                new UOWMongo<MedicationMongoContext>(
                    c.ResolveNamed<string>(Constants.NamedString)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);


            container.Register<IMongoMedicationRepository>(Constants.Domain, c =>
                new MongoMedicationRepository<MedicationMongoContext>(
                    c.ResolveNamed<IUOWMongo<MedicationMongoContext>>(Constants.Domain)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            container.Register<IMongoPatientMedSuppRepository>(Constants.Domain, c =>
                new MongoPatientMedSuppRepository<MedicationMongoContext>(
                    c.ResolveNamed<IUOWMongo<MedicationMongoContext>>(Constants.Domain)
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            return container;
        }
    }
}
