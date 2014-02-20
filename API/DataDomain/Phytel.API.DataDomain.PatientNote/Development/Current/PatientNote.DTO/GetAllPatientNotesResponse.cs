using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    public class GetAllPatientNotesResponse : IDomainResponse
   {
        public List<PatientNote> PatientNotes { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
   }

}
