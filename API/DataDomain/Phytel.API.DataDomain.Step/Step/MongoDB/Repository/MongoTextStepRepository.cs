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
using Phytel.API.DataDomain.Step;
using Phytel.API.Common;
using MongoDB.Bson.Serialization;

namespace Phytel.API.DataDomain.Step
{
    public class MongoTextStepRepository<T> : IStepRepository<T>
    {
        private string _dbName = string.Empty;

        static MongoTextStepRepository()
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
                if (BsonClassMap.IsClassMapRegistered(typeof(METext)) == false)
                    BsonClassMap.RegisterClassMap<METext>();
            }
            catch { }
            #endregion
        }

        public MongoTextStepRepository(string contractDBName)
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
                        TextEntry = meText.Text
                    };
                    response.TextStep = textStep;
                }
            }
            return response;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
           GetAllTextStepDataResponse response = new GetAllTextStepDataResponse();

            List<DTO.TextData> list = new List<DTO.TextData>();

            using (TextStepMongoContext ctx = new TextStepMongoContext(_dbName))
            {
                var steps = (from s in ctx.TextSteps
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
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
