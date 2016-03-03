using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Phytel.API.DataDomain.Contact.DTO
{
    public class CommModeData
    {
        public string ModeId { get; set; }
        public bool Preferred { get; set; }
        public bool OptOut { get; set; }
    }
}
