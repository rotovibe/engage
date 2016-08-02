using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllTimeZonesResponse : IDomainResponse
    {
        public List<TimeZonesLookUp> TimeZones { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }

    public class TimeZonesLookUp : IdNamePair
    {
        public bool DefaultZone { get; set; }
    }
}
