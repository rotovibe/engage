using Phytel.API.DataDomain.Medication.DTO;

namespace AppDomain.Engage.Clinical.DataDomainClient
{
    public interface IMedicationDataDomainClient
    {
        string PostPatientMedications(PatientMedSuppData data);

        // create method to call to the clinical data domains
        // medications
        // supplements
    }
}
