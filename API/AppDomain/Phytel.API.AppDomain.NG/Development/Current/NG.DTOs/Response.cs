using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.DTO
{
    public class Response
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public string StepID { get; set; }
        public bool Nominal { get; set; }
        public string ResponsePathId { get; set; }
    }
}
