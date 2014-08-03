using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class DeletePatientProgramAttributesDataResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public string DeletedId { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
