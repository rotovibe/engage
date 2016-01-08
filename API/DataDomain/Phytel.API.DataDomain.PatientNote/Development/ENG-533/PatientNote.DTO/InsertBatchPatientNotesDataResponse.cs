using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    public class InsertBatchPatientNotesDataResponse : IDomainResponse
    {
        public List<HttpObjectResponse<PatientNoteData>> Responses { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
}
