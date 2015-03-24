using ServiceStack.ServiceHost;
using System;

namespace Phytel.Services.Journal.Dispatch
{
    public interface IJournalDispatcher
    {
        JournalEntry Dispatch(object requestDto, State state, string actionId = null, string name = null, string product = null, string ipAddress = null, string verb = null, DateTime? timeUtc = null, string parentActionId = null, Exception exception = null);

        void DispatchEntries(params JournalEntry[] entries);

        JournalEntry Dispatch(object requestDto, State state, IHttpRequest request, string actionId = null, DateTime? timeUtc = null, string parentActionId = null, Exception exception = null);
    }
}