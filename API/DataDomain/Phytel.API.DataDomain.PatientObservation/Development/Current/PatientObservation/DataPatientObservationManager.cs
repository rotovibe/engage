using Phytel.API.DataDomain.PatientObservation.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.PatientObservation;
using System;
using Phytel.API.Common.Format;

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
    }
}   
