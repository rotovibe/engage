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
using MongoDB.Bson.Serialization;

namespace Phytel.API.DataDomain.Program
{
    public class MongoContractProgramRepository : IProgramRepository
    {
        private string _dbName = string.Empty;

        static MongoContractProgramRepository()
        {
            #region Register ClassMap
            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(ProgramBase)) == false)
                    BsonClassMap.RegisterClassMap<ProgramBase>();
            }
            catch { }

            try
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(MEProgram)) == false)
                    BsonClassMap.RegisterClassMap<MEProgram>();
            }
            catch { }
            #endregion
        }

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

        public DTO.Program FindByName(string entityID)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("DD:ContractProgramRepository:FindByID()::" + ex.Message, ex.InnerException);
            }
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
                        ElementState = (int)r.State,
                        AttrEndDate = r.AttributeEndDate
                    }).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:ContractProgramRepository:GetActiveProgramsInfoList()::" + ex.Message, ex.InnerException);
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

        public string UserId { get; set; }

        public string ContractNumber { get; set; }

        public IEnumerable<object> Find(string Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindByStepId(string entityID)
        {
            throw new NotImplementedException();
        }

        public object FindByPlanElementID(string entityID)
        {
            throw new NotImplementedException();
        }

        public object GetLimitedProgramFields(string objectId)
        {
            throw new NotImplementedException();
        }


        public object InsertAsBatch(object newEntity)
        {
            throw new NotImplementedException();
        }


        public object FindByEntityExistsID(string patientID, string progId)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<object> Find(List<ObjectId> Ids)
        {
            throw new NotImplementedException();
        }

        public bool Save(object entity)
        {
            throw new NotImplementedException();
        }

        public List<Module> GetProgramModules(ObjectId progId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindByPatientId(string patientId)
        {
            throw new NotImplementedException();
        }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
