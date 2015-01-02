using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.ServiceStack.DTO
{
    public interface IEffectiveDateRequest
    {
        DateTime? EffectiveDate { get; set; }
    }
}
