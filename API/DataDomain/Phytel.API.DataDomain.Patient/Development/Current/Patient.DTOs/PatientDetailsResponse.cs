using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PatientDetailsResponse
   {
        public List<PatientResponse> Patients { get; set; }
        public string Version { get; set; }
    }

}
