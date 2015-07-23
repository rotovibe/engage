using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientSystem.DTO
{
    public class InsertEngagePatientSystemsDataResponse : IDomainResponse
    {
        public List<string> Ids { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
