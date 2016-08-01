using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.MongoDB.DTO;

namespace Phytel.Data.ETL
{
    public class SpawnElementHash
    {
        public int SqlId { get; set; }
        public string PlanElementId { get; set; }
        public SpawnElement SpawnElem { get; set; }
    }
}
