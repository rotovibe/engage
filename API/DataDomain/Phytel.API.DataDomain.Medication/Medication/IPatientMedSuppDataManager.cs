using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication
{
    public interface IPatientMedSuppDataManager
    {
        int GetPatientMedSuppsCount(GetPatientMedSuppsCountDataRequest request);
        List<PatientMedSuppData> GetPatientMedSupps(GetPatientMedSuppsDataRequest request);
        PatientMedSuppData SavePatientMedSupps(PutPatientMedSuppDataRequest request);
        DeleteMedSuppsByPatientIdDataResponse DeletePatientMedSupps(DeleteMedSuppsByPatientIdDataRequest request);
        UndoDeletePatientMedSuppsDataResponse UndoDeletePatientMedSupps(UndoDeletePatientMedSuppsDataRequest request);
        DeletePatientMedSuppDataResponse Delete(DeletePatientMedSuppDataRequest request);
    }
}
