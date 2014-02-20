using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    public class GetPatientNoteResponse : IDomainResponse
    {
        public PatientNote PatientNote { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class PatientNote
    {
        public string PatientNoteID { get; set; }
        public string Version { get; set; }
    }
}
