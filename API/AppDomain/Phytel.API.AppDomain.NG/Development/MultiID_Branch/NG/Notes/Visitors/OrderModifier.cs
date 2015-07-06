using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Notes.Visitors
{
    public class OrderModifier : ModifierBase
    {
        public override List<PatientNote> Modify(ref List<PatientNote> result)
        {
            result.OrderByDescending(r => r.CreatedOn).ToList();

            return result;
        }
    }
}
