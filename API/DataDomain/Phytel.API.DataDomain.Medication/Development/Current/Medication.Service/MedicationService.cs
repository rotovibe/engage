using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Service
{
    public class MedicationService : ServiceBase
    {
        protected readonly IMedicationDataManager Manager;

        public MedicationService(IMedicationDataManager mgr)
        {
            Manager = mgr;
        }

        public GetAllMedicationsResponse Get(GetAllMedicationsRequest request)
        {
            var response = new GetAllMedicationsResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response.Medications = Manager.GetMedicationList(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public PutBulkInsertMedicationsResponse Put(PutBulkInsertMedicationsRequest request)
        {
            var response = new PutBulkInsertMedicationsResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                //response.Medications = 
                    var result = Manager.BulkInsertMedications(new List<DTO.MedicationData>(), request);
                response.Status = result;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        public GetMedicationDetailsDataResponse Post(GetMedicationDetailsDataRequest request)
        {
            var response = new GetMedicationDetailsDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response = Manager.GetMedicationDetails(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}