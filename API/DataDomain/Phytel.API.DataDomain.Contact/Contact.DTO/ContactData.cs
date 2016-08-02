using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class ContactData : IAppData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public string Gender { get; set; }
        public List<CommModeData> Modes { get; set; }
        public List<PhoneData> Phones { get; set; }
        public List<EmailData> Emails { get; set; }
        public List<AddressData> Addresses { get; set; }
        public List<int> WeekDays { get; set; }
        public List<string> TimesOfDaysId { get; set; }
        public string TimeZoneId { get; set; }
        public List<LanguageData> Languages { get; set; }
        public List<string> RecentsList { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedById { get; set; }
        public string CreatedById { get; set; }
        public string ExternalRecordId { get; set; }
        public string DataSource { get; set; }
        public int StatusId { get; set; }
        public int DeceasedId { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public List<ContactSubTypeData> ContactSubTypesData { get; set; }
        public string ContactTypeId { get; set; }
    }
}


