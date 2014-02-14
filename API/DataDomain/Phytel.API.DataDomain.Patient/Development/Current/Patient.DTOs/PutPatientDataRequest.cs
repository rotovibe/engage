using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using Phytel.API.DataDomain.Contact.DTO;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/patient", "PUT")]
    public class PutPatientDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "FirstName", Description = "First Name of the patient being created", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string FirstName { get; set; }

        [ApiMember(Name = "LastName", Description = "Last name of the patient being created", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string LastName { get; set; }

        [ApiMember(Name = "MiddleName", Description = "Middle name of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string MiddleName { get; set; }

        [ApiMember(Name = "Suffix", Description = "Suffix of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Suffix { get; set; }

        [ApiMember(Name = "PreferredName", Description = "Preferred name of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string PreferredName { get; set; }

        [ApiMember(Name = "Gender", Description = "Gender of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Gender { get; set; }

        [ApiMember(Name = "DOB", Description = "Date of birth of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string DOB { get; set; }

        [ApiMember(Name = "SystemID", Description = "System ID of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string SystemID { get; set; }

        [ApiMember(Name = "SystemName", Description = "System name of the patient being created", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string SystemName { get; set; }

        [ApiMember(Name = "TimeZone", Description = "Time zone being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string TimeZone { get; set; }

        [ApiMember(Name = "Phone1", Description = "First phone number being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Phone1 { get; set; }

        [ApiMember(Name = "Phone1Preferred", Description = "Preference of first phone number being inserted", ParameterType = "property", DataType = "boolean", IsRequired = false)]
        public bool Phone1Preferred { get; set; }

        [ApiMember(Name = "Phone1Type", Description = "Type of the first phone number being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Phone1Type { get; set; }

        [ApiMember(Name = "Phone2", Description = "Second phone number being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Phone2 { get; set; }

        [ApiMember(Name = "Phone2Preferred", Description = "Preference of second phone number being inserted", ParameterType = "property", DataType = "boolean", IsRequired = false)]
        public bool Phone2Preferred { get; set; }

        [ApiMember(Name = "Phone2Type", Description = "Type of the second phone number being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Phone2Type { get; set; }

        [ApiMember(Name = "Email1", Description = "First email being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Email1 { get; set; }

        [ApiMember(Name = "Email1Preferred", Description = "Preference of first email being inserted", ParameterType = "property", DataType = "boolean", IsRequired = false)]
        public bool Email1Preferred { get; set; }

        [ApiMember(Name = "Email1Type", Description = "Type of the first email being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Email1Type { get; set; }

        [ApiMember(Name = "Email2", Description = "Second email being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Email2 { get; set; }

        [ApiMember(Name = "Email2Preferred", Description = "Preference of second email being inserted", ParameterType = "property", DataType = "boolean", IsRequired = false)]
        public bool Email2Preferred { get; set; }

        [ApiMember(Name = "Email2Type", Description = "Type of the second email being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Email2Type { get; set; }

        [ApiMember(Name = "Address1Line1", Description = "First line of the first address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address1Line1 { get; set; }

        [ApiMember(Name = "Address1Line2", Description = "Second line of the first address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address1Line2 { get; set; }

        [ApiMember(Name = "Address1Line3", Description = "Third line of the first address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address1Line3 { get; set; }

        [ApiMember(Name = "Address1City", Description = "City of the first address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address1City { get; set; }

        [ApiMember(Name = "Address1State", Description = "State of the first address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address1State { get; set; }

        [ApiMember(Name = "Address1Zip", Description = "Zip code of the first address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address1Zip { get; set; }

        [ApiMember(Name = "Address1Preferred", Description = "Third line of the first address being inserted", ParameterType = "property", DataType = "bool", IsRequired = false)]
        public bool Address1Preferred { get; set; }

        [ApiMember(Name = "Address1Type", Description = "Type of the first address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address1Type { get; set; }

        [ApiMember(Name = "Address2Line1", Description = "First line of the second address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address2Line1 { get; set; }

        [ApiMember(Name = "Address2Line2", Description = "Second line of the second address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address2Line2 { get; set; }

        [ApiMember(Name = "Address2Line3", Description = "Third line of the second address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address2Line3 { get; set; }

        [ApiMember(Name = "Address2City", Description = "City of the second address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address2City { get; set; }

        [ApiMember(Name = "Address2State", Description = "State of the second address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address2State { get; set; }

        [ApiMember(Name = "Address2Zip", Description = "Zip code of the second address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address2Zip { get; set; }

        [ApiMember(Name = "Address2Preferred", Description = "Third line of the second address being inserted", ParameterType = "property", DataType = "bool", IsRequired = false)]
        public bool Address2Preferred { get; set; }

        [ApiMember(Name = "Address2Type", Description = "Type of the second address being inserted", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string Address2Type { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context creating the patient", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Version { get; set; }

        [ApiMember(Name = "UserID", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }
    }
}