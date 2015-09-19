using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations.DTO
{
    public class RegistryCompleteMessage
    {
        public string ContractId { get; set; }
        public string ContractDataBase { get; set; }
        public string RunType { get; set; }
        public DateTime? ReportDate { get; set; }
    }
}
