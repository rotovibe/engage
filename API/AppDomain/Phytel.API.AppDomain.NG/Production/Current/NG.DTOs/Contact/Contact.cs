using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Contact
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        public List<CommMode> Modes { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Email> Emails { get; set; }
        public List<Address> Addresses { get; set; }
        public List<int> WeekDays { get; set; }
        public List<string> TimesOfDaysId { get; set; }
        public string TimeZoneId { get; set; }
        public List<Language> Languages { get; set; }
        public string ExternalRecordId { get; set; }
        public string DataSource { get; set; }
        public int StatusId { get; set; }
        public int DeceasedId { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
       // public ContactType Type { get; set; }
        public bool IsPatient { get; set; }
        public bool IsUser { get; set; }
        public List<ContactSubType> ContactSubTypes { get; set; }
        public string ContactTypeId { get; set; }
        public string UpdatedById { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedOn { get; set; }

    }

    public class ContactSubType
    {
        public string Id { get; set; }
        public string SubTypeId { get; set; }
        public string SpecialtyId { get; set; }
        public List<string> SubSpecialtyIds { get; set; }
    }
}
