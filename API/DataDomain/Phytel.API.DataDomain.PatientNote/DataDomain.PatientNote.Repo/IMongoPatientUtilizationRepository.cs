using System;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientNote.Repo
{
    public interface IMongoPatientUtilizationRepository
    {
        string ContractDBName { get; set; }
        string UserId { get; set; }
        object Insert(object newEntity);
        object InsertAll(List<object> entities);
        void Delete(object entity);
        void DeleteAll(List<object> entities);
        object FindByID(string entityID);
        Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression);
        IEnumerable<object> SelectAll();
        object Update(object entity);
        void CacheByID(List<string> entityIDs);
        IEnumerable<object> FindByPatientId(object request);
        void UndoDelete(object entity);
        void RemoveProgram(object entity, List<string> updatedProgramIds);
        IEnumerable<object> FindNotesWithAProgramId(string entityId);
    }
}