using Phytel.API.Common.Format;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.DataDomain.PatientObservation.MongoDB.DTO;
using System;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.PatientObservation.Service
{
    public class PatientObservationService : ServiceStack.ServiceInterface.Service
    {
        public IPatientObservationDataManager Omgr { get; set; }

        public GetPatientObservationResponse Post(GetPatientObservationRequest request)
        {
            GetPatientObservationResponse response = new GetPatientObservationResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Post()::Unauthorized Access");

                response = Omgr.GetPatientObservationByID(request);
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

                response = Omgr.GetPatientObservationByID(request);
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

                response = Omgr.GetPatientProblemList(request);
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

                IPatientObservationRepository repo = new PatientObservationRepositoryFactory().GetRepository(request, RepositoryType.PatientObservation);

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

                response = Omgr.GetStandardObservationsByType(request);
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

                response = Omgr.GetAdditionalObservationItemById(request);
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

        public GetObservationsDataResponse Get(GetObservationsDataRequest request)
        {
            GetObservationsDataResponse response = new GetObservationsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Get()::Unauthorized Access");

                response = Omgr.GetObservationsData(request);
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

                response.Result = Omgr.PutUpdateOfPatientObservationRecord(request);
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

        public PutUpdatePatientObservationsDataResponse Put(PutUpdatePatientObservationsDataRequest request)
        {
            PutUpdatePatientObservationsDataResponse response = new PutUpdatePatientObservationsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Put()::Unauthorized Access");

                response = Omgr.UpdatePatientObservations(request);
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

                response.Result = Omgr.PutUpdateOfPatientObservationRecord(request);
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

                Omgr.PutRegisteredObservation(request);
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

                response = Omgr.GetAllowedStates(request);
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

                response = Omgr.GetInitializeProblem(request);
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

        public DeletePatientObservationByPatientIdDataResponse Delete(DeletePatientObservationByPatientIdDataRequest request)
        {
            DeletePatientObservationByPatientIdDataResponse response = new DeletePatientObservationByPatientIdDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:PatientObservatioDelete()::Unauthorized Access");

                response = Omgr.DeletePatientObservationByPatientId(request);
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

        public UndoDeletePatientObservationsDataResponse Put(UndoDeletePatientObservationsDataRequest request)
        {
            UndoDeletePatientObservationsDataResponse response = new UndoDeletePatientObservationsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:PatientObservatioUndoDelete()::Unauthorized Access");

                response = Omgr.UndoDeletePatientObservations(request);
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

        public GetCurrentPatientObservationsDataResponse Get(GetCurrentPatientObservationsDataRequest request)
        {
            GetCurrentPatientObservationsDataResponse response = new GetCurrentPatientObservationsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Get()::Unauthorized Access");

                response = Omgr.GetCurrentPatientObservations(request);
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

        public GetHistoricalPatientObservationsDataResponse Get(GetHistoricalPatientObservationsDataRequest request)
        {
            GetHistoricalPatientObservationsDataResponse response = new GetHistoricalPatientObservationsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("PatientObservationDD:Get()::Unauthorized Access");

                var data = Omgr.GetHistoricalPatientObservations(request);
                response.PatientObservationsData = data;
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                var aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
    }
}