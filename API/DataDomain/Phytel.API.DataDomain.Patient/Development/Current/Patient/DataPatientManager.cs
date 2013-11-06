using Phytel.API.DataDomain.Patient.DTO;
using System.Data.SqlClient;

namespace Phytel.API.DataDomain.Patient
{
    public static class PatientDataManager
    {
        public static PatientResponse GetPatientByID(PatientRequest request)
        {
            PatientResponse result = new PatientResponse();

            IPatientRepository<PatientResponse> repo = Phytel.API.DataDomain.Patient.PatientRepositoryFactory<PatientResponse>.GetPatientRepository(request.ContractNumber, request.Context);
            MEPatient mePatient =  repo.FindByID(request.PatientID) as MEPatient;

            if (mePatient != null)
            {
                result.FirstName = mePatient.FirstName;
                result.LastName = mePatient.LastName;
                result.PatientID = mePatient.PatientID;
                result.Gender = mePatient.Gender;
                result.DOB = mePatient.DOB;
            }

            return result;
        }
    }
}   
