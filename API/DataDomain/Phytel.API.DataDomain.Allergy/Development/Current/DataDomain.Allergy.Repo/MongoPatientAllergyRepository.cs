
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using DTO = Phytel.API.DataDomain.Allergy.DTO;
using Phytel.API.DataDomain.Allergy.DTO;

namespace DataDomain.Allergy.Repo
{
    public class MongoPatientAllergyRepository<TContext> : IMongoPatientAllergyRepository where TContext : AllergyMongoContext
    {
        protected readonly TContext Context;
        public string UserId { get; set; }

        public MongoPatientAllergyRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoPatientAllergyRepository(TContext context)
        {
            Context = context;
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
            try
            {
                object result = null;

                var findcp = Query.And(
                    Query<MEAllergy>.EQ(b => b.Id, ObjectId.Parse(entityID)),
                    Query<MEAllergy>.EQ(b => b.DeleteFlag, false));

                var cp = Context.Allergy.Collection.Find(findcp).FirstOrDefault();

                if (cp == null) return result;

                result = new DTO.DdAllergy
                {
                    Id = cp.Id.ToString(),
                    DeleteFlag = cp.DeleteFlag,
                    LastUpdatedOn = cp.LastUpdatedOn,
                    RecordCreatedBy = cp.RecordCreatedBy.ToString(),
                    RecordCreatedOn = cp.RecordCreatedOn,
                    TTLDate = cp.TTLDate,
                    UpdatedBy = cp.UpdatedBy.ToString(),
                    Version = cp.Version
                };
                
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:FindByID()::" + ex.Message, ex.InnerException);
            }
        }

        public Tuple<string, IEnumerable<object>> Select(Phytel.API.Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
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
