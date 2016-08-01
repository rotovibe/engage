using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class PutInitializeAllergyDataResponse : IDomainResponse
    {
        public AllergyData AllergyData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
