using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations.Repo.DTOs
{
    public class PatientNote
    {
        public int NoteId { get; set; }
        public string Note { get; set; }
        public int? ActionID { get; set; }
        public string ActionName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int PatientId { get; set; }
        public string Enabled { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeletedStatus { get; set; }
    }
}
