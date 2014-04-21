using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Phytel.API.DataDomain.ProgramDesign;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class MongoProgramRepository<T> : IProgramDesignRepository<T>
    {
        private string _dbName = string.Empty;

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
            MEProgram program = new MEProgram(this.UserId)
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

            //List<MEModule> modules = new List<MEModule>();
            //foreach(Module m in request.Modules)
            //{
            //    MEModule mod = new MEModule()
            //    {
            //        Name = m.Name,
            //        Description = m.Description,
            //        Id = ObjectId.Parse(m.Id),
            //        ProgramId = program.Id,
            //        Status = m.Status,
            //        Version = m.Version
            //    };

            //    List<Objective> objectives = new List<Objective>();
            //    foreach(string o in m.Objectives)
            //    {
            //        Objective obj = new Objective()
            //        {
            //            Id = ObjectId.GenerateNewId(),
            //            Value = o
            //        };
            //        objectives.Add(obj);
            //    }

            //    List<MEAction> actions = new List<MEAction>();
            //    foreach(ActionData a in m.Actions)
            //    {
            //        MEAction action = new MEAction(this.UserId)
            //        {
            //            Id = ObjectId.Parse(a.ID),
            //            Name = a.Name,
            //            Description = a.Description,
            //            CompletedBy = ObjectId.Parse(a.CompletedBy),
            //            Status = Common. a.Status
            //        };
            //    }
            //    modules.Add(mod);
            //}
            //program.Modules = modules;

            using(ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                ctx.Programs.Collection.Insert(program);
            }

            return new PutProgramDataResponse
            {
                Result = true
            };
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

        public Phytel.API.DataDomain.ProgramDesign.DTO.Program FindByName(string entityName)
        {
            try
            {
                Phytel.API.DataDomain.ProgramDesign.DTO.Program result = null;

                using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEProgram>.EQ(b => b.Name, entityName);
                    MEProgram cp = ctx.Programs.Collection.Find(findcp).FirstOrDefault();

                    if (cp != null)
                    {
                        result = new Phytel.API.DataDomain.ProgramDesign.DTO.Program
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

        public object Update(object entity)
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

        public string UserId { get; set; }
        public string ContractNumber { get; set; }

        public IEnumerable<object> Find(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
