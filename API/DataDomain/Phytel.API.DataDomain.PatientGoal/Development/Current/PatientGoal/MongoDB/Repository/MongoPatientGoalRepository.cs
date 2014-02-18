using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.PatientGoal;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.Common.Data;
using Phytel.API.DataDomain.PatientGoal;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class MongoPatientGoalRepository<T> : IPatientGoalRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientGoalRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void Delete(object entity)
        {
            DeletePatientGoalDataRequest request = (DeletePatientGoalDataRequest)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientGoal>.EQ(b => b.Id, ObjectId.Parse(request.PatientGoalId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientGoal.TTLDateProperty, System.DateTime.UtcNow.AddDays(7)));
                    uv.Add(MB.Update.Set(MEPatientGoal.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEPatientGoal.UpdatedByProperty, request.UserId));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientGoals.Collection.Update(q, update);
                }
            }
            catch (Exception ex) { throw; }
        }

        public void DeleteAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object FindByID(string entityID)
        {
            PatientGoalData goalData = null;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MEPatientGoal.IdProperty, ObjectId.Parse(entityID)));
                    queries.Add(Query.EQ(MEPatientGoal.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MEPatientGoal.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    MEPatientGoal mePG = ctx.PatientGoals.Collection.Find(mQuery).FirstOrDefault();
                    if (mePG != null)
                    {
                        goalData = new PatientGoalData
                        {
                            Id = mePG.Id.ToString(),
                            PatientId = mePG.PatientId.ToString(),
                            FocusAreaIds = Helper.ConvertToStringList(mePG.FocusAreas),
                            Name = mePG.Name,
                            SourceId = (mePG.Source == null) ? null : mePG.Source.ToString(),
                            ProgramIds = Helper.ConvertToStringList(mePG.Programs),
                            TypeId =((int)mePG.Type),
                            StatusId = ((int)mePG.Status),
                            StartDate = mePG.StartDate,
                            EndDate = mePG.EndDate,
                            TargetDate = mePG.TargetDate,
                            TargetValue = mePG.TargetValue,
                            CustomAttributes = DTOUtil.GetCustomAttributeIdAndValues(mePG.Attributes)
                        };
                    }
                }
                return goalData;
            }
            catch (Exception ex) { throw ex; }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<object> SelectAll()
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object Update(object entity)
        {
            bool result = false;
            PatientGoalData pt = (PatientGoalData)entity;
            try
            {
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientGoal>.EQ(b => b.Id, ObjectId.Parse(pt.Id));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientGoal.TTLDateProperty, BsonNull.Value));
                    if (pt.PatientId != null) uv.Add(MB.Update.Set(MEPatientGoal.PatientIdProperty, ObjectId.Parse(pt.PatientId)));
                    if (pt.FocusAreaIds != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientGoal.FocusAreaProperty, DTOUtil.ConvertObjectId(pt.FocusAreaIds))); }
                    if (pt.Name != null) uv.Add(MB.Update.Set(MEPatientGoal.NameProperty, pt.Name));
                    if (pt.SourceId != null) uv.Add(MB.Update.Set(MEPatientGoal.SourceProperty, pt.SourceId));
                    if (pt.ProgramIds != null) { uv.Add(MB.Update.SetWrapped<List<ObjectId>>(MEPatientGoal.ProgramProperty, DTOUtil.ConvertObjectId(pt.ProgramIds))); }
                    if (pt.TypeId != null) uv.Add(MB.Update.Set(MEPatientGoal.TypeProperty, pt.TypeId)); 
                    if (pt.StatusId != 0) uv.Add(MB.Update.Set(MEPatientGoal.StatusProperty, pt.StatusId ));
                    if (pt.StartDate != null) uv.Add(MB.Update.Set(MEPatientGoal.StartDateProperty, pt.StartDate));
                    if (pt.EndDate != null) uv.Add(MB.Update.Set(MEPatientGoal.EndDateProperty, pt.EndDate));
                    if (pt.TargetValue != null) uv.Add(MB.Update.Set(MEPatientGoal.TargetValueProperty, pt.TargetValue));
                    if (pt.TargetDate != null) uv.Add(MB.Update.Set(MEPatientGoal.TargetDateProperty, pt.TargetDate));
                    if (pt.CustomAttributes != null) { uv.Add(MB.Update.SetWrapped<List<MAttribute>>(MEPatientGoal.AttributesProperty, DTOUtil.GetAttributes(pt.CustomAttributes))); }
                    

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientGoals.Collection.Update(q, update);
                    if (res.Ok)
                        result = true;
                }
                return result as object;
            }
            catch (Exception ex) { throw; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object Initialize(object newEntity)
        {
            PutInitializeGoalDataRequest request = (PutInitializeGoalDataRequest)newEntity;
            PatientGoalData patientGoalData = null;
            try
            {
                MEPatientGoal mePg = new MEPatientGoal
                {
                    Id = ObjectId.GenerateNewId(),
                    PatientId = ObjectId.Parse(request.PatientId),
                    Status = GoalTaskStatus.Open,
                    StatusDate = DateTime.UtcNow,
                    TTLDate = System.DateTime.UtcNow.AddDays(1),
                    UpdatedBy = request.UserId,
                    LastUpdatedOn = DateTime.UtcNow
                };

                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    WriteConcernResult wcr = ctx.PatientGoals.Collection.Insert(mePg);
                    if (wcr.Ok)
                    {
                        patientGoalData = new PatientGoalData
                        {
                            Id = mePg.Id.ToString()
                        };
                    }
                }
                return patientGoalData;
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<object> Find(string Id)
        {
            try
            {
                List<PatientGoalViewData> goalsViewDataList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEPatientGoal.PatientIdProperty, ObjectId.Parse(Id)));
                queries.Add(Query.EQ(MEPatientGoal.DeleteFlagProperty, false));
                queries.Add(Query.EQ(MEPatientGoal.TTLDateProperty, BsonNull.Value));
                IMongoQuery mQuery = Query.And(queries);
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {
                    
                    List<MEPatientGoal> meGoals = ctx.PatientGoals.Collection.Find(mQuery).ToList();
                    if (meGoals != null)
                    {
                        goalsViewDataList = new List<PatientGoalViewData>();
                        foreach (MEPatientGoal b in meGoals)
                        {
                            PatientGoalViewData goalViewData = new PatientGoalViewData
                            {
                                Id = b.Id.ToString(),
                                PatientId = b.PatientId.ToString(),
                                FocusAreaIds = Helper.ConvertToStringList(b.FocusAreas),
                                Name = b.Name,
                                StatusId = ((int)b.Status)

                            };
                            goalsViewDataList.Add(goalViewData);
                        }
                    }
                }
                return goalsViewDataList;
            }
            catch (Exception ex) { throw ex; }
        }


        public IEnumerable<object> FindByGoalId(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
