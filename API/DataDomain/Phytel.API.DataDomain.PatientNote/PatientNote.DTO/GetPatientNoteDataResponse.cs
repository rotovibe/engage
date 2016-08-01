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

}
