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
using Phytel.API.DataDomain.PatientGoal;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.Common.Data;
using Phytel.API.DataDomain.PatientGoal;

namespace Phytel.API.DataDomain.PatientGoal
{
    public class MongoAttributeLibraryRepository<T> : IAttributeRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoAttributeLibraryRepository(string contractDBName)
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

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindByType(int TypeId)
        {
            try
            {
                List<CustomAttributeData> customAttributesList = null;
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEAttributeLibrary.TypeProperty, TypeId));
                queries.Add(Query.EQ(MEAttributeLibrary.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                using (PatientGoalMongoContext ctx = new PatientGoalMongoContext(_dbName))
                {

                    List<MEAttributeLibrary> meAttributes = ctx.AttributesLibrary.Collection.Find(mQuery).ToList();
                    if (meAttributes != null)
                    {
                        customAttributesList = new List<CustomAttributeData>();
                        foreach (MEAttributeLibrary b in meAttributes)
                        {
                            CustomAttributeData data = new CustomAttributeData
                            {
                                Id = b.Id.ToString(),
                                Name = b.Name,
                                Type = ((int)(b.Type)),
                                ControlType = ((int)(b.ControlType)),
                                Order  = b.Order,
                                Options = b.Options,
                                Required = b.Required
                            };
                            customAttributesList.Add(data);
                        }
                        customAttributesList = customAttributesList.OrderBy(o => o.Order).ToList();
                    }
                }
                return customAttributesList;
            }
            catch (Exception ex) { throw ex; };
        }

        public string UserId { get; set; }
    }
}