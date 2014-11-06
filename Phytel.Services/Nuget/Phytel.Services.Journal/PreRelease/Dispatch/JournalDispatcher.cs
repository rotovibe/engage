using AutoMapper;
using Phytel.Services.Dates;
using Phytel.Services.Dispatch;
using Phytel.Services.ServiceStack.Provider;
using ServiceStack.ServiceHost;
using System;

namespace Phytel.Services.Journal.Dispatch
{
    public class JournalDispatcher : IJournalDispatcher
    {
        protected readonly IActionIdProvider _actionIdProvider;
        protected readonly IDateTimeProxy _dateTimeProxy;
        protected readonly IDispatcher _dispatcher;
        protected readonly IMappingEngine _mappingEngine;
        protected readonly IServiceConfigProxy _serviceConfigProxy;

        public JournalDispatcher(IDispatcher dispatcher, IActionIdProvider actionIdProvider, IDateTimeProxy dateTimeProxy, IServiceConfigProxy serviceConfigProxy, IMappingEngine mappingEngine)
        {
            _dispatcher = dispatcher;
            _actionIdProvider = actionIdProvider;
            _dateTimeProxy = dateTimeProxy;
            _serviceConfigProxy = serviceConfigProxy;
            _mappingEngine = mappingEngine;
        }

        public virtual JournalEntry Dispatch(string actionId, State state, IHttpRequest request, DateTime? timeUtc = null, string parentActionId = null, Exception exception = null)
        {
            AddJournalEntriesMessage message = new AddJournalEntriesMessage();
            JournalEntry entry = new JournalEntry();
            entry.ActionId = actionId;
            entry.ParentActionId = parentActionId;
            entry.Name = request.OperationName;
            entry.Product = _serviceConfigProxy.GetServiceName();
            entry.IPAddress = request.RemoteIp;
            entry.Body = request.GetRawBody();
            entry.Url = request.RawUrl;
            entry.Verb = request.HttpMethod;
            entry.State = state;

            if (timeUtc.HasValue)
            {
                entry.Time = timeUtc.Value;
            }
            else
            {
                entry.Time = _dateTimeProxy.GetCurrentDateTime();
            }
            
            if (exception != null)
            {
                entry.Error = _mappingEngine.Map<Error>(exception);
            }

            message.Entries.Add(entry);

            _dispatcher.Dispatch(message);

            return entry;
        }

        public virtual JournalEntry Dispatch(object requestDto, State state, IHttpRequest request, DateTime? timeUtc = null, Exception exception = null)
        {
            JournalEntry rvalue = new JournalEntry();

            if (requestDto is IJournaledRequest)
            {
                IJournaledRequest journaledRequest = requestDto as IJournaledRequest;
                if (journaledRequest != null)
                {
                    journaledRequest.ActionId = _actionIdProvider.New();
                    rvalue.ActionId = journaledRequest.ActionId;
                }
            }

            if (requestDto is IJournaledAsChildRequest)
            {
                IJournaledAsChildRequest journaledAsChildRequest = requestDto as IJournaledAsChildRequest;
                if (journaledAsChildRequest != null)
                {
                    journaledAsChildRequest.ParentActionId = journaledAsChildRequest.ParentActionId;
                    rvalue.ParentActionId = journaledAsChildRequest.ParentActionId;
                }
            }

            rvalue = Dispatch(rvalue.ActionId, state, request, timeUtc, rvalue.ParentActionId, exception);

            return rvalue;
        }
    }
}