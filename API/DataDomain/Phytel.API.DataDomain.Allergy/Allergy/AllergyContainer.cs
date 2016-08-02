using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Allergy;
using Phytel.API.DataDomain.Allergy.DTO;

namespace DataDomain.Allergy.Repo.Containers
{
    public static class AllergyContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            container = AllergyRepositoryContainer.Configure(container);

            //container.Register<IAllergyDataManager>(c =>
            //    new AllergyDataManager(c.ResolveNamed<IMongoAllergyRepository>(Constants.Domain))
            //    ).ReusedWithin(Funq.ReuseScope.Default);

            //container.Register<IPatientAllergyDataManager>(c =>
            //    new PatientAllergyDataManager(c.ResolveNamed<IMongoPatientAllergyRepository>(Constants.Domain))
            //    ).ReusedWithin(Funq.ReuseScope.Default);

            container.RegisterAutoWiredAs<AllergyDataManager, IAllergyDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<PatientAllergyDataManager, IPatientAllergyDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<Helpers, IHelpers>().ReusedWithin(Funq.ReuseScope.Request);
            return container;
        }
    }
}
