using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class DeletePatientProgramByPatientIdDataResponse : IDomainResponse
    {
        public bool Success { get; set; }
        public List<DeletedPatientProgram> DeletedPatientPrograms { get; set; }
        public double Version { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class DeletedPatientProgram
    {
        public string Id { get; set; }
        public string PatientProgramAttributeId { get; set; }
        public List<string> PatientProgramResponsesIds { get; set; }
    }
}
