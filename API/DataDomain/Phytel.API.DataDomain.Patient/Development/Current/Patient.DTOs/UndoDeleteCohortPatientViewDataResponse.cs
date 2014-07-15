using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class UndoDeleteCohortPatientViewDataResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
