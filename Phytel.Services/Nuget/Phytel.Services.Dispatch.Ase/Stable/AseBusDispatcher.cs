using Phytel.Bus;
using Phytel.Framework.ASE.Bus.Data;
using Phytel.Services.Serializer;
using System;
using System.Collections.Generic;

namespace Phytel.Services.Dispatch.Ase
{
    public class AseBusDispatcher : Dispatcher
    {
        protected readonly MessageBus _messageBus;
        protected readonly string _publishKey;

        public AseBusDispatcher(string publishKey, ISerializer serializer)
            : this(new MessageBus(), publishKey, serializer)
        {
        }

        public AseBusDispatcher(MessageBus messageBus, string publishKey, ISerializer serializer)
            : base(serializer)
        {
            _messageBus = messageBus;
            _publishKey = publishKey;
        }

        protected override void OnDispatch(string message)
        {
            PublishMessage publishMessage = new PublishMessage();
            publishMessage.Message = message;
            publishMessage.PublishKey = _publishKey;
            Tuple<List<SubscriberQueue>, List<PublishResult>> results = _messageBus.PutOnBus(publishMessage);
        }
    }
}