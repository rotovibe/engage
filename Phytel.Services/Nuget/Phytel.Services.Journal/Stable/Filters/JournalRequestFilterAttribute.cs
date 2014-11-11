using Phytel.Services.Journal.Dispatch;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Phytel.Services.Journal.Filters
{
    public class JournalRequestFilterAttribute : RequestFilterAttribute
    {
        public IJournalDispatcher JournalDispatcher { get; set; }

        public override void Execute(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            JournalEntry startedEntry = JournalDispatcher.Dispatch(requestDto, State.Started, req);

            if (startedEntry != null)
            {
                req.Items.Add(Constants.RequestItemkeyStartedJournalEntry, startedEntry);
            }
        }
    }
}