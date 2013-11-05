using Phytel.API.DataDomain.Patient.DTO;
using System.Data.SqlClient;

namespace Phytel.API.DataDomain.Patient
{
    public static class PatientDataManager
    {
        public static DataPatientResponse GetPatientByID(DataPatientRequest request)
        {
            DataPatientResponse result = new DataPatientResponse();

            IPatientRepository<DataPatientResponse> repo = Phytel.API.DataDomain.Patient.PatientRepositoryFactory<DataPatientResponse>.GetPatientRepository(request.ContractID, request.Context);
            MEPatient mePatient =  repo.FindByID(request.PatientID) as MEPatient;

            if (mePatient != null)
            {
                result.FirstName = mePatient.FirstName;
                result.LastName = mePatient.LastName;
                result.PatientID = mePatient.PatientID;

            }

            return result;
        }
    }
}   
