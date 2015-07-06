using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.Test.Stub
{
    public class StubDataPatientUtilizationManager : IDataPatientUtilizationManager
    {
        public string InsertPatientUtilization(PatientUtilizationData data)
        {
            return "111111111111111111111111";
        }

        public bool UpdatePatientUtilization(PatientUtilizationData data)
        {
            return true;
        }

        public List<PatientUtilizationData> GetPatientUtilizations(string userId)
        {
            return new List<PatientUtilizationData>{ TestInit.PatientUtilizationData };
        }

        public PatientUtilizationData GetPatientUtilization(string utilId)
        {
            return TestInit.PatientUtilizationData;
        }

        public bool DeletePatientUtilization(string utilId)
        {
            return true;
        }

        public Repo.IMongoPatientNoteRepository Repository
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

    }
}
