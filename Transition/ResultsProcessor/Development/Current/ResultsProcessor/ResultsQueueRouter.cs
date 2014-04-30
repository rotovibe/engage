using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Phytel.Framework.ASE.Process;
using Phytel.Framework.ASE.Bus.Data;
using Phytel.Framework.ASE.Data.Common;
using Phytel.Bus;

namespace ResultsProcessor
{
    public class ResultsQueueRouter : QueueProcessBase
    {
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Takes PDL Queue Messages and puts them on the Bus to be distributed.
        /// </summary>
        /// <param name="message">The queue message from PDL.</param>
        ///////////////////////////////////////////////////////////////////////
        public override void Execute(QueueMessage message)
        {
            MessageBus bus = new MessageBus();
            PublishMessage pubMessage = new PublishMessage { PublishKey = "resultsprocessrouter", Message = message.Body };
            Tuple<List<SubscriberQueue>, List<PublishResult>> results = bus.PutOnBus(pubMessage);
            if (!AllSuccessful(results))
            {
                base.LogError("Problem publishing message to bus", LogErrorCode.QueueProcessorError, LogErrorSeverity.Critical, message);
            }
        }

        public bool AllSuccessful(Tuple<List<SubscriberQueue>, List<PublishResult>> results)
        {
            foreach (PublishResult r in results.Item2)
                if (r.Type == Phytel.Framework.ASE.Bus.Common.ResultType.Error)
                    return false;

            return true;
        }
    }
}
