using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetTimeZoneDataResponse : IDomainResponse
   {
       public TimeZoneData TimeZone { get; set; }
       public double Version { get; set; }
       public ResponseStatus Status { get; set; }
    }
}
