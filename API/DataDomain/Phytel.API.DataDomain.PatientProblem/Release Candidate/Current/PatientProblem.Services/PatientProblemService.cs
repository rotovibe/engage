using System;
using System.Collections.Generic;
using System.Net;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;

namespace Phytel.API.DataDomain.PatientProblem.Service
{
    public class PatientProblemService : ServiceStack.ServiceInterface.Service
    {
        public GetAllPatientProblemsDataResponse Get(GetAllPatientProblemsDataRequest request)
        {
            GetAllPatientProblemsDataResponse response = new GetAllPatientProblemsDataResponse();
            try
            {
                response = DataPatientProblemManager.GetAllPatientProblem(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;

        }

        public GetPatientProblemsDataResponse Get(GetPatientProblemsDataRequest request)
        {
            GetPatientProblemsDataResponse response = new GetPatientProblemsDataResponse();
            try
            {
                response = DataPatientProblemManager.GetPatientProblem(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;

        }

        public PutNewPatientProblemResponse Put(PutNewPatientProblemRequest request)
        {
            PutNewPatientProblemResponse response = new PutNewPatientProblemResponse();
            try
            {
                response = DataPatientProblemManager.PutPatientProblem(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;

        }

        public PutUpdatePatientProblemResponse Put(PutUpdatePatientProblemRequest request)
        {
            PutUpdatePatientProblemResponse response = new PutUpdatePatientProblemResponse();
            try
            {
                response = DataPatientProblemManager.PutUpdatePatientProblem(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to the SQL database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;

        }
    }
}