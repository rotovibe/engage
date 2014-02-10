using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.API.Common;
using Phytel.API.Common.Data;

namespace Phytel.API.DataDomain.Program
{
    public class MongoResponseRepository<T> : IProgramRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoResponseRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            ResponseDetail rs = (ResponseDetail)newEntity;
            MEResponse mer = new MEResponse
            {
                Id = ObjectId.Parse(rs.Id),
                NextStepId = ObjectId.Parse(rs.NextStepId),
                Nominal = rs.Nominal,
                Order = rs.Order,
                Required = rs.Required,
                Spawn = DTOUtils.GetSpawnElements(rs.SpawnElement),
                StepId = ObjectId.Parse(rs.StepId),
                Text = rs.Text,
                Value = rs.Value
            };

            //MEResponse mer = newEntity as MEResponse;
            bool res = false;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    WriteConcernResult result = ctx.Responses.Collection.Insert(mer);
                    if (result.Ok)
                    {
                        res = true;
                    }
                }
                return res as object;
            }
            catch(Exception ex)
            {
                throw new Exception("DataDomain:Insert():" + ex.Message, ex.InnerException);
            }
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
            MEResponse response = null;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEResponse>.EQ(b => b.Id, ObjectId.Parse(entityID));
                    response = ctx.Responses.Collection.Find(findcp).FirstOrDefault();
                }

                return response;
            }
            catch(Exception ex)
            {
                throw new Exception("DataDomain:FindById():" + ex.Message, ex.InnerException);
            }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            IMongoQuery mQuery = null;
            List<object> rps;

            mQuery = MongoDataUtil.ExpressionQueryBuilder(expression);

            using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            {
                rps = ctx.Responses.Collection.Find(mQuery).ToList<object>();
            }

            return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, rps);
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            MEResponse resp = (MEResponse)entity;
            bool result = false;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var q = MB.Query<MEResponse>.EQ(b => b.Id, resp.Id);

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEResponse.NextStepIdProperty, resp.NextStepId));
                    uv.Add(MB.Update.Set(MEResponse.StepIdProperty, resp.StepId));
                    uv.Add(MB.Update.Set(MEResponse.NominalProperty, resp.Nominal));
                    uv.Add(MB.Update.Set(MEResponse.RequiredProperty, resp.Required));
                    if (resp.Order != 0) uv.Add(MB.Update.Set(MEResponse.OrderProperty, resp.Order));
                    if (resp.Text != null) uv.Add(MB.Update.Set(MEResponse.TextProperty, resp.Text));
                    if (resp.Value != null) uv.Add(MB.Update.Set(MEResponse.ValueProperty, resp.Value));
                    if (resp.Spawn != null) { uv.Add(MB.Update.SetWrapped<List<SpawnElement>>(MEResponse.SpawnElementProperty, resp.Spawn)); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.Responses.Collection.Update(q, update);
                    if (res.Ok)
                        result = true;
                }
                return result as object;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:Update():" + ex.Message, ex.InnerException);
            }
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }


        public MEProgram FindByID(string entityID, bool temp)
        {
            throw new NotImplementedException();
        }
    }
}
