using Phytel.API.DataDomain.PatientObservation.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientObservation;
using System;
using Phytel.API.Common.Format;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Phytel.API.DataDomain.PatientObservation.MongoDB.DTO;
using ServiceStack.Common.Web;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientObservation
{
    public class PatientObservationDataManager : IPatientObservationDataManager
    {
        public IPatientObservationRepositoryFactory Factory { get; set; }
        public IObservationUtil Util { get; set; }

        public GetPatientObservationResponse GetPatientObservationByID(GetPatientObservationRequest request)
        {
            try
            {
                GetPatientObservationResponse result = new GetPatientObservationResponse();

                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.PatientObservation);

                PatientObservationData data = repo.FindByObservationID(request.ObservationID, request.PatientId) as PatientObservationData;

                result.PatientObservation = data;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetPatientObservationByID()::" + ex.Message, ex.InnerException);
            }
        }

        public  GetPatientProblemsSummaryResponse GetPatientProblemList(GetPatientProblemsSummaryRequest request)
        {
            try
            {
                GetPatientProblemsSummaryResponse result = new GetPatientProblemsSummaryResponse();

                IPatientObservationRepository oRepo = Factory.GetRepository(request,RepositoryType.Observation);
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.PatientObservation);

                // 1) get all observationIds that are of type problem that are active.
                List<ObservationData> oData = (List<ObservationData>)oRepo.GetObservationsByType("533d8278d433231deccaa62d", null, null);
                
                List<string> oIds = oData.Select(i => i.Id).ToList();

                // 2) find all current patientobservations within these observationids.
                List<PatientObservationData> ol = 
                    ((MongoPatientObservationRepository)repo).GetAllPatientProblems(request, oIds) as List<PatientObservationData>;
                
                result.PatientObservations = ol;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetPatientObservationList()::" + ex.Message, ex.InnerException);
            }
        }

        public  GetStandardObservationsResponse GetStandardObservationsByType(GetStandardObservationsRequest request)
        {
            try
            {
                GetStandardObservationsResponse result = new GetStandardObservationsResponse();

                // get list of observations
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.Observation);

                List<ObservationData> odl = (List<ObservationData>)repo.GetObservationsByType(request.TypeId, true, true);
                List<PatientObservationData> podl = new List<PatientObservationData>();

                // load and initialize each observation
                string initSetId = ObjectId.GenerateNewId().ToString();
                foreach (ObservationData od in odl)
                {
                    PatientObservationData pod = new PatientObservationData
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        ObservationId = od.Id,
                        Name = od.CommonName ?? od.Description,
                        Order = od.Order,
                        Standard = od.Standard,
                        GroupId = od.GroupId,
                        Units = od.Units,
                        Values = new List<ObservationValueData>(),
                        TypeId = od.ObservationTypeId,
                        PatientId = request.PatientId,
                        Source = od.Source
                    };

                    // do an insert here and get an id from mongo
                    ObservationValueData ovd = InitializePatientObservation(request, request.PatientId, pod.Values, od, initSetId);

                    if (od.GroupId != null)
                    {
                        if (Util.GroupExists(podl, od.GroupId))
                        {
                            Util.FindAndInsert(podl, od.GroupId, ovd);
                        }
                        else
                        {
                            podl.Add(pod);
                        }
                    }
                    else
                    {
                        podl.Add(pod);
                    }
                }

                result.StandardObservations = podl;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetStandardObservationsByType()::" + ex.Message, ex.InnerException);
            }
        }

        public  ObservationValueData InitializePatientObservation(IDataDomainRequest request, string patientId, List<ObservationValueData> list, ObservationData od, string initSetId)
        {
            try
            {
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.PatientObservation);

                PutInitializeObservationDataRequest req = new PutInitializeObservationDataRequest
                {
                    PatientId = patientId,
                    ObservationId = od.Id,
                    Context = request.Context,
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version,
                    SetId = initSetId
                };

                ObservationValueData ovd = new ObservationValueData();

                // get last value for each observation data
                GetPreviousValuesForObservation(ovd, patientId, od.Id, request);

                PatientObservationData pod = (PatientObservationData)repo.Initialize(req);

                ovd.Id = pod.Id;
                ovd.Text = od.Description;
                ovd.Value = string.Empty; // changed to initialize the value.

                list.Add(ovd);

                return ovd;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:InitializePatientObservation()::" + ex.Message, ex.InnerException);
            }
        }

        public  void GetPreviousValuesForObservation(ObservationValueData ovd, string patientId, string observationTypeId, IDataDomainRequest request)
        {
            try
            {
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.PatientObservation);
                
                PatientObservationData val = (PatientObservationData)repo.FindRecentObservationValue(observationTypeId, patientId);

                if (val != null)
                {
                    ovd.PreviousValue = new PreviousValueData
                    {
                        EndDate = val.EndDate,
                        Source = val.Source,
                        StartDate = val.StartDate,
                        Unit = val.Units,
                        Value = Util.GetPreviousValues(val.Values)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetPreviousValuesForObservation()::" + ex.Message, ex.InnerException);
            }
        }

        public  GetObservationsDataResponse GetObservationsData(GetObservationsDataRequest request)
        {
            try
            {
                GetObservationsDataResponse response = new GetObservationsDataResponse();
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.Observation);

                List<ObservationData> odl = (List<ObservationData>)repo.GetActiveObservations();
                response.ObservationsData = odl;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetObservationsData()::" + ex.Message, ex.InnerException);
            }
        }

        public bool PutUpdateOfPatientObservationRecord(PutUpdateObservationDataRequest request)
        {
            try
            {
                bool result = false;
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.PatientObservation);

                // update
                if (request.PatientObservationData != null && request.PatientObservationData.Id != null)
                    result = (bool)repo.Update(request);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:PutUpdateOfPatientObservationRecord()::" + ex.Message, ex.InnerException);
            }
        }

        public PutUpdatePatientObservationsDataResponse UpdatePatientObservations(PutUpdatePatientObservationsDataRequest request)
        {
            try
            {
                PutUpdatePatientObservationsDataResponse response = new PutUpdatePatientObservationsDataResponse();
                List<PatientObservationData> returnDataList = null;
                bool result = false;
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.PatientObservation);

                // update
                if (request.PatientObservationsRecordData != null && request.PatientObservationsRecordData.Count != 0)
                {
                    returnDataList = new List<PatientObservationData>();
                    foreach(PatientObservationRecordData poData in request.PatientObservationsRecordData)
                    {
                        if (!string.IsNullOrEmpty(poData.Id))
                        {
                            PutUpdateObservationDataRequest putUpdateObservationDataRequest = new PutUpdateObservationDataRequest { 
                                 Context  = request.Context,
                                 ContractNumber = request.ContractNumber,
                                 PatientId = request.PatientId,
                                 PatientObservationData = poData,
                                 UserId = request.UserId,
                                 Version = request.Version
                            };
                            result = (bool)repo.Update(putUpdateObservationDataRequest);
                            //fetch & return the update object.
                            PatientObservationData pod = (PatientObservationData)repo.FindByID(poData.Id, true);
                            returnDataList.Add(pod);
                        }
                    }
                    IPatientObservationRepository observationRepo = Factory.GetRepository(request, RepositoryType.Observation);
                    List<ObservationData> observations = (List<ObservationData>)observationRepo.GetActiveObservations();
                    if (observations != null && observations.Count > 0)
                    {
                        List<string> distinctObservations = returnDataList.Select(a => a.ObservationId).Distinct().ToList();
                        // added this to take care of composite observations like BP
                        distinctObservations.ForEach(o => CombineCompositeObservations(o, returnDataList));
                        returnDataList.ForEach(r => 
                        {
                            ObservationData odata = observations.Where(x => x.Id == r.ObservationId).FirstOrDefault();
                            if (odata != null)
                            {
                                r.TypeId = odata.ObservationTypeId;
                                r.Name = odata.CommonName == null ? odata.Description : odata.CommonName;
                                r.Standard = odata.Standard;
                            }
                        });
                    }
                }
                response.PatientObservationsData = returnDataList;
                response.Result = result;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:UpdatePatientObservations()::" + ex.Message, ex.InnerException);
            }
        }

        public  GetAdditionalObservationDataItemResponse GetAdditionalObservationItemById(GetAdditionalObservationDataItemRequest request)
        {
            try
            {
                GetAdditionalObservationDataItemResponse result = new GetAdditionalObservationDataItemResponse();
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.Observation);
                
                ObservationData od = (ObservationData)repo.FindByID(request.ObservationId);
                PatientObservationData pod = MakeAdditionalObservation(request, repo, od);

                result.Observation = pod;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetAdditionalObservationItemById()::" + ex.Message, ex.InnerException);
            }
        }

        public  GetAllowedStatesDataResponse GetAllowedStates(GetAllowedStatesDataRequest request)
        {
            try
            {
                GetAllowedStatesDataResponse response = new GetAllowedStatesDataResponse();
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.Observation);
                response.StatesData = repo.GetAllowedObservationStates();
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetAllowedStates()::" + ex.Message, ex.InnerException);
            }
        }

        public  GetInitializeProblemDataResponse GetInitializeProblem(GetInitializeProblemDataRequest request)
        {
            try
            {
                GetInitializeProblemDataResponse response = new GetInitializeProblemDataResponse();
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.PatientObservation);

                PatientObservationData data = (PatientObservationData)repo.InitializeProblem(request);
                response.PatientObservation = data;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetInitializeProblem()::" + ex.Message, ex.InnerException);
            }
        }

        public  PutRegisterPatientObservationResponse PutRegisteredObservation(PutRegisterPatientObservationRequest request)
        {
            try
            {
                PutRegisterPatientObservationResponse response = new PutRegisterPatientObservationResponse();
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.PatientObservation);

                GetInitializeProblemDataRequest req = new GetInitializeProblemDataRequest
                {
                    PatientId = request.PatientId,
                    ObservationId = request.Id,
                    Context = request.Context,
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version,
                    Initial = "false"
                };

                PatientObservationData pod = (PatientObservationData)repo.InitializeProblem(req);

                if (pod != null)
                {
                    response.Outcome = new Outcome { Result = 1, Reason = "Success" };
                    response.PatientObservation = pod;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:InitializePatientObservation()::" + ex.Message, ex.InnerException);
            }
        }

        public  GetCurrentPatientObservationsDataResponse GetCurrentPatientObservations(GetCurrentPatientObservationsDataRequest request)
        {
            try
            {
                GetCurrentPatientObservationsDataResponse result = new GetCurrentPatientObservationsDataResponse();

                IPatientObservationRepository poRepo = Factory.GetRepository(request, RepositoryType.PatientObservation);
                List<PatientObservationData> patientObservations = poRepo.FindObservationIdByPatientId(request.PatientId) as List<PatientObservationData>;

                List<PatientObservationData> currentPOs = null;
                if (patientObservations != null && patientObservations.Count > 0)
                {
                    currentPOs = new List<PatientObservationData>();
                    IPatientObservationRepository observationRepo = Factory.GetRepository(request, RepositoryType.Observation);
                    List<ObservationData> observations = (List<ObservationData>)observationRepo.GetActiveObservations();
                    if (observations != null && observations.Count > 0)
                    {
                        List<string> distinctObservations = patientObservations.Select(a => a.ObservationId).Distinct().ToList();
                        // added this to take care of composite observations like BP
                        distinctObservations.ForEach(o => CombineCompositeObservations(o, patientObservations));

                        distinctObservations.ForEach(a =>
                        {
                            PatientObservationData current = getCurrentPO(patientObservations.Where(s => s.ObservationId == a).ToList());
                            
                            if(current != null)
                            {
                                ObservationData odata = observations.Where(x => x.Id == a).FirstOrDefault();
                                if(odata != null)
                                {
                                    current.TypeId = odata.ObservationTypeId;
                                    current.Name = odata.CommonName == null ? odata.Description : odata.CommonName;
                                    current.Standard = odata.Standard;
                                    currentPOs.Add(current);
                                }
                            }
                        });
                    }
                }
                result.PatientObservationsData = currentPOs;
                return result;
            }
            catch (Exception ex) { throw ex; }
        }

        private PatientObservationData getCurrentPO(List<PatientObservationData> patientObservations)
        {
            List<PatientObservationData> withNullEndDates = patientObservations.Where(s => s.EndDate == null).ToList();
            if (withNullEndDates != null && withNullEndDates.Count > 0)
            {
                return withNullEndDates.OrderByDescending(o => o.LastUpdatedOn).FirstOrDefault();
            }
            else 
            {
                return patientObservations.OrderByDescending(o => o.EndDate).ThenByDescending(o => o.LastUpdatedOn).FirstOrDefault();
            }
        }

        public List<PatientObservationData> GetHistoricalPatientObservations(GetHistoricalPatientObservationsDataRequest request)
        {
            try
            {
                //var currentPOs = new List<PatientObservationData>();
                List<PatientObservationData> po = null;
                var poRepo = Factory.GetRepository(request, RepositoryType.PatientObservation);
                var patientObservations = poRepo.FindObservationIdByPatientId(request.PatientId) as List<PatientObservationData>;
                if (patientObservations == null || patientObservations.Count <= 0) return po;

                var observationRepo = Factory.GetRepository(request, RepositoryType.Observation);
                var observations = (List<ObservationData>)observationRepo.GetActiveObservations();
                if (observations == null || observations.Count <= 0) return po;

                CombineCompositeObservations(request.ObservationId, patientObservations);

                var distinctObservations = patientObservations.Select(a => a.ObservationId).Distinct().ToList();
                
                distinctObservations.ForEach(a =>
                {
                    po = patientObservations.Where(s => s.ObservationId == request.ObservationId).OrderByDescending(o => o.LastUpdatedOn).ToList();

                    po.ForEach(pod =>
                    {
                        var odata = observations.FirstOrDefault(x => x.Id == request.ObservationId);
                        if (odata == null) return;
                        pod.GroupId = odata.GroupId;
                        pod.TypeId = odata.ObservationTypeId;
                        pod.Name = odata.CommonName ?? odata.Description;
                    });
                });

                return po;
            }
            catch (Exception ex) { throw new Exception("DD.DataPatientObservationManager:GetHistoricalPatientObservations()::" + ex.Message, ex.InnerException); }
        }

        public void CombineCompositeObservations(string observationId,
            List<PatientObservationData> patientObservations)
        {
            try
            {
                // handle BP readings if they exist - check request.observationid
                if (observationId.Equals("530c270afe7a592f64473e38"))
                {
                    // Order by lastupdatedOn is very important since it uses that field to group systolic and distolic values.
                    var systol = patientObservations.Where(o => o.ObservationId == "530c270afe7a592f64473e38").OrderBy(p => p.StartDate).ThenBy(o => o.LastUpdatedOn).ToList();
                    var diastol = patientObservations.Where(o => o.ObservationId == "530c26fcfe7a592f64473e37").OrderBy(p => p.StartDate).ThenBy(o => o.LastUpdatedOn).ToList();
                    // Commenting out the code where it trims off milliseconds on LastUpdatedOn, since QA found some PO values that had same seconds values, but different milliseconds values.
                    //diastol.ForEach(d => 
                    //{
                    //    if (d.LastUpdatedOn != null)
                    //    {
                    //        d.LastUpdatedOn = trimMilliseconds((DateTime)d.LastUpdatedOn);
                    //    }
                    //});

                    systol.ForEach(dt =>
                    {
                        dt.Values.First().Text = "Systolic blood pressure";
                        //PatientObservationData selectedDistol = null;
                        List<PatientObservationData> matchingDiastols = diastol.Where(o => o.StartDate == dt.StartDate).ToList();
                        // A systolic value will have atleast one matching distolic value with same start date. If there are more then one, then find a distolic value that has a matching LastUpdatedDateTime. 
                        //if (matchingDiastols.Count == 1)
                        //{
                        //    selectedDistol = matchingDiastols[0];
                        //}
                        //else
                        //{
                        //    selectedDistol = matchingDiastols.Where(o => o.LastUpdatedOn == trimMilliseconds((DateTime)dt.LastUpdatedOn)).FirstOrDefault();
                        //}
                        if (matchingDiastols != null && matchingDiastols.Count > 0)
                        {
                            matchingDiastols[0].Values.First().Text = "Diastolic blood pressure";
                            dt.Values.Add(matchingDiastols[0].Values.First());
                            //Once we have found a systolic-distolic pair, remove the selected dystolic from the list.
                            diastol.Remove(matchingDiastols[0]);
                        }
                    });

                    //once parsed, remove diastol from patientobservations list.
                    patientObservations.RemoveAll(o => o.ObservationId == "530c26fcfe7a592f64473e37");
                }
            }
            catch (Exception ex) { throw new Exception("DD.DataPatientObservationManager:CombineCompositeObservations()::" + ex.Message, ex.InnerException); }
        }

        public DeletePatientObservationByPatientIdDataResponse DeletePatientObservationByPatientId(DeletePatientObservationByPatientIdDataRequest request)
        {
            DeletePatientObservationByPatientIdDataResponse response = null;
            try
            {
                response = new DeletePatientObservationByPatientIdDataResponse();
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.PatientObservation);
                List<PatientObservationData> patientObservations = repo.FindObservationIdByPatientId(request.PatientId) as List<PatientObservationData>;
                List<string> deletedIds = null;
                if (patientObservations != null)
                {
                    deletedIds = new List<string>();
                    patientObservations.ForEach(u =>
                    {
                        //request.Id = u.Id;
                        DeletePatientObservationRequest deletePatientObservationRequest = new DeletePatientObservationRequest { 
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            UserId = request.UserId,
                            Version = request.Version,
                            PatientObservationId = u.Id,
                        };
                        repo.Delete(deletePatientObservationRequest);
                        deletedIds.Add(deletePatientObservationRequest.PatientObservationId);
                    });
                    response.DeletedIds = deletedIds;
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public UndoDeletePatientObservationsDataResponse UndoDeletePatientObservations(UndoDeletePatientObservationsDataRequest request)
        {
            UndoDeletePatientObservationsDataResponse response = null;
            try
            {
                response = new UndoDeletePatientObservationsDataResponse();
                IPatientObservationRepository repo = Factory.GetRepository(request, RepositoryType.PatientObservation);

                if (request.Ids != null && request.Ids.Count > 0)
                {
                    request.Ids.ForEach(u =>
                    {
                        request.PatientObservationId = u;
                        repo.UndoDelete(request);
                    });
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }

        public PatientObservationData MakeAdditionalObservation(GetAdditionalObservationDataItemRequest request, IPatientObservationRepository repo, ObservationData od)
        {
            try
            {
                PatientObservationData pod = CreatePatientObservationData(request, od);
                ObservationValueData ovd = InitializePatientObservation(request, request.PatientId, pod.Values, od, request.SetId);

                // account for composite BP observation
                if (pod.GroupId != null && pod.GroupId.Equals("530cb50dfe7a591ee4a58c51"))
                {
                    string observationId = string.Empty;
                    observationId = od.Id.Equals("530c26fcfe7a592f64473e37") ? "530c270afe7a592f64473e38" : "530c26fcfe7a592f64473e37";
                    ObservationData od2 = (ObservationData)repo.FindByID(observationId);
                    PatientObservationData pod2 = CreatePatientObservationData(request, od2);
                    ObservationValueData ovd2 = InitializePatientObservation(request, request.PatientId, pod.Values, od2, request.SetId);
                }
                return pod;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PatientObservationData CreatePatientObservationData(GetAdditionalObservationDataItemRequest request, ObservationData od)
        {
            PatientObservationData pod = new PatientObservationData
            {
                Id = ObjectId.GenerateNewId().ToString(),
                ObservationId = od.Id,
                Name = od.CommonName ?? od.Description,
                Order = od.Order,
                Standard = od.Standard,
                GroupId = od.GroupId,
                Units = od.Units,
                Values = new List<ObservationValueData>(),
                TypeId = od.ObservationTypeId,
                PatientId = request.PatientId,
                Source = od.Source
            };
            return pod;
        }

        private DateTime trimMilliseconds(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0);
        }
    }
}   
