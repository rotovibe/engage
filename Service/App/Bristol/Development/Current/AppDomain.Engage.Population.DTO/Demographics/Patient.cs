using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace AppDomain.Engage.Population.DTO.Demographics
{
    public class Patient
    {
        [ApiMember(Name = "ListKey", Description = "ListKey")]
        public string ListKey { get; set; }
        [ApiMember(Name = "Sid", Description = "Sid")]
        public string Sid { get; set; }
        [ApiMember(Name = "FirstName", Description = "FirstName", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string FirstName { get; set; }

        [ApiMember(Name = "MiddleName", Description = "MiddleName", ParameterType = "property", DataType = "string")]
        public string MiddleName { get; set; }

        [ApiMember(Name = "LastName", Description = "LastName", ParameterType = "property", DataType = "string",IsRequired = true)]
        public string LastName { get; set; }
        // these fields added for PB-102

        [ApiMember(Name = "PreferredName", Description = "PreferredName", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PreferredName { get; set; }

        [ApiMember(Name = "Prefix", Description = "Prefix", ParameterType = "property", DataType = "string")]
        public string Prefix { get; set; }
        [ApiMember(Name = "Suffix", Description = "Suffix", ParameterType = "property", DataType = "string")]

        public string Suffix { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string MaritalStatusId { get; set; }
        public int PriorityData { get; set; }
        public int DeceasedId { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Email> Emails { get; set; }
        public string FullSsn { get; set; }

        public string LastFourSSN { get; set; }
        public string Mrn { get; set; }

        public string ExternalRecordId { get; set; }

        public string DataSource { get; set; }

        public string StatusCode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        

    }
}
