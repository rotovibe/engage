using MongoDB.Bson;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.API.DataDomain.Program.DTO;

namespace Phytel.API.DataDomain.Program.Test.Stubs
{
    public class StubMongoPatientProgramRepository : IProgramRepository
    {
        string userId = "000000000000000000000000";
        public StubMongoPatientProgramRepository(string contract)
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
            MEPatientProgram prog = new MEPatientProgram(this.userId)
                {
                    Name = "test patient program",
                    Description = "test program description",
                    Id = ObjectId.Parse("000000000000000000000000"),
                    EligibilityRequirements = "Test eligibility requirements",
                    EligibilityStartDate = System.DateTime.UtcNow,
                    EligibilityEndDate = System.DateTime.UtcNow,
                    Objectives = new List<Objective> { new Objective{ Id = ObjectId.Parse("123456789012345678901234"),
                                         Value = "testing",
                                         Units = "lbs",
                                         Status = Status.Active
                    }},
                    Modules = new List<Module>() { 
                                    new Module { 
                                        Id = ObjectId.Parse("000000000000000000000000"), 
                                        Name = "Test stub module 1", 
                                        Description = "BSHSI - Outreach & Enrollment", 
                                        SourceId = ObjectId.Parse("532b5585a381168abe00042c"),
                                        Actions = new List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action>(){ 
                                            new Phytel.API.DataDomain.Program.MongoDB.DTO.Action{ 
                                                Id = ObjectId.Parse("000000000000000000000000"), 
                                                SourceId = ObjectId.Parse("123456789012345678901234"),
                                                State = ElementState.InProgress, 
                                                Name ="test action from stub", 
                                                Description = "BSHSI - Outreach & Enrollment action description",
                                                AttributeStartDate = Convert.ToDateTime("1/1/1800"),
                                                AttributeEndDate = Convert.ToDateTime("1/1/1801"),
                                                AssignedOn = Convert.ToDateTime("1/1/1899"),
                                                AssignedTo = ObjectId.Parse("123456789011111111112232"),
                                                AssignedBy = ObjectId.Parse("123456789011111111112233"),
                                                Objectives = new List<Objective> { 
                                                    new Objective{ 
                                                        Id = ObjectId.Parse("123456789012345678905678"), 
                                                        Value = "testing",
                                                        Units = "lbs",
                                                        Status =  Status.Active
                                                    },
                                                    new Objective{ 
                                                        Id = ObjectId.Parse("567856789012345678905678"), 
                                                        Value = "testing action 2",
                                                        Units = "hdl",
                                                        Status =  Status.Inactive
                                                    }
                                                }
                                            } },
                                        AttributeStartDate = Convert.ToDateTime("1/1/1900"),
                                        AttributeEndDate = Convert.ToDateTime("1/1/1901"),
                                        AssignedOn = Convert.ToDateTime("1/1/1999"),
                                        AssignedTo = ObjectId.Parse("123456789011111111112222"),
                                        AssignedBy = ObjectId.Parse("123456789011111111112223"),
                                        Objectives = new List<Objective> { 
                                            new Objective{ 
                                                Id = ObjectId.Parse("123456789012345678901234"), 
                                                Value = "testing", 
                                                Units = "lbs", 
                                                Status =  Status.Active
                                            }
                                        }
                                    }
                    }
                };
            return prog;
        }

        public Tuple<string, IEnumerable<object>> Select(APIExpression expression)
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
