using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PatientData
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Version { get; set; }
        public string DisplayPatientSystemID { get; set; }
        public Priority Priority { get; set; }
        public bool Flagged { get; set; }
        public string ContactId { get; set; }
    }

    public enum Priority
    {
        NotSet = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }
}
