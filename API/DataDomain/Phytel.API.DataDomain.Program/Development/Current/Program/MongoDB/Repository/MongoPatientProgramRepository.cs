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

        public object Insert(object newEntity)
        {
            try
            {
                PutProgramToPatientRequest request = (PutProgramToPatientRequest)newEntity;
                PutProgramToPatientResponse result = new PutProgramToPatientResponse();
                result.Outcome = new Outcome();
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findQ = MB.Query.And(
                        MB.Query.In(MEPatientProgram.ProgramStateProperty, new List<BsonValue> { BsonValue.Create(0), BsonValue.Create(1) }),
                        MB.Query<MEPatientProgram>.EQ(b => b.PatientId, ObjectId.Parse(request.PatientId)),
                        MB.Query<MEPatientProgram>.EQ(b => b.ContractProgramId, ObjectId.Parse(request.ContractProgramId)));

                    List<MEPatientProgram> pp = ctx.PatientPrograms.Collection.Find(findQ).ToList();

                    if (pp.Count == 0)
                    {
                        var findcp = MB.Query<MEContractProgram>.EQ(b => b.Id, ObjectId.Parse(request.ContractProgramId));
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
                            Population = null,
                            OptOut = null,
                            NotEnrollReason = null,
                            DisEnrollReason = null,
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
                            Modules = DTOUtils.SetValidModules(cp.Modules),
                            Name = cp.Name,
                            ObjectivesInfo = cp.ObjectivesInfo,
                            UpdatedBy = request.UserId,
                            //ObjectivesInfo = cp.ObjectivesInfo.Where(e => e.Status == Common.Status.Active).Select(f => new ObjectivesInfo()
                            //{
                            //    Id = f.Id,
                            //    Status = f.Status,
                            //    Value = f.Value,
                            //    Unit = f.Unit
                            //}).ToList(),
                            ShortName = cp.ShortName,
                            Status = cp.Status,
                            Version = cp.Version,
                            Spawn = cp.Spawn,
                            Completed = cp.Completed,
                            Enabled = cp.Enabled,
                            ExtraElements = cp.ExtraElements,
                            Next = cp.Next,
                            Order = cp.Order,
                            Previous = cp.Previous
                        };

                        ctx.PatientPrograms.Collection.Insert(patientProgDoc);

                        // update programid in modules
                        var q = MB.Query<MEPatientProgram>.EQ(b => b.Id, patientProgDoc.Id);
                        patientProgDoc.Modules.ForEach(s => UpdateProgramIdInModules(s, patientProgDoc.Id));
                        ctx.PatientPrograms.Collection.Update(q, MB.Update.SetWrapped<List<Modules>>("modules", patientProgDoc.Modules));

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

        private void UpdateProgramIdInModules(Modules s, ObjectId objectId)
        {
            s.ProgramId = objectId;
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
            try
            {
                GetProgramDetailsSummaryResponse result = new GetProgramDetailsSummaryResponse();

                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var findcp = MB.Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(entityID));
                    MEPatientProgram cp = ctx.PatientPrograms.Collection.Find(findcp).FirstOrDefault();

                    if (cp != null)
                    {
                        result.Program = new ProgramDetail
                        {
                            Id = cp.Id.ToString(),
                            Client = cp.Client,
                            ContractProgramId = cp.ContractProgramId.ToString(),
                            Description = cp.Description,
                            Name = cp.Name,
                            PatientId = cp.PatientId.ToString(),
                            ProgramState = (int)cp.ProgramState,
                            ShortName = cp.ShortName,
                            StartDate = cp.StartDate,
                            Status = (int)cp.Status,
                            Version = cp.Version,
                            EligibilityEndDate = cp.EligibilityEndDate,
                            EligibilityRequirements = cp.EligibilityRequirements,
                            EligibilityStartDate = cp.EligibilityStartDate,
                            EndDate = cp.EndDate,
                            Completed = cp.Completed,
                            Enabled = cp.Enabled,
                            Next = cp.Next,
                            Order = cp.Order,
                            Previous = cp.Previous,
                            ObjectivesInfo = cp.ObjectivesInfo
                            .Select(r => new ObjectivesDetail
                            {
                                Id = r.Id.ToString(),
                                Unit = r.Unit,
                                Status = (int)r.Status,
                                Value = r.Value
                            }).ToList(),
                            SpawnElement = GetSpawnElement(cp),
                            Modules = cp.Modules.Where(h => h.Status == Common.Status.Active).Select(r => new ModuleDetail
                            {
                                Id = r.Id.ToString(),
                                ProgramId = r.ProgramId.ToString(),
                                Description = r.Description,
                                Name = r.Name,
                                Status = (int)r.Status,
                                Completed = r.Completed,
                                Enabled = r.Enabled,
                                Next = r.Next,
                                Previous = r.Previous,
                                Order = r.Order,
                                SpawnElement = GetSpawnElement(r),
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
                                    ModuleId = a.ModuleId.ToString(),
                                    Name = a.Name,
                                    Status = (int)a.Status,
                                    Completed = a.Completed,
                                    Enabled = a.Enabled,
                                    Next = a.Next,
                                    Previous = a.Previous,
                                    Order = a.Order,
                                    SpawnElement = GetSpawnElement(a),
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
                                        ActionId = s.ActionId.ToString(),
                                        Notes = s.Notes,
                                        Question = s.Question,
                                        Status = (int)s.Status,
                                        Title = s.Title,
                                        Text = s.Text,
                                        StepTypeId = s.StepTypeId,
                                        Completed = s.Completed,
                                        Enabled = s.Enabled,
                                        Next = s.Next,
                                        Previous = s.Previous,
                                        Order = s.Order,
                                        ControlType = s.ControlType,
                                        Header = s.Header,
                                        SelectedResponseId = s.SelectedResponseId,
                                        Responses = GetResponses(s),
                                        SpawnElement = GetSpawnElement(s)
                                    }).ToList()
                                }).ToList()
                            }).ToList()
                        };
                    }
                    else
                    {
                        throw new ArgumentException("ProgramID is not valid or is missing from the records.");
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private static SpawnElementDetail GetSpawnElement(MEPlanElement a)
        {
            SpawnElementDetail spawnDetail = null;

            if (a.Spawn != null)
            {
                spawnDetail = new SpawnElementDetail { ElementId = a.Spawn.SpawnId.ToString(), ElementType = a.Spawn.Type };
            }

            return spawnDetail;
        }

        private List<ResponseDetail> GetResponses(StepsInfo step)
        {
            List<ResponseDetail> resp = null;
            if (step.Responses != null)
            {
                resp = step.Responses.Select(x => new ResponseDetail
                                           {
                                               Id = x.Id.ToString(),
                                               NextStepId = x.NextStepId.ToString(),
                                               Nominal = x.Nominal,
                                               Order = x.Order,
                                               Required = x.Required,
                                               StepID = x.StepId.ToString(),
                                               Text = x.Text,
                                               Value = x.Value
                                           }).ToList<ResponseDetail>();
            }
            return resp;
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            IMongoQuery mQuery = null;
            List<object> patList = new List<object>();

            List<SelectExpression> selectExpressions = expression.Expressions.ToList();
            selectExpressions.Where(s => s.GroupID == 1).OrderBy(o => o.ExpressionOrder).ToList();

            SelectExpressionGroupType groupType = SelectExpressionGroupType.AND;

            if (selectExpressions.Count > 0)
            {
                IList<IMongoQuery> queries = new List<IMongoQuery>();
                for (int i = 0; i < selectExpressions.Count; i++)
                {
                    groupType = selectExpressions[0].NextExpressionType;

                    IMongoQuery query = SelectExpressionHelper.ApplyQueryOperators(selectExpressions[i].Type, selectExpressions[i].FieldName, selectExpressions[i].Value);
                    if (query != null)
                    {
                        queries.Add(query);
                    }
                }

                mQuery = SelectExpressionHelper.BuildQuery(groupType, queries);
            }

            GetProgramDetailsSummaryResponse result = new GetProgramDetailsSummaryResponse();

            using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            {
                //var findcp = Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(request.PatientProgramId));
                //MEPatientProgram cp = ctx.PatientPrograms.Collection.Find(findcp).FirstOrDefault();
                MEPatientProgram cp = ctx.PatientPrograms.Collection.Find(mQuery).FirstOrDefault();

                if (cp != null)
                {
                    ProgramDetail patProgram = new ProgramDetail
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
                                    Title = s.Title,
                                    Text = s.Text,
                                    StepTypeId = s.StepTypeId
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
                    patList.Add(patProgram);
                }
            }

            return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, patList);
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
