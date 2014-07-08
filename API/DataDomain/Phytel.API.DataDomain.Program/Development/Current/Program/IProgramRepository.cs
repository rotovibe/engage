using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using MongoDB.Bson;

namespace Phytel.API.DataDomain.Program
{
    public interface IProgramRepository : IRepository
    {
        List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request);
        IEnumerable<object> Find(string Id);
        DTO.Program FindByName(string entityName);
        object FindByPlanElementID(string entityID);
        object GetLimitedProgramFields(string objectId);
        object InsertAsBatch(object newEntity);
        object FindByEntityExistsID(string patientID, string progId);
        IEnumerable<object> Find(List<ObjectId> Ids);
        bool Save(object entity);
        IEnumerable<object> FindByStepId(string entityID);
        List<Module> GetProgramModules(ObjectId progId);
        IEnumerable<object> FindByPatientId(string patientId);
    }
}
