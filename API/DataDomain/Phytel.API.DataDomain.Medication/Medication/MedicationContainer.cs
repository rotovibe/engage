using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Medication;
using Phytel.API.DataDomain.Medication.DTO;

namespace DataDomain.Medication.Repo.Containers
{
    public static class MedicationContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            container = MedicationRepositoryContainer.Configure(container);

    //        container.Register<IMedicationDataManager>(c =>
    //            new MedicationDataManager(c.ResolveNamed<IMongoMedicationRepository>(Constants.Domain))
    //            ).ReusedWithin(Funq.ReuseScope.Default);

    //        container.Register<IPatientMedSuppDataManager>(c =>
    //new PatientMedSuppDataManager(c.ResolveNamed<IMongoPatientMedSuppRepository>(Constants.Domain))
    //).ReusedWithin(Funq.ReuseScope.Default);

            container.RegisterAutoWiredAs<MedicationDataManager, IMedicationDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<MedicationMappingDataManager, IMedicationMappingDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<PatientMedSuppDataManager, IPatientMedSuppDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<PatientMedFrequencyDataManager, IPatientMedFrequencyDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<Helpers, IHelpers>().ReusedWithin(Funq.ReuseScope.Request);
            return container;
        }
    }
}
