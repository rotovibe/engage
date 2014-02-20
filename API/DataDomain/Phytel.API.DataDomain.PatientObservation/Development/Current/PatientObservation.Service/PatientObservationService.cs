using System;
using System.Net;
using Phytel.API.DataDomain.PatientObservation;
using Phytel.API.DataDomain.PatientObservation.DTO;
using Phytel.API.Common.Format;

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
    }
}