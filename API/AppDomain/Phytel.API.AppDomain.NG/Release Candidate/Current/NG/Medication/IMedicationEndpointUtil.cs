using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.AppDomain.NG.Medication
{
    public interface IMedicationEndpointUtil
    {
        List<PatientMedSuppData> GetPatientMedSupps(GetPatientMedSuppsRequest request);
        PatientMedSuppData SavePatientMedSupp(PostPatientMedSuppRequest request);
        List<string> GetMedicationNDCs(PostPatientMedSuppRequest request);
        MedicationMapData InitializeMedicationMap(PostInitializeMedicationMapRequest request);
        MedicationMapData UpdateMedicationMap(PutMedicationMapRequest request);
        MedicationMapData InsertMedicationMap(PostMedicationMapRequest request);
        List<MedicationMapData> DeleteMedicationMap(PutDeleteMedMapRequest request);
        List<MedicationMapData> SearchMedicationMap(GetMedicationMapsRequest request);
        void DeletePatientMedSupp(DeletePatientMedSuppRequest request);
        List<PatientMedFrequencyData> GetPatientMedFrequencies(GetPatientMedFrequenciesRequest request);
        string InsertPatientMedFrequency(PostPatientMedFrequencyRequest request);
        void DeleteMedicationMaps(DeleteMedicationMapsRequest request);
        int GetPatientMedSuppsCount(GetPatientMedSuppsCountRequest request);
    }
}