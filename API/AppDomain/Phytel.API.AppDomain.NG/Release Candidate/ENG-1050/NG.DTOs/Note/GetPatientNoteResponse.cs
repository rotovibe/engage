using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetPatientNoteResponse : IDomainResponse
    {
        public PatientNote Note { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
