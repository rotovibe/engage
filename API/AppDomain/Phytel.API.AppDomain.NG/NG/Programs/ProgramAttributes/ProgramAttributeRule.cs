using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Program = Phytel.API.AppDomain.NG.DTO.Program;

namespace Phytel.API.AppDomain.NG.Programs.ProgramAttributes
{
    public abstract class ProgramAttributeRule
    {
        public abstract void Execute(SpawnElement r, Program program, string userId, ProgramAttributeData progAttr);
    }
}
