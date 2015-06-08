using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.Medication.DTO.Request;
using Phytel.API.DataDomain.Medication.DTO.Response;

namespace Phytel.API.DataDomain.Medication.Service
{
    public class MedicationMapService : ServiceBase
    {
        public IMedicationMappingDataManager Manager { get; set; }

        public GetMedicationMapDataResponse Post(GetMedicationMapDataRequest request)
        {
            var response = new GetMedicationMapDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response.MedicationMapsData = Manager.GetMedicationMap(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public PostMedicationMapDataResponse Post(PostMedicationMapDataRequest request)
        {
            var response = new PostMedicationMapDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                var result = Manager.InsertMedicationMap(request);
                response.MedMapData = result;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public PutInitializeMedicationMapDataResponse Put(PutInitializeMedicationMapDataRequest request)
        {
            PutInitializeMedicationMapDataResponse response = new PutInitializeMedicationMapDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.MedicationMappingData = Manager.InitializeMedicationMap(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public PutMedicationMapDataResponse Put(PutMedicationMapDataRequest request)
        {
            PutMedicationMapDataResponse response = new PutMedicationMapDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response.MedicationMappingData = Manager.UpdateMedicationMap(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public PutDeleteMedMapDataResponse Put(PutDeleteMedMapDataRequest request)
        {
            PutDeleteMedMapDataResponse response = new PutDeleteMedMapDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                if (Manager.DeleteMedicationMaps(request))
                    response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus
                    {
                        Message = "Success"
                    };
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}