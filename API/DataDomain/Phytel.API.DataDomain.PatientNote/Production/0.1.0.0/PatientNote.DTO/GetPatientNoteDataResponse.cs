using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    public class GetPatientNoteDataResponse : IDomainResponse
    {
        public PatientNoteData PatientNote { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class PatientNoteData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string Text { get; set; }
        public List<string> ProgramIds { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedById { get; set; }
    }
}
