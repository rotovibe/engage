using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Notes.Visitors
{
    public abstract class ModifierBase
    {
        public abstract List<PatientNote> Modify(ref List<PatientNote> list);
    }
}
