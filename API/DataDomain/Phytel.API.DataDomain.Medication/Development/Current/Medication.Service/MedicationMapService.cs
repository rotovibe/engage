using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Service
{
    public class MedicationMapService : ServiceBase
    {
        public IMedicationMappingDataManager Manager { get; set; }

        public PutInsertMedicationMappingResponse Put(PutInsertMedicationMappingRequest request)
        {
            var response = new PutInsertMedicationMappingResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                var result = Manager.InsertMedicationMapping(request, request.MedicationMapping);
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
    }
}