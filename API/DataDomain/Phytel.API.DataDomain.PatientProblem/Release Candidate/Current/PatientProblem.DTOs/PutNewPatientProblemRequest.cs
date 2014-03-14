using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Problem/Insert", "PUT")]
    public class PutNewPatientProblemRequest : IDataDomainRequest
    {
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user. Not required.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "ProblemId", Description = "The Id of the problem.", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string ProblemId { get; set; }

        [ApiMember(Name = "Active", Description = "Is the problem active?", ParameterType = "body", DataType = "bool", IsRequired = true)]
        public bool Active { get; set; }

        [ApiMember(Name = "Featured", Description = "Is the problem featured?", ParameterType = "body", DataType = "bool", IsRequired = true)]
        public bool Featured { get; set; }

        [ApiMember(Name = "Level", Description = "Level to set the problem to.", ParameterType = "body", DataType = "int", IsRequired = true)]
        public int Level { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}
