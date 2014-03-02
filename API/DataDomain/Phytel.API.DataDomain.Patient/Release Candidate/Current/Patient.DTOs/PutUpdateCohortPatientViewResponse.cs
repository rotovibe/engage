using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PutUpdateCohortPatientViewResponse : IDomainResponse
    {
        public string CohortPatientViewId { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
