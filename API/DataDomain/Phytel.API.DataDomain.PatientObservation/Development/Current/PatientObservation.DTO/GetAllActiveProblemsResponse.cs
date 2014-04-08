using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class GetAllActiveProblemsResponse : IDomainResponse
    {
        public List<ObservationLibraryItemData> Library { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
