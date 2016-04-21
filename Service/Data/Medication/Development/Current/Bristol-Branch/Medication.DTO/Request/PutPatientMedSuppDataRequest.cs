using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Medication.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/PatientMedSupp/Save", "PUT")]
    public class PutPatientMedSuppDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientMedSuppData", Description = "PatientMedSupp that need to be updated", ParameterType = "property", DataType = "PatientMedSuppData", IsRequired = true)]
        public PatientMedSuppData PatientMedSuppData { get; set; }

        [ApiMember(Name = "Insert", Description = "Indicates if the action is to create or update a patient med/supp", ParameterType = "path", DataType = "boolean", IsRequired = false)]
        public bool Insert { get; set; }

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
