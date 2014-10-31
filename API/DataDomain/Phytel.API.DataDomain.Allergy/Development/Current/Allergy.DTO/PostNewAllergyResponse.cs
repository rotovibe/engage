using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class PostNewAllergyResponse : DomainResponse
    {
        public AllergyData Allergy { get; set; }
    }
}
