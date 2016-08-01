using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class DereferencePatientDataResponse: IDomainResponse
    {
        public bool IsSuccessful { get; set; }        
        public List<ContactWithUpdatedRecentList> ContactWithUpdatedRecentLists { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    
    }
}
