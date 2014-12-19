
using System;
using ServiceStack.ServiceInterface.ServiceModel;
using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contract.DTO
{
    public class ContractData
    {
        public int ContractId { get; set; }
        public string ContractNumber { get; set; }
        public string ContractName { get; set; }
        public string StatusCode { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
