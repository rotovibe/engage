using System;
using System.Configuration;
using Phytel.API.Common.Format;
using Common = Phytel.API.Common;
using Phytel.API.Interface;
using ServiceStack.ServiceClient.Web;

namespace AppDomain.Engage.Population.Service
{
    public class ServiceBase : ServiceStack.ServiceInterface.Service
    {
        public void FormatException(IDomainResponse response, Exception ex)
        {
            CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            if ((ex is WebServiceException) == false)
            {
                LogException(ex);
            }
        }

        public void LogException(Exception ex)
        {
            var aseProcessId = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
            Common.Helper.LogException(int.Parse(aseProcessId), ex);
        }
    }
}