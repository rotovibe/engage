using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.Engage.Integrations.DomainEvents
{
    public interface IHandle<T> where T : IDomainEvent
    {
        void Handle(T args);
        event EtlEventHandler EtlEvent;
    }
}
