using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Phytel.API.Common;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Phytel.API.DataAudit;


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
                GetPatientsDataResponse response = new GetPatientsDataResponse();
                Dictionary<string, PatientData> patients = null;
                IPatientRepository repo = Factory.GetRepository(request, RepositoryType.Patient);
                List<PatientData> list = repo.Select(request.PatientIds);
                if (list != null && list.Count > 0)
                {
                    patients = new Dictionary<string, PatientData>();
                    list.ForEach( p =>
                    {
                        patients.Add(p.Id, p);
                    });
                }
                response.Patients = patients;
                return response;
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
                PatientSystemData data = insertEngagePatientSystem(result.Id, request);
                if (data != null)
                {
                    result.EngagePatientSystemId = data.Id;
                }
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
                BulkInsertResult result = (BulkInsertResult)repo.InsertAll(request.PatientsData.Cast<object>().ToList());
                if (result != null)
                {
                    if(result.ProcessedIds != null && result.ProcessedIds.Count > 0)
                    {
                        // Get the patients that were newly inserted. 
                        List<PatientData> insertedPatients = repo.Select(result.ProcessedIds);
                        if (insertedPatients != null && insertedPatients.Count > 0)
                        {
                            List<string> insertedPatientIds = insertedPatients.Select(p => p.Id).ToList();

                            #region DataAudit for Patients
                            AuditHelper.LogDataAudit(request.UserId, MongoCollectionName.Patient.ToString(), insertedPatientIds, Common.DataAuditType.Insert, request.ContractNumber);
                            #endregion

                            #region BulkInsert CohortPatientView
                            List<CohortPatientViewData> cpvList = getMECohortPatientView(insertedPatients);
                            IPatientRepository cpvRepo = Factory.GetRepository(request, RepositoryType.CohortPatientView);
                            cpvRepo.InsertAll(cpvList.Cast<object>().ToList()); 
                            #endregion

                            #region BulkInsert EngagePatientSystems.
                            List<string> processedPatientSystemIds = insertBatchEngagePatientSystem(insertedPatientIds, request);
                            List<PatientSystemData> insertedPatientSystems = getPatientSystems(processedPatientSystemIds, request);

                            #region DataAudit for EngagePatientSystems
                            List<string> insertedPatientSystemIds = insertedPatientSystems.Select(p => p.Id).ToList();
                            AuditHelper.LogDataAudit(request.UserId, MongoCollectionName.PatientSystem.ToString(), insertedPatientSystemIds, Common.DataAuditType.Insert, request.ContractNumber);
                            #endregion

                            insertedPatients.ForEach(r =>
                            {
                                string engageValue = string.Empty;
                                var x = insertedPatientSystems.Where(s => s.PatientId == r.Id).FirstOrDefault();
                                if (x != null)
                                    engageValue = x.Value;
                                list.Add(new HttpObjectResponse<PatientData> { Code = HttpStatusCode.Created, Body = (PatientData)new PatientData { Id = r.Id, ExternalRecordId = r.ExternalRecordId, EngagePatientSystemValue = engageValue } });
                            }); 
                            #endregion
                        }
                    }
                    result.ErrorMessages.ForEach(e =>
                    {
                        list.Add(new HttpObjectResponse<PatientData> { Code = HttpStatusCode.InternalServerError, Message = e });
                    });
                }
                response.Responses = list;
            }
            
            return response;
        }

        private List<PatientSystemData> getPatientSystems(List<string> processedPatientSystemIds, IDataDomainRequest request)
        {
            List<PatientSystemData> psData = new List<PatientSystemData>();
            try
            {
                if (processedPatientSystemIds != null && processedPatientSystemIds.Count > 0)
                {
                    GetPatientSystemByIdsDataRequest psRequest = new GetPatientSystemByIdsDataRequest
                    {
                        Ids = processedPatientSystemIds,
                        Context = request.Context,
                        ContractNumber = request.ContractNumber,
                        UserId = request.UserId,
                        Version = request.Version
                    };

                    string DDPatientSystemServiceUrl = ConfigurationManager.AppSettings["DDPatientSystemServiceUrl"];
                    IRestClient client = new JsonServiceClient();
                    //[Route("/{Context}/{Version}/{ContractNumber}/PatientSystems/Ids", "POST")]
                    string url = Helpers.BuildURL(string.Format("{0}/{1}/{2}/{3}/PatientSystems/Ids", DDPatientSystemServiceUrl, psRequest.Context, psRequest.Version, psRequest.ContractNumber), psRequest.UserId);
                    GetPatientSystemByIdsDataResponse dataDomainResponse = client.Post<GetPatientSystemByIdsDataResponse>(url, psRequest as object);
                    if (dataDomainResponse != null)
                    {
                        psData = dataDomainResponse.PatientSystems;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return psData;
        }

        private List<CohortPatientViewData> getMECohortPatientView(List<PatientData> patients)
        {
            List<CohortPatientViewData> list = new List<CohortPatientViewData>();
            foreach(var p in patients)
            { 
                List<SearchFieldData> data = new List<SearchFieldData>();
                data.Add(new SearchFieldData { Active = true, FieldName = Constants.FN, Value = p.FirstName });
                data.Add(new SearchFieldData { Active = true, FieldName = Constants.LN, Value = p.LastName });
                data.Add(new SearchFieldData { Active = true, FieldName = Constants.G, Value = p.Gender.ToUpper() });
                data.Add(new SearchFieldData { Active = true, FieldName = Constants.DOB, Value = p.DOB });
                data.Add(new SearchFieldData { Active = true, FieldName = Constants.MN, Value = p.MiddleName });
                data.Add(new SearchFieldData { Active = true, FieldName = Constants.SFX, Value = p.Suffix });
                data.Add(new SearchFieldData { Active = true, FieldName = Constants.PN, Value = p.PreferredName });
                data.Add(new SearchFieldData { Active = true, FieldName = Constants.PCM }); //value left null on purpose 
                CohortPatientViewData cpvData = new CohortPatientViewData
                {
                    PatientID = p.Id,
                    LastName = p.LastName,
                    SearchFields = data,
                };
                list.Add(cpvData);
            };
            return list;
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
        /// <returns>PatientSystemData object.</returns>
        private PatientSystemData insertEngagePatientSystem(string patientId, IDataDomainRequest request)
        {
            PatientSystemData data = null;
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
                if (dataDomainResponse != null)
                {
                    data = dataDomainResponse.PatientSystemData;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        /// <summary>
        /// Calls PatientSystem data domain to insert bulk Engage System records for the provided patient ids.
        /// </summary>
        /// <param name="request">IDataDomainRequest object</param>
        /// <returns>List of ids of the engage patient systems inserted.</returns>
        private List<string> insertBatchEngagePatientSystem(List<string> patientIds, IDataDomainRequest request)
        {
            List<string> ids = null;
            try
            {
                if(patientIds != null && patientIds.Count > 0)
                {
                    InsertBatchEngagePatientSystemsDataRequest psRequest = new InsertBatchEngagePatientSystemsDataRequest
                    {
                        PatientIds = patientIds,
                        Context = request.Context,
                        ContractNumber = request.ContractNumber,
                        UserId = Constants.SystemContactId,// the requirement says that the engage Id should have createdby user as 'system'.
                        Version = request.Version
                    };

                    string DDPatientSystemServiceUrl = ConfigurationManager.AppSettings["DDPatientSystemServiceUrl"];
                    IRestClient client = new JsonServiceClient();
                    //[Route("/{Context}/{Version}/{ContractNumber}/Batch/Engage/PatientSystems", "POST")]
                    string url = Helpers.BuildURL(string.Format("{0}/{1}/{2}/{3}/Batch/Engage/PatientSystems", DDPatientSystemServiceUrl, psRequest.Context, psRequest.Version, psRequest.ContractNumber), psRequest.UserId);
                    InsertBatchEngagePatientSystemsDataResponse dataDomainResponse = client.Post<InsertBatchEngagePatientSystemsDataResponse>(url, psRequest as object);
                    if (dataDomainResponse != null && dataDomainResponse.Result != null)
                    {
                        ids = dataDomainResponse.Result.ProcessedIds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ids;
        }


        public SyncPatientInfoDataResponse SyncPatient(SyncPatientInfoDataRequest request)
        {
            var response = new SyncPatientInfoDataResponse();
            try
            {
                var repo = Factory.GetRepository(request, RepositoryType.Patient);
                var isSuccessful = repo.SyncPatient(request);

               response.IsSuccessful = isSuccessful;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public AddPCMToCohortPatientViewDataResponse AddPcmToCohortPatientView(AddPCMToCohortPatientViewDataRequest request)
        {
            var response = new AddPCMToCohortPatientViewDataResponse();
            try
            {
                var repo = Factory.GetRepository(request, RepositoryType.CohortPatientView);
                var isSuccessful = repo.AddPCMToPatientCohortView(request);

                response.IsSuccessful = isSuccessful;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
            
        }

        public RemovePCMFromCohortPatientViewDataResponse RemovePcmFromCohortPatientView(RemovePCMFromCohortPatientViewDataRequest request)
        {
            var response = new RemovePCMFromCohortPatientViewDataResponse();
            try
            {
                var repo = Factory.GetRepository(request, RepositoryType.CohortPatientView);
                var isSuccessful = repo.RemovePCMFromCohortPatientView(request);

                response.IsSuccessful = isSuccessful;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public AssignContactsToCohortPatientViewDataResponse AssignContactsToCohortPatientView(AssignContactsToCohortPatientViewDataRequest request)
        {
            var response = new AssignContactsToCohortPatientViewDataResponse();
            try
            {
                var repo = Factory.GetRepository(request, RepositoryType.CohortPatientView);
                var isSuccessful = repo.AddContactsToCohortPatientView(request);

                response.IsSuccessful = isSuccessful;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}   
