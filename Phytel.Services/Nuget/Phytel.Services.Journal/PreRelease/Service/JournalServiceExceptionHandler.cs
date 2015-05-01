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
        protected readonly ILogDispatcher _logDispatch;
        protected readonly IMappingEngine _mappingEngine;

        public JournalServiceExceptionHandler(ILogDispatcher logDispatch, IMappingEngine mappingEngine)
        {
            _logDispatch = logDispatch;
            _mappingEngine = mappingEngine;
        }

        public virtual ErrorResponse HandleServiceException(IHttpRequest req, object requestDto, Exception exception)
        {
            if (req.Items.Any(x => x.Key == Constants.RequestItemKeyStartedLogEvent))
            {
                LogEvent startedLogEvent = req.Items[Constants.RequestItemKeyStartedLogEvent] as LogEvent;
                if (startedLogEvent != null)
                {
                    _logDispatch.Dispatch(State.Failed, req, actionId: startedLogEvent.ActionId, parentActionId: startedLogEvent.ParentActionId, exception: exception);
                }
            }

            return _mappingEngine.Map<ErrorResponse>(exception);
        }
    }
}