using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.Programs.ElementActivation;
using Phytel.API.DataDomain.Program.DTO;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG.Programs.ProgramAttributes
{
    public class ProgramAttributeStrategy : IProgramAttributeStrategy
    {
        private readonly List<IProgramAttributeRule> _rules;

        public ProgramAttributeStrategy()
        {
            _rules = new List<IProgramAttributeRule>
            {
                // add other implementations here
                new EligibilityAttributeRule()
            };
        }

        public void Run(SpawnElement r, Program program, string userId, ProgramAttributeData progAttr)
        {
            _rules.First(a => a.ProgramAttributeType == r.ElementType).Execute(r, program, userId, progAttr);
        }
    }
}
