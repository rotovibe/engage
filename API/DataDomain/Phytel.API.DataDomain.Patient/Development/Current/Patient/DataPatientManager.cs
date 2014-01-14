using Phytel.API.DataDomain.Patient.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Patient;
using System;
using System.Configuration;
using ServiceStack.ServiceClient.Web;
using Phytel.API.DataDomain.Cohort.DTO;

namespace Phytel.API.DataDomain.Patient
{
    public static class PatientDataManager
    {
        public static GetCohortPatientsDataResponse GetCohortPatients(GetCohortPatientsDataRequest request)
        {
            try
            {
                string DDCohortServiceURL = ConfigurationManager.AppSettings["DDCohortServiceUrl"];

                GetCohortPatientsDataResponse result = new GetCohortPatientsDataResponse();

                JsonServiceClient client = new JsonServiceClient();

                // 1) lookup query for cohortid in cohorts collection
                string cohortID = request.CohortID;
                GetCohortDataResponse response1 = client.Get<GetCohortDataResponse>
                    (string.Format("{0}/{1}/{2}/{3}/cohort/{4}", DDCohortServiceURL, request.Context, request.Version, request.ContractNumber, request.CohortID));

                // 2) get patientIDs through cohortpatients view
                IPatientRepository<PatientData> repo = PatientRepositoryFactory<PatientData>.GetPatientRepository(request.ContractNumber, request.Context);

                string field1 = string.Empty;
                string field2 = string.Empty;

                if (string.IsNullOrEmpty(request.SearchFilter) == false)
                {
                    //is there a comma in the string?
                    if (request.SearchFilter.IndexOf(',') > -1)
                    {
                        string[] info = request.SearchFilter.Split(",".ToCharArray());
                        field1 = info[1].Trim();
                        field2 = info[0].Trim();
                    }
                    else
                    {
                        string[] info = request.SearchFilter.Split(" ".ToCharArray());
                        field1 = info[0].Trim();
                        if (info.Length > 1)
                            field2 = info[1].Trim();
                    }
                }

                string[] filterParms = new string[] { field1, field2 };

                result.CohortPatients = repo.Select(response1.Cohort.Query, filterParms, response1.Cohort.Sort, request.Skip, request.Take);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static GetPatientDataResponse GetPatientByID(GetPatientDataRequest request)
        {
            try
            {
                GetPatientDataResponse result = new GetPatientDataResponse();

                IPatientRepository<GetPatientDataResponse> repo = PatientRepositoryFactory<GetPatientDataResponse>.GetPatientRepository(request.ContractNumber, request.Context);
                result.Patient = repo.FindByID(request.PatientID, request.UserId) as DTO.PatientData;

                return (result != null ? result : new GetPatientDataResponse());
            }
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
            }
        }

        public static PutPatientDataResponse InsertPatient(PutPatientDataRequest request)
        {
           IPatientRepository<PutPatientDataRequest> repo = PatientRepositoryFactory<PutPatientDataRequest>.GetPatientRepository(request.ContractNumber, request.Context);
           PutPatientDataResponse result = repo.Insert(request) as PutPatientDataResponse;
           return result;
        }

        public static PutPatientPriorityResponse UpdatePatientPriority(PutPatientPriorityRequest request)
        {
            PutPatientPriorityResponse response = new PutPatientPriorityResponse();
            IPatientRepository<PutPatientPriorityRequest> repo = PatientRepositoryFactory<PutPatientPriorityRequest>.GetPatientRepository(request.ContractNumber, request.Context);
            response = repo.UpdatePriority(request) as PutPatientPriorityResponse;
            return response;
        }

        public static PutPatientFlaggedResponse UpdatePatientFlagged(PutPatientFlaggedRequest request)
        {
            PutPatientFlaggedResponse response = new PutPatientFlaggedResponse();
            IPatientRepository<PutPatientFlaggedRequest> repo = PatientRepositoryFactory<PutPatientFlaggedRequest>.GetPatientRepository(request.ContractNumber, request.Context);
            response = repo.UpdateFlagged(request) as PutPatientFlaggedResponse;
            return response;
        }

        public static PutUpdatePatientDataResponse UpdatePatient(PutUpdatePatientDataRequest request)
        {
            IPatientRepository<PutUpdatePatientDataRequest> repo = PatientRepositoryFactory<PutUpdatePatientDataRequest>.GetPatientRepository(request.ContractNumber, request.Context);
            PutUpdatePatientDataResponse result = repo.Update(request) as PutUpdatePatientDataResponse;
            return result;
        }
    }
}   
