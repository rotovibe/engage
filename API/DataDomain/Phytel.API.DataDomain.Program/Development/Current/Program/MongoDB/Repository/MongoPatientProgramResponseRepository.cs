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
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.API.Common;
using Phytel.API.Common.Data;

namespace Phytel.API.DataDomain.Program
{
    public class MongoPatientProgramResponseRepository<T> : IProgramRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientProgramResponseRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            MEPatientProgramResponse mer = newEntity as MEPatientProgramResponse;
            bool res = false;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    WriteConcernResult result = ctx.PatientProgramResponses.Collection.Insert(mer);
                    if (result.Ok)
                    {
                        res = true;
                    }
                }
                return res as object;
            }
            catch(Exception ex)
            {
                throw new Exception("DataDomain:Insert()::" + ex.Message, ex.InnerException);
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
            MEPatientProgramResponse response = null;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEResponse>.EQ(b => b.Id, ObjectId.Parse(entityID));
                    response = ctx.PatientProgramResponses.Collection.Find(findcp).FirstOrDefault();
                }

                return response;
            }
            catch(Exception ex)
            {
                throw new Exception("DataDomain:FindById()::" + ex.Message, ex.InnerException);
            }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            IMongoQuery mQuery = null;
            List<object> rps;

            mQuery = MongoDataUtil.ExpressionQueryBuilder(expression);

            using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            {
                rps = ctx.PatientProgramResponses.Collection.Find(mQuery).ToList<object>();
            }

            return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, rps);
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            MEPatientProgramResponse resp = (MEPatientProgramResponse)entity;
            bool result = false;
            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var q = MB.Query<MEResponse>.EQ(b => b.Id, resp.Id);

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.NextStepIdProperty, resp.NextStepId));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.StepIdProperty, resp.StepId));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.NominalProperty, resp.Nominal));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.RequiredProperty, resp.Required));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.SelectedProperty, resp.Selected));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.VersionProperty, resp.Version));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientProgramResponse.DeleteFlagProperty, resp.DeleteFlag));
                    if (resp.Order != 0) uv.Add(MB.Update.Set(MEPatientProgramResponse.OrderProperty, resp.Order));
                    if (resp.Text != null) uv.Add(MB.Update.Set(MEPatientProgramResponse.TextProperty, resp.Text));
                    if (resp.Value != null) uv.Add(MB.Update.Set(MEPatientProgramResponse.ValueProperty, resp.Value));
                    if (resp.Spawn != null) { uv.Add(MB.Update.SetWrapped<List<SpawnElement>>(MEPatientProgramResponse.SpawnElementProperty, resp.Spawn)); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    WriteConcernResult res = ctx.PatientProgramResponses.Collection.Update(q, update);
                    if (res.Ok)
                        result = true;
                }
                return result as object;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:Update()::" + ex.Message, ex.InnerException);
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
