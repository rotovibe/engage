using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;

namespace Phytel.API.DataDomain.Program
{
    public class MongoContractProgramRepository<T> : IProgramRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoContractProgramRepository(string contractDBName)
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

        public MEProgram FindByID(string entityID, bool temp)
        {
            MEProgram program = null;
            using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            {
                var findcp = Query<MEProgram>.EQ(b => b.Id, ObjectId.Parse(entityID));
                program = ctx.Programs.Collection.Find(findcp).FirstOrDefault();
            }
            return program;
        }

        public object FindByID(string entityID)
        {
            ContractProgram program = null;
            using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            {
                program = (from p in ctx.Programs
                           where p.Id == ObjectId.Parse(entityID)
                           select new ContractProgram
                           {
                               Delete = p.DeleteFlag,
                               Id = p.Id.ToString(),
                               Name = p.Name,
                               ShortName = p.ShortName,
                               Status = (int)p.Status
                           }).FirstOrDefault();
            }
            return program;
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            List<ProgramInfo> result = new List<ProgramInfo>();

            try
            {
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    IMongoQuery mQuery = Query.EQ(MEProgram.StatusProperty, 1);
                    MongoCursor<MEProgram> fnd = ctx.Programs.Collection.Find(mQuery);

                    result = ctx.Programs.Collection.Find(mQuery).Select(r => new ProgramInfo
                    {
                        Name = r.Name,
                        Id = r.Id.ToString(),
                        ShortName = r.ShortName,
                        Status = (int)r.Status,
                        ElementState = (int)r.State
                    }).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetActiveProgramsInfoList()::" + ex.Message, ex.InnerException);
            }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
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
    }
}
