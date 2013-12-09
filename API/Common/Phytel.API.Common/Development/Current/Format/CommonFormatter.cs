using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.Common.Format
{
    public static class CommonFormatter
    {
        public static void FormatExceptionResponse<T>(T response, IHttpResponse httpResponse, Exception ex) where T : IDomainResponse
        {
            httpResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.Status = new ResponseStatus(ex.ToErrorCode(), ex.Message);
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
