using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using Phytel.API.DataDomain.ProgramDesign;
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
using Phytel.API.DataAudit;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class MongoProgramRepository<T> : IProgramDesignRepository<T>
    {
        private string _dbName = string.Empty;
        private int _expireDays = Convert.ToInt32(ConfigurationManager.AppSettings["ExpireDays"]);

        static MongoProgramRepository()
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

        public MongoProgramRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            PutProgramDataRequest request = newEntity as PutProgramDataRequest;

            MEProgram program = null;
            using(ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                IMongoQuery query = Query.And(
                                Query.EQ(MEProgram.NameProperty, request.Name));
                program = ctx.Modules.Collection.FindOneAs<MEProgram>(query);
                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                if (program == null)
                {
                    program = new MEProgram(this.UserId)
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = request.Name,
                        Description = request.Description,
                        ShortName = request.ShortName,
                        AssignedBy = request.AssignedBy,
                        AssignedOn = DateTime.Parse(request.AssignedOn),
                        Client = ObjectId.Parse(request.Client),
                        Order = request.Order
                    };
                }
                ctx.Programs.Collection.Insert(program);

                AuditHelper.LogDataAudit(this.UserId,
                                           MongoCollectionName.Program.ToString(),
                                           program.Id.ToString(),
                                           Common.DataAuditType.Insert,
                                           request.ContractNumber);
            }

            return new PutProgramDataResponse
            {
                Id = program.Id.ToString()
            };
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            DeleteProgramDataRequest request = (DeleteProgramDataRequest) entity;
            try
            {
                using(ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var q = MB.Query<MEProgram>.EQ(b => b.Id, ObjectId.Parse(request.ProgramId));

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEProgram.TTLDateProperty, System.DateTime.UtcNow.AddDays(_expireDays)));
                    uv.Add(MB.Update.Set(MEProgram.DeleteFlagProperty, true));
                    uv.Add(MB.Update.Set(MEProgram.UpdatedByProperty, ObjectId.Parse(this.UserId)));
                    uv.Add(MB.Update.Set(MEProgram.LastUpdatedOnProperty, DateTime.UtcNow));

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.Programs.Collection.Update(q, update);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Program.ToString(),
                                            request.ProgramId.ToString(),
                                            Common.DataAuditType.Delete,
                                            request.ContractNumber);
                }

            }
            catch(Exception ex)
            {
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
                    var findcp = MB.Query<MEProgram>.EQ(b => b.Id, ObjectId.Parse(entityID));
                    cp = ctx.Programs.Collection.Find(findcp).FirstOrDefault();
                }
                return cp;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:Program:FindByID()::" + ex.Message, ex.InnerException);
            }
        }

        public Program FindByName(string entityName)
        {
            try
            {
                Program result = null;

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEProgram>.EQ(b => b.Name, entityName);
                    MEProgram cp = ctx.Programs.Collection.Find(findcp).FirstOrDefault();

                    if (cp != null)
                    {
                        result = new Program
                            {
                                ProgramID = cp.Id.ToString()
                            };
                    }
                    else
                    {
                        throw new ArgumentException("ProgramName is not valid or is missing from the records.");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:FindByName()::" + ex.Message, ex.InnerException);
            }
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public GetAllProgramsResponse SelectAll(double versionNumber)
        {
            GetAllProgramsResponse response = new GetAllProgramsResponse
            {
                Version = versionNumber 
            };

            List<Program> list = new List<Program>();

            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                list = (from m in ctx.Programs
                        select new DTO.Program
                        {
                            ProgramID = m.Id.ToString(),
                            Name = m.Name,
                            Description = m.Description,
                            //Status = m.Status,
                            Version = m.Version
                        }).ToList();
            }
            response.Programs = list;

            return response;
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public object Update(PutUpdateProgramDataRequest request)
        {
            PutUpdateProgramDataResponse response = new PutUpdateProgramDataResponse();
            try
            {
                if (request.UserId == null)
                    throw new ArgumentException("UserId is missing from the DataDomain request.");

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(MEProgram.IdProperty, ObjectId.Parse(request.Id));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.Name != null)
                    {
                        if (request.Name == "\"\"" || (request.Name == "\'\'"))
                            updt.Set(MEProgram.NameProperty, string.Empty);
                        else
                            updt.Set(MEProgram.NameProperty, request.Name);
                    }
                    if (request.AssignedBy != null)
                    {
                        if (request.AssignedBy == "\"\"" || (request.AssignedBy == "\'\'"))
                            updt.Set(MEProgram.AssignByProperty, string.Empty);
                        else
                            updt.Set(MEProgram.AssignByProperty, request.AssignedBy);
                    }
                    if (request.AssignedOn != null)
                    {
                        if (request.AssignedOn == "\"\"" || (request.AssignedOn == "\'\'"))
                            updt.Set(MEProgram.AssignDateProperty, string.Empty);
                        else
                            updt.Set(MEProgram.AssignDateProperty, request.AssignedOn);
                    }
                    if (request.Client != null)
                    {
                        if (request.Client == "\"\"" || (request.Client == "\'\'"))
                            updt.Set(MEProgram.ClientProperty, string.Empty);
                        else
                            updt.Set(MEProgram.ClientProperty, request.Client);
                    }
                    if (request.Description != null)
                    {
                        if (request.Description == "\"\"" || (request.Description == "\'\'"))
                            updt.Set(MEProgram.DescriptionProperty, string.Empty);
                        else
                            updt.Set(MEProgram.DescriptionProperty, request.Description);
                    }
                    if (request.ShortName != null)
                    {
                        if (request.ShortName == "\"\"" || (request.ShortName == "\'\'"))
                            updt.Set(MEProgram.ShortNameProperty, string.Empty);
                        else
                            updt.Set(MEProgram.ShortNameProperty, request.ShortName);
                    }

                    updt.Set(MEProgram.OrderProperty, request.Order);
                    updt.Set(MEProgram.LastUpdatedOnProperty, System.DateTime.UtcNow);
                    updt.Set(MEProgram.UpdatedByProperty, ObjectId.Parse(this.UserId));
                    updt.Set(MEProgram.VersionProperty, request.Version);

                    var pt = ctx.Programs.Collection.FindAndModify(pUQuery, SortBy.Null, updt, true);

                    AuditHelper.LogDataAudit(this.UserId,
                                            MongoCollectionName.Program.ToString(),
                                            request.Id.ToString(),
                                            Common.DataAuditType.Update,
                                            request.ContractNumber);

                    response.Id = request.Id;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
        
        public MEProgram FindByID(string entityID, bool temp)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }
        public string ContractNumber { get; set; }

        public IEnumerable<object> Find(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
