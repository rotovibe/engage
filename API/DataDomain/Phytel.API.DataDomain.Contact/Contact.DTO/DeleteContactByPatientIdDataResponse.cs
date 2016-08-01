using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class DeleteContactByPatientIdDataResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public string DeletedId { get; set; }
        public List<ContactWithUpdatedRecentList> ContactWithUpdatedRecentLists { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class ContactWithUpdatedRecentList
    {
        public string ContactId { get; set; }
        public int PatientIndex { get; set; }
    }
}


