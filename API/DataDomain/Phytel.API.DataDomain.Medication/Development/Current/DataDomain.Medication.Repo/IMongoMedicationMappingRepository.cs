using System;
using System.Collections.Generic;

namespace DataDomain.Medication.Repo
{
    public interface IMongoMedicationMappingRepository
    {
        string ContractDBName { get; set; }
        string UserId { get; set; }
        object Insert(object newEntity);
        object InsertAll(List<object> entities);
        void Delete(object entity);
        void DeleteAll(List<object> entities);
        object FindByID(string entityID);
        Tuple<string, IEnumerable<object>> Select(Phytel.API.Interface.APIExpression expression);
        IEnumerable<object> SelectAll();
        object Update(object entity);
        void CacheByID(List<string> entityIDs);
        void UndoDelete(object entity);
        object FindByPatientId(object request);
        object FindNDCCodes(object request);
        object Initialize(object newEntity);
    }
}