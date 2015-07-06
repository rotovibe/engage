using DataDomain.PatientNote.Repo;
using Funq;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.PatientNote.Repo;
using ServiceStack.Common;

namespace Phytel.API.DataDomain.PatientNote
{
    public static class PatientNoteContainer
    {
        public static Container Configure(Container container)
        {
            container = PatientNoteRepositoryContainer.Configure(container);

            container.Register<IPatientNoteRepositoryFactory>(c =>
                new PatientNoteRepositoryFactory(HostContext.Instance.Items["Contract"].ToString(),
                    HostContext.Instance.Items["UserId"].ToString())).ReusedWithin(ReuseScope.Request);

            container.Register<IDataPatientUtilizationManager>(c =>
                new DataPatientUtilizationManager(c.Resolve<IPatientNoteRepositoryFactory>() ) ).ReusedWithin(ReuseScope.Request);

            container.Register<IPatientNoteDataManager>(c =>
                new PatientNoteDataManager(c.Resolve<IPatientNoteRepositoryFactory>())).ReusedWithin(ReuseScope.Request);

            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<Helpers, IHelpers>().ReusedWithin(ReuseScope.Request);
            return container;
        }
    }
}
