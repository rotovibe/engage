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
                response.Medications = Manager.GetMedicationList(request.UserId, request.ContractNumber);
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
                    var result = Manager.BulkInsertMedications(new List<DTO.MedicationData>(), request.UserId, request.ContractNumber);
                response.Status = result;
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
    }
}