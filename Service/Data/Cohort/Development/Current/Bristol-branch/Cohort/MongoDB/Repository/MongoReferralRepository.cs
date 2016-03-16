using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.DataDomain.Cohort.DTO;
using MongoDB.Bson;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;

namespace Phytel.API.DataDomain.Cohort
{
    public class MongoReferralRepository<T> : IReferralRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        private int _initializeDays = Convert.ToInt32(ConfigurationManager.AppSettings["InitializeDays"]);

        static MongoReferralRepository()
        {
            try
            {
                #region Register ClassMap
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MEReferral)) == false)
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MEReferral>();
                #endregion
            }
            catch { }
        }

        public MongoReferralRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            try
            {
                //Patient
                PostReferralDefinitionRequest request = newEntity as PostReferralDefinitionRequest;
                if (request != null)
                {
                    ReferralData rd = request.Referral;
                    using (CohortMongoContext ctx = new CohortMongoContext(_dbName))
                    {
                        MEReferral referral = new MEReferral(request.UserId)
                        {
                            CohortId = rd.CohortId,
                            Description = rd.Description,
                            Name = rd.Name,
                            Reason = rd.Reason,
                            DataSource = rd.DataSource, //Helper.TrimAndLimit(rd.DataSource, 50),
                            Version = request.Version,
                            TTLDate = DateTime.UtcNow.AddDays(_expireDays),
                            DeleteFlag = false,
                        };
                        ctx.Referrals.Insert(referral);
                    }
                }
                else
                {
                    throw new ApplicationException(string.Format("Invalid Referral Data."));
                }
                return new PostReferralDefinitionResponse
                {
                    Version = request.Version
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            return null;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            IEnumerable<object> query = null;
            return query;
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
