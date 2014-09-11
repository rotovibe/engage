using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    public class PatientNoteData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string Text { get; set; }
        public List<string> ProgramIds { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public int TypeId { get; set; }
        public string MethodId { get; set; }
        public string OutcomeId { get; set; }
        public string WhoId { get; set; }
        public string SourceId { get; set; }
        public string DurationId { get; set; }
        public DateTime? ContactedOn { get; set; }
        public bool ValidatedIndentity { get; set; }
    }
}
