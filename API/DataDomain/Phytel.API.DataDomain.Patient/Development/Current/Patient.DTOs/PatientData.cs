using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PatientData
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public double Version { get; set; }
        public string DisplayPatientSystemId { get; set; }
        public PriorityData PriorityData { get; set; }
        public bool Flagged { get; set; }
        public string Background { get; set; }
        public int? LastFourSSN { get; set; }
        public string FullSSN { get; set; }
    }

    public enum PriorityData
    {
        NotSet = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }
}
