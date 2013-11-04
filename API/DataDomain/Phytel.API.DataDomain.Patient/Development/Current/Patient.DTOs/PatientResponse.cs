using System.Collections.Generic;

namespace Phytel.API.DataDomain.Patient.DTO
{
    public class PatientResponse
    {
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
