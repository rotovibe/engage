using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Allergy.Repo;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy.Test
{
    public class StubAllergyRepository<TContext> : IMongoAllergyRepository where TContext : AllergyMongoContext
    {

        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string userId { get; set; }

        public StubAllergyRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public StubAllergyRepository(TContext context)
        {
            Context = context;
        }

        public StubAllergyRepository(string dbName)
        {
            ContractDBName = dbName;
        }
        
        public object Initialize(object newEntity)
        {
            PutInitializeAllergyDataRequest request = (PutInitializeAllergyDataRequest)newEntity;
            AllergyData data = null;
            MEAllergy meA = new MEAllergy(this.UserId)
            {
                Name = request.AllergyName,
                TTLDate = System.DateTime.UtcNow.AddDays(1),
                DeleteFlag = false
            };
            data = new AllergyData
            {
                Id = meA.Id.ToString(),
                Name = meA.Name
            };
            return data;
        }

        public object FindByPatientId(object request)
        {
 	        throw new NotImplementedException();
        }

        public string UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
            }
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
 	        return new AllergyData {
                DeleteFlag = false,
                Id = "5453cea0d433232a387d51b9",
                Name = "allergyName",
                TypeIds = new List<string> { "5447d6ddfe7a59146485b512", "5446db5efe7a591e74013b6b", "5446db5efe7a591e74013b6c" },
                Version = 1.0
            };
        }

        public Tuple<string,IEnumerable<object>> Select(Interface.APIExpression expression)
        {
 	        throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            List<AllergyData> data = new List<AllergyData>();
            data.Add( 
                new AllergyData  {
                    DeleteFlag = false,
                    Id = "5453cea0d433232a387d51b9",
                    Name = "allergyName",
                    TypeIds = new List<string> { "5447d6ddfe7a59146485b512", "5446db5efe7a591e74013b6b", "5446db5efe7a591e74013b6c" },
                    Version = 1.0
            });
            data.Add(
                new AllergyData
                {
                    DeleteFlag = false,
                    Id = "5453cea0d433232a387d6789",
                    Name = "AllergyName1",
                    TypeIds = new List<string> { "5447d6ddfe7a59146485b512" },
                    Version = 1.0
                });
            return data;
        }

        public object Update(object entity)
        {
            PutAllergyDataRequest pa = (PutAllergyDataRequest)entity;
            AllergyData pt = pa.AllergyData;
            MEAllergy meA = new MEAllergy(pa.UserId);
            meA.DeleteFlag = pt.DeleteFlag;
            meA.CodingSystemCode = pt.CodingSystemCode;
            meA.Name = pt.Name;
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
    }
}
