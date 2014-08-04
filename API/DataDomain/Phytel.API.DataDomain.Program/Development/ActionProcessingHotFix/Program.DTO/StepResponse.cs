using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.DTO
{
    public class StepResponse
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public string StepId { get; set; }
        public string Value { get; set; }
        public bool Nominal { get; set; }
        public bool Required { get; set; }
        public string NextStepId { get; set; }
        public List<SpawnElementDetail> Spawn { get; set; }
    }
}
