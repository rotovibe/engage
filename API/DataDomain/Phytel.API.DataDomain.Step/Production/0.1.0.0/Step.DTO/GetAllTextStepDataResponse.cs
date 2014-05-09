using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Step.DTO
{
    public class GetAllTextStepDataResponse : IDomainResponse
   {
        public List<TextData> Steps { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
