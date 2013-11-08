using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PatientProblemResponse
    {
        public List<PatientProblem> PatientProblems { get; set; }
        public ResponseStatus Status { get; set; }
    }

    public class PatientProblem
    {
        public string PatientID { get; set; }
        public string ProblemID { get; set; }
        public string DisplayName { get; set; }
    }
}
