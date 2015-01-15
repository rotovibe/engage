using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.AppDomain.NG.Medication
{
    public interface IMedicationEndpointUtil
    {
        List<PatientMedSuppData> GetPatientMedSupps(GetPatientMedSuppsRequest request);
        PatientMedSuppData UpdatePatientMedSupp(PostPatientMedSuppRequest request);
        List<string> GetMedicationNDCs(PostPatientMedSuppRequest request);
        PatientMedSuppData InitializePatientMedSupp(PostInitializePatientMedSuppRequest request);
        MedicationMapData InitializeMedicationMap(PostInitializeMedicationMapRequest request);
        MedicationMapData UpdateMedicationMap(PostMedicationMapRequest request);
    }
}