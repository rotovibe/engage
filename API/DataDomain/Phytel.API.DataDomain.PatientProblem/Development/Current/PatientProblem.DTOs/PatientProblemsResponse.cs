using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    public class PatientProblemsResponse
    {
        public List<Problem> PatientProblems { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
    
    public class Problem
    {
        public string PatientID { get; set; }
        public string ProblemID { get; set; }
        public string ConditionID { get; set; }
    }
}
