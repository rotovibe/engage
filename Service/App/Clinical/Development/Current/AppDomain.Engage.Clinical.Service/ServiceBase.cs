using System;
using AppDomain.Engage.Clinical.DTO.Context;
using Phytel.API.AppDomain.Platform.Security.DTO;
using Phytel.API.ASE.Client.Interface;
using Phytel.API.Common.Format;
using Common = Phytel.API.Common;
using Phytel.API.DataDomain.ASE.DTO.Common;
using Phytel.API.DataDomain.ASE.DTO.Common.Enums;
using ServiceStack.ServiceClient.Web;

namespace AppDomain.Engage.Clinical.Service
{
    public class ServiceBase : ServiceStack.ServiceInterface.Service
    {

        protected IClinicalManager Manager;
        protected ServiceBase(IClinicalManager manager)
        {
            Manager = manager;
        }
        public ICommonFormatterUtil CommonFormatter { get; set; }
        public Common.IHelpers Helpers { get; set; }

        public void FormatException(Phytel.API.Interface.IAppDomainRequest request, Phytel.API.Interface.IDomainResponse response, IASEClient client, Exception ex)
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