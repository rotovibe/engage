using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientProblem.DTO
{
    public class PatientProblemData
    {
        public string ID { get; set; }
        public string PatientID { get; set; }
        public string ProblemID { get; set; }
        public int Level { get; set; }
        public bool Active { get; set; }
    }
}
