using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class GetContactDataResponse : IDomainResponse
    {
        public ContactData Contact { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class ContactData
    {
        public string ContactId { get; set; }
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
    }
}
