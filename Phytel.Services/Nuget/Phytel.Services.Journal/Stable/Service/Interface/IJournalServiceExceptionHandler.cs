using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;

namespace Phytel.Services.Journal
{
    public interface IJournalServiceExceptionHandler
    {
        ErrorResponse HandleServiceException(IHttpRequest req, object requestDto, Exception exception);
    }
}