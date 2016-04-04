using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Test
{
    public class StubMedicationMappingRepository<TContext> : IMongoMedicationRepository, IMongoMedicationMappingRepository where TContext : MedicationMongoContext
    {
        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }

        public StubMedicationMappingRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public StubMedicationMappingRepository(TContext context)
        {
            Context = context;
        }
        
        public object FindByPatientId(object request)
        {
            throw new NotImplementedException();
        }

        public object FindNDCCodes(object request)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            return new MedicationMapData { Id = "532371e4072ef721b8b05b97", Custom = true, Verified = false, FullName = "TESLA", TTLDate = DateTime.Now };
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            return new MedicationMapData { Id = "532371e4072ef721b8b05b97", Custom = true, Verified = false, FullName = "TESLA1", TTLDate = null };
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            return true;
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }

        public object Search(object request)
        {
            throw new NotImplementedException();
        }


        public object FindByName(object request)
        {
            throw new NotImplementedException();
        }

        public object Find(object request)
        {
            throw new NotImplementedException();
        }
    }
}
