using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Test
{
    class StubPatientMedSuppRepository<TContext> : IMongoMedicationRepository, IMongoPatientMedSuppRepository where TContext : MedicationMongoContext
    {
        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }

        public StubPatientMedSuppRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public StubPatientMedSuppRepository(TContext context)
        {
            Context = context;
        }
        
        public object FindByPatientId(object request)
        {
            List<PatientMedSuppData> results = new List<PatientMedSuppData>();
            results.Add(new PatientMedSuppData { CategoryId = 1, Dosage = "2", EndDate = DateTime.Now, Form = "TABLET", Id = "54bdde96fe7a5b27384a970b", Name = "ASPIRIN", Notes = "test notes 1", PatientId = "54bdde96fe7a5b27384a9b76", Route = "", StatusId = 1, Strength = "30 mg" });
            results.Add(new PatientMedSuppData { CategoryId = 2, Dosage = "10", EndDate = DateTime.Now, Form = "TABLET", Id = "54bdde96fe7a5b27384aab1c", Name = "TYLENOL PM", Notes = "test notes 2", PatientId = "54bdde96fe7a5b27384a9b76", Route = "", StatusId = 1, Strength = "25 mg" });
            return results;
        }

        public object FindNDCCodes(object request)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object Insert(object newEntity)
        {
            return new PatientMedSuppData { 
                CategoryId = 1,
                CreatedOn = DateTime.Now,
                DeleteFlag = false,
                Dosage = "2",
                EndDate = DateTime.Now.AddDays(10),
                Form = "ORAL",
                Id = "54bdde96fe7a5b27384aaad9",
                Name = "ADVIL",
                PatientId = "54bdde96fe7a5b27384a9551",
                Reason = "test reason",
                Route = "ORAL",
                StatusId = 1,
                Strength = "30 mg",
                SystemName = "Engage"
            };
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            // return nothing.
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            return new PatientMedSuppData
            {
                CategoryId = 1,
                CreatedOn = DateTime.Now,
                DeleteFlag = false,
                Dosage = "2",
                EndDate = DateTime.Now.AddDays(10),
                Form = "ORAL",
                Id = "54bdde96fe7a5b27384aaad9",
                Name = "ADVIL",
                PatientId = "54bdde96fe7a5b27384a9551",
                Reason = "test reason",
                Route = "ORAL",
                StatusId = 1,
                Strength = "30 mg",
                SystemName = "Engage"
            };
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
            // return nothing.
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
