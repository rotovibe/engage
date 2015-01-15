using System;
using System.Collections.Generic;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.AppDomain.NG.Medication
{
    public interface IMedicationManager
    {
        void LogException(Exception ex);
        List<PatientMedSupp> GetPatientMedSupps(GetPatientMedSuppsRequest request);
        PatientMedSupp UpdatePatientMedSupp(PostPatientMedSuppRequest request);
        PatientMedSupp InitializePatientMedSupp(PostInitializePatientMedSuppRequest request);
        MedicationMap InitializeMedicationMap(PostInitializeMedicationMapRequest request);
    }
}