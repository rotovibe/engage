using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.PatientNote.Repo;

namespace Phytel.API.DataDomain.PatientNote
{
    public static class PatientNoteContainer
    {
        public static Funq.Container Configure(Funq.Container container)
        {
            container = PatientNoteRepositoryContainer.Configure(container);
            container.RegisterAutoWiredAs<PatientNoteDataManager, IPatientNoteDataManager>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<CommonFormatterUtil, ICommonFormatterUtil>().ReusedWithin(Funq.ReuseScope.Request);
            container.RegisterAutoWiredAs<Helpers, IHelpers>().ReusedWithin(Funq.ReuseScope.Request);
            return container;
        }
    }
}
