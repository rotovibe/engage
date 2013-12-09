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
    public class MongoYesNoStepRepository<T> : IStepRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoYesNoStepRepository(string contractDBName)
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
            GetYesNoStepDataResponse response = null;
            using (YesNoStepMongoContext ctx = new YesNoStepMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEYesNo.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MEYesNo.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MEYesNo meYesNo = ctx.YesNoSteps.Collection.Find(mQuery).FirstOrDefault();
                if (meYesNo != null)
                {
                    response = new GetYesNoStepDataResponse();
                    API.DataDomain.Step.DTO.YesNoData yesnoStep = new API.DataDomain.Step.DTO.YesNoData
                    {
                        ID = meYesNo.Id.ToString(),
                        Type = meYesNo.Type.ToString(),
                        Status = Helper.ToFriendlyString(meYesNo.Status),
                        Question = meYesNo.Question,
                        Notes = meYesNo.Notes
                    };
                    response.YesNoStep = yesnoStep;
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
