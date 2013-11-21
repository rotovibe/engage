using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Cohort.DTO
{
    public class CohortData
    {
        public string ID { get; set; }
        public string SName { get; set; }
        public string Query { get; set; }
        public string QueryWithFilter { get; set; }
        public string Sort { get; set; }
    }
}
