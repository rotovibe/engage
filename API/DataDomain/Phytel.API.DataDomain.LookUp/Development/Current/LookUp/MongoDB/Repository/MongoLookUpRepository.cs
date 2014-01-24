using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.LookUp;

namespace Phytel.API.DataDomain.LookUp
{
    public class MongoLookUpRepository<T> : ILookUpRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoLookUpRepository(string contractDBName)
        {
            _dbName = contractDBName;
            #region Register ClassMap
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MEProblem)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MEProblem>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MEObjective)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MEObjective>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MECategory)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MECategory>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MECommMode)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MECommMode>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MEState)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MEState>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(METimesOfDay)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<METimesOfDay>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(METimeZone)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<METimeZone>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MECommType)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MECommType>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MELanguage)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MELanguage>();
            } 
            #endregion
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
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

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, IEnumerable<object>> Select(APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        #region Problem
        public object FindProblemByID(string entityID)
        {
            GetProblemDataResponse problemResponse = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Problem));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data.Count > 0)
                    {
                        problemResponse = new GetProblemDataResponse();
                        MEProblem meProblem = (MEProblem)meLookup.Data.Where(a => a.DataID == ObjectId.Parse(entityID)).FirstOrDefault();
                        if (meProblem != null)
                        {
                            ProblemData problemData = new ProblemData { ID = meProblem.DataID.ToString(), Name = meProblem.Name, Active = meProblem.Active };
                            problemResponse.Problem = problemData;
                        }
                    }
                }
            }
            return problemResponse;
        }

        public List<ProblemData> SearchProblem(SearchProblemsDataRequest request)
        {
            List<ProblemData> problemList = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Problem));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data != null)
                    {
                        problemList = new List<ProblemData>();
                        List<MEProblem> meproblems = new List<MEProblem>();
                        if (string.IsNullOrEmpty(request.Type))
                        {
                            meproblems = meLookup.Data.Cast<MEProblem>().Where(a => a.Active == request.Active).ToList();
                        }
                        else
                        {
                            meproblems = meLookup.Data.Cast<MEProblem>().Where(a => a.Type == request.Type && a.Active == request.Active).ToList();
                        }

                        foreach (MEProblem m in meproblems)
                        {
                            ProblemData problem = new ProblemData { ID = m.DataID.ToString(), Name = m.Name, Active = m.Active };
                            problemList.Add(problem);
                        }
                    }

                }
            }
            return problemList;
        }

        public List<ProblemData> GetAllProblems()
        {
            List<ProblemData> problemList = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Problem));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data != null)
                    {
                        problemList = new List<ProblemData>();
                        foreach (MEProblem m in meLookup.Data)
                        {
                            ProblemData problem = new ProblemData { ID = m.DataID.ToString(), Name = m.Name, Active = m.Active };
                            problemList.Add(problem);
                        }
                    }

                }
            }
            return problemList;
        } 
        #endregion

        #region Category
        public object FindCategoryByID(string entityID)
        {
            GetCategoryDataResponse categoryResponse = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.ObjectiveCategory));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data.Count > 0)
                    {
                        categoryResponse = new GetCategoryDataResponse();
                        MECategory meCategory = (MECategory)meLookup.Data.Where(a => a.DataID == ObjectId.Parse(entityID)).FirstOrDefault();
                        if (meCategory != null)
                        {
                            API.DataDomain.LookUp.DTO.LookUpData objective = new API.DataDomain.LookUp.DTO.LookUpData
                            {
                                ID = meCategory.DataID.ToString(),
                                Name = meCategory.Name
                            };
                            categoryResponse.Category = objective;

                        }
                    }
                }
            }
            return categoryResponse;
        } 
        #endregion

        #region Objective
        public object FindObjectiveByID(string entityID)
        {
            GetObjectiveDataResponse objectiveResponse = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Objective));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data.Count > 0)
                    {
                        objectiveResponse = new GetObjectiveDataResponse();
                        MEObjective meObjective = (MEObjective)meLookup.Data.Where(a => a.DataID == ObjectId.Parse(entityID)).FirstOrDefault();
                        if (meObjective != null)
                        {
                            API.DataDomain.LookUp.DTO.LookUpData objective = new API.DataDomain.LookUp.DTO.LookUpData
                            {
                                ID = meObjective.DataID.ToString(),
                                Name = meObjective.Name
                            };
                            objectiveResponse.Objective = objective;

                        }
                    }
                }
            }
            return objectiveResponse;
        } 
        #endregion

        #region ContactRelated LookUps
        public List<LookUpData> GetAllCommModes()
        {
            List<LookUpData> commModeList = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.CommMode));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data != null)
                    {
                        commModeList = new List<LookUpData>();
                        foreach (MECommMode m in meLookup.Data)
                        {
                            LookUpData data = new LookUpData { ID = m.DataID.ToString(), Name = m.Name};
                            commModeList.Add(data);
                        }
                    }

                }
            }
            return commModeList;
        }
        
        public List<StateData> GetAllStates()
        {
            List<StateData> stateList = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.State));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data != null)
                    {
                        stateList = new List<StateData>();
                        foreach (MEState m in meLookup.Data)
                        {
                            StateData data = new StateData { ID = m.DataID.ToString(), Name = m.Name, Code = m.Code };
                            stateList.Add(data);
                        }
                    }

                }
            }
            return stateList;
        }

        public List<LookUpData> GetAllTimesOfDays()
        {
            List<LookUpData> timesOfDayList = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.TimesOfDay));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data != null)
                    {
                        timesOfDayList = new List<LookUpData>();
                        foreach (METimesOfDay m in meLookup.Data)
                        {
                            LookUpData data = new LookUpData { ID = m.DataID.ToString(), Name = m.Name };
                            timesOfDayList.Add(data);
                        }
                    }

                }
            }
            return timesOfDayList;
        }

        public List<TimeZoneData> GetAllTimeZones()
        {
            List<TimeZoneData> timeZoneList = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.TimeZone));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data != null)
                    {
                        timeZoneList = new List<TimeZoneData>();
                        foreach (METimeZone m in meLookup.Data)
                        {
                            TimeZoneData data = new TimeZoneData { ID = m.DataID.ToString(), Name = m.Name, Default = m.Default };
                            timeZoneList.Add(data);
                        }
                    }

                }
            }
            return timeZoneList;
        }

        public List<CommTypeData> GetAllCommTypes()
        {
            List<CommTypeData> commTypeList = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.CommType));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data != null)
                    {
                        commTypeList = new List<CommTypeData>();
                        foreach (MECommType m in meLookup.Data)
                        {
                            List<string> commModes = null;
                            if (m.CommModes.Count > 0)
                            {
                                commModes = new List<string>();
                                foreach (ObjectId id in m.CommModes)
                                {
                                    commModes.Add(id.ToString());
                                }
                            }
                            CommTypeData data = new CommTypeData { ID = m.DataID.ToString(), Name = m.Name, CommModes = commModes };
                            commTypeList.Add(data);
                        }
                    }
                }
            }
            return commTypeList;
        }

        public List<LanguageData> GetAllLanguages()
        {
            List<LanguageData> LanguageList = null;
            using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Language));
                queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                if (meLookup != null)
                {
                    if (meLookup.Data != null)
                    {
                        LanguageList = new List<LanguageData>();
                        foreach (MELanguage m in meLookup.Data)
                        {
                            LanguageData data = new LanguageData { ID = m.DataID.ToString(), Name = m.Name, Code = m.Code };
                            LanguageList.Add(data);
                        }
                    }
                }
            }
            return LanguageList;
        }

        #endregion
    }
}
