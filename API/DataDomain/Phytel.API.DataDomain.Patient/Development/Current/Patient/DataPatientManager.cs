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
using ServiceStack.Service;
using Phytel.API.DataDomain.PatientSystem.DTO;
using System.Net;
using Phytel.API.Common;


namespace Phytel.API.DataDomain.Patient
{
    public class PatientDataManager : IPatientDataManager
    {
        public IPatientRepositoryFactory Factory { get; set; }
        public Phytel.API.Common.IHelpers Helpers { get; set; }

        public GetCohortPatientsDataResponse GetCohortPatients(GetCohortPatientsDataRequest request)
        {
            try
            {
                string DDCohortServiceURL = ConfigurationManager.AppSettings["DDCohortServiceUrl"];

                GetCohortPatientsDataResponse result = new GetCohortPatientsDataResponse();

                IRestClient client = new JsonServiceClient();

                string url = Helpers.BuildURL(string.Format("{0}/{1}/{2}/{3}/cohort/{4}", DDCohortServiceURL, request.Context, request.Version, request.ContractNumber, request.CohortID), request.UserId);

                // 1) lookup query for cohortid in cohorts collection
                string cohortID = request.CohortID;
                GetCohortDataResponse response = client.Get<GetCohortDataResponse>(url);

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
                IPatientRepository repo = Factory.GetRepository(request, RepositoryType.CohortPatientView);

                result.CohortPatients = repo.Select(cohortQuery, filterParms, response.Cohort.Sort, request.Skip, request.Take);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public GetPatientDataResponse GetPatientByID(GetPatientDataRequest request)
        {
            try
            {
                GetPatientDataResponse result = new GetPatientDataResponse();

                IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);

                if (string.IsNullOrEmpty(request.UserId))
                {
                    result.Patient = repo.FindByID(request.PatientID) as DTO.PatientData;
                }
                else
                {
                    result.Patient = repo.FindByID(request.PatientID, request.UserId) as DTO.PatientData;
                }
                return (result != null ? result : new GetPatientDataResponse());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public GetPatientSSNDataResponse GetPatientSSN(GetPatientSSNDataRequest request)
        {
            try
            {
                GetPatientSSNDataResponse result = new GetPatientSSNDataResponse();

                IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);

                result.SSN = repo.GetSSN(request.PatientId) as string;

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public GetPatientsDataResponse GetPatients(GetPatientsDataRequest request)
        {
            try
            {
                IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);

                GetPatientsDataResponse result = repo.Select(request.PatientIds);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PatientData> GetAllPatients(GetAllPatientsDataRequest request)
        {
            try
            {
                IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);
                List<PatientData> result = repo.SelectAll() as List<PatientData>;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PutPatientDataResponse InsertPatient(PutPatientDataRequest request)
        {
            IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);
            PutPatientDataResponse result = repo.Insert(request) as PutPatientDataResponse;
            if (!string.IsNullOrEmpty(result.Id))
            {
                //Create Engage system record for the newly created patient in PatientSystem collection.
                result.EngagePatientSystemId = insertEngagePatientSystem(result.Id, request);
            }
            return result;
        }

        public PutCohortPatientViewDataResponse InsertCohortPatientView(PutCohortPatientViewDataRequest request)
        {
            IPatientRepository repo = Factory.GetRepository(request, RepositoryType.CohortPatientView);

            PutCohortPatientViewDataResponse result = repo.Insert(request) as PutCohortPatientViewDataResponse;
            return result;
        }

        public PutPatientPriorityResponse UpdatePatientPriority(PutPatientPriorityRequest request)
        {
            PutPatientPriorityResponse response = new PutPatientPriorityResponse();
            IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);

            response = repo.UpdatePriority(request) as PutPatientPriorityResponse;
            return response;
        }

        public PutPatientFlaggedResponse UpdatePatientFlagged(PutPatientFlaggedRequest request)
        {
            PutPatientFlaggedResponse response = new PutPatientFlaggedResponse();
            IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);

            response = repo.UpdateFlagged(request) as PutPatientFlaggedResponse;
            return response;
        }

        public PutPatientSystemIdDataResponse UpdatePatientSystem(PutPatientSystemIdDataRequest request)
        {
            PutPatientSystemIdDataResponse response = new PutPatientSystemIdDataResponse();
            IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);

            response = repo.UpdatePatientSystem(request) as PutPatientSystemIdDataResponse;
            return response;
        }

        public PutInitializePatientDataResponse InitializePatient(PutInitializePatientDataRequest request)
        {
            PutInitializePatientDataResponse response = null;
            try
            {
                response = new PutInitializePatientDataResponse();
                IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);

                response.PatientData = (PatientData)repo.Initialize(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public PutUpdatePatientDataResponse UpdatePatient(PutUpdatePatientDataRequest request)
        {
            PutUpdatePatientDataResponse response = new PutUpdatePatientDataResponse();
            try
            {
                IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);
                if (request.PatientData != null)
                {
                    if (request.Insert)
                    {
                        if (request.InsertDuplicate) // the user has ignored the warning message about a duplicate patient entry.
                        {
                            response = repo.Update(request) as PutUpdatePatientDataResponse;
                            if (!string.IsNullOrEmpty(response.Id))
                            {
                                //Create Engage system record for the newly created patient in PatientSystem collection.
                                insertEngagePatientSystem(response.Id, request);
                            }
                        }
                        else
                        {
                            if (repo.FindDuplicatePatient(request) == null)
                            {
                                response = repo.Update(request) as PutUpdatePatientDataResponse;
                                if (!string.IsNullOrEmpty(response.Id))
                                {
                                    //Create Engage system record for the newly created patient in PatientSystem collection.
                                    insertEngagePatientSystem(response.Id, request);
                                }
                            }
                            else
                            {
                                Outcome outcome = new Outcome
                                {
                                    Result = 0,
                                    Reason = "An individual by the same first name, last name and date of birth already exists."
                                };
                                response.Outcome = outcome;
                            }
                        }
                    }
                    else
                    {
                        response = repo.Update(request) as PutUpdatePatientDataResponse;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public PutUpdateCohortPatientViewResponse UpdateCohortPatientViewProblem(PutUpdateCohortPatientViewRequest request)
        {
            IPatientRepository repo = Factory.GetRepository(request, RepositoryType.CohortPatientView);

            PutUpdateCohortPatientViewResponse result = repo.Update(request) as PutUpdateCohortPatientViewResponse;
            return result;
        }

        public GetCohortPatientViewResponse GetCohortPatientView(GetCohortPatientViewRequest request)
        {
            IPatientRepository repo = Factory.GetRepository(request, RepositoryType.CohortPatientView);

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

        public InsertBatchPatientsDataResponse InsertBatchPatients(InsertBatchPatientsDataRequest request)
        {
            InsertBatchPatientsDataResponse response = new InsertBatchPatientsDataResponse();
            if (request.PatientsData != null && request.PatientsData.Count > 0)
            {
                List<HttpObjectResponse<PatientData>> list = new List<HttpObjectResponse<PatientData>>();
                IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);
                request.PatientsData.ForEach(p =>
                {
                    PutPatientDataRequest insertReq = new PutPatientDataRequest { Patient = p, Context = request.Context, ContractNumber = request.ContractNumber, UserId = request.UserId, Version = request.Version };
                    HttpStatusCode code = HttpStatusCode.OK;
                    PatientData patientData = null;
                    string message = string.Empty;
                    try
                    {
                        PutPatientDataResponse patientResponse = repo.Insert(insertReq) as PutPatientDataResponse;
                        if (!string.IsNullOrEmpty(patientResponse.Id))
                        {
                            // Create Engage system record for the newly created patient in PatientSystem collection.
                            string engageId = insertEngagePatientSystem(patientResponse.Id, insertReq);
                            code = HttpStatusCode.Created;
                            patientData = new PatientData { Id = patientResponse.Id, ExternalRecordId = p.ExternalRecordId, EngagePatientSystemId = engageId };
                        }
                    }
                    catch (Exception ex)
                    {
                        code = HttpStatusCode.InternalServerError;
                        message = string.Format("ExternalRecordId: {0}, Message: {1}, StackTrace: {2}", p.ExternalRecordId, ex.Message, ex.StackTrace);
                    }
                    list.Add(new HttpObjectResponse<PatientData> { Code = code, Body = (PatientData)patientData, Message = message });
                });
                response.Responses = list;
            }
            return response;
        }

        #region Delete
        public DeletePatientDataResponse DeletePatient(DeletePatientDataRequest request)
        {
            DeletePatientDataResponse response = null;
            try
            {
                response = new DeletePatientDataResponse();

                IPatientRepository patientRepo = Factory.GetRepository(request, RepositoryType.Patient);
                patientRepo.Delete(request);
                response.DeletedId = request.Id;
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public DeletePatientUserByPatientIdDataResponse DeletePatientUserByPatientId(DeletePatientUserByPatientIdDataRequest request)
        {
            DeletePatientUserByPatientIdDataResponse response = null;
            try
            {
                response = new DeletePatientUserByPatientIdDataResponse();

                IPatientRepository patientUserRepo = Factory.GetRepository(request, RepositoryType.PatientUser);

                List<PatientUserData> puData = patientUserRepo.FindPatientUsersByPatientId(request.PatientId);
                List<string> deletedIds = null;
                if (puData != null)
                {
                    deletedIds = new List<string>();
                    puData.ForEach(u =>
                    {
                        request.Id = u.Id;
                        patientUserRepo.Delete(request);
                        deletedIds.Add(request.Id);
                    });
                    response.DeletedIds = deletedIds;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public DeleteCohortPatientViewDataResponse DeleteCohortPatientViewByPatientId(DeleteCohortPatientViewDataRequest request)
        {
            DeleteCohortPatientViewDataResponse response = null;
            try
            {
                response = new DeleteCohortPatientViewDataResponse();

                IPatientRepository cpvRepo = Factory.GetRepository(request, RepositoryType.CohortPatientView);
                CohortPatientViewData cpvData = cpvRepo.FindCohortPatientViewByPatientId(request.PatientId);
                if (cpvData != null)
                {
                    request.Id = cpvData.Id;
                    cpvRepo.Delete(request);
                    response.DeletedId = request.Id;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        } 
        #endregion

        #region UndoDelete
        public UndoDeletePatientDataResponse UndoDeletePatient(UndoDeletePatientDataRequest request)
        {
            UndoDeletePatientDataResponse response = null;
            try
            {
                response = new UndoDeletePatientDataResponse();
                IPatientRepository patientRepo = Factory.GetRepository(request, RepositoryType.Patient);
                if(request.Id != null)
                {
                    patientRepo.UndoDelete(request);
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public UndoDeletePatientUsersDataResponse UndoDeletePatientUser(UndoDeletePatientUsersDataRequest request)
        {
            UndoDeletePatientUsersDataResponse response = null;
            try
            {
                response = new UndoDeletePatientUsersDataResponse();
                IPatientRepository patientUserRepo = Factory.GetRepository(request, RepositoryType.PatientUser);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        request.PatientUserId = u;
                        patientUserRepo.UndoDelete(request);
                    });
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public UndoDeleteCohortPatientViewDataResponse UndoDeleteCohortPatientView(UndoDeleteCohortPatientViewDataRequest request)
        {
            UndoDeleteCohortPatientViewDataResponse response = null;
            try
            {
                response = new UndoDeleteCohortPatientViewDataResponse();
                IPatientRepository cpvRepo = Factory.GetRepository(request, RepositoryType.CohortPatientView);
                if (request.Id != null)
                {
                    cpvRepo.UndoDelete(request);
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        /// <summary>
        /// Calls PatientSystem data domain to insert an Engage System record for the newly created patient.
        /// </summary>
        /// <param name="request">IDataDomainRequest object</param>
        /// <returns>Id of the Engage patient system.</returns>
        private string insertEngagePatientSystem(string patientId, IDataDomainRequest request)
        {
            string id = null;
            try
            {
                InsertPatientSystemDataRequest psRequest = new InsertPatientSystemDataRequest
                {
                    PatientId = patientId,
                    IsEngageSystem = true,
                    PatientSystemsData = new PatientSystemData { PatientId = patientId },
                    Context = request.Context,
                    ContractNumber = request.ContractNumber,
                    UserId = Constants.SystemContactId,// the requirement says that the engage Id should have createdby user as 'system'.
                    Version = request.Version
                };
                
                string DDPatientSystemServiceUrl = ConfigurationManager.AppSettings["DDPatientSystemServiceUrl"];
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystem", "POST")]
                string url = Helpers.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/PatientSystem", DDPatientSystemServiceUrl, psRequest.Context, psRequest.Version, psRequest.ContractNumber, psRequest.PatientId), psRequest.UserId);
                InsertPatientSystemDataResponse dataDomainResponse = client.Post<InsertPatientSystemDataResponse>(url, psRequest as object);
                if (dataDomainResponse != null && !string.IsNullOrEmpty(dataDomainResponse.Id))
                {
                    id = dataDomainResponse.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return id;
        }
    }
}   
