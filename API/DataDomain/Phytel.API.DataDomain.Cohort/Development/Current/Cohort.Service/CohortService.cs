using System;
using System.Net;
using Phytel.API.DataDomain.Cohort;
using Phytel.API.DataDomain.Cohort.DTO;

namespace Phytel.API.DataDomain.Cohort.Service
{
    public class CohortService : ServiceStack.ServiceInterface.Service
    {
        public GetCohortResponse Get(GetCohortRequest request)
        {
            GetCohortResponse response = new GetCohortResponse();
            try
            {
                response = DataCohortManager.GetCohortByID(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ServiceStack.ServiceInterface.ServiceModel.ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetAllCohortResponse Get(GetAllCohortRequest request)
        {
            GetAllCohortResponse response = new GetAllCohortResponse();
            try
            {
                response = DataCohortManager.GetCohorts(request);
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