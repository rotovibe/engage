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
using MongoDB.Bson.Serialization;

namespace Phytel.API.DataDomain.LookUp
{
    public class MongoSettingRepository: ILookUpRepository
    {
        private string _dbName = string.Empty;

        static MongoSettingRepository()
        {             
            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MESetting)) == false)
                    BsonClassMap.RegisterClassMap<MESetting>();
            }
            catch { }
            
            #endregion
        }

        public MongoSettingRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object FindProblemByID(string entityID)
        {
            throw new NotImplementedException();
        }

        public List<ProblemData> GetAllProblems()
        {
            throw new NotImplementedException();
        }

        public List<ProblemData> SearchProblem(SearchProblemsDataRequest request)
        {
            throw new NotImplementedException();
        }

        public object FindObjectiveByID(string entityID)
        {
            throw new NotImplementedException();
        }

        public object FindCategoryByID(string entityID)
        {
            throw new NotImplementedException();
        }

        public List<IdNamePair> GetAllCommModes()
        {
            throw new NotImplementedException();
        }

        public List<StateData> GetAllStates()
        {
            throw new NotImplementedException();
        }

        public List<IdNamePair> GetAllTimesOfDays()
        {
            throw new NotImplementedException();
        }

        public List<TimeZoneData> GetAllTimeZones()
        {
            throw new NotImplementedException();
        }

        public List<CommTypeData> GetAllCommTypes()
        {
            throw new NotImplementedException();
        }

        public List<LanguageData> GetAllLanguages()
        {
            throw new NotImplementedException();
        }

        public TimeZoneData GetDefaultTimeZone()
        {
            throw new NotImplementedException();
        }

        public List<IdNamePair> GetLookps(string type)
        {
            throw new NotImplementedException();
        }

        public List<ObjectiveData> GetAllObjectives()
        {
            throw new NotImplementedException();
        }

        public List<LookUpDetailsData> GetLookUpDetails(string type)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }

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
            try
            {
                using (LookUpMongoContext ctx = new LookUpMongoContext(_dbName))
                {
                    List<IMongoQuery> queries = new List<IMongoQuery>();
                    queries.Add(Query.EQ(MESetting.DeleteFlagProperty, false));
                    queries.Add(Query.EQ(MESetting.TTLDateProperty, BsonNull.Value));
                    IMongoQuery mQuery = Query.And(queries);
                    List<MESetting> meS =  ctx.Settings.Collection.Find(mQuery).ToList();
                    return ((IEnumerable<object>)meS);
                }
            }
            catch (Exception) { throw; }
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
