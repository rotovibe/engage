using Phytel.Services.Journal.Dispatch;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Phytel.Services.Journal.Filters
{
    public class JournalRequestFilterAttribute : RequestFilterAttribute
    {
        public ILogDispatcher LogDispatcher { get; set; }

        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            LogEvent startedLogEvent = LogDispatcher.Dispatch(State.Started, req, requestDto: requestDto);

            if (startedLogEvent != null)
            {
                req.Items.Add(Constants.RequestItemKeyStartedLogEvent, startedLogEvent);
            }
        }
    }
}