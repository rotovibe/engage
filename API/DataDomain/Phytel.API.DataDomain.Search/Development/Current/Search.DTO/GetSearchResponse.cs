
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Search.DTO
{
    public class GetSearchResponse : DomainResponse
    {
        public List<TextValuePair> Results { get; set; }
    }
}
