using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.C3User.DTO
{
    public class C3UserDataResponse
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public List<ContractInfo> Contracts { get; set; }
    }

    public class ContractInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
