using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class MongoActionRepository<T> : IProgramDesignRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        static MongoActionRepository()
        {
            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEAction)) == false)
                    BsonClassMap.RegisterClassMap<MEAction>();
            }
            catch { }
            
            #endregion
        }

        public MongoActionRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PutActionDataRequest request = newEntity as PutActionDataRequest;

            MEAction action = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                IMongoQuery query = Query.And(
                                Query.EQ(MEAction.NameProperty, request.Name));
                action = ctx.Actions.Collection.FindOneAs<MEAction>(query);
                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                if (action == null)
                {
                    action = new MEAction(this.UserId)
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = request.Name,
                        Description = request.Description,
                        //CompletedBy = ObjectId.Parse(request.CompletedBy),
                        Version = request.Version
                    };
                }
                ctx.Actions.Collection.Insert(action);

                AuditHelper.LogDataAudit(this.UserId,
                                           MongoCollectionName.Action.ToString(),
                                           action.Id.ToString(),
                                           Common.DataAuditType.Insert,
                                           request.ContractNumber);
            }

            return new PutActionDataResponse
            {
                Id = action.Id.ToString()
            };
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            DeleteActionDataRequest request = (DeleteActionDataRequest)entity;
            try
            {
                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var q = MB.Query<MEAction>.EQ(b => b.Id, ObjectId.Parse(request.ActionId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEAction.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEAction.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEAction.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEAction.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.Actions.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Action.ToString(),
                                            request.ActionId.ToString(),
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
            GetActionDataResponse actionResponse = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEAction.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MEAction.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MEAction meAction = ctx.Actions.Collection.Find(mQuery).FirstOrDefault();
                if (meAction != null)
                {
                    actionResponse = new GetActionDataResponse();

                    List<string> objectiveIDs = new List<string>();

                    if (meAction.Objectives != null)
                    {
                        foreach (Objective oi in meAction.Objectives)
                        {
                            objectiveIDs.Add(oi.Id.ToString());
                        }
                    }
                    
                    ActionData action = new ActionData
                    {
                        ID = meAction.Id.ToString(),
                        Name = meAction.Name,
                        Description = meAction.Description,
                        CompletedBy = meAction.CompletedBy.ToString(),
                        Objectives = objectiveIDs,
                        Status = Helper.ToFriendlyString(meAction.Status)
                    };
                    actionResponse.Action = action;
                }
            }
            return actionResponse;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public GetAllActionsDataResponse SelectAll(double versionNumber, Status status)
        {
            GetAllActionsDataResponse response = new GetAllActionsDataResponse()
            {
                Version = versionNumber
            };

            List<DTO.ActionData> list = new List<DTO.ActionData>();

            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                 //var x = (from a in ctx.Actions
                 //       //where a.CompletedBy.ToString().Length > 5
                 //       select new DTO.ActionData
                 //       {
                 //           ID = a.Id.ToString(),
                 //           Name = a.Name,
                 //           Description = a.Description
                 //           //Status = status.ToString()
                 //       });


                var l = from a in ctx.Actions
                        select a;

                 //foreach (ActionData act in x)
                 //{
                 //    if(act.CompletedBy.ToString().Length > 5)
                 //       response.Actions.Add(act);
                 //}

            }
           
            return response;
        }

        public object Update(object entity)
        {
            PutUpdateActionDataRequest request = entity as PutUpdateActionDataRequest;
            PutUpdateActionDataResponse response = new PutUpdateActionDataResponse();
            try
            {
                if (request.UserId == null)
                    throw new ArgumentException("UserId is missing from the DataDomain request.");

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(MEAction.IdProperty, ObjectId.Parse(request.ActionId));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.Name != null)
                    {
                        if (request.Name == "\"\"" || (request.Name == "\'\'"))
                            updt.Set(MEAction.NameProperty, string.Empty);
                        else
                            updt.Set(MEAction.NameProperty, request.Name);
                    }
                    if (request.Description != null)
                    {
                        if (request.Description == "\"\"" || (request.Description == "\'\'"))
                            updt.Set(MEAction.DescriptionProperty, string.Empty);
                        else
                            updt.Set(MEAction.DescriptionProperty, request.Description);
                    }

                    updt.Set(MEAction.LastUpdatedOnProperty, System.DateTime.UtcNow);
                    updt.Set(MEAction.UpdatedByProperty, ObjectId.Parse(this.UserId));
                    updt.Set(MEAction.VersionProperty, request.Version);

                    var pt = ctx.Actions.Collection.FindAndModify(pUQuery, SortBy.Null, updt, true);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Action.ToString(),
                                            request.ActionId.ToString(),
                                            Common.DataAuditType.Update,
                                            request.ContractNumber);

                    response.Id = request.ActionId;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object Update(PutUpdateActionDataRequest request)
        {
            PutUpdateActionDataResponse response = new PutUpdateActionDataResponse();
            try
            {
                if (request.UserId == null)
                    throw new ArgumentException("UserId is missing from the DataDomain request.");

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(MEAction.IdProperty, ObjectId.Parse(request.ActionId));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.Name != null)
                    {
                        if (request.Name == "\"\"" || (request.Name == "\'\'"))
                            updt.Set(MEAction.NameProperty, string.Empty);
                        else
                            updt.Set(MEAction.NameProperty, request.Name);
                    }
                    if (request.Description != null)
                    {
                        if (request.Description == "\"\"" || (request.Description == "\'\'"))
                            updt.Set(MEAction.DescriptionProperty, string.Empty);
                        else
                            updt.Set(MEAction.DescriptionProperty, request.Description);
                    }

                    updt.Set(MEAction.LastUpdatedOnProperty, System.DateTime.UtcNow);
                    updt.Set(MEAction.UpdatedByProperty, ObjectId.Parse(this.UserId));
                    updt.Set(MEAction.VersionProperty, request.Version);

                    var pt = ctx.Actions.Collection.FindAndModify(pUQuery, SortBy.Null, updt, true);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Action.ToString(),
                                            request.ActionId.ToString(),
                                            Common.DataAuditType.Update,
                                            request.ContractNumber);

                    response.Id = request.ActionId;
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
