using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Medication.DTO
{
    public class PostPatientMedFrequencyDataResponse : DomainResponse
   {
        public string Id { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
