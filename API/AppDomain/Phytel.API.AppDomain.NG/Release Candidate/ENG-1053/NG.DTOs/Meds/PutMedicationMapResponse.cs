using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PutMedicationMapResponse : IDomainResponse
    {
        public MedicationMap MedicationMap { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
