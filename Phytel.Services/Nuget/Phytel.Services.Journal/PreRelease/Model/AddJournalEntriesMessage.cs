using System.Collections.Generic;

namespace Phytel.Services.Journal
{
    public class AddJournalEntriesMessage
    {
        public AddJournalEntriesMessage()
        {
            Entries = new List<JournalEntry>();
        }

        public List<JournalEntry> Entries { get; set; }
    }
}