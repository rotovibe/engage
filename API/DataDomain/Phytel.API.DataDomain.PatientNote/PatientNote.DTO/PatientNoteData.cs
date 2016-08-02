using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    
    public class PatientNoteData : IAppData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string Text { get; set; }
        public List<string> ProgramIds { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string TypeId { get; set; }
        public string MethodId { get; set; }
        public string OutcomeId { get; set; }
        public string WhoId { get; set; }
        public string SourceId { get; set; }
        public int? Duration { get; set; }
        public DateTime? ContactedOn { get; set; }
        public bool ValidatedIdentity { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedById { get; set; }
        public string DataSource { get; set; }
        // added for atmosphere integration
        public string ExternalRecordId { get; set; }
    }
}
