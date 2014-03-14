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
using Phytel.API.DataDomain.LookUp;
using Phytel.API.Common.CustomObject;

namespace Phytel.API.DataDomain.LookUp
{
    public class MongoLookUpRepository<T> : ILookUpRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoLookUpRepository(string contractDBName)
        {
            _dbName = contractDBName;
            #region Register ClassMap
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MELookup)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MELookup>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Problem)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Problem>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(DTO.Objective)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<DTO.Objective>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Category)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Category>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(CommMode)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CommMode>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(State)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<State>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(TimesOfDay)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<TimesOfDay>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(DTO.TimeZone)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<DTO.TimeZone>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(CommType)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CommType>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Language)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Language>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(FocusArea)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<FocusArea>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Source)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Source>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(BarrierCategory)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<BarrierCategory>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(InterventionCategory)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<InterventionCategory>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(ObservationType)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<ObservationType>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(CareMemberType)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CareMemberType>();
            }
            if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(CodingSystem)) == false)
            {
                MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CodingSystem>();
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

        public string UserId { get; set; }

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
                        Problem meProblem = (Problem)meLookup.Data.Where(a => a.DataId == ObjectId.Parse(entityID)).FirstOrDefault();
                        if (meProblem != null)
                        {
                            ProblemData problemData = new ProblemData { Id = meProblem.DataId.ToString(), Name = meProblem.Name, Active = meProblem.Active };
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
                        List<Problem> meproblems = new List<Problem>();
                        if (string.IsNullOrEmpty(request.Type))
                        {
                            meproblems = meLookup.Data.Cast<Problem>().Where(a => a.Active == request.Active).ToList();
                        }
                        else
                        {
                            meproblems = meLookup.Data.Cast<Problem>().Where(a => a.Type == request.Type && a.Active == request.Active).ToList();
                        }

                        foreach (Problem m in meproblems)
                        {
                            ProblemData problem = new ProblemData {  Id = m.DataId.ToString(), Name = m.Name, Active = m.Active };
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
                        foreach (Problem m in meLookup.Data)
                        {
                            ProblemData problem = new ProblemData { Id = m.DataId.ToString(), Name = m.Name, Active = m.Active };
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
                        Category meCategory = (Category)meLookup.Data.Where(a => a.DataId == ObjectId.Parse(entityID)).FirstOrDefault();
                        if (meCategory != null)
                        {
                            IdNamePair objective = new IdNamePair
                            {
                                Id = meCategory.DataId.ToString(),
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
                        DTO.Objective meObjective = (DTO.Objective)meLookup.Data.Where(a => a.DataId == ObjectId.Parse(entityID)).FirstOrDefault();
                        if (meObjective != null)
                        {
                            IdNamePair objective = new IdNamePair
                            {
                                Id = meObjective.DataId.ToString(),
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
        public List<IdNamePair> GetAllCommModes()
        {
            List<IdNamePair> commModeList = null;
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
                        commModeList = new List<IdNamePair>();
                        foreach (CommMode m in meLookup.Data)
                        {
                            IdNamePair data = new IdNamePair { Id = m.DataId.ToString(), Name = m.Name };
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
                        foreach (State m in meLookup.Data)
                        {
                            StateData data = new StateData { Id = m.DataId.ToString(), Name = m.Name, Code = m.Code };
                            stateList.Add(data);
                        }
                    }

                }
            }
            return stateList;
        }

        public List<IdNamePair> GetAllTimesOfDays()
        {
            List<IdNamePair> timesOfDayList = null;
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
                        timesOfDayList = new List<IdNamePair>();
                        foreach (TimesOfDay m in meLookup.Data)
                        {
                            IdNamePair data = new IdNamePair { Id = m.DataId.ToString(), Name = m.Name };
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
                        foreach (DTO.TimeZone m in meLookup.Data)
                        {
                            TimeZoneData data = new TimeZoneData { Id = m.DataId.ToString(), Name = m.Name, Default = m.Default };
                            timeZoneList.Add(data);
                        }
                        timeZoneList = timeZoneList.OrderBy(s => s.Name).ToList();
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
                        foreach (CommType m in meLookup.Data)
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
                            CommTypeData data = new CommTypeData { Id = m.DataId.ToString(), Name = m.Name, CommModes = commModes };
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
                        foreach (Language m in meLookup.Data)
                        {
                            LanguageData data = new LanguageData { Id = m.DataId.ToString(), Name = m.Name, Code = m.Code, Active = m.Active };
                            LanguageList.Add(data);
                        }
                    }
                }
            }
            return LanguageList;
        }


        public TimeZoneData GetDefaultTimeZone()
        {
           TimeZoneData tz = null;
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
                        tz = new TimeZoneData();
                        DTO.TimeZone meTz = meLookup.Data.Cast<DTO.TimeZone>().Where(a => a.Default == true).FirstOrDefault();
                        if (meTz != null)
                        {
                            tz = new TimeZoneData { Id = meTz.DataId.ToString(), Name = meTz.Name, Default = meTz.Default };
                        }
                    }
                }
            }
            return tz;
        }

        #endregion

        #region Refactored Common LookUps
        public List<IdNamePair> GetLookps(string type)
        {
            List<IdNamePair> lookupList = null;
            try
            {
                LookUpType lookUpValue;
                if (Enum.TryParse(type, true, out lookUpValue))
                {
                    using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
                    {
                        List<IMongoQuery> queries = new List<IMongoQuery>();
                        queries.Add(Query.EQ(MELookup.TypeProperty, lookUpValue));
                        queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                        IMongoQuery mQuery = Query.And(queries);
                        MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                        if (meLookup != null)
                        {
                            if (meLookup.Data != null)
                            {
                                lookupList = new List<IdNamePair>();
                                foreach (LookUpBase m in meLookup.Data)
                                {
                                    IdNamePair data = new IdNamePair { Id = m.DataId.ToString(), Name = m.Name };
                                    lookupList.Add(data);
                                }
                                lookupList = lookupList.OrderBy(s => s.Name).ToList();
                            }
                        }
                    }
                }
                else
                {
                    throw new ApplicationException("Type requested does not exists.");
                }
            }
            catch (Exception ex)
            { 
                throw ex;
            }
            return lookupList;
        }
        #endregion  
    }
}
