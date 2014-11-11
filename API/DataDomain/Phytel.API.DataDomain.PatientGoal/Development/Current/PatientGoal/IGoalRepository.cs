using System.Collections.Generic;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.PatientGoal
{
    public interface IGoalRepository : IRepository
    {
        object Initialize(object newEntity);
        IEnumerable<object> Find(string Id);
        IEnumerable<object> FindGoalsWithAProgramId(string entityId);
        void RemoveProgram(object entity, List<string> updatedProgramIds);
        IEnumerable<object> Search(object request, List<string> patientGoalIds);
        object FindByTemplateId(string patientId, string entityID);
    }
}
