using Phytel.API.DataDomain.Patient.DTO;

namespace AppDomain.Engage.Population.DataDomainClient
{
    public interface IPatientDataDomainClient
    {
        string PostPatientDetails(PatientData patients);
    }
}
