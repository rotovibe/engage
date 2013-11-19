using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    public class GetAllPatientProblemResponse
    {
        public List<PProb> PatientProblems { get; set; }
        public string Version { get; set; }
        public ResponseStatus Status { get; set; }
    }
    
    public class PProb
    {
        public string PatientID { get; set; }
        public string ID { get; set; }
        public string ProblemID { get; set; }
    }
}
