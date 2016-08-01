using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class GetAllSettingsDataResponse : IDomainResponse
   {
       public Dictionary<string, string> SettingsData { get; set; }
       public double Version { get; set; }
       public ResponseStatus Status { get; set; }
    }
}
