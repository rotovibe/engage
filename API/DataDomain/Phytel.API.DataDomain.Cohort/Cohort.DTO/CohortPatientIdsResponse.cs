using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    public class CohortPatientDetailsListResponse
    {
        public List<CohortPatient> Patients { get; set; }
    }

    public class CohortPatient
    {
        public string PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Version { get; set; }
    }
}
