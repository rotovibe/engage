using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PatientListResponse
   {
        public List<PatientResponse> Patients { get; set; }
        public string Version { get; set; }
    }

}
