using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Journal
{
    public class LogEvent
    {
        public string ActionId { get; set; }

        public string Body { get; set; }

        public Error Error { get; set; }

        public string IPAddress { get; set; }

        public string Name { get; set; }

        public string ParentActionId { get; set; }

        public string Product { get; set; }

        public State State { get; set; }

        public DateTime Time { get; set; }

        public string Url { get; set; }

        public string Verb { get; set; }

        public int RetryCount { get; set; }
    }
}
