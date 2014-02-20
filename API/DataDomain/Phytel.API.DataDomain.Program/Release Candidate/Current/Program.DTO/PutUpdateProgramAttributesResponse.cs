using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class PutUpdateProgramAttributesResponse : IDomainResponse
    {
        public bool Result { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
