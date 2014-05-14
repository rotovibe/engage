using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class PutRecentPatientResponse : IDomainResponse
    {
        public int Limit { get; set; }
        public List<string> Recent { get; set; }
        public bool SuccessData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
