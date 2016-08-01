using System.Collections.Generic;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Notes.Visitors
{
    public class OrderModifier : ModifierBase
    {
        public override List<PatientNote> Modify(ref List<PatientNote> result)
        {
            result = result.OrderByDescending(r => r.CreatedOn.Date).ThenByDescending(r => r.CreatedOn.TimeOfDay).ToList();

            return result;
        }
    }
}
