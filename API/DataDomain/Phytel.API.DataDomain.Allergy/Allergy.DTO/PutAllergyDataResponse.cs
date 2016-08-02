using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class PutAllergyDataResponse : IDomainResponse
    {
        public AllergyData AllergyData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
