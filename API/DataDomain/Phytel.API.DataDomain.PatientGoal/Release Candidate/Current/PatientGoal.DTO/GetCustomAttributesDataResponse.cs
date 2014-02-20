using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class GetCustomAttributesDataResponse : IDomainResponse
   {
        public List<CustomAttributeData> CustomAttributes { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }
}
