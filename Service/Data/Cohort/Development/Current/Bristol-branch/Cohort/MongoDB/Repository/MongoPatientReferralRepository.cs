using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using Phytel.API.DataDomain.Cohort.DTO;
using MongoDB.Bson.Serialization;
using Phytel.API.DataDomain.Cohort.DTO.Model;
using Phytel.API.DataDomain.Cohort.DTO.Referrals;

namespace Phytel.API.DataDomain.Cohort
{
    public class MongoPatientReferralRepository<T> : IPatientReferralRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);
        protected readonly IMappingEngine _mappingEngine;

        static MongoPatientReferralRepository()
        {
            try
            {
                #region Register ClassMap
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientReferral)) == false)
                    BsonClassMap.RegisterClassMap<MEPatientReferral>();
                #endregion
            }
            catch { }
        }

        public MongoPatientReferralRepository(IMappingEngine mappingEngine, string contractDBName)
        {
            _mappingEngine = mappingEngine;
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {

            try
            {
                //PatientReferral
                var patientreferralid = "";
                PostPatientReferralDefinitionRequest request = newEntity as PostPatientReferralDefinitionRequest;
                if (request != null)
                {
                    PatientReferralData prd = request.PatientReferral;
                    using (CohortMongoContext ctx = new CohortMongoContext(_dbName))
                    {
                        MEPatientReferral patientReferral = new MEPatientReferral(request.UserId);
                        _mappingEngine.Map(request.PatientReferral, patientReferral);
                        patientReferral.Version = request.Version;
                        ctx.PatientReferrals.Insert(patientReferral);
                        patientreferralid = patientReferral.Id.ToString();
                    }
                }
                else
                {
                    throw new ApplicationException(string.Format("Invalid Referral Data."));
                }
                return new PostPatientReferralDefinitionResponse
                {
                    PatientReferralId = patientreferralid,
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
