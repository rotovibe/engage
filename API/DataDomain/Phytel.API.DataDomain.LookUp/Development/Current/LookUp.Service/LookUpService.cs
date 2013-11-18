using System;
using System.Collections.Generic;
using System.Net;
using Phytel.API.DataDomain.LookUp;
using Phytel.API.DataDomain.LookUp.DTO;

namespace Phytel.API.DataDomain.Patient.Service
{
    public class LookUpService : ServiceStack.ServiceInterface.Service
    {
        public ProblemResponse Get(GetProblemRequest request)
        {
            ProblemResponse response = new ProblemResponse();
            try
            {
                response = LookUpDataManager.GetProblemByID(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public ProblemsResponse Get(FindProblemsRequest request)
        {
            ProblemsResponse response = new ProblemsResponse();
            try
            {
                response = LookUpDataManager.FindProblems(request);
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