using System.Collections.Generic;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Notes.Visitors
{
    public class OrderModifier : ModifierBase
    {
        public override List<PatientNote> Modify(List<PatientNote> result)
        {
            var list = result.OrderByDescending(r => r.CreatedOn).ToList();

            return list;
        }
    }
}
