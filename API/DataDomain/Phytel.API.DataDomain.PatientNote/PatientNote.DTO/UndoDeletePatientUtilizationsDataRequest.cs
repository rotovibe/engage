using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/PatientUtilization/UndoDelete", "PUT")]
    public class UndoDeletePatientUtilizationsDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "Ids", Description = "PatientUtilization Ids that need to be un-deleted.", ParameterType = "property", DataType = "List<string>", IsRequired = true)]
        public List<string> Ids { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientUtilization", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
