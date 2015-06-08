using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Meds.Request;
using Phytel.API.DataDomain.Medication.DTO;

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
        bool DeleteMedicationMap(DeleteMedMapRequest request);
    }
}