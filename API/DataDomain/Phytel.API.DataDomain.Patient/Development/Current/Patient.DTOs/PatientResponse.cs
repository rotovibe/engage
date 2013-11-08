using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PatientResponse
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
