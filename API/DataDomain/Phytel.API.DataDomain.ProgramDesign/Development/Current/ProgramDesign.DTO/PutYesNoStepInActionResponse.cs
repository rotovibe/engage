using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.ProgramDesign.DTO
{
    public class PutYesNoStepInActionResponse : IDomainResponse
    {
        public string Id { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
