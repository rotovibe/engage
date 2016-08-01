using System;
using System.Net;
using Phytel.API.DataDomain.Cohort;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.API.Common.Format;
using System.Configuration;
using System.Web;

namespace Phytel.API.DataDomain.Cohort.Service
{
    public class CohortService : ServiceStack.ServiceInterface.Service
    {
        public GetCohortDataResponse Get(GetCohortDataRequest request)
        {
            GetCohortDataResponse response = new GetCohortDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CohortDD:Get()::Unauthorized Access");

                response = DataCohortManager.GetCohortByID(request);
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

        public GetAllCohortsDataResponse Get(GetAllCohortsDataRequest request)
        {
            GetAllCohortsDataResponse response = new GetAllCohortsDataResponse();
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    throw new UnauthorizedAccessException("CohortDD:Get()::Unauthorized Access");

                response = DataCohortManager.GetCohorts(request);
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
    }
}