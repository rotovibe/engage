using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PostPatientAllergiesResponse : IDomainResponse
    {
        public List<PatientAllergy> PatientAllergies { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
