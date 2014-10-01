using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class PutInsertResponseResponse : IDomainResponse
    {
        public bool Result { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
