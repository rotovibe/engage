using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class MongoModuleRepository<T> : IProgramDesignRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

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
            PutModuleDataRequest request = newEntity as PutModuleDataRequest;

            MEModule module = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                //Does the module exist?
                IMongoQuery query = MB.Query.And(
                                MB.Query.EQ(MEModule.NameProperty, request.Name));

                module = ctx.Modules.Collection.FindOneAs<MEModule>(query);
                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;
                
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
                    var q = MB.Query<MEProgram>.EQ(b => b.Id, ObjectId.Parse(request.ModuleId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEProgram.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEProgram.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEProgram.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEProgram.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.Programs.Collection.Update(q, update);

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
            try
            {
                MEProgram cp = null;
                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEModule>.EQ(b => b.Id, ObjectId.Parse(entityID));
                    cp = ctx.Programs.Collection.Find(findcp).FirstOrDefault();
                }
                return cp;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:ProgramDesign:FindByID()::" + ex.Message, ex.InnerException);
            }
        }

        public object FindByName(string entityName)
        {
            try
            {
                Module result = null;

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEModule>.EQ(b => b.Name, entityName);
                    MEModule cp = ctx.Modules.Collection.Find(findcp).FirstOrDefault();

                    if (cp != null)
                    {
                        result = new Module
                        {
                            Id = cp.Id.ToString()
                        };
                    }
                    else
                    {
                        throw new ArgumentException("ModuleName is not valid or is missing from the records.");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:MongoModuleRepository:FindByName()::" + ex.Message, ex.InnerException);
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
                    {
                        if (request.Name == "\"\"" || (request.Name == "\'\'"))
                            updt.Set(MEModule.NameProperty, string.Empty);
                        else
                            updt.Set(MEModule.NameProperty, request.Name);
                    }
                    if (request.Description != null)
                    {
                        if (request.Description == "\"\"" || (request.Description == "\'\'"))
                            updt.Set(MEModule.DescriptionProperty, string.Empty);
                        else
                            updt.Set(MEModule.DescriptionProperty, request.Description);
                    }

                    updt.Set(MEProgram.OrderProperty, request.Order);
                    updt.Set(MEProgram.LastUpdatedOnProperty, System.DateTime.UtcNow);
                    updt.Set(MEProgram.UpdatedByProperty, ObjectId.Parse(this.UserId));
                    updt.Set(MEProgram.VersionProperty, request.Version);
                    
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


    }
}
