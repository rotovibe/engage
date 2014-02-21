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
        public string CreatedBy { get; set; }
    }
}
