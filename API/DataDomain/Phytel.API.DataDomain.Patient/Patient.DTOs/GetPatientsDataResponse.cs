using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class GetPatientsDataResponse : IDomainResponse
   {
        public Dictionary<string, PatientData> Patients { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
