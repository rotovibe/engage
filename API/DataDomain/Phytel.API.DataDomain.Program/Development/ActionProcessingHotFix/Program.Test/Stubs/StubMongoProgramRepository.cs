using MongoDB.Bson;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.Test.Stubs
{
    public class StubMongoProgramRepository : IProgramRepository
    {
        string userId = "testuser";
        public StubMongoProgramRepository(string contract)
        {
        }

        public List<DTO.ProgramInfo> GetActiveProgramsInfoList(DTO.GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> Find(string Id)
        {
            throw new NotImplementedException();
        }

        public DTO.Program FindByName(string entityName)
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

        public object GetLimitedProgramFields(string objectId)
        {
            MEProgram mep = new MEProgram(this.userId)
            {
                AuthoredBy = "123456789012345678901234",
                TemplateName = "template stub name",
                TemplateVersion = "1.0",
                ProgramVersion = "1.0",
                ProgramVersionUpdatedOn = System.DateTime.UtcNow,
                Objectives = new List<Objective> { new Objective { Id = ObjectId.GenerateNewId(), Status = Status.Active, Units = "lbs", Value = "134" },
                new Objective { Id = ObjectId.GenerateNewId(), Status = Status.Inactive, Units = "oz", Value = "55" }}
            };
            return mep;
        }

        public object InsertAsBatch(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object FindByEntityExistsID(string patientID, string progId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> Find(List<global::MongoDB.Bson.ObjectId> Ids)
        {
            throw new NotImplementedException();
        }

        public bool Save(object entity)
        {
            throw new NotImplementedException();
        }

        public string UserId
        {
            get
            {
                return this.userId;
            }
            set
            {
                this.userId = value;
            }
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

        public List<Module> GetProgramModules(ObjectId progId)
        {
            List<Module> mods = new List<Module> {
            new Module{
             Id = ObjectId.GenerateNewId(),
             SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
             Name = "testmodule",
             Objectives = new List<Objective>{ new Objective{ Id=ObjectId.GenerateNewId(), Status = Status.Active, Value = "90", Units="lbs"},
             new Objective{ Id=ObjectId.GenerateNewId(), Status = Status.Inactive, Value = "99", Units="hdl"}}
            }
            };

            return mods;
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
