using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations.Repo.DTOs
{
    public class PatientInfo
    {
        public string GroupName { get; set; }
        public int? GroupId { get; set; }
        public int? SubscriberId { get; set; }
        public int? PatientId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string SSN { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Status { get; set; }
    }
}
