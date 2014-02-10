using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceHost;

namespace Phytel.API.DataDomain.Contact.DTO
{
    [Api(Description = "A Request object to insert a new contact.")]
    [Route("/{Context}/{Version}/{ContractNumber}/Patient/Contact/{PatientId}", "PUT")]
    public class PutContactDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientId", Description = "Id of the patient for whom Contact is inserted.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string PatientId { get; set; }

        [ApiMember(Name = "Modes", Description = "List of CommModes being updated", ParameterType = "property", DataType = "List<CommModeData>", IsRequired = false)]
        public List<CommModeData> Modes { get; set; }

        [ApiMember(Name = "WeekDays", Description = "List of Week of days being updated", ParameterType = "property", DataType = "List<int>", IsRequired = false)]
        public List<int> WeekDays { get; set; }

        [ApiMember(Name = "TimesOfDaysId", Description = "List of Times of days being updated", ParameterType = "property", DataType = "List<string>", IsRequired = false)]
        public List<string> TimesOfDaysId { get; set; }

        [ApiMember(Name = "TimeZoneId", Description = "ID of timezone being updated", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string TimeZoneId { get; set; }

        [ApiMember(Name = "Languages", Description = "List of langugages being updated", ParameterType = "property", DataType = "List<LanguageData>", IsRequired = false)]
        public List<LanguageData> Languages { get; set; }

        [ApiMember(Name = "Phones", Description = "List of Phones being updated", ParameterType = "property", DataType = "List<PhoneData>", IsRequired = false)]
        public List<PhoneData> Phones { get; set; }

        [ApiMember(Name = "Emails", Description = "List of Emails being updated", ParameterType = "property", DataType = "List<EmailData>", IsRequired = false)]
        public List<EmailData> Emails { get; set; }

        [ApiMember(Name = "Addresses", Description = "List of Addresses being updated", ParameterType = "property", DataType = "List<AddressData>", IsRequired = false)]
        public List<AddressData> Addresses { get; set; }
        [ApiMember(Name = "Context", Description = "Product Context requesting the Contact", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Version { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }
    }
}

