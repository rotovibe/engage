using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Medication
{
    public interface IMedicationManager
    {
        void LogException(Exception ex);
        List<PatientMedSupp> GetPatientMedSupps(GetPatientMedSuppsRequest request);
        PatientMedSupp SavePatientMedSupp(PostPatientMedSuppRequest request);
        MedicationMap InitializeMedicationMap(PostInitializeMedicationMapRequest request);
        void DeletePatientMedSupp(DeletePatientMedSuppRequest request);
        string InsertPatientMedFrequency(PostPatientMedFrequencyRequest request);
        List<PatientMedFrequency> GetPatientMedFrequencies(GetPatientMedFrequenciesRequest request);
        void DeleteMedicationMap(PutDeleteMedMapRequest request);
        List<MedicationMap> GetMedicationMaps(GetMedicationMapsRequest request);
        void DeleteMedicationMaps(DeleteMedicationMapsRequest request);
        int GetPatientMedSuppsCount(GetPatientMedSuppsCountRequest request);
    }
}