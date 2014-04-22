using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using Phytel.API.DataDomain.Program.MongoDB.DTO;

namespace Phytel.API.DataDomain.Program
{
    public interface IProgramRepository<T> : IRepository<T>
    {
        List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request);
        IEnumerable<object> Find(string Id);
        DTO.Program FindByName(string entityName);
        object FindByPlanElementID(string entityID);
    }
}
