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
    public class MongoYesNoStepRepository<T> : IStepRepository<T>
    {
        private string _dbName = string.Empty;

        static MongoYesNoStepRepository()
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
                if (BsonClassMap.IsClassMapRegistered(typeof(MEYesNo)) == false)
                    BsonClassMap.RegisterClassMap<MEYesNo>();
            }
            catch {  }
            #endregion
        }

        public MongoYesNoStepRepository(string contractDBName)
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

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            GetAllYesNoStepDataResponse response = new GetAllYesNoStepDataResponse();

            List<DTO.YesNoData> list = new List<DTO.YesNoData>();

            using (YesNoStepMongoContext ctx = new YesNoStepMongoContext(_dbName))
            {
                //var steps = (from a in ctx.YesNoSteps
                //             where a.Id == new ObjectId("531a2d05c3478604270000f8")
                //             select new DTO.YesNoData
                //              {
                //                  ID = a.Id.ToString(),
                //                  //Question = a.Question
                //              }).First();

                var step = from a in ctx.YesNoSteps
                           where a.Id.Equals(new ObjectId("531a2d05c3478604270000f8"))
                           select a;

                //var x = from a in ctx.YesNoSteps
                //             select a;

                //response.Steps = steps;
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
