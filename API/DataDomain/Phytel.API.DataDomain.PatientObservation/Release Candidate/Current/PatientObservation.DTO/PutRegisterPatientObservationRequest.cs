using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/Observation/Insert", "PUT")]
    public class PutRegisterPatientObservationRequest : IDataDomainRequest
    {
        [ApiMember(Name = "UserId", Description = "UserId of the logged in user. Not required.", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "PatientId", Description = "PatientId", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PatientId { get; set; }

        [ApiMember(Name = "Id", Description = "The Id of the observation.", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string Id { get; set; }

        [ApiMember(Name = "DisplayId", Description = "Display to set the observation to.", ParameterType = "body", DataType = "int", IsRequired = true)]
        public int DisplayId { get; set; }

        [ApiMember(Name = "StateId", Description = "State", ParameterType = "body", DataType = "int", IsRequired = true)]
        public int StateId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the Program", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }
    }
}
