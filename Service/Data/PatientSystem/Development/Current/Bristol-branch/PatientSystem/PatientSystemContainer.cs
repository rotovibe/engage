using Funq;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.PatientSystem.Repo;
using ServiceStack.Common;

namespace Phytel.API.DataDomain.PatientSystem
{
    public static class PatientSystemContainer
    {
        public static Container Configure(Container container)
        {
            container = PatientSystemRepositoryContainer.Configure(container);
            
            container.Register<IPatientSystemRepositoryFactory>(c =>
                new PatientNoteRepositoryFactory(HostContext.Instance.Items["Contract"].ToString(),
                    HostContext.Instance.Items["UserId"].ToString())).ReusedWithin(ReuseScope.Request);

            container.Register<ISystemDataManager>(c =>
                new SystemDataManager(c.Resolve<IPatientSystemRepositoryFactory>())).ReusedWithin(ReuseScope.Request);

            container.Register<IPatientSystemDataManager>(c =>
                new PatientSystemDataManager(c.Resolve<IPatientSystemRepositoryFactory>())).ReusedWithin(ReuseScope.Request);

            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<Helpers, IHelpers>().ReusedWithin(ReuseScope.Request);
            return container;
        }
    }
}
