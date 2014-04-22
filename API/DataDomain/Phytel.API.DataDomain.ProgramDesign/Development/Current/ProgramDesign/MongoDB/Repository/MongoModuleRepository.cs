using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Phytel.API.DataDomain.Module.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
//using Phytel.API.DataDomain.Module;
//using Phytel.API.DataDomain.Module.MongoDB.DTO;
using MongoDB.Bson.Serialization;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO;
using Phytel.API.DataAudit;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class MongoModuleRepository<T> : IProgramDesignRepository<T>
    {
        private string _dbName = string.Empty;

        static MongoModuleRepository()
        {
            try 
            { 
                #region Register ClassMap
                if (BsonClassMap.IsClassMapRegistered(typeof(MEModule)) == false)
                    BsonClassMap.RegisterClassMap<MEModule>();
                #endregion
            }
            catch { }
        }
        
        public MongoModuleRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            GetModuleResponse response = null;
            PutModuleDataRequest request = newEntity as PutModuleDataRequest;

            MEModule module = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                //Does the module exist?
                IMongoQuery query = Query.And(
                                Query.EQ(MEModule.NameProperty, request.Name));

                module = ctx.Modules.Collection.FindOneAs<MEModule>(query);
                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                if (module == null)
                {
                    module = new MEModule()
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = request.Name,
                        ProgramId = new ObjectId(request.ProgramId.ToString()),
                        Version = request.Version,
                        UpdatedBy = ObjectId.Parse(this.UserId),
                        TTLDate = null,
                        DeleteFlag = false,
                        LastUpdatedOn = System.DateTime.UtcNow
                    };
                }
                ctx.Modules.Collection.Insert(module);

                AuditHelper.LogDataAudit(this.UserId,
                                           MongoCollectionName.Module.ToString(),
                                           module.Id.ToString(),
                                           Common.DataAuditType.Insert,
                                           request.ContractNumber);

                return new PutModuleDataResponse
                {
                    Id = module.Id.ToString()
                };

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
            DTO.Module module = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                module = (from m in ctx.Modules
                          where m.Id == ObjectId.Parse(entityID)
                          select new DTO.Module
                          {
                              Id = m.Id.ToString(),
                              Name = m.Name,
                              Description = m.Description,
                              //Objectives = m.Objectives.Select(i => i.ID).ToList(),
                              Status = m.Status,
                              Version = m.Version
                          }).FirstOrDefault();
            }
            return module;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }
        
        public GetAllModulesResponse SelectAll(double versionNumber, Common.Status status)
        {
            GetAllModulesResponse response = new GetAllModulesResponse()
            {
                Version = versionNumber
            };
            
            List<DTO.Module> list = new List<DTO.Module>();
           
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                list = (from m in ctx.Modules
                        select new DTO.Module
                        {
                            Id = m.Id.ToString(),
                            Name = m.Name,
                            Description = m.Description,
                            Status = m.Status,
                            Version = m.Version
                        }).ToList();
            }
            response.Modules = list;

            return response;
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


        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.Program FindByName(string entityName)
        {
            throw new NotImplementedException();
        }
    }
}
