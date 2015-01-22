using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication
{
    public class StubPatientMedSuppDataManager : IPatientMedSuppDataManager
    {

        public List<PatientMedSuppData> GetPatientMedSupps(GetPatientMedSuppsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public PatientMedSuppData SavePatientMedSupps(PutPatientMedSuppDataRequest request)
        {
            throw new NotImplementedException();
        }

        public DeleteMedSuppsByPatientIdDataResponse DeletePatientMedSupps(DeleteMedSuppsByPatientIdDataRequest request)
        {
            throw new NotImplementedException();
        }

        public UndoDeletePatientMedSuppsDataResponse UndoDeletePatientMedSupps(UndoDeletePatientMedSuppsDataRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
