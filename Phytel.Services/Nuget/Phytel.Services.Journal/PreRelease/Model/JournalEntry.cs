using System;

namespace Phytel.Services.Journal
{
    public class JournalEntry
    {
        public string ActionId { get; set; }

        public string Body { get; set; }

        public Error Error { get; set; }

        public string Id { get; set; }

        public string IPAddress { get; set; }

        public string Name { get; set; }

        public string ParentActionId { get; set; }

        public string Product { get; set; }

        public State State { get; set; }

        public DateTime Time { get; set; }

        public string Url { get; set; }

        public string Verb { get; set; }
    }
}