using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Allergy.DTO
{
    public class DeleteAllergiesByPatientIdDataResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public List<string> DeletedIds { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
