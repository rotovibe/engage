using Phytel.Services.Journal.Dispatch;
using ServiceStack.ServiceHost;
using System;
using System.Linq;

namespace Phytel.Services.Journal
{
    public class JournalExceptionHandler : IJournalExceptionHandler
    {
        protected ILogDispatcher _logDispatch;

        public JournalExceptionHandler(ILogDispatcher logDispatch)
        {
            _logDispatch = logDispatch;
        }

        public virtual void HandleException(IHttpRequest req, IHttpResponse res, string operationName, Exception exception)
        {
            if (req.Items.Any(x => x.Key == Constants.RequestItemKeyStartedLogEvent))
            {
                LogEvent startedLogEvent = req.Items[Constants.RequestItemKeyStartedLogEvent] as LogEvent;
                if (startedLogEvent != null)
                {
                    _logDispatch.Dispatch(State.Failed, req, actionId: startedLogEvent.ActionId, parentActionId: startedLogEvent.ParentActionId, exception: exception);
                }
            }
        }
    }
}