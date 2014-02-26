using Phytel.API.DataDomain.PatientObservation.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientObservation;
using System;
using Phytel.API.Common.Format;
using System.Collections.Generic;
using System.Linq;

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
                List<ObservationData> odl = (List<ObservationData>)repo.GetObservationsByType(request.TypeId, true);
                List<PatientObservationData> podl = new List<PatientObservationData>();

                // load and initialize each observation
                foreach (ObservationData od in odl)
                {
                    PatientObservationData pod = new PatientObservationData
                    {
                        Id = od.Id,
                        Name = od.CommonName != null ? od.CommonName : od.Description,
                        Order = od.Order,
                        Standard = od.Standard,
                        GroupId = od.GroupId,
                        Units = od.Units,
                        Values = new List<ObservationValueData>(),
                        TypeId = od.ObservationType,
                         PatientId = request.PatientId
                    };

                    // do an insert here and get an id from mongo
                    ObservationValueData ovd = InitializePatientObservation(request, pod.Values, od);

                    if (od.GroupId != null)
                    {
                        if (GroupExists(podl, od.GroupId))
                        {
                            FindAndInsert(podl, od.GroupId, ovd);
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

        private static void FindAndInsert(List<PatientObservationData> podl, string gid, ObservationValueData ovd)
        {
            try
            {
                if (podl != null && podl.Count > 0)
                {
                    foreach (PatientObservationData p in podl)
                    {
                        if (p.GroupId != null)
                        {
                            if (p.GroupId.Equals(gid))
                            {
                                if (!p.Values.Exists(x => x.Id == ovd.Id)) p.Values.Add(ovd);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static bool GroupExists(List<PatientObservationData> list, string gid)
        {
            bool result = false;
            try
            {
                if (list != null && list.Count > 0)
                {
                    foreach (PatientObservationData p in list)
                    {
                        if (p.GroupId != null)
                        {
                            if (p.GroupId.Equals(gid))
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static ObservationValueData InitializePatientObservation(GetStandardObservationsRequest request, List<ObservationValueData> list, ObservationData od)
        {
            try
            {
                IPatientObservationRepository<GetStandardObservationsResponse> repo = PatientObservationRepositoryFactory<GetStandardObservationsResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context);
                PutInitializeObservationDataRequest req = new PutInitializeObservationDataRequest
                {
                    PatientId = request.PatientId,
                    ObservationId = od.Id,
                    Context = request.Context,
                    ContractNumber = request.ContractNumber,
                    UserId = request.UserId,
                    Version = request.Version
                };

                PatientObservationData pod = (PatientObservationData)repo.Initialize(req);

                ObservationValueData ovd = new ObservationValueData
                {
                    Id = pod.Id,
                    Text = od.Description
                };

                list.Add(ovd);

                return ovd;
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
    }
}   
