using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PutUpdatePatientDataResponse : IDomainResponse
    {
        public string Id { get; set; }
        public Outcome Outcome { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class Outcome
    {
        public int Result { get; set; }
        public string Reason { get; set; }
    }
}
