using System.Collections.Generic;
using Phytel.API.Interface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllPatientNotesResponse : IDomainResponse
    {
        public List<PatientNote> Notes { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
