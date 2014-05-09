using Phytel.API.Common.Format;
using Phytel.API.DataDomain.PatientObservation.DTO;
using System;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.PatientObservation.Service
{
    public class PatientObservationService : ServiceStack.ServiceInterface.Service
    {
        public GetPatientObservationResponse Post(GetPatientObservationRequest request)
        {
            GetPatientObservationResponse response = new GetPatientObservationResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Post()::Unauthorized Access");

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
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Get()::Unauthorized Access");

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

        public GetPatientProblemsSummaryResponse Get(GetPatientProblemsSummaryRequest request)
        {
            GetPatientProblemsSummaryResponse response = new GetPatientProblemsSummaryResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Post()::Unauthorized Access");

                response = PatientObservationDataManager.GetPatientProblemList(request);
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
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Insert()");

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
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Get()::Unauthorized Access");

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
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Post()::Unauthorized Access");

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
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Get()::Unauthorized Access");

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
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Put()::Unauthorized Access");

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
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Post()::Unauthorized Access");

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

        public PutRegisterPatientObservationResponse Put(PutRegisterPatientObservationRequest request)
        {
            PutRegisterPatientObservationResponse response = new PutRegisterPatientObservationResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Put()::Unauthorized Access");

                PatientObservationDataManager.PutRegisteredObservation(request);
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

        public GetAllowedStatesDataResponse Get(GetAllowedStatesDataRequest request)
        {
            GetAllowedStatesDataResponse response = new GetAllowedStatesDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Get()::Unauthorized Access");

                response = PatientObservationDataManager.GetAllowedStates(request);
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

        public GetInitializeProblemDataResponse Get(GetInitializeProblemDataRequest request)
        {
            GetInitializeProblemDataResponse response = new GetInitializeProblemDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Get()::Unauthorized Access");

                response = PatientObservationDataManager.GetInitializeProblem(request);
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