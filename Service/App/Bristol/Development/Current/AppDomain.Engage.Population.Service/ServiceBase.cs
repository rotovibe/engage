using System;
using AppDomain.Engage.Population.DTO.Context;
using Phytel.API.AppDomain.Platform.Security.DTO;
using Phytel.API.ASE.Client.Interface;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.ASE.DTO.Common;
using Phytel.API.DataDomain.ASE.DTO.Common.Enums;
using Common = Phytel.API.Common;
using Phytel.API.Interface;
using Phytel.Services.API.Platform.Filter;
using ServiceStack.ServiceClient.Web;

namespace AppDomain.Engage.Population.Service
{
    public class ServiceBase : ServiceStack.ServiceInterface.Service
    {
        public ICommonFormatterUtil CommonFormatter { get; set; }
        public Common.IHelpers Helpers { get; set; }

        public void FormatException(IAppDomainRequest request,  IDomainResponse response,IASEClient client, Exception ex)
        {
            CommonFormatter.FormatExceptionResponse(response, base.Response, ex);
            if (ex is WebServiceException != false) return;
            var log = BuildErrorProcessLog(ex, "ReferralDefinitionService");
            client.PostProcessLog(log); //.Log(this.RequestContext, request, response, TimeSpan.MaxValue);
        }

        public UserContext BuildUserContext(AppDomainSessionInfo sessionInfo)
        {
            var userContext = new UserContext
            {
                UserId = sessionInfo.UserInfo.UserId,
                UserName = sessionInfo.UserInfo.UserName,
                SQLUserId = sessionInfo.UserInfo.LegacyUserId,
                TokenId = sessionInfo.SessionId
            };

            return userContext;
        }

        public static ProcessLog BuildErrorProcessLog(Exception exception, string serviceName)
        {
            var processLog = new ProcessLog
            {
                Code = LogErrorCode.Error,
                DateTime = DateTime.UtcNow,
                DayOfYear = DateTime.UtcNow.DayOfYear,
                Severity = LogErrorSeverity.Critical,
                Message = exception.Message,
                Source = exception.Source,
                Description = exception.StackTrace,
                ServerProcessName = serviceName,
                LogType = LogType.Error.ToString()

            };

            return processLog;
        }
    }
}