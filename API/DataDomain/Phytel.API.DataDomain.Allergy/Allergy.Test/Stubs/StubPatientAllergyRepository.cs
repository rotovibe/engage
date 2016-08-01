
using System;
using System.Collections.Generic;
using DataDomain.Allergy.Repo;
using MongoDB.Bson;
using Phytel.API.DataDomain.Allergy.DTO;

namespace Phytel.API.DataDomain.Allergy.Test
{
    public class StubPatientAllergyRepository<TContext> : IMongoPatientAllergyRepository, IMongoAllergyRepository where TContext : AllergyMongoContext
    {
        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }

        public StubPatientAllergyRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public StubPatientAllergyRepository(TContext context)
        {
            Context = context;
        }

        public object Initialize(object newEntity)
        {
            PutInitializePatientAllergyDataRequest request = (PutInitializePatientAllergyDataRequest)newEntity;
            PatientAllergyData data = null;
            MEPatientAllergy mePA = new MEPatientAllergy(this.UserId)
            {
                PatientId = ObjectId.Parse(request.PatientId),
                AllergyId = ObjectId.Parse(request.AllergyId),
                TTLDate = System.DateTime.UtcNow.AddDays(1),
                SystemName = request.SystemName,
                DeleteFlag = false
            };
            data = new PatientAllergyData
            {
                Id = mePA.Id.ToString(),
                PatientId = mePA.PatientId.ToString(),
                AllergyId = mePA.AllergyId.ToString(),
                SystemName = mePA.SystemName
            };
            return data;
        }

        public object FindByPatientId(object request)
        {
            List<PatientAllergyData> list = new List<PatientAllergyData>();
            PatientAllergyData p1 = new PatientAllergyData
            {
                AllergyId = "54489a72fe7a59146485bce5",
                EndDate = DateTime.UtcNow,
                Id = "5452567ed433231b9c516d8e",
                Notes = "first note for patient allergy",
                PatientId = "54087f43d6a48509407d69cb",
                ReactionIds = new List<string> { "54494b5ad433232a446f7323", "54494b5dd433232a446f7324", "54494b60d433232a446f7325" },
                SeverityId = "54494a96d433232a446f7313",
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 1,
                SystemName = "Engage1",
                UpdatedOn = DateTime.UtcNow
            };
            list.Add(p1);
            PatientAllergyData p2 = new PatientAllergyData
            {
                AllergyId = "54489a79fe7a59146485bd1e",
                EndDate = DateTime.UtcNow,
                Id = "5452584ed4332305d8fa10b5",
                Notes = "asdasfddfjskdfjsldfugiosdgjksgj",
                PatientId = "54087f43d6a48509407d69cb",
                ReactionIds = new List<string> { "54494b5ad433232a446f7323" },
                SeverityId = "54494a96d433232a446f7313",
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 1,
                SystemName = "Engage2",
                UpdatedOn = DateTime.UtcNow
            };
            list.Add(p2);
            return list;
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(System.Collections.Generic.List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            MEPatientAllergy mePA = new MEPatientAllergy(this.UserId)
            {
                TTLDate = System.DateTime.UtcNow.AddDays(7),
                DeleteFlag = true
            };
        }

        public void DeleteAll(System.Collections.Generic.List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
          return  new PatientAllergyData
            {
                AllergyId = "54489a72fe7a59146485bce5",
                EndDate = DateTime.UtcNow,
                Id = "5452567ed433231b9c516d8e",
                Notes = "first note for patient allergy",
                PatientId = "54087f43d6a48509407d69cb",
                ReactionIds = new List<string> { "54494b5ad433232a446f7323", "54494b5dd433232a446f7324", "54494b60d433232a446f7325" },
                SeverityId = "54494a96d433232a446f7313",
                SourceId = "544e9976d433231d9c0330ae",
                StartDate = DateTime.UtcNow,
                StatusId = 1,
                SystemName = "Engage1",
                UpdatedOn = DateTime.UtcNow
            };
        }

        public Tuple<string, System.Collections.Generic.IEnumerable<object>> Select(Phytel.API.Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            PutPatientAllergiesDataRequest pa = (PutPatientAllergiesDataRequest)entity;
            PatientAllergyData pt = pa.PatientAllergiesData[0];
            MEPatientAllergy mePA = new MEPatientAllergy(pa.UserId);
            mePA.AllergyId = ObjectId.Parse(pt.AllergyId);
            mePA.SeverityId = ObjectId.Parse(pt.SeverityId);
            mePA.DeleteFlag = pt.DeleteFlag;
            mePA.SourceId = ObjectId.Parse(pt.SourceId);
            mePA.StatusId = (Status)pt.StatusId;
            return true;
        }

        public void CacheByID(System.Collections.Generic.List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void UndoDelete(object entity)
        {
            MEPatientAllergy mePA = new MEPatientAllergy(this.UserId)
            {
                TTLDate = null,
                DeleteFlag = true
            };
        }
    }
}
