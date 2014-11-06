using Phytel.Services.Journal.Dispatch;
using ServiceStack.ServiceHost;
using System;
using System.Linq;

namespace Phytel.Services.Journal
{
    public class JournalExceptionHandler : IJournalExceptionHandler
    {
        protected IJournalDispatcher _journalDispatch;

        public JournalExceptionHandler(IJournalDispatcher journalDispatcher)
        {
            _journalDispatch = journalDispatcher;
        }

        public virtual void HandleException(IHttpRequest req, IHttpResponse res, string operationName, Exception exception)
        {
            if (req.Items.Any(x => x.Key == Constants.RequestItemkeyStartedJournalEntry))
            {
                JournalEntry startedJournalEntry = req.Items[Constants.RequestItemkeyStartedJournalEntry] as JournalEntry;
                if (startedJournalEntry != null)
                {
                    _journalDispatch.Dispatch(startedJournalEntry.ActionId, State.Failed, req, parentActionId: startedJournalEntry.ParentActionId, exception: exception);
                }
            }
        }
    }
}