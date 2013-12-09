using Phytel.API.DataDomain.Patient.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Patient;
using System;

namespace Phytel.API.DataDomain.Patient
{
    public static class PatientDataManager
    {
        public static GetPatientDataResponse GetPatientByID(GetPatientDataRequest request)
        {
            try
            {
                GetPatientDataResponse result = new GetPatientDataResponse();

                IPatientRepository<GetPatientDataResponse> repo = PatientRepositoryFactory<GetPatientDataResponse>.GetPatientRepository(request.ContractNumber, request.Context);
                result.Patient = repo.FindByID(request.PatientID) as DTO.PatientData;

                return (result != null ? result : new GetPatientDataResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GetPatientsDataResponse GetPatients(GetPatientsDataRequest request)
        {
            try
            {
                IPatientRepository<GetPatientsDataResponse> repo = PatientRepositoryFactory<GetPatientsDataResponse>.GetPatientRepository(request.ContractNumber, request.Context);
                GetPatientsDataResponse result = repo.Select(request.PatientIDs);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
