using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class GetAllPatientsDataResponse : IDomainResponse
   {
        public List<PatientData> PatientsData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
