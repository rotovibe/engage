using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Step.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.Step;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Step
{
    public class MongoTextStepRepository<T> : IStepRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoTextStepRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public T Insert(T newEntity)
        {
            throw new NotImplementedException();
        }

        public T InsertAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            GetTextStepDataResponse response = null;
            using (TextStepMongoContext ctx = new TextStepMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(METext.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(METext.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                METext meText = ctx.TextSteps.Collection.Find(mQuery).FirstOrDefault();
                if (meText != null)
                {
                    response = new GetTextStepDataResponse();
                    API.DataDomain.Step.DTO.TextData textStep = new API.DataDomain.Step.DTO.TextData
                    {
                        ID = meText.Id.ToString(),
                        Type = meText.Type.ToString(),
                        Status = Helper.ToFriendlyString(meText.Status),
                        Title = meText.Title,
                        Description = meText.Description,
                        TextEntry = meText.TextEntry
                    };
                    response.TextStep = textStep;
                }
            }
            return response;
        }

        public Tuple<string, IQueryable<T>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> SelectAll()
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
