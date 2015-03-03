using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Search.DTO
{
    public class GetAllSearchsResponse : DomainResponse
   {
        public List<Search> Searchs { get; set; }
   }

}
