﻿using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/api/{Context}/{Version}/{ContractNumber}/patient/{PatientId}/flagged/{Flagged}", "PUT")]
    public class PutPatientFlaggedRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient being updated", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "Flagged", Description = "Flagged value of the patient being updated", ParameterType = "property", DataType = "string", IsRequired = true)]
        public int Flagged { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context creating the patient", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        public PutPatientFlaggedRequest() { }
    }
}