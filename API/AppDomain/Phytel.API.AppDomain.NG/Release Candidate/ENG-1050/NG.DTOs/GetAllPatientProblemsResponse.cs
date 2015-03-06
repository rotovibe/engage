using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.Interface;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class GetAllPatientProblemsResponse : IDomainResponse
    {
        public List<PatientProblem> PatientProblems { get; set; }
        public ResponseStatus Status { get; set; }
        public double Version { get; set; }
    }

    public class PatientProblem
    {
        public string PatientID { get; set; }
        public string ID { get; set; }
        public string ProblemID { get; set; }
        public int Level { get; set; }
    }
}
