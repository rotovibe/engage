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
        public List<CommMode> Modes { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Text> Texts { get; set; }
        public List<Email> Emails { get; set; }
        public List<Address> Addresses { get; set; }
        public List<int> WeekDays { get; set; }
        public List<string> TimesOfDaysId { get; set; }
        public string TimeZoneId { get; set; }
        public List<Language> Languages { get; set; }
    }
}
