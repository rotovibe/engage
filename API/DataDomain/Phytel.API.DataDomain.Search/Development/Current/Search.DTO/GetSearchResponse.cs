
using System.Collections.Generic;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.Search.DTO
{
    public class GetSearchResponse : DomainResponse
    {
        public List<TextValuePair> MedResults { get; set; }
        public List<IdNamePair> AllergyResults { get; set; }
    }
}
