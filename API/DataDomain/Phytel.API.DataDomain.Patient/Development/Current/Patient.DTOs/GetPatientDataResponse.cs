using Phytel.API.Interface;
using ServiceStack;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
   public class GetPatientDataResponse : IDomainResponse
   {
       public PatientData Patient { get; set; }
       public string Version { get; set; }
       public ResponseStatus Status { get; set; }
    }
}
