using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.PatientNote.DTO.Response.Utilization
{
    public class GetPatientUtilizationDataResponse : IDomainResponse
    {
        public string Id { get; set; }
        public double Version { get; set; }
        public PatientUtilizationData Utilization { get; set; } 
        public ResponseStatus Status { get; set; }
    }
}
