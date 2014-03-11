using Phytel.API.Common.Format;
using Phytel.API.DataDomain.PatientObservation.DTO;
using System;
using System.Configuration;

namespace Phytel.API.DataDomain.PatientObservation.Service
{
    public class PatientObservationService : ServiceStack.ServiceInterface.Service
    {
        public GetPatientObservationResponse Post(GetPatientObservationRequest request)
        {
            GetPatientObservationResponse response = new GetPatientObservationResponse();
            try
            {
                response = PatientObservationDataManager.GetPatientObservationByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetPatientObservationResponse Get(GetPatientObservationRequest request)
        {
            GetPatientObservationResponse response = new GetPatientObservationResponse();
            try
            {
                response = PatientObservationDataManager.GetPatientObservationByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAllPatientObservationsResponse Post(GetAllPatientObservationsRequest request)
        {
            GetAllPatientObservationsResponse response = new GetAllPatientObservationsResponse();
            try
            {
                response = PatientObservationDataManager.GetPatientObservationList(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutInitializeObservationDataResponse InsertNewPatientObservation(PutInitializeObservationDataRequest request)
        {
            PutInitializeObservationDataResponse response = new PutInitializeObservationDataResponse();

            try
            {
                IPatientObservationRepository<PutInitializeObservationDataResponse> repo = PatientObservationRepositoryFactory<PutInitializeObservationDataResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context);

                response.ObservationData = (PatientObservationData)repo.Initialize(request);
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetStandardObservationsResponse Get(GetStandardObservationsRequest request)
        {
            GetStandardObservationsResponse response = new GetStandardObservationsResponse();
            try
            {
                response = PatientObservationDataManager.GetStandardObservationsByType(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAdditionalObservationDataItemResponse Post(GetAdditionalObservationDataItemRequest request)
        {
            GetAdditionalObservationDataItemResponse response = new GetAdditionalObservationDataItemResponse();
            try
            {
                response = PatientObservationDataManager.GetAdditionalObservationItemById(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public GetAdditionalLibraryObservationsResponse Get(GetAdditionalLibraryObservationsRequest request)
        {
            GetAdditionalLibraryObservationsResponse response = new GetAdditionalLibraryObservationsResponse();
            try
            {
                response = PatientObservationDataManager.GetAdditionalObservationsLibraryByType(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateObservationDataResponse Put(PutUpdateObservationDataRequest request)
        {
            PutUpdateObservationDataResponse response = new PutUpdateObservationDataResponse();
            try
            {
                response.Result = PatientObservationDataManager.PutUpdateOfPatientObservationRecord(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }

        public PutUpdateObservationDataResponse Post(PutUpdateObservationDataRequest request)
        {
            PutUpdateObservationDataResponse response = new PutUpdateObservationDataResponse();
            try
            {
                response.Result = PatientObservationDataManager.PutUpdateOfPatientObservationRecord(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
    }
}