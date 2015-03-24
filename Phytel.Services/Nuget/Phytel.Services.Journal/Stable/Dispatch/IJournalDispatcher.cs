using ServiceStack.ServiceHost;
using System;

namespace Phytel.Services.Journal.Dispatch
{
    public interface IJournalDispatcher
    {
        JournalEntry Dispatch(State state, object requestDto = null, string actionId = null, string name = null, string product = null, string ipAddress = null, string verb = null, DateTime? timeUtc = null, string parentActionId = null, Exception exception = null);

        void DispatchEntries(params JournalEntry[] entries);

        JournalEntry Dispatch(State state, IHttpRequest request, object requestDto = null, string actionId = null, DateTime? timeUtc = null, string parentActionId = null, Exception exception = null);
    }
}