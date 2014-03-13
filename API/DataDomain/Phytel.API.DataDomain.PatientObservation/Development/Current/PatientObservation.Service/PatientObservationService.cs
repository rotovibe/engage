using Phytel.API.Common.Format;
using Phytel.API.DataDomain.PatientObservation.DTO;
using System;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.PatientObservation.Service
{
    public class PatientObservationService : ServiceStack.ServiceInterface.Service
    {
        private const string _phytelUserIDToken = "x-Phytel-UserID";

        public GetPatientObservationResponse Post(GetPatientObservationRequest request)
        {
            GetPatientObservationResponse response = new GetPatientObservationResponse();
            try
            {
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
                IPatientObservationRepository<PutInitializeObservationDataResponse> repo = PatientObservationRepositoryFactory<PutInitializeObservationDataResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context, request.UserId);

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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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
                //Get the UserId from the Header and update the request object
                request.UserId = HttpContext.Current.Request.Headers.Get(_phytelUserIDToken);
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