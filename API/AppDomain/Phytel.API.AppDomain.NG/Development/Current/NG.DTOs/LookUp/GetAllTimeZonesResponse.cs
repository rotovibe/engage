using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllTimeZonesResponse : IDomainResponse
    {
        public List<TimeZonesLookUp> TimeZones { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }

    public class TimeZonesLookUp : LookUp
    {
        public bool Default { get; set; }
    }
}
