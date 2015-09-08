using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceHost;
using System;
using System.Net;

namespace Phytel.API.Common.Format
{
    public static class CommonFormatter
    {
        public static void FormatExceptionResponse<T>(T response, IHttpResponse httpResponse, Exception ex) where T : IDomainResponse
        {
            if (ex is UnauthorizedAccessException || ex.Message == "UnauthorizedAccessException" || ex.Message == "Unauthorized")
            {
                httpResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                httpResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            response.Status = ex.ToResponseStatus();
            response.Status.ErrorCode = ex.ToErrorCode();
            response.Status.Message = ex.Message;
            response.Status.StackTrace = ex.StackTrace;
        }

        public static string FormatDateOfBirth(string val)
        {
            string result = string.Empty;
            DateTime parsed;
            if (DateTime.TryParse(val, out parsed))
                result = val;

            return result;
        }
    }
}
