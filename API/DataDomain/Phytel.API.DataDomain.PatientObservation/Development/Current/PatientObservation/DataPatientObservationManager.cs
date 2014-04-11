using Phytel.API.DataDomain.PatientObservation.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientObservation;
using System;
using Phytel.API.Common.Format;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using ServiceStack.Common.Web;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientObservation
{
    public static class PatientObservationDataManager
    {
        public static GetPatientObservationResponse GetPatientObservationByID(GetPatientObservationRequest request)
        {
            try
            {
                GetPatientObservationResponse result = new GetPatientObservationResponse();

                IPatientObservationRepository<GetPatientObservationResponse> repo = PatientObservationRepositoryFactory<GetPatientObservationResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context, request.UserId);

                PatientObservationData data = ((MongoPatientObservationRepository<GetPatientObservationResponse>)repo).FindByObservationID(request.ObservationID, request.PatientId) as PatientObservationData;

                result.PatientObservation = data;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetPatientObservationByID()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetPatientProblemsSummaryResponse GetPatientProblemList(GetPatientProblemsSummaryRequest request)
        {
            try
            {
                GetPatientProblemsSummaryResponse result = new GetPatientProblemsSummaryResponse();

                IPatientObservationRepository<GetPatientProblemsSummaryResponse> oRepo =
                    PatientObservationRepositoryFactory<GetPatientProblemsSummaryResponse>.GetObservationRepository(request.ContractNumber, request.Context, request.UserId);
                IPatientObservationRepository<GetPatientProblemsSummaryResponse> repo = 
                    PatientObservationRepositoryFactory<GetPatientProblemsSummaryResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context, request.UserId);

                // 1) get all observationIds that are of type problem that are active.
                List<ObservationData> oData = (List<ObservationData>)oRepo.GetObservationsByType("533d8278d433231deccaa62d", null, null);
                
                List<string> oIds = oData.Select(i => i.Id).ToList();

                // 2) find all current patientobservations within these observationids.
                List<PatientObservationData> ol = 
                    ((MongoPatientObservationRepository<GetPatientProblemsSummaryResponse>)repo).GetAllPatientProblems(request, oIds) as List<PatientObservationData>;
                
                result.PatientObservations = ol;
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetPatientObservationList()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetStandardObservationsResponse GetStandardObservationsByType(GetStandardObservationsRequest request)
        {
            try
            {
                GetStandardObservationsResponse result = new GetStandardObservationsResponse();

                // get list of observations
                IPatientObservationRepository<GetStandardObservationsResponse> repo = PatientObservationRepositoryFactory<GetStandardObservationsResponse>.GetObservationRepository(request.ContractNumber, request.Context, request.UserId);

                List<ObservationData> odl = (List<ObservationData>)repo.GetObservationsByType(request.TypeId, true, null);
                List<PatientObservationData> podl = new List<PatientObservationData>();

                // load and initialize each observation
                string initSetId = ObjectId.GenerateNewId().ToString();
                foreach (ObservationData od in odl)
                {
                    PatientObservationData pod = new PatientObservationData
                    {
                        Id = ObjectId.GenerateNewId().ToString(),
                        ObservationId = od.Id,
                        Name = od.CommonName != null ? od.CommonName : od.Description,
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
                        if (ObservationUtil.GroupExists(podl, od.GroupId))
                        {
                            ObservationUtil.FindAndInsert(podl, od.GroupId, ovd);
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

        public static ObservationValueData InitializePatientObservation(IDataDomainRequest request, string patientId, List<ObservationValueData> list, ObservationData od, string initSetId)
        {
            try
            {
                IPatientObservationRepository<GetStandardObservationsResponse> repo = PatientObservationRepositoryFactory<GetStandardObservationsResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context, request.UserId);

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
                GetPreviousValuesForObservation(ovd, patientId, od.Id, request.Context, request.ContractNumber, request.UserId);

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

        private static void GetPreviousValuesForObservation(ObservationValueData ovd, string patientId, string observationTypeId, string context, string contract, string userId)
        {
            try
            {
                IPatientObservationRepository<GetStandardObservationsResponse> repo =
                    PatientObservationRepositoryFactory<GetStandardObservationsResponse>.GetPatientObservationRepository(contract, context, userId);
                
                PatientObservationData val = (PatientObservationData)repo.FindRecentObservationValue(observationTypeId, patientId);

                if (val != null)
                {
                    ovd.PreviousValue = new PreviousValueData
                    {
                        EndDate = val.EndDate,
                        Source = val.Source,
                        StartDate = val.StartDate,
                        Unit = val.Units,
                        Value = ObservationUtil.GetPreviousValues(val.Values)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetPreviousValuesForObservation()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetAdditionalLibraryObservationsResponse GetAdditionalObservationsLibraryByType(GetAdditionalLibraryObservationsRequest request)
        {
            try
            {
                GetAdditionalLibraryObservationsResponse response = new GetAdditionalLibraryObservationsResponse();
                List<ObservationLibraryItemData> oli = new List<ObservationLibraryItemData>();
                IPatientObservationRepository<GetAdditionalLibraryObservationsResponse> repo =
                    PatientObservationRepositoryFactory<GetAdditionalLibraryObservationsResponse>.GetObservationRepository(request.ContractNumber, request.Context, request.UserId);
                
                List<ObservationData> odl = (List<ObservationData>)repo.GetObservationsByType(request.TypeId, false, true);

                odl.ForEach(o =>
                {
                    oli.Add(new ObservationLibraryItemData
                    {
                        Id = o.Id,
                        Name = o.CommonName != null ? o.CommonName : o.Description
                    });
                });

                response.Library = oli;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetAdditionalObservationsLibraryByType()::" + ex.Message, ex.InnerException);
            }
        }

        public static bool PutUpdateOfPatientObservationRecord(PutUpdateObservationDataRequest request)
        {
            try
            {
                bool result = false;
                IPatientObservationRepository<PutUpdateObservationDataResponse> repo =
                    PatientObservationRepositoryFactory<PutUpdateObservationDataResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context, request.UserId);

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

        public static GetAdditionalObservationDataItemResponse GetAdditionalObservationItemById(GetAdditionalObservationDataItemRequest request)
        {
            try
            {
                GetAdditionalObservationDataItemResponse result = new GetAdditionalObservationDataItemResponse();
                IPatientObservationRepository<GetAdditionalObservationDataItemResponse> repo = PatientObservationRepositoryFactory<GetAdditionalObservationDataItemResponse>.GetObservationRepository(request.ContractNumber, request.Context, request.UserId);
                
                ObservationData od = (ObservationData)repo.FindByID(request.ObservationId);
                PatientObservationData pod = ObservationUtil.MakeAdditionalObservation(request, repo, od);

                result.Observation = pod;

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetAdditionalObservationItemById()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetAllowedStatesDataResponse GetAllowedStates(GetAllowedStatesDataRequest request)
        {
            try
            {
                GetAllowedStatesDataResponse response = new GetAllowedStatesDataResponse();
                IPatientObservationRepository<GetAllowedStatesDataResponse> repo =
                    PatientObservationRepositoryFactory<GetAllowedStatesDataResponse>.GetObservationRepository(request.ContractNumber, request.Context, request.UserId);

                List<IdNamePair> allowedStates = repo.GetAllowedObservationStates(request);
                response.StatesData = allowedStates;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetAllowedStates()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetInitializeProblemDataResponse GetInitializeProblem(GetInitializeProblemDataRequest request)
        {
            try
            {
                GetInitializeProblemDataResponse response = new GetInitializeProblemDataResponse();
                IPatientObservationRepository<GetInitializeProblemDataResponse> repo =
                    PatientObservationRepositoryFactory<GetInitializeProblemDataResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context, request.UserId);

                PatientObservationData data = (PatientObservationData)repo.InitializeProblem(request);
                response.PatientObservation = data;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD.DataPatientObservationManager:GetInitializeProblem()::" + ex.Message, ex.InnerException);
            }
        }


        public static PutRegisterPatientObservationResponse PutRegisteredObservation(PutRegisterPatientObservationRequest request)
        {
            try
            {
                PutRegisterPatientObservationResponse response = new PutRegisterPatientObservationResponse();
                IPatientObservationRepository<GetStandardObservationsResponse> repo = PatientObservationRepositoryFactory<GetStandardObservationsResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context, request.UserId);

                GetInitializeProblemDataRequest req = new GetInitializeProblemDataRequest
                {
                    PatientId = request.PatientId,
                    ObservationId = request.Id,
                    Context = request.Context,
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version,
                    Initial = false
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
    }
}   
