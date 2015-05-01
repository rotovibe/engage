using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Journal
{
    public static class Constants
    {
        public const string RequestItemKeyStartedLogEvent = "StartedJournalEntry";

        public static class AppSettingDefaultValues
        {
            public const string PutEventPublishKey = "logputevent";
        }

        public static class AppSettingKeys
        {
            public const string PutEventPublishKey = "Service.Data.Log.PutEvent.PublishKey";
        }
    }
}
