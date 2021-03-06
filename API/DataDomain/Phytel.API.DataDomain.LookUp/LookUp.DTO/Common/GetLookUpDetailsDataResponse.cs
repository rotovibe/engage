using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp.DTO
{
   public class GetLookUpDetailsDataResponse : IDomainResponse
   {
       public List<LookUpDetailsData> LookUpDetailsData { get; set; }
       public double Version { get; set; }
       public ResponseStatus Status { get; set; }
    }

    public class LookUpDetailsData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
