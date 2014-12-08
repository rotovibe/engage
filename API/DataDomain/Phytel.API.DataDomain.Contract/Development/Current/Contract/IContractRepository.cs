using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contract.DTO;
using Phytel.API.Interface;
using MongoDB.Bson;
using Phytel.API.Common.Audit;

namespace Phytel.API.DataDomain.Contract
{
    public interface IContractRepository : IRepository
    {
        object FindContracts(GetContractsDataRequest request);

        //object FindContractByPatientId(GetContractsDataRequest request);
        //IEnumerable<object> FindCareManagers();
        //bool UpdateRecentList(PutRecentPatientRequest request, List<string> recentList);
        //IEnumerable<object> SearchContracts(SearchContractsDataRequest request);
        //object FindContractByUserId(GetContractByUserIdDataRequest request);
        //IAuditHelpers AuditHelpers { get; set; }
        //object GetContracts(string patientId);
        //IEnumerable<object> FindContractsWithAPatientInRecentList(string entityId);
    }
}
