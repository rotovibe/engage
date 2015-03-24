using Phytel.Services.Journal.Dispatch;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using System.Linq;

namespace Phytel.Services.Journal.Filters
{
    public class JournalResponseFilterAttribute : ResponseFilterAttribute
    {
        public IJournalDispatcher JournalDispatcher { get; set; }
        
        public override void Execute(IHttpRequest req, IHttpResponse res, object responseDto)
        {
            if (req.Items.Any(x => x.Key == Constants.RequestItemkeyStartedJournalEntry))
            {
                JournalEntry startedJournalEntry = req.Items[Constants.RequestItemkeyStartedJournalEntry] as JournalEntry;
                if (startedJournalEntry != null)
                {
                    JournalDispatcher.Dispatch(State.Ended, req, actionId: startedJournalEntry.ActionId, parentActionId: startedJournalEntry.ParentActionId);
                }
            }
        }
    }
}