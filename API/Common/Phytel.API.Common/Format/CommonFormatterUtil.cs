using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceHost;
using System;
using System.Net;

namespace Phytel.API.Common.Format
{
    public class CommonFormatterUtil : ICommonFormatterUtil
    {
        public void FormatExceptionResponse<T>(T response, IHttpResponse httpResponse, Exception ex) where T : IDomainResponse
        {
            if (ex != null && (ex is UnauthorizedAccessException || ex.Message == "UnauthorizedAccessException" || ex.Message == "Unauthorized"))
                httpResponse.StatusCode = (int) HttpStatusCode.Unauthorized;
            else
                httpResponse.StatusCode = (int) HttpStatusCode.InternalServerError;

            response.Status = new ResponseStatus(ex.ToErrorCode(), ex.Message)
            {
                ErrorCode = ex.ToErrorCode(),
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };
        }

        public string FormatDateOfBirth(string val)
        {
            var result = string.Empty;
            DateTime parsed;
            if (DateTime.TryParse(val, out parsed))
                result = val;

            return result;
        }
    }
}
