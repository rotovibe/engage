using Phytel.API.Interface;
using ServiceStack.ServiceHost;
using System.Collections.Generic;
using Phytel.API.DataDomain.PatientNote.DTO;

namespace Phytel.API.DataDomain.PatientNote.DTO
{
     [Route("/{Context}/{Version}/{ContractNumber}/Note/RemoveProgram/{ProgramId}/Update", "PUT")]
    public class RemoveProgramInPatientNotesDataRequest : IDataDomainRequest
    {
        [ApiMember(Name = "NoteId", Description = "NoteId", ParameterType = "property", DataType = "string", IsRequired = false)]
        public string NoteId { get; set; }

        [ApiMember(Name = "ProgramId", Description = "ProgramId used in Notes.", ParameterType = "property", DataType = "string", IsRequired = true)]
        public string ProgramId { get; set; }

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
