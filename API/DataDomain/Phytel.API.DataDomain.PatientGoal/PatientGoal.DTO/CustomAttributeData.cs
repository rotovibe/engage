using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.PatientGoal.DTO
{
    public class CustomAttributeData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public List<string> Values { get; set; }
        public int ControlType { get; set; }
        public int Order { get; set; }
        public Dictionary<int, string> Options { get; set; }
        public bool Required { get; set; }
    }
}
