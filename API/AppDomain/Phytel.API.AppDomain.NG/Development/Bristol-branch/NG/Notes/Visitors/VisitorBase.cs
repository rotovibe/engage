using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Notes.Visitors
{
    public abstract class VisitorBase
    {
        protected static readonly string DDPatientNoteUrl = ConfigurationManager.AppSettings["DDPatientNoteUrl"];

        public string ContractNumber { get; set; }
        public double Version { get; set; }
        public string PatientId { get; set; }
        public int Count { get; set; }
        public string UserId { get; set; }

        public abstract List<PatientNote> Visit(ref List<PatientNote> list);
    }
}
