using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllTimeZonesDataResponse : IDomainResponse
   {
        public List<TimeZoneData> TimeZones { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class TimeZoneData : IdNamePair
    {
        public bool Default { get; set; }
    }
}
