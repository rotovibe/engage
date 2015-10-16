using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
    [Route("/{Context}/{Version}/{ContractNumber}/Batch/PatientNotes", "POST")]
    public class InsertBatchPatientNotesDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "PatientNotesData", Description = "List of PatientNotes to be inserted, if they exist, update them.", ParameterType = "property", DataType = "List<PatientNoteData>", IsRequired = true)]
        public List<PatientNoteData> PatientNotesData { get; set; }

        [ApiMember(Name = "UserId", Description = "UserId of the logged in user", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string UserId { get; set; }

        [ApiMember(Name = "Context", Description = "Product Context requesting the PatientNote", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string Context { get; set; }

        [ApiMember(Name = "ContractNumber", Description = "Contract Number to retrieve data from", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ContractNumber { get; set; }

        [ApiMember(Name = "Version", Description = "Version of the API being called", ParameterType = "property", DataType = "double", IsRequired = true)]
        public double Version { get; set; }
    }
}
