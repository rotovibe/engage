using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllTimeZonesDataResponse : IDomainResponse
   {
        public List<TimeZoneData> TimeZones { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class TimeZoneData : LookUpData
    {
        public bool Default { get; set; }
    }
}
