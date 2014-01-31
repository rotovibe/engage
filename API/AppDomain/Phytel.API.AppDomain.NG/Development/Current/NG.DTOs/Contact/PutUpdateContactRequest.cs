using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.AppDomain.NG.DTO
{
    [Api(Description = "A Request object to update the contact card details.")]
    [Route("/{Version}/{ContractNumber}/Contact", "POST")]
    [Route("/{Version}/{ContractNumber}/Patient/Contact", "POST")]
    public class PutUpdateContactRequest : IAppDomainRequest
    {
        [ApiMember(Name = "ContactId", Description = "ID of the Contact being updated", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContactId { get; set; }

        [ApiMember(Name = "Modes", Description = "List of CommModes being updated", ParameterType = "property", DataType = "List<CommModeData>", IsRequired = false)]
        public List<CommMode> Modes { get; set; }

        [ApiMember(Name = "WeekDays", Description = "List of Week of days being updated", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<int> WeekDays { get; set; }

        [ApiMember(Name = "TimesOfDaysId", Description = "List of Times of days being updated", ParameterType = "property", DataType = "List<string>", IsRequired = false)]
        public List<string> TimesOfDaysId { get; set; }

        [ApiMember(Name = "TimeZoneId", Description = "ID of timezone being updated", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string TimeZoneId { get; set; }

        [ApiMember(Name = "Languages", Description = "List of langugages being updated", ParameterType = "property", DataType = "List<LanguageData>", IsRequired = false)]
        public List<Language> Languages { get; set; }

        [ApiMember(Name = "Phones", Description = "List of Phones being updated", ParameterType = "property", DataType = "List<PhoneData>", IsRequired = false)]
        public List<Phone> Phones { get; set; }

        [ApiMember(Name = "Emails", Description = "List of Emails being updated", ParameterType = "property", DataType = "List<EmailData>", IsRequired = false)]
        public List<Email> Emails { get; set; }

        [ApiMember(Name = "Addresses", Description = "List of Addresses being updated", ParameterType = "property", DataType = "List<AddressData>", IsRequired = false)]
        public List<Address> Addresses { get; set; }

        [ApiMember(Name = "UserId", Description = "ID of the user making the request (Internally used ONLY)", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string UserId { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Version { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "path", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Token", Description = "Request Token", ParameterType = "QueryString", DataType = "string", IsRequired = true)]
        public string Token { get; set; }

        public PutUpdateContactRequest() { }
    }
}

