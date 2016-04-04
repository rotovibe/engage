
using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using DTO = Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.Medication.DTO;
using System.Configuration;
using MB = MongoDB.Driver.Builders;
using Phytel.API.DataAudit;
using Phytel.API.Common;

namespace DataDomain.Medication.Repo
{
    public class MongoPatientMedFrequencyRepository<TContext> : IMongoMedicationRepository, IMongoPatientMedFrequencyRepository where TContext : MedicationMongoContext
    {
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);
        
        protected readonly TContext Context;
        public string ContractDBName { get; set; }
        public string UserId { get; set; }

        public MongoPatientMedFrequencyRepository(IUOWMongo<TContext> uow)
        {
            Context = uow.MongoContext;
        }

        public MongoPatientMedFrequencyRepository(TContext context)
        {
            Context = context;
        }


        public MongoPatientMedFrequencyRepository(string dbName)
        {
            ContractDBName = dbName;
        }

        public object Insert(object newEntity)
        {
            var request = newEntity as PostPatientMedFrequencyDataRequest;
            var mm = request.PatientMedFrequencyData;
            try
            {
                var mePFreq = new MEPatientMedFrequency(this.UserId)
                {
                    Name = mm.Name,
                    PatientId = ObjectId.Parse(mm.PatientId),
                };

                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    ctx.PatientMedFrequencies.Collection.Insert(mePFreq);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientMedFrequency.ToString(),
                                            mePFreq.Id.ToString(),
                                            DataAuditType.Insert,
                                            request.ContractNumber);
                    return mePFreq.Id.ToString();
                }
            }
            catch (Exception) { throw; }
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
            throw new NotImplementedException();
        }

        public object Search(object request)
        {
            throw new NotImplementedException();
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

        public object FindByPatientId(object request)
        {
            List<PatientMedFrequencyData> list = null;
            GetPatientMedFrequenciesDataRequest dataRequest = (GetPatientMedFrequenciesDataRequest)request;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientMedFrequency.PatientIdProperty, ObjectId.Parse(dataRequest.PatientId)));
                    queries.Add(Query.EQ(MEPatientMedFrequency.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientMedFrequency.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MEPatientMedFrequency> meFreqs = ctx.PatientMedFrequencies.Collection.Find(mQuery).ToList();
                    if (meFreqs != null && meFreqs.Count > 0)
                    {
                        list = new List<PatientMedFrequencyData>();
                        meFreqs.ForEach(p =>
                        {
                            PatientMedFrequencyData data = AutoMapper.Mapper.Map<PatientMedFrequencyData>(p);
                            list.Add(data);
                        });
                    }
                }
                return list;
            }
            catch (Exception) { throw; }
        }

        public object Find(object request)
        {
            throw new NotImplementedException();
        }

        public object FindByName(object request)
        {
            PostPatientMedFrequencyDataRequest dataRequest = (PostPatientMedFrequencyDataRequest)request;
            string id = null;
            try
            {
                using (MedicationMongoContext ctx = new MedicationMongoContext(ContractDBName))
                {
                    var query =
                    from meFreq in ctx.PatientMedFrequencies.AsQueryable<MEPatientMedFrequency>()
                    where   meFreq.Name.ToLower() == dataRequest.PatientMedFrequencyData.Name.ToLower() 
                            && meFreq.DeleteFlag == false 
                            && meFreq.TTLDate == BsonNull.Value
                            && meFreq.PatientId == ObjectId.Parse(dataRequest.PatientMedFrequencyData.PatientId)
                    select meFreq;
                    if (query.FirstOrDefault() != null)
                    {
                        id = query.FirstOrDefault().Id.ToString();
                    }
                }
                return id;
            }
            catch (Exception) { throw; }
        }

        public object FindNDCCodes(object request)
        {
            throw new NotImplementedException();
        }

        public object Initialize(object newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
