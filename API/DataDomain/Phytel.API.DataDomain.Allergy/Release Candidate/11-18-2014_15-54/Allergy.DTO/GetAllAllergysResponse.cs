using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class GetAllAllergysResponse : DomainResponse
   {
        public List<AllergyData> Allergys { get; set; }
   }

}
