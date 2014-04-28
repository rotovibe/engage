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
using Phytel.API.DataAudit;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class MongoTextStepRepository<T> : IProgramDesignRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        static MongoTextStepRepository()
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
                if (BsonClassMap.IsClassMapRegistered(typeof(METext)) == false)
                    BsonClassMap.RegisterClassMap<METext>();
            }
            catch { }
            #endregion
        }

        public MongoTextStepRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PutTextStepDataRequest request = newEntity as PutTextStepDataRequest;
            METext step = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                IMongoQuery query = Query.And(
                                Query.EQ(METext.TitleProperty, request.Title));
                step = ctx.TextSteps.Collection.FindOneAs<METext>(query);
                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                if (step == null)
                {
                    step = new METext()
                    {
                        Id = ObjectId.GenerateNewId(),
                        Title = request.Title,
                        Description = request.Description,
                        Text = request.Text,
                        Type = StepType.Text,
                        Version = request.Version
                    };
                }
                ctx.TextSteps.Collection.Insert(step);

                AuditHelper.LogDataAudit(this.UserId,
                                          MongoCollectionName.Step.ToString(),
                                          step.Id.ToString(),
                                          Common.DataAuditType.Insert,
                                          request.ContractNumber);
            }

            return new PutTextStepDataResponse
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
            DeleteTextStepDataRequest request = (DeleteTextStepDataRequest)entity;
            try
            {
                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var q = MB.Query<METext>.EQ(b => b.Id, ObjectId.Parse(request.StepId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(METext.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(METext.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(METext.UpdatedByProperty, ObjectId.Parse(request.UserId)));
                    uv.Add(MB.Update.Set(METext.LastUpdatedOnProperty, DateTime.UtcNow));

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
            GetTextStepDataResponse response = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(METext.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(METext.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                METext meText = ctx.TextSteps.Collection.Find(mQuery).FirstOrDefault();
                if (meText != null)
                {
                    response = new GetTextStepDataResponse();
                    DTO.TextData textStep = new DTO.TextData
                    {
                        ID = meText.Id.ToString(),
                        Type = meText.Type.ToString(),
                        Status = Helper.ToFriendlyString(meText.Status),
                        Title = meText.Title,
                        Description = meText.Description,
                        TextEntry = meText.Text
                    };
                    response.TextStep = textStep;
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
           GetAllTextStepDataResponse response = new GetAllTextStepDataResponse();

            List<DTO.TextData> list = new List<DTO.TextData>();

            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                var steps = (from s in ctx.TextSteps
                         select new DTO.TextData
                         {
                              ID = s.Id.ToString(),
                              Title = s.Text
                         }).ToList();

                response.Steps = steps;

            }

            return response.Steps;
        }

        public object Update(object entity)
        {
            PutUpdateTextStepDataRequest request = entity as PutUpdateTextStepDataRequest;
            PutUpdateTextStepDataResponse response = new PutUpdateTextStepDataResponse();
            try
            {
                if (request.UserId == null)
                    throw new ArgumentException("UserId is missing from the DataDomain request.");

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(METext.IdProperty, ObjectId.Parse(request.StepId));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.Title != null)
                    {
                        if (request.Title == "\"\"" || (request.Title == "\'\'"))
                            updt.Set(METext.QuestionProperty, string.Empty);
                        else
                            updt.Set(METext.QuestionProperty, request.Title);
                    }
                    if (request.Description != null)
                    {
                        if (request.Description == "\"\"" || (request.Description == "\'\'"))
                            updt.Set(METext.DescriptionProperty, string.Empty);
                        else
                            updt.Set(METext.DescriptionProperty, request.Description);
                    }
                    if (request.Text != null)
                    {
                        if (request.Text == "\"\"" || (request.Text == "\'\'"))
                            updt.Set(METext.TextPromptProperty, string.Empty);
                        else
                            updt.Set(METext.TextPromptProperty, request.Text);
                    }

                    updt.Set(METext.LastUpdatedOnProperty, System.DateTime.UtcNow);
                    updt.Set(METext.UpdatedByProperty, ObjectId.Parse(request.UserId));
                    updt.Set(METext.VersionProperty, request.Version);

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

        public object Update(PutUpdateTextStepDataRequest request)
        {
            PutUpdateTextStepDataResponse response = new PutUpdateTextStepDataResponse();
            try
            {
                if (request.UserId == null)
                    throw new ArgumentException("UserId is missing from the DataDomain request.");

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(METext.IdProperty, ObjectId.Parse(request.StepId));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.Title != null)
                    {
                        if (request.Title == "\"\"" || (request.Title == "\'\'"))
                            updt.Set(METext.QuestionProperty, string.Empty);
                        else
                            updt.Set(METext.QuestionProperty, request.Title);
                    }
                    if (request.Description != null)
                    {
                        if (request.Description == "\"\"" || (request.Description == "\'\'"))
                            updt.Set(METext.DescriptionProperty, string.Empty);
                        else
                            updt.Set(METext.DescriptionProperty, request.Description);
                    }
                    if (request.Text != null)
                    {
                        if (request.Text == "\"\"" || (request.Text == "\'\'"))
                            updt.Set(METext.TextPromptProperty, string.Empty);
                        else
                            updt.Set(METext.TextPromptProperty, request.Text);
                    }

                    updt.Set(METext.LastUpdatedOnProperty, System.DateTime.UtcNow);
                    updt.Set(METext.UpdatedByProperty, ObjectId.Parse(this.UserId));
                    updt.Set(METext.VersionProperty, request.Version);

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
