using System;
namespace Phytel.API.Common.Format
{
    public interface ICommonFormatterUtil
    {
        string FormatDateOfBirth(string val);
        void FormatExceptionResponse<T>(T response, ServiceStack.ServiceHost.IHttpResponse httpResponse, Exception ex) where T : Phytel.API.Interface.IDomainResponse;
    }
}
