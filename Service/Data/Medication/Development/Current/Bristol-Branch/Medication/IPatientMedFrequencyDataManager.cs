using System.Collections.Generic;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication
{
    public interface IPatientMedFrequencyDataManager
    {
        List<PatientMedFrequencyData> GetPatientMedFrequencies(GetPatientMedFrequenciesDataRequest request);
        string InsertPatientMedFrequency(PostPatientMedFrequencyDataRequest request);
    }
}