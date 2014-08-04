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
using MongoDB.Bson.Serialization;

namespace Phytel.API.DataDomain.Program
{
    public class MongoProgramRepository : IProgramRepository
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
            try
            {
                MEProgram cp = null;
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
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

        public object GetLimitedProgramFields(string objectId)
        {
            try
            {
                MEProgram cp = null;
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                     var query = MB.Query.And(
                        MB.Query<MEProgram>.EQ(b => b.Id, ObjectId.Parse(objectId)),
                        MB.Query<MEProgram>.EQ(b => b.DeleteFlag, false)
                    );
                    cp = ctx.Programs.Collection.Find(query).SetFields(
                        MEProgram.AuthoredByProperty,
                        MEProgram.TemplateNameProperty,
                        MEProgram.TemplateVersionProperty,
                        MEProgram.ProgramVersionProperty,
                        MEProgram.ProgramVersionUpdatedOnProperty,
                        MEProgram.ObjectivesInfoProperty
                    ).FirstOrDefault();
                }
                return cp;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:Program:GetLimitedProgramFields()::" + ex.Message, ex.InnerException);
            }
        }

        public DTO.Program FindByName(string entityName)
        {
            try
            {
                DTO.Program result = null;

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEProgram>.EQ(b => b.Name, entityName);
                    MEProgram cp = ctx.Programs.Collection.Find(findcp).FirstOrDefault();

                    if (cp != null)
                    {
                        result = new DTO.Program
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


        public object FindByPlanElementID(string entityID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> FindByStepId(string entityID)
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
            try
            {
                List<Module> mods = null;

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEProgram>.EQ(b => b.Id, progId);
                    MEProgram cp = ctx.Programs.Collection.Find(findcp).FirstOrDefault();

                    if (cp != null)
                    {
                        mods = cp.Modules;
                    }
                    else
                    {
                        throw new ArgumentException("ProgramId is not valid or is missing from the records.");
                    }
                }
                return mods;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PatientProgramRepository:GetProgramModules()::" + ex.Message, ex.InnerException);
            }
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
