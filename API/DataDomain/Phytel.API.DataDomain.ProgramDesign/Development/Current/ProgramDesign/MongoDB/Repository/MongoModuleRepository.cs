using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Phytel.API.DataDomain.Module.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MB = MongoDB.Driver.Builders;
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
                IMongoQuery query = MB.Query.And(
                                MB.Query.EQ(MEModule.NameProperty, request.Name));

                module = ctx.Modules.Collection.FindOneAs<MEModule>(query);
                
                if (module == null)
                {
                    module = new MEModule(request.UserId)
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = request.Name,
                        ProgramId = new ObjectId(request.ProgramId.ToString()),
                        Version = request.Version,
                        UpdatedBy = ObjectId.Parse(request.UserId),
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
            DeleteModuleDataRequest request = entity as DeleteModuleDataRequest;

            try
            {
                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var mUQuery = new QueryDocument(MEModule.IdProperty, ObjectId.Parse(request.ModuleId));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();

                    updt.Set(MEModule.DeleteFlagProperty, true);

                    var module = ctx.Modules.Collection.FindAndModify(mUQuery, MB.SortBy.Null, updt, true);

                    //set audit call
                    AuditHelper.LogDataAudit(this.UserId,
                                           MongoCollectionName.Module.ToString(),
                                           request.ModuleId.ToString(),
                                           Common.DataAuditType.Delete,
                                           request.ContractNumber);
                }
            }
            catch (Exception ex)
            {
                //TODO: handle this error
                throw;
            }
            
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
                              Status = m.Status.ToString(),
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
        
        public GetAllModulesResponse SelectAll(double versionNumber,Status status)
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
                            Status = m.Status.ToString(),
                            Version = m.Version
                        }).ToList();
            }
            response.Modules = list;

            return response;
        }

        //public object Update(object entity)
        //{
        //    throw new NotImplementedException();
        //}

        public object Update(object entity)
        {
            PutUpdateModuleDataRequest request = entity as PutUpdateModuleDataRequest;

            PutUpdateModuleDataResponse response = new PutUpdateModuleDataResponse();
            try
            {
                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var mUQuery = new QueryDocument(MEModule.IdProperty, ObjectId.Parse(request.Id));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();

                    if (request.Name != null)
                        updt.Set(MEModule.NameProperty, GetRequestValue(request.Name));
                        
                    
                    var module = ctx.Modules.Collection.FindAndModify(mUQuery, MB.SortBy.Null, updt, true);

                    //set audit call
                    AuditHelper.LogDataAudit(this.UserId,
                                           MongoCollectionName.Module.ToString(),
                                           request.Id.ToString(),
                                           Common.DataAuditType.Update,
                                           request.ContractNumber);
                }
            }
            catch(Exception ex)
            {
               //TODO: handle this error
                throw;
            }
            
            return response;
            
        }
        

        private string GetRequestValue(string requestValue)
        { 
            if (requestValue == "\"\"" || requestValue == "\'\'")
                return string.Empty;
           else
                return requestValue;
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
