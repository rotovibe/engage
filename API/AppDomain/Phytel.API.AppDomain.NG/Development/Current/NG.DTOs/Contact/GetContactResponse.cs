using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetContactResponse : IDomainResponse
    {
        public Contact Contact { get; set; }
        public ResponseStatus Status { get; set; }
        public string Version { get; set; }
    }

    public class Contact
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string UserId { get; set; }
        public List<CommMode> Modes { get; set; }
        public List<Phone> Phones { get; set; }
        public List<Email> Emails { get; set; }
        public List<Address> Addresses { get; set; }
        public List<int> WeekDays { get; set; }
        public List<string> TimesOfDaysId { get; set; }
        public string TimeZoneId { get; set; }
        public List<Language> Languages { get; set; }

    }
}
