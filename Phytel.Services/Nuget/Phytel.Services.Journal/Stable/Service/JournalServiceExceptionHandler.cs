using AutoMapper;
using Phytel.Services.Journal.Dispatch;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Linq;

namespace Phytel.Services.Journal
{
    public class JournalServiceExceptionHandler : IJournalServiceExceptionHandler
    {
        protected readonly IJournalDispatcher _journalDispatch;
        protected readonly IMappingEngine _mappingEngine;

        public JournalServiceExceptionHandler(IJournalDispatcher journalDispatcher, IMappingEngine mappingEngine)
        {
            _journalDispatch = journalDispatcher;
            _mappingEngine = mappingEngine;
        }

        public virtual ErrorResponse HandleServiceException(IHttpRequest req, object requestDto, Exception exception)
        {
            if (req.Items.Any(x => x.Key == Constants.RequestItemkeyStartedJournalEntry))
            {
                JournalEntry startedJournalEntry = req.Items[Constants.RequestItemkeyStartedJournalEntry] as JournalEntry;
                if (startedJournalEntry != null)
                {
                    _journalDispatch.Dispatch(startedJournalEntry.ActionId, State.Failed, req, parentActionId: startedJournalEntry.ParentActionId, exception: exception);
                }
            }

            return _mappingEngine.Map<ErrorResponse>(exception);
        }
    }
}