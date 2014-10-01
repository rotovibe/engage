using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.PatientObservation.DTO
{
    public class GetAllowedStatesDataResponse : IDomainResponse
    {
        public List<ObservationStateData> StatesData { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class ObservationStateData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> TypeIds { get; set; }
    }
}
