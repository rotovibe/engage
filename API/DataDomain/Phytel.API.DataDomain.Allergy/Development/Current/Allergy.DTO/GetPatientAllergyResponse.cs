
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class GetPatientAllergyResponse : DomainResponse
    {
        public List<DdAllergy> Allergies { get; set; }
    }
}
