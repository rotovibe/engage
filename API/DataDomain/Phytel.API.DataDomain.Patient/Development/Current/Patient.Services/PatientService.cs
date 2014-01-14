using Phytel.API.Common.Format;
using Phytel.API.DataDomain.Patient;
using Phytel.API.DataDomain.Patient.DTO;
using System;
using System.Net;

namespace Phytel.API.DataDomain.Patient.Service
{
    public class PatientService : ServiceStack.ServiceInterface.Service
    {
        public GetPatientDataResponse Get(GetPatientDataRequest request)
        {
            GetPatientDataResponse response = new GetPatientDataResponse();
            try
            {
                response = PatientDataManager.GetPatientByID(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            return response;
        }

        public GetPatientsDataResponse Post(GetPatientsDataRequest request)
        {
            GetPatientsDataResponse response = new GetPatientsDataResponse();
            try
            {
                response = PatientDataManager.GetPatients(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            return response;
        }

        public PutUpdatePatientDataResponse Put(PutUpdatePatientDataRequest request)
        {
            PutUpdatePatientDataResponse response = new PutUpdatePatientDataResponse();
            try
            {
                response = PatientDataManager.UpdatePatient(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutPatientDataResponse Put(PutPatientDataRequest request)
        {
            PutPatientDataResponse response = new PutPatientDataResponse();
            try
            {
                response = PatientDataManager.InsertPatient(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public PutPatientPriorityResponse Put(PutPatientPriorityRequest request)
        {
            PutPatientPriorityResponse response = new PutPatientPriorityResponse();
            try
            {
                response = PatientDataManager.UpdatePatientPriority(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            return response;
        }

        public PutPatientFlaggedResponse Put(PutPatientFlaggedRequest request)
        {
            PutPatientFlaggedResponse response = new PutPatientFlaggedResponse();
            try
            {
                response = PatientDataManager.UpdatePatientFlagged(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            return response;
        }

        public GetCohortPatientsDataResponse Get(GetCohortPatientsDataRequest request)
        {
            GetCohortPatientsDataResponse response = new GetCohortPatientsDataResponse();

            try
            {
                response = PatientDataManager.GetCohortPatients(request);
                response.Version = request.Version;
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            }
            return response;
        }

    }
}