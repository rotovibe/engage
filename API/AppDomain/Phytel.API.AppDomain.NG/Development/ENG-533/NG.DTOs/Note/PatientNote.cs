using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientNote
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
    }
}
