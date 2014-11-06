﻿using AutoMapper;
using Phytel.Services.IOC;
using Phytel.Services.Journal.Filters;
using Phytel.Services.Journal.Map;
using ServiceStack.WebHost.Endpoints;
using System.Reflection;

namespace Phytel.Services.Journal
{
    public class JournaledAppHost<TContainerProfile> : AppHostBase
        where TContainerProfile : IContainerProfile, new()
    {
        protected readonly IContainerProfile _containerProfile;
        protected bool _globalJournalingEnabled;

        public JournaledAppHost(string serviceName, bool globalJournalingEnabled = true, params Assembly[] assemblies)
            : this(serviceName, new TContainerProfile(), globalJournalingEnabled, assemblies)
        {
        }

        public JournaledAppHost(string serviceName, IContainerProfile containerProfile, bool globalJournalingEnabled = true, params Assembly[] assemblies)
            : base(serviceName, assemblies)
        {
            _containerProfile = containerProfile;
            _globalJournalingEnabled = globalJournalingEnabled;
        }

        public override void Configure(Funq.Container container)
        {
            container = OnConfigureContainer(container);

            Mapper.AddProfile<ErrorMap>();
            this.PreRequestFilters.Insert(0, (req, res) =>
            {
                req.UseBufferedStream = true;
            });

            OnConfigureFilters(container);
        }

        protected virtual Funq.Container OnConfigureContainer(Funq.Container container)
        {
            container =
                _containerProfile.GetBuilder(container)
                .DecorateWith(cb => new JournalDispatcherContainerDecorator(cb))
                .DecorateWith(cb => new JournalExceptionHandlerContainerDecorator(cb))
                .Build();

            return container;
        }

        protected virtual void OnConfigureFilters(Funq.Container container)
        {
            if (_globalJournalingEnabled)
            {
                JournalRequestFilterAttribute journalRequest = TryResolve<JournalRequestFilterAttribute>();
                RequestFilters.Add(journalRequest.RequestFilter);
                JournalResponseFilterAttribute journalResponse = TryResolve<JournalResponseFilterAttribute>();
                ResponseFilters.Add(journalResponse.ResponseFilter);
            }

            IJournalExceptionHandler exceptionHander = container.TryResolve<IJournalExceptionHandler>();
            if (exceptionHander != null)
            {
                this.ExceptionHandler = exceptionHander.HandleException;
            }

            IJournalServiceExceptionHandler serviceExceptionHander = container.TryResolve<IJournalServiceExceptionHandler>();
            if (serviceExceptionHander != null)
            {
                this.ServiceExceptionHandler = serviceExceptionHander.HandleServiceException;
            }
        }
    }
}