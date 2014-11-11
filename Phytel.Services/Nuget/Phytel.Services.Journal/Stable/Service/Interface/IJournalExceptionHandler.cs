using ServiceStack.ServiceHost;
using System;

namespace Phytel.Services.Journal
{
    public interface IJournalExceptionHandler
    {
        void HandleException(IHttpRequest req, IHttpResponse res, string operationName, Exception exception);
    }
}