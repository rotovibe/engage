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
using Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO;
using Phytel.API.DataAudit;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class MongoStepRepository<T> : IProgramDesignRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        static MongoStepRepository()
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
                if (BsonClassMap.IsClassMapRegistered(typeof(MEStep)) == false)
                    BsonClassMap.RegisterClassMap<MEStep>();
            }
            catch { }
            #endregion
        }

        public MongoStepRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PutStepDataRequest request = newEntity as PutStepDataRequest;


            MEStep step = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                IMongoQuery query = Query.And(
                                Query.EQ(MEStep.TProperty, request.Title));
                step = ctx.Steps.Collection.FindOneAs<MEStep>(query);
                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                if (step == null)
                {
                    StepType type = new StepType();
                    switch(request.Type)
                    {
                        case ("text"):
                            type = StepType.Text;
                            break;
                        case ("yesno"):
                            type = StepType.YesNo;
                            break;
                    }
                    step = new MEStep(request.UserId)
                    {
                        Id = ObjectId.GenerateNewId(),
                        Title = request.Title,
                        Description = request.Description,
                        Text = request.Text,
                        Question = request.Question,
                        Notes = request.Notes,
                        StepTypeId = (int) type,
                        Version = request.Version,
                        UpdatedBy = ObjectId.Parse(request.UserId),
                        TTLDate = null,
                        DeleteFlag = false,
                        LastUpdatedOn = System.DateTime.UtcNow
                    };
                }
                ctx.Steps.Collection.Insert(step);

                AuditHelper.LogDataAudit(this.UserId,
                                          MongoCollectionName.Step.ToString(),
                                          step.Id.ToString(),
                                          Common.DataAuditType.Insert,
                                          request.ContractNumber);
            }

            return new PutStepDataResponse
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
            DeleteStepDataRequest request = entity as DeleteStepDataRequest;
            try
            {
                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var q = MB.Query<MEStep>.EQ(b => b.Id, ObjectId.Parse(request.StepId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEStep.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEStep.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEStep.UpdatedByProperty, ObjectId.Parse(request.UserId)));
                    uv.Add(MB.Update.Set(MEStep.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.Steps.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Step.ToString(),
                                            request.StepId.ToString(),
                                            Common.DataAuditType.Delete,
                                            request.ContractNumber);
                }

            }
            catch (Exception ex)
            {
                //TODO: handle this error
                throw;
            }
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            try
            {
                MEStep cp = null;
                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEStep>.EQ(b => b.Id, ObjectId.Parse(entityID));
                    cp = ctx.Steps.Collection.Find(findcp).FirstOrDefault();
                }

                DTO.StepData step = new DTO.StepData
                {
                    ID = cp.Id.ToString(),
                    Question = cp.Question,
                    Description = cp.Description,
                    Notes = cp.Notes,
                    Title = cp.Title,
                    TextEntry = cp.Text,
                    Status = cp.Status.ToString(),
                    Type = cp.StepTypeId.ToString()
                };
                return step;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:ProgramDesign:FindByID()::" + ex.Message, ex.InnerException);
            }
        }

        public object FindByName(string entityName)
        {
            try
            {
                StepData result = null;

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEStep>.EQ(b => b.Title, entityName);
                    MEStep cp = ctx.Steps.Collection.Find(findcp).FirstOrDefault();

                    if (cp != null)
                    {
                        result = new StepData
                        {
                            ID = cp.Id.ToString()
                        };
                    }
                    else
                    {
                        throw new ArgumentException("StepName is not valid or is missing from the records.");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:MongoStepRepository:FindByName()::" + ex.Message, ex.InnerException);
            }
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
                var steps = (from s in ctx.Steps
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
            PutUpdateStepDataRequest request = entity as PutUpdateStepDataRequest;
            PutUpdateStepDataResponse response = new PutUpdateStepDataResponse();
            try
            {
                if (request.UserId == null)
                    throw new ArgumentException("UserId is missing from the DataDomain request.");

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(MEStep.IdProperty, ObjectId.Parse(request.StepId));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.Title != null)
                    {
                        if (request.Title == "\"\"" || (request.Title == "\'\'"))
                            updt.Set(MEStep.QuestionProperty, string.Empty);
                        else
                            updt.Set(MEStep.QuestionProperty, request.Title);
                    }
                    if (request.Description != null)
                    {
                        if (request.Description == "\"\"" || (request.Description == "\'\'"))
                            updt.Set(MEStep.DescriptionProperty, string.Empty);
                        else
                            updt.Set(MEStep.DescriptionProperty, request.Description);
                    }
                    if (request.Text != null)
                    {
                        if (request.Text == "\"\"" || (request.Text == "\'\'"))
                            updt.Set(MEStep.TextProperty, string.Empty);
                        else
                            updt.Set(MEStep.TextProperty, request.Text);
                    }
                    if (request.Question != null)
                    {
                        if (request.Question == "\"\"" || (request.Question == "\'\'"))
                            updt.Set(MEStep.QuestionProperty, string.Empty);
                        else
                            updt.Set(MEStep.QuestionProperty, request.Question);
                    }
                    if (request.Notes != null)
                    {
                        if (request.Notes == "\"\"" || (request.Notes == "\'\'"))
                            updt.Set(MEStep.NotesProperty, string.Empty);
                        else
                            updt.Set(MEStep.NotesProperty, request.Notes);
                    }

                    updt.Set(MEStep.LastUpdatedOnProperty, System.DateTime.UtcNow);
                    updt.Set(MEStep.UpdatedByProperty, ObjectId.Parse(request.UserId));
                    updt.Set(MEStep.VersionProperty, request.Version);

                    var pt = ctx.Steps.Collection.FindAndModify(pUQuery, SortBy.Null, updt, true);

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
                //TODO: handle this error
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

        public object Insert(object newEntity, string type)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
