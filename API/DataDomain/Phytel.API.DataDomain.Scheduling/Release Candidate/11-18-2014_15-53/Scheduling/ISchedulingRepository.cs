using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Scheduling
{
    public interface ISchedulingRepository : IRepository
    {
        IEnumerable<object> FindToDos(object request);
        IEnumerable<object> FindToDosWithAProgramId(string entityId);
        void RemoveProgram(object entity, List<string> updatedProgramIds);
        object FindByID(string entityID, bool includeDeletedToDo);
    }
}
