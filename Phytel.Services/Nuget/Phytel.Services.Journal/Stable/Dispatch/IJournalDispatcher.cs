using ServiceStack.ServiceHost;
using System;

namespace Phytel.Services.Journal.Dispatch
{
    public interface IJournalDispatcher
    {
        JournalEntry Dispatch(object requestDto, State state, IHttpRequest request, DateTime? timeUtc = null, Exception exception = null);

        JournalEntry Dispatch(string actionId, State state, IHttpRequest request, DateTime? timeUtc = null, string parentActionId = null, Exception exception = null);
    }
}