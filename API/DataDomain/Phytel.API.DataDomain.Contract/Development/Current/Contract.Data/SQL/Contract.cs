using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Contract.Data
{
    public partial class Contract
    {
        public int ContractID { get; set; }
        public string ContractNumber { get; set; }
        public string ContractName { get; set; }
        public string StatusCode { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
