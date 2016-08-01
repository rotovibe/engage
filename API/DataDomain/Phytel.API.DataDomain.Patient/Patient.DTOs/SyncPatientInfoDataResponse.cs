using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class SyncPatientInfoDataResponse : IDomainResponse
    {
        public bool IsSuccessful { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
