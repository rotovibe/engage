﻿using System;
using System.Collections.Generic;
using Funq;


namespace Phytel.Engage.Integrations.DomainEvents
{
    public static class LoggerDomainEvent
    {
        [ThreadStatic] //so that each thread has its own callbacks
        private static List<Delegate> actions;

        public static IHandle<LogStatus> Logger = new LogHandler<LogStatus>();

        //Registers a callback for the given domain event
        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
                actions = new List<Delegate>();

            actions.Add(callback);
        }

        //Clears callbacks passed to Register on the current thread
        public static void ClearCallbacks()
        {
            actions = null; 
        }

        //Raises the given domain event
        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (Logger != null)
                    Logger.Handle(args as LogStatus);

            //if (actions != null)
            //    foreach (var action in actions)
            //        if (action is Action<T>)
            //            ((Action<T>) action)(args);
        }
    }
}
