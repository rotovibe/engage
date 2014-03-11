using System;
using System.Net;
using Phytel.API.DataDomain.Cohort;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.Common.Format;
using System.Configuration;

namespace Phytel.API.DataDomain.Cohort.Service
{
    public class CohortService : ServiceStack.ServiceInterface.Service
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
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
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
                CommonFormatter.FormatExceptionResponse(response, base.Response, ex);

                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            return response;
        }
    }
}