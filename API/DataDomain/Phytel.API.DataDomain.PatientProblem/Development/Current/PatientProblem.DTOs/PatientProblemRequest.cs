using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    [Route("/{Context}/{Version}/Contract/{ContractNumber}/patientproblem/{PatientID}", "GET")]
    public class PatientProblemRequest: IDataDomainRequest, IReturn<List
        <PatientProblemResponse>>
    {
        
        [ApiMember(Name = "PatientID", Description = "ID parameter", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string PatientID { get; set; }

        [ApiMember(Name = "Context", Description = "Product parameter will be defined in the route.", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract parameter will be defined in the route.", ParameterType = "path", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the Request", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Version { get; set; }

    }
}
