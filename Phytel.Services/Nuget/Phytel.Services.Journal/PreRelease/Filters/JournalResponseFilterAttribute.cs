using Phytel.Services.Journal.Dispatch;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System.Linq;

namespace Phytel.Services.Journal.Filters
{
    public class JournalResponseFilterAttribute : ResponseFilterAttribute
    {
        public ILogDispatcher LogDispatcher { get; set; }
        
        public override void Execute(IHttpRequest req, IHttpResponse res, object responseDto)
        {
            if (req.Items.Any(x => x.Key == Constants.RequestItemKeyStartedLogEvent))
            {
                LogEvent startedLogEvent = req.Items[Constants.RequestItemKeyStartedLogEvent] as LogEvent;
                if (startedLogEvent != null)
                {
                    LogDispatcher.Dispatch(State.Ended, req, actionId: startedLogEvent.ActionId, parentActionId: startedLogEvent.ParentActionId);
                }
            }
        }
    }
}