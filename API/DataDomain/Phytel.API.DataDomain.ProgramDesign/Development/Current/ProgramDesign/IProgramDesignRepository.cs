using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public interface IProgramDesignRepository<T> : IRepository<T>
    {
        List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request);
        //IEnumerable<object> Find(string Id);
        object FindByName(string entityName);
    }
}
