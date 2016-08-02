using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Patient
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Suffix { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string DisplaySystemId { get; set; }
        public string DisplaySystemName { get; set; }
        public string DisplayLabel { get; set; }
        public int Priority { get; set; }
        public int Flagged { get; set; }
        public string Background { get; set; }
        public string ClinicalBackground { get; set; }
        public string LastFourSSN { get; set; }
        public string FullSSN { get; set; }
        public string DataSource { get; set; }
        public string ReasonId { get; set; }
        public int StatusId { get; set; }
        public string StatusDataSource { get; set; }
        public string MaritalStatusId { get; set; }
        public bool Protected { get; set; }
        public int DeceasedId { get; set; }
        public string Prefix { get; set; }
        public string ContactId { get; set; }
    }
}
