using System;
using System.Net;
using Phytel.API.DataDomain.PatientObservation;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.Common.Format;
using ServiceStack.Common.Web;

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
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
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
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
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
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public static PutInitializeObservationDataResponse InsertNewPatientObservation(PutInitializeObservationDataRequest request)
        {
            try
            {
                PutInitializeObservationDataResponse result = new PutInitializeObservationDataResponse();

                IPatientObservationRepository<PutInitializeObservationDataResponse> repo = PatientObservationRepositoryFactory<PutInitializeObservationDataResponse>.GetPatientObservationRepository(request.ContractNumber, request.Context);

                result.ObservationData = (PatientObservationData)repo.Initialize(request);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetAdditionalObservationsResponse Post(GetAdditionalObservationsResponse request)
        {
            GetAdditionalObservationsResponse response = new GetAdditionalObservationsResponse();
            try
            {
                response = PatientObservationDataManager.GetAdditionalObservationsByType(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
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
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
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
                //TODO: Log this to C3 database via ASE
                string ercode = ((HttpError)ex).ErrorCode;
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus(ercode, ex.Message);
            }
            return response;
        }
    }
}