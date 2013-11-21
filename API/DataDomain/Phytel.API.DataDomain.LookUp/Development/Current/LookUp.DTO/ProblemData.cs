using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDomain.LookUp.DTO
{
    public class ProblemData
    {
        public string ProblemID { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Type { get; set; }
    }
}
