using Phytel.API.DataDomain.Patient;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack;
using System;
using System.Net;

namespace Phytel.API.DataDomain.Patient.Service
{
    public class PatientService : ServiceStack.Service
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
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ResponseStatus("Exception", ex.Message);
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
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ResponseStatus("Exception", ex.Message);
            }
            return response;
        }
    }
}