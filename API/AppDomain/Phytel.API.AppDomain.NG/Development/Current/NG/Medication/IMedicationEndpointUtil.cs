using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.AppDomain.NG.Medication
{
    public interface IMedicationEndpointUtil
    {
        List<PatientMedSuppData> GetPatientMedSupps(GetPatientMedSuppsRequest request);
        PatientMedSuppData SavePatientMedSupp(PostPatientMedSuppRequest request);
    }
}