using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    public class DeletePatientNoteDataResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
