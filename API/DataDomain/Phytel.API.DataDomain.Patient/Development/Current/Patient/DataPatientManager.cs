using Phytel.API.DataDomain.Patient.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Patient;
using System;
using System.Configuration;
using ServiceStack.ServiceClient.Web;
using Phytel.API.DataDomain.Cohort.DTO;
using System.Collections.Generic;
using Phytel.API.Interface;
using System.Linq;

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
                GetCohortDataResponse response = client.Get<GetCohortDataResponse>
                    (string.Format("{0}/{1}/{2}/{3}/cohort/{4}", DDCohortServiceURL, request.Context, request.Version, request.ContractNumber, request.CohortID));

                string cohortQuery = response.Cohort.Query;
                //If #USER_ID# is present in the cohort query, replace it with the ContactId.
                if (!string.IsNullOrEmpty(request.UserId))
                {
                    cohortQuery = response.Cohort.Query.Replace("#USER_ID#", request.UserId);
                }

                //Get the filter parameters
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

                // 2) get patientIDs through cohortpatients view
                IPatientRepository<CohortPatientViewData> repo = PatientRepositoryFactory<CohortPatientViewData>.GetCohortPatientViewRepository(request.ContractNumber, request.Context);
                result.CohortPatients = repo.Select(cohortQuery, filterParms, response.Cohort.Sort, request.Skip, request.Take);

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

        public static PutCohortPatientViewDataResponse InsertCohortPatientView(PutCohortPatientViewDataRequest request)
        {
            IPatientRepository<PutCohortPatientViewDataRequest> repo = PatientRepositoryFactory<PutCohortPatientViewDataRequest>.GetCohortPatientViewRepository(request.ContractNumber, request.Context);
            PutCohortPatientViewDataResponse result = repo.Insert(request) as PutCohortPatientViewDataResponse;
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

        public static PutPatientBackgroundDataResponse UpdatePatientBackground(PutPatientBackgroundDataRequest request)
        {
            PutPatientBackgroundDataResponse response = new PutPatientBackgroundDataResponse();
            IPatientRepository<PutPatientBackgroundDataRequest> repo = PatientRepositoryFactory<PutPatientBackgroundDataRequest>.GetPatientRepository(request.ContractNumber, request.Context);
            response = repo.UpdateBackground(request) as PutPatientBackgroundDataResponse;
            return response;
        }

        public static PutUpdatePatientDataResponse UpdatePatient(PutUpdatePatientDataRequest request)
        {
            IPatientRepository<PutUpdatePatientDataRequest> repo = PatientRepositoryFactory<PutUpdatePatientDataRequest>.GetPatientRepository(request.ContractNumber, request.Context);
            PutUpdatePatientDataResponse result = repo.Update(request) as PutUpdatePatientDataResponse;
            return result;
        }

        public static PutUpdateCohortPatientViewResponse UpdateCohortPatientViewProblem(PutUpdateCohortPatientViewRequest request)
        {
            IPatientRepository<PutUpdateCohortPatientViewRequest> repo = PatientRepositoryFactory<PutUpdateCohortPatientViewRequest>.GetCohortPatientViewRepository(request.ContractNumber, request.Context);
            PutUpdateCohortPatientViewResponse result = repo.Update(request) as PutUpdateCohortPatientViewResponse;
            return result;
        }

        public static GetCohortPatientViewResponse GetCohortPatientView(GetCohortPatientViewRequest request)
        {
            IPatientRepository<GetCohortPatientViewRequest> repo = PatientRepositoryFactory<GetCohortPatientViewRequest>.GetCohortPatientViewRepository(request.ContractNumber, request.Context);
            GetCohortPatientViewResponse result = new GetCohortPatientViewResponse();
            ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

            // PatientID
            SelectExpression patientSelectExpression = new SelectExpression();
            patientSelectExpression.FieldName = MECohortPatientView.PatientIDProperty;
            patientSelectExpression.Type = SelectExpressionType.EQ;
            patientSelectExpression.Value = request.PatientID;
            patientSelectExpression.ExpressionOrder = 1;
            patientSelectExpression.GroupID = 1;
            selectExpressions.Add(patientSelectExpression);

            APIExpression apiExpression = new APIExpression();
            apiExpression.Expressions = selectExpressions;

            Tuple<string, IEnumerable<object>> cohortPatientView = repo.Select(apiExpression);

            if (cohortPatientView != null)
            {
                List<CohortPatientViewData> cpd = cohortPatientView.Item2.Cast<CohortPatientViewData>().ToList();
                result.CohortPatientView = cpd[0];
            }

            return result;
        }
    }
}   
