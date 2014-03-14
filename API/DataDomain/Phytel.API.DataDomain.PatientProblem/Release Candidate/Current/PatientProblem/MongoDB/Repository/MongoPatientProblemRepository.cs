using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.Interface;
using MB = MongoDB.Driver.Builders;
using Phytel.API.DataAudit;
using Phytel.API.Common;
using MongoDB.Bson.Serialization;

namespace Phytel.API.DataDomain.PatientProblem
{
    public class MongoPatientProblemRepository<T> : IPatientProblemRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientProblemRepository(string contractDBName)
        {
            _dbName = contractDBName;

            #region Register ClassMap
            if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientProblem)) == false)
                BsonClassMap.RegisterClassMap<MEPatientProblem>();
            #endregion
        }

        public object Insert(object newEntity)
        {
            PutNewPatientProblemRequest request = (PutNewPatientProblemRequest)newEntity;
            DTO.PatientProblem pb = null;
            MEPatientProblem pp = null;
            try
            {
                using (PatientProblemMongoContext ctx = new PatientProblemMongoContext(_dbName))
                {
                    pp = new MEPatientProblem(this.UserId)
                    {
                        Active = request.Active,
                        Featured = request.Featured,
                        Level = request.Level,
                        PatientID = ObjectId.Parse(request.PatientId),
                        ProblemID = ObjectId.Parse(request.ProblemId),
                        Version = 1,
                        DeleteFlag = false,
                        LastUpdatedOn = System.DateTime.UtcNow,
                        UpdatedBy = ObjectId.Parse(this.UserId)
                    };
                    ctx.PatientProblems.Collection.Insert(pp);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientProblem.ToString(), 
                                            pp.Id.ToString(), 
                                            Common.DataAuditType.Insert, 
                                            request.ContractNumber);

                    pb = new DTO.PatientProblem
                    {
                        Active = pp.Active,
                        Featured = pp.Featured,
                        Level = pp.Level,
                        PatientID = pp.PatientID.ToString(),
                        ProblemID = pp.ProblemID.ToString(),
                        LastUpdatedOn = pp.LastUpdatedOn,
                        DeleteFlag = pp.DeleteFlag,
                        Version = pp.Version
                    };
                }

                return pb as object;
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

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            IEnumerable<object> returnQuery = null;
            IMongoQuery mQuery = null;

            List<SelectExpression> selectExpressions = expression.Expressions.ToList();
            selectExpressions.Where(s => s.GroupID == 1).OrderBy(o => o.ExpressionOrder).ToList();

            SelectExpressionGroupType groupType = SelectExpressionGroupType.AND;

            if(selectExpressions.Count > 0)
            {
                IList<IMongoQuery> queries = new List<IMongoQuery>();
                for (int i = 0; i < selectExpressions.Count; i++)
                {
                    groupType = selectExpressions[0].NextExpressionType;

                    IMongoQuery query = SelectExpressionHelper.ApplyQueryOperators(selectExpressions[i].Type, selectExpressions[i].FieldName, selectExpressions[i].Value);
                    if (query != null)
                    {
                        queries.Add(query);
                    }
                }

                mQuery = SelectExpressionHelper.BuildQuery(groupType, queries);

                List<Phytel.API.DataDomain.PatientProblem.DTO.PatientProblemData> patientProblemList = null;
                using (PatientProblemMongoContext ctx = new PatientProblemMongoContext(_dbName))
                {
                    List<MEPatientProblem> mePatientProblems = ctx.PatientProblems.Collection.Find(mQuery).ToList();
                    if (mePatientProblems != null)
                    {
                        patientProblemList = new List<PatientProblemData>();
                        foreach (MEPatientProblem p in mePatientProblems)
                        {
                            PatientProblemData problem = new PatientProblemData
                            {
                                ProblemID = p.ProblemID.ToString(),
                                PatientID = p.PatientID.ToString(),
                                ID = p.Id.ToString(),
                                Level = p.Level,
                                Active = p.Active
                            };
                            patientProblemList.Add(problem);
                        }
                    }
                }
                returnQuery = patientProblemList.AsQueryable<object>();
            }

            return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, returnQuery);
        }
        
        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            PutUpdatePatientProblemRequest p = (PutUpdatePatientProblemRequest)entity;
            PutUpdatePatientProblemResponse pr = new PutUpdatePatientProblemResponse();

            try
            {
                using (PatientProblemMongoContext ctx = new PatientProblemMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientProblem>.EQ(b => b.Id, ObjectId.Parse(p.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientProblem.ActiveProperty, p.Active));
                    uv.Add(MB.Update.Set(MEPatientProblem.FeaturedProperty, p.Featured));
                    uv.Add(MB.Update.Set(MEPatientProblem.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientProblem.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    if (p.Level != 0) { uv.Add(MB.Update.Set(MEPatientProblem.LevelProperty, p.Level)); }
                    
                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientProblems.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientProblem.ToString(), 
                                            p.Id, 
                                            Common.DataAuditType.Update, 
                                            p.ContractNumber);

                }
                return pr;
            }
            catch (Exception ex)
            {
                throw new Exception("PatientProblemDD:Update()::" + ex.Message, ex.InnerException);
            }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }
    }

}
