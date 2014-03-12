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

namespace Phytel.API.DataDomain.PatientObservation
{
    public static class PatientObservationDataManager
    {
        public static GetPatientObservationResponse GetPatientObservationByID(GetPatientObservationRequest request)
        {
            try
            {
                GetPatientObservationResponse result = new GetPatientObservationResponse();

                IPatientObservationRepository<GetPatientObservationResponse> repo = PatientObservationRepositoryFactory<GetPatientObservationResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;
                result = repo.FindByID(request.PatientObservationID) as GetPatientObservationResponse;

                // if cross-domain service call has error
                //if (result.Status != null)
                //{
                //    throw new ArgumentException(result.Status.Message, new Exception() { Source = result.Status.StackTrace });
                //}

                return (result != null ? result : new GetPatientObservationResponse());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GetAllPatientObservationsResponse GetPatientObservationList(GetAllPatientObservationsRequest request)
        {
            try
            {
                GetAllPatientObservationsResponse result = new GetAllPatientObservationsResponse();

                IPatientObservationRepository<GetAllPatientObservationsResponse> repo = PatientObservationRepositoryFactory<GetAllPatientObservationsResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;
                
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static GetStandardObservationsResponse GetStandardObservationsByType(GetStandardObservationsRequest request)
        {
            try
            {
                GetStandardObservationsResponse result = new GetStandardObservationsResponse();

                // get list of observations
                IPatientObservationRepository<GetStandardObservationsResponse> repo = PatientObservationRepositoryFactory<GetStandardObservationsResponse>.GetObservationRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;
                List<ObservationData> odl = (List<ObservationData>)repo.GetObservationsByType(request.TypeId, true);
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
                        TypeId = od.ObservationType,
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
                throw ex;
            }
        }

        public static ObservationValueData InitializePatientObservation(IDataDomainRequest request, string patientId, List<ObservationValueData> list, ObservationData od, string initSetId)
        {
            try
            {
                IPatientObservationRepository<GetStandardObservationsResponse> repo = PatientObservationRepositoryFactory<GetStandardObservationsResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;
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

                list.Add(ovd);

                return ovd;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void GetPreviousValuesForObservation(ObservationValueData ovd, string patientId, string observationTypeId, string context, string contract, string userId)
        {
            try
            {
                IPatientObservationRepository<GetStandardObservationsResponse> repo =
                    PatientObservationRepositoryFactory<GetStandardObservationsResponse>.GetPatientObservationRepository(contract, context);

                repo.UserId = userId;
                
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
                throw ex;
            }
        }

        public static GetAdditionalLibraryObservationsResponse GetAdditionalObservationsLibraryByType(GetAdditionalLibraryObservationsRequest request)
        {
            try
            {
                GetAdditionalLibraryObservationsResponse response = new GetAdditionalLibraryObservationsResponse();
                List<ObservationLibraryItemData> oli = new List<ObservationLibraryItemData>();
                IPatientObservationRepository<GetAdditionalLibraryObservationsResponse> repo = 
                    PatientObservationRepositoryFactory<GetAdditionalLibraryObservationsResponse>.GetObservationRepository(request.ContractNumber, request.Context);

                repo.UserId = request.UserId;
                
                List<ObservationData> odl = (List<ObservationData>)repo.GetObservationsByType(request.TypeId, false);

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
                throw ex;
            }
        }

        public static bool PutUpdateOfPatientObservationRecord(PutUpdateObservationDataRequest request)
        {
            try
            {
                bool result = false;
                IPatientObservationRepository<PutUpdateObservationDataResponse> repo =
                    PatientObservationRepositoryFactory<PutUpdateObservationDataResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;
                
                //List<PatientObservationData> pod = (List<PatientObservationData>)repo.FindObservationIdByPatientId(request.PatientId);
                //List<string> dbPatientObservationIdList = ObservationUtil.GetPatientObservationIds(pod);

                //// update existing patientobservation entries with a delete
                //List<string> excludes = dbPatientObservationIdList.Except(request.PatientObservationIdsList).ToList<string>();

                //if (excludes != null && excludes.Count > 0)
                //{
                //    excludes.ForEach(ex =>
                //    {
                //        // create delete patientobservation request
                //        DeletePatientObservationRequest dpo = new DeletePatientObservationRequest { PatientObservationId = ex, SetId = request.PatientObservationData.SetId, PatientId = request.PatientId, UserId = request.UserId };
                //        repo.Delete(dpo);
                //    });
                //}

                // update
                if (request.PatientObservationData != null && request.PatientObservationData.Id != null)
                    result = (bool)repo.Update(request);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static GetAdditionalObservationDataItemResponse GetAdditionalObservationItemById(GetAdditionalObservationDataItemRequest request)
        {
            try
            {
                GetAdditionalObservationDataItemResponse result = new GetAdditionalObservationDataItemResponse();
                IPatientObservationRepository<GetAdditionalObservationDataItemResponse> repo = PatientObservationRepositoryFactory<GetAdditionalObservationDataItemResponse>.GetObservationRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;
                
                ObservationData od = (ObservationData)repo.FindByID(request.ObservationId);
                PatientObservationData pod = ObservationUtil.MakeAdditionalObservation(request, repo, od);

                result.Observation = pod;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
