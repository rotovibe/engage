using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Service
{
    public class MedicationService : ServiceBase
    {
        //protected readonly IMedicationDataManager Manager;
        public IMedicationDataManager Manager { get; set; }

        //public MedicationService(IMedicationDataManager mgr)
        //{
        //    Manager = mgr;
        //}

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

        public GetMedicationNDCsDataResponse Post(GetMedicationNDCsDataRequest request)
        {
            var response = new GetMedicationNDCsDataResponse { Version = request.Version };
            try
            {
                RequireUserId(request);
                response = Manager.GetMedicationNDCs(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }

        #region Initialize
        public PutInitializeMedSuppDataResponse Put(PutInitializeMedSuppDataRequest request)
        {
            PutInitializeMedSuppDataResponse response = new PutInitializeMedSuppDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.MedicationData = Manager.InitializeMedication(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
        #endregion

        #region Puts
        public PutMedicationDataResponse Put(PutMedicationDataRequest request)
        {
            PutMedicationDataResponse response = new PutMedicationDataResponse { Version = request.Version };

            try
            {
                RequireUserId(request);
                response.MedicationData = Manager.UpdateMedication(request);
            }
            catch (Exception ex)
            {
                RaiseException(response, ex);
            }
            return response;
        }
        #endregion
    }
}