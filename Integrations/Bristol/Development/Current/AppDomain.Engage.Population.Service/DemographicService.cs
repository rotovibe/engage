using System;
using AppDomain.Engage.Population.DTO.Demographics;
using Phytel.API.Common.Audit;
using Phytel.API.DataAudit;
using ServiceStack.ServiceInterface.ServiceModel;

namespace AppDomain.Engage.Population.Service
{
    public class DemographicService : ServiceBase
    {
        public IDemographicsManager Manager { get; set; }
        public IAuditHelpers AuditHelper { get; set; }

        public PostPatientDemographicsResponse Post(PostPatientDemographicsRequest request)
        {
            var response = new PostPatientDemographicsResponse();
            try
            {
                var status = new ResponseStatus {Message = Manager.DoSomething()};
                response.ResponseStatus = status;

                return response;
            }
            catch (Exception ex)
            {
                FormatException(response, ex);
            }
            finally
            {
                // format to have the appropriate arguments.
                AuditHelper.LogAuditData(request, "SQLUserId", null, System.Web.HttpContext.Current.Request, request.GetType().Name);
            }
            return response;
        }
    }
}