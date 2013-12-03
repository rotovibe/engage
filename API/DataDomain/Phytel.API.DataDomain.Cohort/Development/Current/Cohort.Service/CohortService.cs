using System;
using System.Net;
using Phytel.API.DataDomain.Cohort;
using Phytel.API.DataDomain.Cohort.DTO;
using ServiceStack;

namespace Phytel.API.DataDomain.Cohort.Service
{
    public class CohortService : ServiceStack.Service
    {
        public GetCohortDataResponse Get(GetCohortDataRequest request)
        {
            GetCohortDataResponse response = new GetCohortDataResponse();
            try
            {
                response = DataCohortManager.GetCohortByID(request);
            }
            catch (Exception ex)
            {
                //TODO: Log this to C3 database via ASE
                base.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = new ResponseStatus("Exception", ex.Message);
            }
            return response;
        }

        public GetAllCohortsDataResponse Get(GetAllCohortsDataRequest request)
        {
            GetAllCohortsDataResponse response = new GetAllCohortsDataResponse();
            try
            {
                response = DataCohortManager.GetCohorts(request);
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