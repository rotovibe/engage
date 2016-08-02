using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.PatientNote.DTO.Response.Utilization
{
    public class PutPatientUtilizationDataResponse : IDomainResponse
    {
        public PatientUtilizationData Utilization { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
