using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations.DomainEvents
{
    public delegate void EtlEventHandler(object sender, LogStatus e);

    public class LogHandler<T> : IHandle<T> where T : IDomainEvent
    {
        public event EtlEventHandler EtlEvent;

        private void OnEtlEvent(LogStatus e)
        {
            if (EtlEvent != null)
            {
                EtlEvent(null, e);
            }
        }

        public void Handle(T args)
        {
            OnEtlEvent(args as LogStatus);
            // Send an email to the customer, informing them their password has changed. Put a message on a bus to be handled by another service.
        }
    }
}
