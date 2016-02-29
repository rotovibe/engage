using System;
using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace AppDomain.Engage.Population.DTO.Demographics
{
    [Route("/{Version}/{ContractNumber}/Patients/{Sid}/Demographics", "POST")]
    public class PostPatientDemographicsRequest : IAppDomainRequest
    {
        [ApiMember(Name = "ListKey", Description = "Name of the list param.", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string ListKey { get; set; }

        [ApiMember(Name = "Sid", Description = "Name of the list param.", ParameterType = "body", DataType = "string", IsRequired = true)]
        public string Sid { get; set; }

        [ApiMember(Name = "FirstName", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string FirstName { get; set; }

        [ApiMember(Name = "MiddleName", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string MiddleName { get; set; }

        [ApiMember(Name = "LastName", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string LastName { get; set; }

        [ApiMember(Name = "Gender", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Gender { get; set; }

        [ApiMember(Name = "Dob", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Dob { get; set; }

        [ApiMember(Name = "Phones", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public List<Phone> Phones { get; set; }

        [ApiMember(Name = "Addresses", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public List<Address> Addresses { get; set; }

        [ApiMember(Name = "Emails", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public List<Email> Emails { get; set; }

        [ApiMember(Name = "Ssn", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Ssn { get; set; }

        [ApiMember(Name = "Mrn", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Mrn { get; set; }

        [ApiMember(Name = "CreatedOn", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public DateTime? CreatedOn { get; set; }

        [ApiMember(Name = "CreatedBy", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string CreatedBy { get; set; }

        [ApiMember(Name = "Version", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public double Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        [ApiMember(Name = "UserId", Description = "Name Description", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}
