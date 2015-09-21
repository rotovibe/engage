using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllSettingsResponse : IDomainResponse
    {
        public Dictionary<string,string> Settings { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }
}
