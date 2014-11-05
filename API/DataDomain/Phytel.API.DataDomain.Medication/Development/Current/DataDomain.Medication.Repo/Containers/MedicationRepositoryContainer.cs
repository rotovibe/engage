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
                    //c.ResolveNamed<IUOWMongo<MedicationMongoContext>>(Constants.Domain)
                    "InHealth001"
                    )
                ).ReusedWithin(Funq.ReuseScope.Request);

            /*
             * Uncomment this and register MongoMedicationRepository without a UOW wrapper.
             * Delete the one above.
             * 
            container.Register<IMongoMedicationRepository>(Constants.Domain, c =>
                new MongoMedicationRepository<MedicationMongoContext>(
                    new MedicationMongoContext(c.ResolveNamed<string>(Constants.NamedString)))
                ).ReusedWithin(Funq.ReuseScope.Request);
            */

            return container;
        }
    }
}
