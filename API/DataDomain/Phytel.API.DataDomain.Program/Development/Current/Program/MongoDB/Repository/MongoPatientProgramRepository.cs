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
using Phytel.API.AppDomain.Program;
using Phytel.API.DataDomain.Program.MongoDB.DTO;

namespace Phytel.API.DataDomain.Program
{
    public class MongoPatientProgramRepository<T> : IProgramRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoPatientProgramRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(T newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            throw new NotImplementedException();
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public GetProgramDetailsSummaryResponse GetPatientProgramDocumentDetailsById(GetProgramDetailsSummaryRequest request)
        {
            try
            {
                GetProgramDetailsSummaryResponse result = new GetProgramDetailsSummaryResponse();

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findcp = Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(request.ProgramId));
                    MEPatientProgram cp = ctx.PatientPrograms.Collection.Find(findcp).FirstOrDefault();

                    if (cp != null)
                    {
                        result.Program = new ProgramDetail
                        {
                            Id = cp.Id.ToString(),
                            Client = cp.Client,
                            ContractProgramId = cp.ContractProgramId.ToString(),
                            Description = cp.Description,
                            EligibilityEndDate = cp.EligibilityEndDate,
                            EligibilityRequirements = cp.EligibilityRequirements,
                            EligibilityStartDate = cp.EligibilityStartDate,
                            EndDate = cp.EndDate,
                            Modules = cp.Modules.Where(h => h.Status == Common.Status.Active).Select(r => new ModuleDetail
                            {
                                Id = r.Id.ToString(),
                                Description = r.Description,
                                Name = r.Name,
                                Status = (int)r.Status,
                                Objectives = r.Objectives
                                .Select(o => new ObjectivesDetail
                                {
                                    Id = o.Id.ToString(),
                                    Value = o.Value,
                                    Status = (int)o.Status,
                                    Unit = o.Unit
                                }).ToList(),
                                Actions = r.Actions.Where(i => i.Status == Common.Status.Active).Select(a => new ActionsDetail
                                {
                                    CompletedBy = a.CompletedBy,
                                    Description = a.Description,
                                    Id = a.Id.ToString(),
                                    Name = a.Name,
                                    Status = (int)a.Status,
                                    Objectives = a.Objectives
                                    .Select(x => new ObjectivesDetail
                                    {
                                        Id = x.Id.ToString(),
                                        Unit = x.Unit,
                                        Status = (int)x.Status,
                                        Value = x.Value
                                    }).ToList(),
                                    Steps = a.Steps.Where(j => j.Status == Common.Status.Active).Select(s => new StepsDetail
                                    {
                                        Description = s.Description,
                                        Ex = s.Ex,
                                        Id = s.Id.ToString(),
                                        Notes = s.Notes,
                                        Question = s.Question,
                                        Status = (int)s.Status,
                                        T = s.T,
                                        Text = s.Text,
                                        Type = s.Type
                                    }).ToList()
                                }).ToList()
                            }).ToList(),
                            Name = cp.Name,
                            ObjectivesInfo = cp.ObjectivesInfo
                            .Select(r => new ObjectivesDetail
                            {
                                Id = r.Id.ToString(),
                                Unit = r.Unit,
                                Status = (int)r.Status,
                                Value = r.Value
                            }).ToList(),
                            PatientId = cp.PatientId.ToString(),
                            ProgramState = (int)cp.ProgramState,
                            ShortName = cp.ShortName,
                            StartDate = cp.StartDate,
                            Status = (int)cp.Status,
                            Version = cp.Version
                        };
                    }
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public PutProgramToPatientResponse InsertPatientToProgramAssignment(PutProgramToPatientRequest request)
        {
            try
            {
                PutProgramToPatientResponse result = new PutProgramToPatientResponse();
                result.Outcome = new Outcome();
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findQ = Query.And(
                        Query.In(MEPatientProgram.ProgramStateProperty, new List<BsonValue> { BsonValue.Create(0), BsonValue.Create(1) }),
                        Query<MEPatientProgram>.EQ(b => b.PatientId, ObjectId.Parse(request.PatientId)),
                        Query<MEPatientProgram>.EQ(b => b.ContractProgramId, ObjectId.Parse(request.ContractProgramId)));

                    List<MEPatientProgram> pp = ctx.PatientPrograms.Collection.Find(findQ).ToList();

                    if (pp.Count == 0)
                    {
                        var findcp = Query<MEContractProgram>.EQ(b => b.Id, ObjectId.Parse(request.ContractProgramId));
                        MEContractProgram cp = ctx.ContractPrograms.Collection.Find(findcp).FirstOrDefault();

                        MEPatientProgram patientProgDoc = new MEPatientProgram
                        {
                            PatientId = ObjectId.Parse(request.PatientId),
                            AuthoredBy = cp.AuthoredBy,
                            Client = cp.Client,
                            ProgramState = Common.ProgramState.NotStarted,
                            StartDate = System.DateTime.UtcNow, // utc time
                            EndDate = null,
                            GraduatedFlag = false,
                            OptOut = null,
                            Eligibility = Common.GenericStatus.Pending,
                            EligibilityStartDate = System.DateTime.UtcNow,
                            EligibilityEndDate = null,
                            EligibilityRequirements = cp.EligibilityRequirements,
                            Enrollment = Common.GenericStatus.Pending,
                            EligibilityOverride = Common.GenericSetting.No,
                            ContractProgramId = cp.Id,
                            DeleteFlag = cp.DeleteFlag,
                            Description = cp.Description,
                            LastUpdatedOn = System.DateTime.UtcNow, // utc time
                            Locked = cp.Locked,
                            Modules = SetValidModules(cp.Modules),
                            Name = cp.Name,
                            ObjectivesInfo = cp.ObjectivesInfo,
                            //ObjectivesInfo = cp.ObjectivesInfo.Where(e => e.Status == Common.Status.Active).Select(f => new ObjectivesInfo()
                            //{
                            //    Id = f.Id,
                            //    Status = f.Status,
                            //    Value = f.Value,
                            //    Unit = f.Unit
                            //}).ToList(),
                            ShortName = cp.ShortName,
                            Status = cp.Status
                        };

                        ctx.PatientPrograms.Collection.Insert(patientProgDoc);

                        result.program = new ProgramInfo
                        {
                            Id = patientProgDoc.Id.ToString(),
                            Name = patientProgDoc.Name,
                            ShortName = patientProgDoc.ShortName,
                            Status = (int)patientProgDoc.Status,
                             PatientId = patientProgDoc.PatientId.ToString(),
                            ProgramState = (int)patientProgDoc.ProgramState
                        };

                        result.Outcome.Result = 1;
                        result.Outcome.Reason = "Successfully assigned this program for the patient";
                    }
                    else
                    {
                        result.Outcome.Result = 0;
                        result.Outcome.Reason = pp[0].Name + " is already assigned";
                    }

                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        private List<Modules> SetValidModules(List<Modules> list)
        {
            List<StepsInfo> steps = new List<StepsInfo>();
            List<ActionsInfo> acts = new List<ActionsInfo>();
            List<Modules> mods = new List<Modules>();

            foreach (Modules m in list)
            {
                if (m.Status == Common.Status.Active)
                {
                    Modules mod = new Modules()
                    {
                        Id = m.Id,
                        Description = m.Description,
                        Name = m.Name,
                        Status = m.Status,
                        Objectives = m.Objectives,
                        //Objectives = m.Objectives.Where(a => a.Status == Common.Status.Active).Select(z => new ObjectivesInfo()
                        //{
                        //    Id = z.Id,
                        //    Status = z.Status,
                        //    Unit = z.Unit,
                        //    Value = z.Value
                        //}).ToList(),
                        Actions = new List<ActionsInfo>()
                    };

                    foreach (ActionsInfo ai in m.Actions)
                    {
                        if (ai.Status == Common.Status.Active)
                        {
                            ActionsInfo ac = new ActionsInfo()
                            {
                                CompletedBy = ai.CompletedBy,
                                Description = ai.Description,
                                Id = ai.Id,
                                Name = ai.Name,
                                Status = ai.Status,
                                Objectives = ai.Objectives,
                                //Objectives = ai.Objectives.Where(r => r.Status == Common.Status.Active).Select(x => new ObjectivesInfo()
                                //{
                                //    Id = x.Id,
                                //    Status = x.Status,
                                //    Unit = x.Unit,
                                //    Value = x.Value
                                //}).ToList(),
                                Steps = ai.Steps.Where(s => s.Status == Common.Status.Active).Select(b => new StepsInfo()
                                {
                                    Status = b.Status,
                                    Description = b.Description,
                                    Ex = b.Ex,
                                    Id = b.Id,
                                    Notes = b.Notes,
                                    Question = b.Question,
                                    T = b.T,
                                    Text = b.Text,
                                    Type = b.Type
                                }).ToList()
                            };
                            mod.Actions.Add(ac);
                        }
                    }
                    mods.Add(mod);
                }
            }

            return mods;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
