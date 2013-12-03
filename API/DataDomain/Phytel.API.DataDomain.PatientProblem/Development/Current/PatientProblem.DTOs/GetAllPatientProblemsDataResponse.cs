using System.Collections.Generic;
using ServiceStack;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    public class GetAllPatientProblemsDataResponse : IDomainResponse
    {
        public List<PatientProblemData> PatientProblems { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
   
}
