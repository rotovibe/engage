using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Builders;
using System.Configuration;

namespace Phytel.API.DataDomain.Program
{
    public class MongoPatientProgramRepository : IProgramRepository
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        static MongoPatientProgramRepository()
        {
            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(ProgramBase)) == false)
                    BsonClassMap.RegisterClassMap<ProgramBase>();
            }
            catch { }
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEPatientProgram)) == false)
                    BsonClassMap.RegisterClassMap<MEPatientProgram>();
            }
            catch { }
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEProgram)) == false)
                    BsonClassMap.RegisterClassMap<MEProgram>();
            }
            catch { }
            #endregion
        }

        public MongoPatientProgramRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            try
            {
                MEPatientProgram nmePP = (MEPatientProgram)newEntity;
                ProgramInfo pi = null;
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    nmePP.UpdatedBy = null;
                    nmePP.LastUpdatedOn = null;
                    ctx.PatientPrograms.Collection.Insert(nmePP);

                    // update programid in modules
                    var q = MB.Query<MEPatientProgram>.EQ(b => b.Id, nmePP.Id);
                    nmePP.Modules.ForEach(s => s.ProgramId = nmePP.Id);
                    nmePP.Modules.ForEach(s => s.Objectives = null);
                    ctx.PatientPrograms.Collection.Update(q, MB.Update.SetWrapped<List<Module>>(MEPatientProgram.ModulesProperty, nmePP.Modules));

                    // hydrate response object
                    pi = new ProgramInfo
                    {
                        Id = nmePP.Id.ToString(),
                        Name = nmePP.Name,
                        ShortName = nmePP.ShortName,
                        Status = (int)nmePP.Status,
                        PatientId = nmePP.PatientId.ToString(),
                        //ProgramState = (int)nmePP.ProgramState, // depricated - Use Element state instead.
                        ElementState = (int)nmePP.State,
                        ProgramSourceId = (nmePP.SourceId == null) ? null : nmePP.SourceId.ToString()
                    };

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientProgram.ToString(),
                                            nmePP.Id.ToString(),
                                            Common.DataAuditType.Insert,
                                            _dbName);
                }
                return pi;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgram:Insert()::" + ex.Message, ex.InnerException);
            }
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }


        public void Delete(object entity)
        {
            DeletePatientProgramDataRequest request = (DeletePatientProgramDataRequest)entity;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var query = MB.Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(request.Id));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEPatientProgram.TTLDateProperty, DateTime.UtcNow.AddDays(_expireDays)));
                    builder.Add(MB.Update.Set(MEPatientProgram.DeleteFlagProperty, true));
                    builder.Add(MB.Update.Set(MEPatientProgram.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEPatientProgram.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.PatientPrograms.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientProgram.ToString(),
                                            request.Id.ToString(),
                                            Common.DataAuditType.Delete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByEntityExistsID(string patientID, string progId)
        {
            try
            {
                List<MEPatientProgram> pp = null;

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findQ = MB.Query.And(
                        MB.Query<MEPatientProgram>.EQ(b => b.PatientId, ObjectId.Parse(patientID)),
                        MB.Query<MEPatientProgram>.EQ(b => b.DeleteFlag, false),
                        MB.Query<MEPatientProgram>.EQ(b => b.ContractProgramId, ObjectId.Parse(progId)),
                        MB.Query.In(MEPatientProgram.StateProperty, new List<BsonValue> { BsonValue.Create(ElementState.NotStarted), BsonValue.Create(ElementState.Started), BsonValue.Create(ElementState.InProgress), BsonValue.Create(ElementState.Closed) }));

                    pp = ctx.PatientPrograms.Collection.Find(findQ).ToList();
                }
                return pp;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:FindByEntityExistsID()::" + ex.Message, ex.InnerException);
            }
        }

        public object FindByID(string entityID)
        {
            try
            {
                MEPatientProgram result = null;

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findcp = MB.Query.And(
                        MB.Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(entityID)),
                        MB.Query<MEPatientProgram>.EQ(b => b.DeleteFlag, false));
                    MEPatientProgram cp = ctx.PatientPrograms.Collection.Find(findcp).FirstOrDefault();

                    if (cp != null)
                    {
                        result = cp;
                    }
                    else
                    {
                        throw new ArgumentException("ProgramID is not valid or is missing from the records.");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:FindByID()::" + ex.Message, ex.InnerException);
            }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            try
            {
                IMongoQuery mQuery = null;
                List<object> patList = new List<object>();

                List<SelectExpression> selectExpressions = expression.Expressions.ToList();
                selectExpressions.Where(s => s.GroupID == 1).OrderBy(o => o.ExpressionOrder).ToList();

                SelectExpressionGroupType groupType = SelectExpressionGroupType.AND;

                if (selectExpressions.Count > 0)
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
                }

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    //var findcp = Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(request.PatientProgramId));
                    //MEPatientProgram cp = ctx.PatientPrograms.Collection.Find(findcp).FirstOrDefault();
                    List<MEPatientProgram> cps = ctx.PatientPrograms.Collection.Find(mQuery).ToList();

                    if (cps != null)
                    {
                        cps.ForEach(cp => patList.Add(new ProgramDetail
                        {
                            Id = cp.Id.ToString(),
                            Client = cp.Client != null ? cp.Client.ToString() : null,
                            ContractProgramId = cp.ContractProgramId.ToString(),
                            Description = cp.Description,
                            EndDate = cp.EndDate,
                            AssignBy = cp.AssignedBy.ToString(),
                            AssignDate = cp.AssignedOn,
                            Completed = cp.Completed,
                            CompletedBy = cp.CompletedBy,
                            DateCompleted = cp.DateCompleted,
                            ElementState = (int)cp.State,
                            StateUpdatedOn = cp.StateUpdatedOn,
                            Enabled = cp.Enabled,
                            Next = cp.Next != null ? cp.Next.ToString() : string.Empty,
                            Order = cp.Order,
                            Previous = cp.Previous != null ? cp.Previous.ToString() : string.Empty,
                            SourceId = cp.SourceId.ToString(),
                            SpawnElement = DTOUtils.GetResponseSpawnElement(cp.Spawn),
                            Modules = DTOUtils.GetModules(cp.Modules, _dbName, this.UserId),
                            Name = cp.Name,
                            PatientId = cp.PatientId.ToString(),
                            // ProgramState = (int)cp.ProgramState, depricated - Use Element state instead.
                            ShortName = cp.ShortName,
                            StartDate = cp.StartDate,
                            Status = (int)cp.Status,
                            Version = cp.Version
                        }));
                    }
                }

                return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, patList);
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:Select()::" + ex.Message, ex.InnerException); 
            }
        }

        public IEnumerable<object> SelectAll()
        {
            try
            {
                List<MEPatientProgram> cps = null;
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    cps = ctx.PatientPrograms.Collection.FindAll().ToList();
                }
                return cps;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:SelectAll()::" + ex.Message, ex.InnerException);
            }
        }

        public bool Save(object entity)
        {
            bool success = false;
            MEPatientProgram p = (MEPatientProgram)entity;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    ctx.PatientPrograms.Collection.Save(p);
                    success = true;

                    AuditHelper.LogDataAudit(this.UserId,
                        MongoCollectionName.PatientProgram.ToString(),
                        p.Id.ToString(),
                        Common.DataAuditType.Update,
                        _dbName);
                }
                return success;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:Save()::" + ex.Message, ex.InnerException);
            }
        }

        public object Update(object entity)
        {
            PutProgramActionProcessingRequest p = (PutProgramActionProcessingRequest)entity;
            ProgramDetail pg = p.Program;
            
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(p.ProgramId));
                    List<Module> mods = DTOUtils.CloneAppDomainModules(pg.Modules, this.UserId);

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientProgram.CompletedProperty, pg.Completed));
                    uv.Add(MB.Update.Set(MEPatientProgram.EnabledProperty, pg.Enabled));
                    uv.Add(MB.Update.Set(MEPatientProgram.OrderProperty, pg.Order));
                    //uv.Add(MB.Update.Set(MEPatientProgram.ProgramStateProperty, (ProgramState)pg.ProgramState)); // depricated - Use Element state instead.
                    uv.Add(MB.Update.Set(MEPatientProgram.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientProgram.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEPatientProgram.VersionProperty, pg.Version));
                    if (pg.AssignTo != null && !string.IsNullOrEmpty(pg.AssignTo))
                    {
                        uv.Add(MB.Update.Set(MEPatientProgram.AssignToProperty, pg.AssignTo));
                    }
                    else
                    {
                        uv.Add(MB.Update.Set(MEPatientProgram.AssignToProperty, BsonNull.Value));
                    }

                    if (pg.ElementState != 0) uv.Add(MB.Update.Set(MEPatientProgram.StateProperty, (ElementState)pg.ElementState));
                    if (pg.StateUpdatedOn != null) { uv.Add(MB.Update.Set(MEPatientProgram.StateUpdatedOnProperty, pg.StateUpdatedOn)); }
                    if (pg.Status != 0) uv.Add(MB.Update.Set(MEPatientProgram.StatusProperty, (Status)pg.Status));
                    if (pg.AssignBy != null && !string.IsNullOrEmpty(pg.AssignBy)) { uv.Add(MB.Update.Set(MEPatientProgram.AssignByProperty, pg.AssignBy)); }
                    if (pg.AssignDate != null) { uv.Add(MB.Update.Set(MEPatientProgram.AssignDateProperty, pg.AssignDate)); }
                    if (pg.AttrEndDate != null) { uv.Add(MB.Update.Set(MEPatientProgram.AttributeEndDateProperty, pg.AttrEndDate)); }
                    if (pg.AttrStartDate != null) { uv.Add(MB.Update.Set(MEPatientProgram.AttributeStartDateProperty, pg.AttrStartDate)); }
                    if (pg.Client != null) { uv.Add(MB.Update.Set(MEPatientProgram.ClientProperty, pg.Client)); }
                    if (pg.CompletedBy != null) { uv.Add(MB.Update.Set(MEPatientProgram.CompletedByProperty, pg.CompletedBy)); }
                    if (pg.ContractProgramId != null) { uv.Add(MB.Update.Set(MEPatientProgram.ContractProgramIdProperty, ObjectId.Parse(pg.ContractProgramId))); }
                    if (pg.DateCompleted != null) { uv.Add(MB.Update.Set(MEPatientProgram.CompletedOnProperty, pg.DateCompleted)); }
                    if (pg.Description != null) { uv.Add(MB.Update.Set(MEPatientProgram.DescriptionProperty, pg.Description)); }
                    if (pg.EndDate != null) { uv.Add(MB.Update.Set(MEPatientProgram.EndDateProperty, pg.EndDate)); }
                    if (pg.Name != null) { uv.Add(MB.Update.Set(MEPatientProgram.NameProperty, pg.Name)); }
                    if (pg.Next != null) { uv.Add(MB.Update.Set(MEPatientProgram.NextProperty, DTOUtils.ParseObjectId(pg.Next))); }
                    if (pg.Previous != null) { uv.Add(MB.Update.Set(MEPatientProgram.PreviousProperty, DTOUtils.ParseObjectId(pg.Previous))); }
                    if (pg.ShortName != null) { uv.Add(MB.Update.Set(MEPatientProgram.ShortNameProperty, pg.ShortName)); }
                    if (pg.SourceId != null) { uv.Add(MB.Update.Set(MEPatientProgram.SourceIdProperty, pg.SourceId)); }
                    if (pg.StartDate != null) { uv.Add(MB.Update.Set(MEPatientProgram.StartDateProperty, pg.StartDate)); }
                    if (mods != null) { uv.Add(MB.Update.SetWrapped<List<Module>>(MEPatientProgram.ModulesProperty, mods)); }
                    if (pg.SpawnElement != null) { uv.Add(MB.Update.SetWrapped<List<SpawnElement>>(MEPatientProgram.SpawnProperty, DTOUtils.GetSpawnElements(pg.SpawnElement))); }
                    if (pg.ObjectivesData != null) { uv.Add(MB.Update.SetWrapped<List<Objective>>(MEPatientProgram.ObjectivesInfoProperty, DTOUtils.GetObjectives(pg.ObjectivesData))); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientPrograms.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId, 
                                            MongoCollectionName.PatientProgram.ToString(),
                                            p.ProgramId, 
                                            Common.DataAuditType.Update, 
                                            _dbName);

                }
                return pg;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:Update()::" + ex.Message, ex.InnerException);
            }
        }

        public IEnumerable<object> FindByPatientId(string patientId)
        {
            List<MEPatientProgram> mePPList = null;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientProgram.PatientIdProperty, ObjectId.Parse(patientId)));
                    queries.Add(Query.EQ(MEPatientProgram.DeleteFlagProperty, false));
                    IMongoQuery mQuery = Query.And(queries);
                    mePPList = ctx.PatientPrograms.Collection.Find(mQuery).ToList();
                }
                return mePPList;
            }
            catch (Exception) { throw; }
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
        
        public DTO.Program FindByName(string entityID)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }
        public string ContractNumber { get; set; }

        public IEnumerable<object> Find(string Id)
        {
            throw new NotImplementedException();
        }


        public object FindByPlanElementID(string entityID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindByStepId(string entityID)
        {
            throw new NotImplementedException();
        }

        public object GetLimitedProgramFields(string objectId)
        {
            throw new NotImplementedException();
        }


        public object InsertAsBatch(object newEntity)
        {
            throw new NotImplementedException();
        }

        public static List<BsonValue> ConvertToBsonValueList(List<ObjectId> p)
        {
            List<BsonValue> bsonValues = null;

            if (p.Count() > 0)
            {
                bsonValues = new List<BsonValue>();
                foreach (ObjectId s in p)
                {
                    bsonValues.Add(BsonValue.Create(s));
                }
            }
            return bsonValues;
        }

        public IEnumerable<object> Find(List<ObjectId> Ids)
        {
            try
            {
                List<MEPatientProgramResponse> responses = null;

                IList<IMongoQuery> queries = new List<IMongoQuery>();

                IMongoQuery mQuery = null;
                List<BsonValue> bsonList = ConvertToBsonValueList(Ids);
                if (bsonList != null)
                {
                    mQuery = Query.In(MEPatientProgramResponse.StepIdProperty, bsonList);
                }

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {

                    responses = ctx.PatientProgramResponses.Collection.Find(mQuery).ToList();

                }
                return responses;
            }
            catch (Exception) { throw; }
        }

        public List<Module> GetProgramModules(ObjectId progId)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            UndoDeletePatientProgramDataRequest request = (UndoDeletePatientProgramDataRequest)entity;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var query = MB.Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(request.PatientProgramId));
                    var builder = new List<MB.UpdateBuilder>();
                    builder.Add(MB.Update.Set(MEPatientProgram.TTLDateProperty, BsonNull.Value));
                    builder.Add(MB.Update.Set(MEPatientProgram.DeleteFlagProperty, false));
                    builder.Add(MB.Update.Set(MEPatientProgram.LastUpdatedOnProperty, DateTime.UtcNow));
                    builder.Add(MB.Update.Set(MEPatientProgram.UpdatedByProperty, ObjectId.Parse(this.UserId)));

                    IMongoUpdate update = MB.Update.Combine(builder);
                    ctx.PatientPrograms.Collection.Update(query, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.PatientProgram.ToString(),
                                            request.PatientProgramId.ToString(),
                                            Common.DataAuditType.UndoDelete,
                                            request.ContractNumber);
                }
            }
            catch (Exception) { throw; }
        }
    }
}
