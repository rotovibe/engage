using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.Common;
using MongoDB.Bson.Serialization;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using MB = MongoDB.Driver.Builders;
using Phytel.API.DataAudit;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class MongoYesNoStepRepository<T> : IProgramDesignRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        static MongoYesNoStepRepository()
        {

            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEStepBase)) == false)
                    BsonClassMap.RegisterClassMap<MEStepBase>();
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEYesNo)) == false)
                    BsonClassMap.RegisterClassMap<MEYesNo>();
            }
            catch {  }
            #endregion
        }

        public MongoYesNoStepRepository(string contractDBName)
        {
            _dbName = contractDBName;

        }

        public object Insert(object newEntity)
        {
            PutYesNoStepDataRequest request = newEntity as PutYesNoStepDataRequest;
            MEYesNo step = null;
            using(ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                IMongoQuery query = Query.And(
                                Query.EQ(MEYesNo.QuestionProperty, request.Question));
                step = ctx.YesNoSteps.Collection.FindOneAs<MEYesNo>(query);
                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                if(step == null)
                {
                    step = new MEYesNo()
                    {
                        Id = ObjectId.GenerateNewId(),
                        Question = request.Question,
                        Notes = request.Notes,
                        Type = StepType.YesNo
                    };
                }
                ctx.YesNoSteps.Collection.Insert(step);

                AuditHelper.LogDataAudit(this.UserId,
                                          MongoCollectionName.Step.ToString(),
                                          step.Id.ToString(),
                                          Common.DataAuditType.Insert,
                                          request.ContractNumber);
            }

            return new PutYesNoStepDataResponse
            {
                Id = step.Id.ToString()
            };
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            DeleteYesNoStepDataRequest request = (DeleteYesNoStepDataRequest)entity;
            try
            {
                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var q = MB.Query<MEYesNo>.EQ(b => b.Id, ObjectId.Parse(request.StepId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEYesNo.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEYesNo.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEYesNo.UpdatedByProperty, ObjectId.Parse(request.UserId)));
                    uv.Add(MB.Update.Set(MEYesNo.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.YesNoSteps.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Step.ToString(),
                                            request.StepId.ToString(),
                                            Common.DataAuditType.Delete,
                                            request.ContractNumber);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            GetYesNoStepDataResponse response = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEYesNo.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MEYesNo.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MEYesNo meYesNo = ctx.YesNoSteps.Collection.Find(mQuery).FirstOrDefault();
                if (meYesNo != null)
                {
                    response = new GetYesNoStepDataResponse();
                    DTO.YesNoData yesnoStep = new DTO.YesNoData
                    {
                        ID = meYesNo.Id.ToString(),
                        Type = meYesNo.Type.ToString(),
                        Status = Helper.ToFriendlyString(meYesNo.Status),
                        Question = meYesNo.Question,
                        Notes = meYesNo.Notes
                    };
                    response.YesNoStep = yesnoStep;
                }
            }
            return response;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            GetAllYesNoStepDataResponse response = new GetAllYesNoStepDataResponse();

            List<DTO.YesNoData> list = new List<DTO.YesNoData>();

            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                //var steps = (from a in ctx.YesNoSteps
                //             where a.Id == new ObjectId("531a2d05c3478604270000f8")
                //             select new DTO.YesNoData
                //              {
                //                  ID = a.Id.ToString(),
                //                  //Question = a.Question
                //              }).First();

                var step = from a in ctx.YesNoSteps
                           where a.Id.Equals(new ObjectId("531a2d05c3478604270000f8"))
                           select a;

                //var x = from a in ctx.YesNoSteps
                //             select a;

                //response.Steps = steps;
            }
            return response.Steps;
        }

        public object Update(object entity)
        {
            PutUpdateYesNoStepDataRequest request = entity as PutUpdateYesNoStepDataRequest;
            PutUpdateYesNoStepDataResponse response = new PutUpdateYesNoStepDataResponse();
            try
            {
                if (request.UserId == null)
                    throw new ArgumentException("UserId is missing from the DataDomain request.");

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(MEYesNo.IdProperty, ObjectId.Parse(request.StepId));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.Question != null)
                    {
                        if (request.Question == "\"\"" || (request.Question == "\'\'"))
                            updt.Set(MEYesNo.QuestionProperty, string.Empty);
                        else
                            updt.Set(MEYesNo.QuestionProperty, request.Question);
                    }
                    if (request.Notes != null)
                    {
                        if (request.Notes == "\"\"" || (request.Notes == "\'\'"))
                            updt.Set(MEYesNo.NotesProperty, string.Empty);
                        else
                            updt.Set(MEYesNo.NotesProperty, request.Notes);
                    }

                    updt.Set(MEYesNo.LastUpdatedOnProperty, System.DateTime.UtcNow);
                    updt.Set(MEYesNo.UpdatedByProperty, ObjectId.Parse(request.UserId));
                    updt.Set(MEYesNo.VersionProperty, request.Version);

                    var pt = ctx.YesNoSteps.Collection.FindAndModify(pUQuery, SortBy.Null, updt, true);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Step.ToString(),
                                            request.StepId.ToString(),
                                            Common.DataAuditType.Update,
                                            request.ContractNumber);

                    response.Id = request.StepId;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object Update(PutUpdateYesNoStepDataRequest request)
        {
            PutUpdateYesNoStepDataResponse response = new PutUpdateYesNoStepDataResponse();
            try
            {
                if (request.UserId == null)
                    throw new ArgumentException("UserId is missing from the DataDomain request.");

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(MEYesNo.IdProperty, ObjectId.Parse(request.StepId));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.Question != null)
                    {
                        if (request.Question == "\"\"" || (request.Question == "\'\'"))
                            updt.Set(MEYesNo.QuestionProperty, string.Empty);
                        else
                            updt.Set(MEYesNo.QuestionProperty, request.Question);
                    }
                    if (request.Notes != null)
                    {
                        if (request.Notes == "\"\"" || (request.Notes == "\'\'"))
                            updt.Set(MEYesNo.NotesProperty, string.Empty);
                        else
                            updt.Set(MEYesNo.NotesProperty, request.Notes);
                    }

                    updt.Set(MEYesNo.LastUpdatedOnProperty, System.DateTime.UtcNow);
                    updt.Set(MEYesNo.UpdatedByProperty, ObjectId.Parse(this.UserId));
                    updt.Set(MEYesNo.VersionProperty, request.Version);

                    var pt = ctx.YesNoSteps.Collection.FindAndModify(pUQuery, SortBy.Null, updt, true);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Step.ToString(),
                                            request.StepId.ToString(),
                                            Common.DataAuditType.Update,
                                            request.ContractNumber);

                    response.Id = request.StepId;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.Program FindByName(string entityName)
        {
            throw new NotImplementedException();
        }
    }
}
