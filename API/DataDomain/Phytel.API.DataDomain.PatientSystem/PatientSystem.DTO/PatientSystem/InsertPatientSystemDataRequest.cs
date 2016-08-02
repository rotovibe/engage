using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystem", "POST")]
    public class InsertPatientSystemDataRequest : IDataDomainRequest
    {
       [ApiMember(Name = "PatientId", Description = "Id of the patient whose PatientSystem record needs to be inserted", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

       [ApiMember(Name = "IsEngageSystem", Description = "Deteremines whether the PatientSystem record to be inserted is System or not.", ParameterType = "property", DataType = "bool", IsRequired = true)]
       public bool IsEngageSystem { get; set; }

        [ApiMember(Name = "PatientSystemsData", Description = "PatientSystem object that need to be inserted.", ParameterType = "property", DataType = "PatientSystemData", IsRequired = true)]
        public PatientSystemData PatientSystemsData { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientSystem", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = false)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}
