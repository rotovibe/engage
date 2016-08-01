using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class InterventionAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ControlType { get; set; } // need to change to enum type
    }
}
