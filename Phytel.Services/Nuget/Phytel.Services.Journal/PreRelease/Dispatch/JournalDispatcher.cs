using AutoMapper;
using Phytel.Framework.ASE.Bus;
using Phytel.Services.API.DTO;
using Phytel.Services.API.Provider;
using Phytel.Services.Dates;
using Phytel.Services.Dispatch;
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

        public virtual JournalEntry Dispatch(State state, object requestDto = null, string actionId = null, string name = null, string product = null, string ipAddress = null, string verb = null, DateTime? timeUtc = null, string parentActionId = null, Exception exception = null)
        {
            if (string.IsNullOrEmpty(actionId))
            {
                actionId = _actionIdProvider.New();
            }

            if (requestDto is IJournaledRequest)
            {
                IJournaledRequest journaledRequest = requestDto as IJournaledRequest;
                if (journaledRequest != null)
                {
                    journaledRequest.ActionId = _actionIdProvider.New();
                }
            }

            if (requestDto is IJournaledAsChildRequest)
            {
                IJournaledAsChildRequest journaledAsChildRequest = requestDto as IJournaledAsChildRequest;
                if (journaledAsChildRequest != null)
                {
                    journaledAsChildRequest.ParentActionId = parentActionId;
                }
            }

            JournalEntry entry = new JournalEntry();
            entry.ActionId = actionId;
            entry.ParentActionId = parentActionId;
            entry.Name = name;
            entry.Product = product;
            entry.IPAddress = ipAddress;
            entry.Verb = verb;
            entry.State = state;

            entry.Body = ServiceStack.Text.JsonSerializer.SerializeToString(requestDto, requestDto.GetType());         

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

            DispatchEntries(entry);

            return entry;
        }

        public virtual void DispatchEntries(params JournalEntry[] entries)
        {
            AddJournalEntriesMessage message = new AddJournalEntriesMessage();
            if(message.Entries == null)
            {
                message.Entries = new System.Collections.Generic.List<JournalEntry>();
            }
            foreach (JournalEntry entry in entries)
            {
                message.Entries.Add(entry);
            }

            _dispatcher.DispatchAsync(message);
        }


        public virtual JournalEntry Dispatch(State state, IHttpRequest request, object requestDto = null, string actionId = null, DateTime? timeUtc = null, string parentActionId = null, Exception exception = null)
        {
            return Dispatch(
                state,
                requestDto,
                actionId,
                request.OperationName,
                _serviceConfigProxy.GetServiceName(),
                request.RemoteIp,
                request.HttpMethod,
                timeUtc,
                parentActionId,
                exception
                );
        }
    }
}