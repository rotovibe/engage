using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class PlanElements
    {
        public List<Program> Programs { get; set; }
        public List<Module> Modules { get; set; }
        public List<Actions> Actions { get; set; }
        public List<Step> Steps { get; set; }
        public ProgramAttribute Attributes { get; set; }

        public PlanElements()
        {
            Programs = new List<Program>();
            Modules = new List<Module>();
            Actions = new List<Actions>();
            Steps = new List<Step>();
        }
    }
}
