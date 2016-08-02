using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class UpdatePatientNoteResponse : IDomainResponse
    {
        public PatientNote PatientNote { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
