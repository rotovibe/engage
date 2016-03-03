using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp.DTO
{
    public class ProblemData: IdNamePair
    {
        public bool Active { get; set; }
        public string Type { get; set; }
    }
}
