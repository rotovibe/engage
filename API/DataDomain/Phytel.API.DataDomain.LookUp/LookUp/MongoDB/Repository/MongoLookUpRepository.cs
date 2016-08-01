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
using System.Configuration;

namespace Phytel.API.DataDomain.LookUp
{
    public class MongoLookUpRepository: ILookUpRepository
    {
        private string _dbName = string.Empty;
        static readonly string redisClientIPAddress;
        static readonly int redisCacheExpiry = 0;

        static MongoLookUpRepository()
        {
            
            #region Register ClassMap
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MELookup)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MELookup>();
                }
            }
            catch {  }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Problem)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Problem>();
                }
            }
            catch { }
                           
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(DTO.Objective)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<DTO.Objective>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Category)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Category>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(CommMode)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CommMode>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(State)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<State>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(TimesOfDay)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<TimesOfDay>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(DTO.TimeZone)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<DTO.TimeZone>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(CommType)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CommType>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Language)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Language>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(FocusArea)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<FocusArea>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Source)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Source>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(BarrierCategory)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<BarrierCategory>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(InterventionCategory)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<InterventionCategory>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(ObservationType)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<ObservationType>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(CareMemberType)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CareMemberType>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(CodingSystem)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CodingSystem>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(ToDoCategory)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<ToDoCategory>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(NoteMethod)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<NoteMethod>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(NoteOutcome)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<NoteOutcome>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(NoteWho)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<NoteWho>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(NoteSource)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<NoteSource>();
                }
            }
            catch { }

            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(NoteDuration)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<NoteDuration>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(AllergyType)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<AllergyType>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Reaction)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Reaction>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Severity)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Severity>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(AllergySource)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<AllergySource>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MedSuppType)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MedSuppType>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(FreqHowOften)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<FreqHowOften>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(FreqWhen)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<FreqWhen>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(NoteType)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<NoteType>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Frequency)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Frequency>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(VisitType)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<VisitType>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(UtilizationSource)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<UtilizationSource>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Disposition)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Disposition>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(UtilizationLocation)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<UtilizationLocation>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(Reason)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<Reason>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MaritalStatus)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MaritalStatus>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(CareTeamFrequency)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<CareTeamFrequency>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MedicationReview)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MedicationReview>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(DurationUnit)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<DurationUnit>();
                }
            }
            catch { }
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(RefusalReason)) == false)
                {
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<RefusalReason>();
                }
            }
            catch { } 
            #endregion

            // Get the redis IP address from config file.
            redisClientIPAddress = ConfigurationManager.AppSettings.Get("RedisClientIPAddress");
            // Get the cache expiry time from config file.
            string expiry = ConfigurationManager.AppSettings.Get("RedisCacheExpireInMinutes");
            if (!string.IsNullOrEmpty(expiry))
                redisCacheExpiry = Convert.ToInt32(expiry);
        }
        public MongoLookUpRepository(string contractDBName)
        {
            _dbName = contractDBName;
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
            ProblemData problemData = null;
            try
            {
                problemData = new ProblemData();
                string redisKey = string.Format("{0}{1}{2}", "Lookup", "Problem", entityID);
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                // if(!string.IsNullOrEmpty(redisClientIPAddress))
                //  client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get problemData from Redis using the redisKey now
                    problemData = client.Get<ProblemData>(redisKey);
                }
                else
                {
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
                                Problem meProblem = (Problem)meLookup.Data.Where(a => a.DataId == ObjectId.Parse(entityID)).FirstOrDefault();
                                if (meProblem != null)
                                {
                                    problemData.Id = meProblem.DataId.ToString();
                                    problemData.Name = meProblem.Name;
                                    problemData.Active = meProblem.Active ;
                                }
                            }
                        }
                    }
                    //put problemData into cache using redisKey now
                    if (client != null)
                        client.Set<ProblemData>(redisKey, problemData, TimeSpan.FromMinutes(redisCacheExpiry));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return problemData;
        }

        public List<ProblemData> SearchProblem(SearchProblemsDataRequest request)
        {
            List<ProblemData> problemList = null;
            try
            {
                problemList = new List<ProblemData>();
                string redisKey = string.Format("{0}{1}{2}{3}", "Lookup", "Problem", request.Active, request.Type);
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if (!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get problemList from Redis using the redisKey now
                    problemList = client.Get<List<ProblemData>>(redisKey);
                }
                else
                {
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
                    //put problemList into cache using redisKey now
                    if (client != null)
                        client.Set<List<ProblemData>>(redisKey, problemList, TimeSpan.FromMinutes(redisCacheExpiry));
                }
            }
            catch (Exception ex)
            { 
                throw ex;
            }
            return problemList;
        }

        public List<ProblemData> GetAllProblems()
        {
            List<ProblemData> problemList = null;
            try
            {
                problemList = new List<ProblemData>();
                string redisKey = string.Format("{0}{1}", "Lookup", "Problems");
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if (!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get problemList from Redis using the redisKey now
                    problemList = client.Get<List<ProblemData>>(redisKey);
                }
                else
                {
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
                                foreach (Problem m in meLookup.Data)
                                {
                                    ProblemData problem = new ProblemData { Id = m.DataId.ToString(), Name = m.Name, Active = m.Active };
                                    problemList.Add(problem);
                                }
                            }

                        }
                    }
                    //put problemList into cache using redisKey now
                    if (client != null)
                        client.Set<List<ProblemData>>(redisKey, problemList, TimeSpan.FromMinutes(redisCacheExpiry));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return problemList;
        } 
        #endregion

        #region Category
        public object FindCategoryByID(string entityID)
        {
            IdNamePair category = null;
            try
            {
                category = new IdNamePair();
                string redisKey = string.Format("{0}{1}{2}", "Lookup", "Category", entityID);
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                // if(!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get category from Redis using the redisKey now
                    category = client.Get<IdNamePair>(redisKey);
                }
                else
                {
                    using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
                    {
                        List<IMongoQuery> queries = new List<IMongoQuery>();
                        queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Category));
                        queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                        IMongoQuery mQuery = Query.And(queries);
                        MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                        if (meLookup != null)
                        {
                            if (meLookup.Data.Count > 0)
                            {
                                Category meCategory = (Category)meLookup.Data.Where(a => a.DataId == ObjectId.Parse(entityID)).FirstOrDefault();
                                if (meCategory != null)
                                {
                                    IdNamePair objective = new IdNamePair
                                    {
                                        Id = meCategory.DataId.ToString(),
                                        Name = meCategory.Name
                                    };
                                    category = objective;

                                }
                            }
                        }
                    }
                    //put category into cache using redisKey now
                    if (client != null)
                        client.Set<IdNamePair>(redisKey, category, TimeSpan.FromMinutes(redisCacheExpiry));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return category;
        } 
        #endregion

        #region Objective
        public object FindObjectiveByID(string entityID)
        {
            IdNamePair objective = null;
            try
            {
                objective = new IdNamePair();
                string redisKey = string.Format("{0}{1}{2}", "Lookup", "Objective", entityID);
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if (!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get objective from Redis using the redisKey now
                    objective = client.Get<IdNamePair>(redisKey);
                }
                else
                {
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
                                DTO.Objective meObjective = (DTO.Objective)meLookup.Data.Where(a => a.DataId == ObjectId.Parse(entityID)).FirstOrDefault();
                                if (meObjective != null)
                                {
                                    objective.Id = meObjective.DataId.ToString();
                                    objective.Name = meObjective.Name;
                                }
                            }
                        }
                    }
                    //put objective into cache using redisKey now
                    if (client != null)
                        client.Set<IdNamePair>(redisKey, objective, TimeSpan.FromMinutes(redisCacheExpiry));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objective;
        } 
        #endregion

        #region ContactRelated LookUps
        public List<IdNamePair> GetAllCommModes()
        {
            List<IdNamePair> commModeList = null;
            try 
            {
                commModeList = new List<IdNamePair>();
                string redisKey = string.Format("{0}{1}", "Lookup", "CommModes");
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                // if (!string.IsNullOrEmpty(redisClientIPAddress))
                //   client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get commModeList from Redis using the redisKey now
                    commModeList = client.Get<List<IdNamePair>>(redisKey);
                }
                else
                {
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
                                foreach (CommMode m in meLookup.Data)
                                {
                                    IdNamePair data = new IdNamePair { Id = m.DataId.ToString(), Name = m.Name };
                                    commModeList.Add(data);
                                }
                            }

                        }
                    }
                    //put commModeList into cache using redisKey now
                    if (client != null)
                        client.Set<List<IdNamePair>>(redisKey, commModeList, TimeSpan.FromMinutes(redisCacheExpiry));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return commModeList;
        }
        
        public List<StateData> GetAllStates()
        {
            List<StateData> stateList = null;
            try
            {
                stateList = new List<StateData>();
                string redisKey = string.Format("{0}{1}", "Lookup", "States");
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if (!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get stateList from Redis using the redisKey now
                    stateList = client.Get<List<StateData>>(redisKey);
                }
                else
                {
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
                                foreach (State m in meLookup.Data)
                                {
                                    StateData data = new StateData { Id = m.DataId.ToString(), Name = m.Name, Code = m.Code };
                                    stateList.Add(data);
                                }
                            }

                        }
                    }
                }
                //put stateList into cache using redisKey now
                if (client != null)
                    client.Set<List<StateData>>(redisKey, stateList, TimeSpan.FromMinutes(redisCacheExpiry));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return stateList;
        }

        public List<IdNamePair> GetAllTimesOfDays()
        {
            List<IdNamePair> timesOfDayList = null;
            try
            {
                timesOfDayList = new List<IdNamePair>();
                string redisKey = string.Format("{0}{1}", "Lookup", "TimesOfDays");
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if (!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get timesOfDayList from Redis using the redisKey now
                    timesOfDayList = client.Get<List<IdNamePair>>(redisKey);
                }
                else
                {   
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
                                foreach (TimesOfDay m in meLookup.Data)
                                {
                                    IdNamePair data = new IdNamePair { Id = m.DataId.ToString(), Name = m.Name };
                                    timesOfDayList.Add(data);
                                }
                            }

                        }
                    }
                }
                //put timesOfDayList into cache using redisKey now
                if (client != null)
                    client.Set<List<IdNamePair>>(redisKey, timesOfDayList, TimeSpan.FromMinutes(redisCacheExpiry));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return timesOfDayList;
        }

        public List<TimeZoneData> GetAllTimeZones()
        {
            List<TimeZoneData> timeZoneList = null;
            try
            {
                timeZoneList = new List<TimeZoneData>();
                string redisKey = string.Format("{0}{1}", "Lookup", "TimeZones");
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if (!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get timeZoneList from Redis using the redisKey now
                    timeZoneList = client.Get<List<TimeZoneData>>(redisKey);
                }
                else
                {
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
                            foreach (DTO.TimeZone m in meLookup.Data)
                            {
                                TimeZoneData data = new TimeZoneData { Id = m.DataId.ToString(), Name = m.Name, Default = m.Default };
                                timeZoneList.Add(data);
                            }
                            timeZoneList = timeZoneList.OrderBy(s => s.Name).ToList();
                        }

                    }
                }
            }
            //put timeZoneList into cache using redisKey now
            if (client != null)
                client.Set<List<TimeZoneData>>(redisKey, timeZoneList, TimeSpan.FromMinutes(redisCacheExpiry));

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return timeZoneList;
        }

        public List<CommTypeData> GetAllCommTypes()
        {
            List<CommTypeData> commTypeList = null;
            try 
            {
                commTypeList = new List<CommTypeData>();
                string redisKey = string.Format("{0}{1}", "Lookup", "CommTypes");
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if (!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get commTypeList from Redis using the redisKey now
                    commTypeList = client.Get<List<CommTypeData>>(redisKey);
                }
                else
                {
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
                    //put commTypeList into cache using redisKey now
                    if (client != null)
                        client.Set<List<CommTypeData>>(redisKey, commTypeList, TimeSpan.FromMinutes(redisCacheExpiry));
                }
            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return commTypeList;
        }

        public List<LanguageData> GetAllLanguages()
        {
            List<LanguageData> languageList = null;
            try
            {
                languageList = new List<LanguageData>();
                string redisKey = string.Format("{0}{1}", "Lookup", "Languages");
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if (!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get languageList from Redis using the redisKey now
                    languageList = client.Get<List<LanguageData>>(redisKey);
                }
                else
                {
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
                                foreach (Language m in meLookup.Data)
                                {
                                    LanguageData data = new LanguageData { Id = m.DataId.ToString(), Name = m.Name, Code = m.Code, Active = m.Active };
                                    languageList.Add(data);
                                }
                            }
                        }
                    }
                    //put languageList into cache using redisKey now
                    if (client != null)
                        client.Set<List<LanguageData>>(redisKey, languageList, TimeSpan.FromMinutes(redisCacheExpiry));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return languageList;
        }


        public TimeZoneData GetDefaultTimeZone()
        {
           TimeZoneData tz = null;
            try
            {
                tz = new TimeZoneData();
                string redisKey = string.Format("{0}{1}", "Lookup", "DefaultTimeZone");
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if (!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get tz from Redis using the redisKey now
                    tz = client.Get<TimeZoneData>(redisKey);
                }
                else
                {
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
                                DTO.TimeZone meTz = meLookup.Data.Cast<DTO.TimeZone>().Where(a => a.Default == true).FirstOrDefault();
                                if (meTz != null)
                                {
                                    tz = new TimeZoneData { Id = meTz.DataId.ToString(), Name = meTz.Name, Default = meTz.Default };
                                }
                            }
                        }
                    }
                    //put tz into cache using redisKey now
                    if (client != null)
                        client.Set<TimeZoneData>(redisKey, tz, TimeSpan.FromMinutes(redisCacheExpiry));
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    lookupList = new List<IdNamePair>();
                    string redisKey = string.Format("{0}{1}", "Lookup", type);
                    ServiceStack.Redis.RedisClient client = null;

                    //TODO: Uncomment the following 2 lines to turn Redis cache on
                    //if(!string.IsNullOrEmpty(redisClientIPAddress))
                    //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                    //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                    if (client != null && client.ContainsKey(redisKey))
                    {
                        //go get lookupList from Redis using the redisKey now
                        lookupList = client.Get<List<IdNamePair>>(redisKey);
                    }
                    else
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
                                
                                    foreach (LookUpBase m in meLookup.Data)
                                    {
                                        IdNamePair data = new IdNamePair { Id = m.DataId.ToString(), Name = m.Name };
                                        lookupList.Add(data);
                                    }
                                    lookupList = lookupList.OrderBy(s => s.Name).ToList();
                                }
                            }
                        }
                        //put lookupList into cache using redisKey now
                        if (client != null)
                            client.Set<List<IdNamePair>>(redisKey, lookupList, TimeSpan.FromMinutes(redisCacheExpiry));
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

        public List<LookUpDetailsData> GetLookUpDetails(string type)
        {
            List<LookUpDetailsData> lookupList = null;
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
                            if (meLookup.Data != null && meLookup.Data.Count > 0)
                            {
                                lookupList = new List<LookUpDetailsData>();
                                foreach (LookUpDetailsBase m in meLookup.Data)
                                {
                                    if(m.Active)
                                    {
                                        LookUpDetailsData data = new LookUpDetailsData { Id = m.DataId.ToString(), Name = m.Name, IsDefault = m.Default };
                                        lookupList.Add(data);
                                    }       
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

        #region Program
        public List<ObjectiveData> GetAllObjectives()
        {
            List<ObjectiveData> objectiveList = null;
            try
            {
                objectiveList = new List<ObjectiveData>();
                string redisKey = string.Format("{0}{1}", "Lookup", "Objectives");
                ServiceStack.Redis.RedisClient client = null;

                //TODO: Uncomment the following 2 lines to turn Redis cache on
                //if (!string.IsNullOrEmpty(redisClientIPAddress))
                //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

                //If the redisKey is already in Cache (REDIS) get it from there, else re-query
                if (client != null && client.ContainsKey(redisKey))
                {
                    //go get languageList from Redis using the redisKey now
                    objectiveList = client.Get<List<ObjectiveData>>(redisKey);
                }
                else
                {
                    using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
                    {
                        List<IMongoQuery> queries = new List<IMongoQuery>();
                        queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Objective));
                        queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
                        IMongoQuery mQuery = Query.And(queries);
                        MELookup meLookup = ctx.LookUps.Collection.Find(mQuery).FirstOrDefault();
                        if (meLookup != null)
                        {
                            if (meLookup.Data != null & meLookup.Data.Count > 0)
                            {
                                // Get all objective cateogories.
                                List<IdNamePair> categoryData = GetLookps(Enum.GetName(typeof(LookUpType), (LookUpType.Category)));
                                foreach (Objective m in meLookup.Data)
                                {
                                    List<IdNamePair> categories = null; 
                                    if(m.CategoryIds != null && m.CategoryIds.Count > 0)
                                    {
                                        categories = new List<IdNamePair>();
                                        foreach (ObjectId id in m.CategoryIds)
                                        {
                                            IdNamePair cat = categoryData.Where(a => a.Id == id.ToString()).FirstOrDefault();
                                            if(cat != null)
                                            {
                                                categories.Add(cat);
                                            }
                                        }
                                    }
                                    ObjectiveData data = new ObjectiveData { Id = m.DataId.ToString(), Name = m.Name, CategoriesData = categories };
                                    objectiveList.Add(data);
                                }
                            }
                        }
                    }
                    //put languageList into cache using redisKey now
                    if (client != null)
                        client.Set<List<ObjectiveData>>(redisKey, objectiveList, TimeSpan.FromMinutes(redisCacheExpiry));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objectiveList;
        } 
        #endregion


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }

    }
}
