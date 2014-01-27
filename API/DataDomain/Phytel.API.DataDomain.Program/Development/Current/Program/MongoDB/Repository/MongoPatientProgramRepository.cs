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
using Phytel.API.Common;

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
                            AssignedBy = cp.AssignedBy,
                            AssignedOn = cp.AssignedOn,
                            State = cp.State,
                            StartDate = System.DateTime.UtcNow, // utc time
                            EndDate = null,
                            GraduatedFlag = false,
                            Population = null,
                            OptOut = null,
                            DidNotEnrollReason = null,
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
                            CompletedBy = cp.CompletedBy,
                            UpdatedBy = request.UserId,
                            SourceId = cp.Id.ToString(),
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

                        // update to new ids and their references
                        DTOUtils.RecurseAndReplaceIds(patientProgDoc.Modules);

                        ctx.PatientPrograms.Collection.Insert(patientProgDoc);

                        // update programid in modules
                        var q = MB.Query<MEPatientProgram>.EQ(b => b.Id, patientProgDoc.Id);
                        patientProgDoc.Modules.ForEach(s => s.ProgramId = patientProgDoc.Id);
                        ctx.PatientPrograms.Collection.Update(q, MB.Update.SetWrapped<List<MEModules>>(MEPatientProgram.ModulesProperty, patientProgDoc.Modules));

                        // hydrate response object
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
            catch(Exception ex)
            {
                throw new Exception("DataDomain:Insert():" + ex.Message, ex.InnerException);
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
                            SourceId = cp.SourceId,
                            AssignBy = cp.AssignedBy,
                            AssignDate = cp.AssignedOn,
                            ElementState = (int)cp.State,
                            CompletedBy = cp.CompletedBy,
                            DateCompleted = cp.DateCompleted,
                            ObjectivesInfo = DTOUtils.GetObjectives(cp.ObjectivesInfo),
                            SpawnElement = DTOUtils.GetSpawnElement(cp),
                            Modules = DTOUtils.GetModules(cp.Modules)
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
                throw new Exception("DataDomain:FindById():" + ex.Message, ex.InnerException);
            }
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

            using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
            {
                //var findcp = Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(request.PatientProgramId));
                //MEPatientProgram cp = ctx.PatientPrograms.Collection.Find(findcp).FirstOrDefault();
                List<MEPatientProgram> cps = ctx.PatientPrograms.Collection.Find(mQuery).ToList();

                if (cps != null)
                {
                    cps.ForEach(cp => patList.Add(new ProgramDetail
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
                    }));
                }
            }

            return new Tuple<string, IEnumerable<object>>(expression.ExpressionID, patList);
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            PutProgramActionProcessingRequest p = (PutProgramActionProcessingRequest)entity;

            try
            {
                ProgramDetail pg = p.Program;
                using (ProgramMongoContext ctx = new ProgramMongoContext(_dbName))
                {
                    var q = MB.Query<MEPatientProgram>.EQ(b => b.Id, ObjectId.Parse(p.ProgramId));
                    List<MEModules> mods = DTOUtils.CloneAppDomainModules(pg.Modules);

                    var uv = new List<MB.UpdateBuilder>();
                    uv.Add(MB.Update.Set(MEPatientProgram.CompletedProperty, pg.Completed));
                    uv.Add(MB.Update.Set(MEPatientProgram.StateProperty, (ElementState)pg.ElementState));
                    uv.Add(MB.Update.Set(MEPatientProgram.EnabledProperty, pg.Enabled));
                    uv.Add(MB.Update.Set(MEPatientProgram.OrderProperty, pg.Order));
                    uv.Add(MB.Update.Set(MEPatientProgram.ProgramStateProperty, (ProgramState)pg.ProgramState));
                    uv.Add(MB.Update.Set(MEPatientProgram.StatusProperty, (Status)pg.Status));
                    uv.Add(MB.Update.Set(MEPatientProgram.LastUpdatedOnProperty, System.DateTime.UtcNow));
                    uv.Add(MB.Update.Set(MEPatientProgram.UpdatedByProperty, p.UserId));
                    if (pg.AssignBy != null) { uv.Add(MB.Update.Set(MEPatientProgram.AssignByProperty, pg.AssignBy)); }
                    if (pg.AssignDate != null) { uv.Add(MB.Update.Set(MEPatientProgram.AssignDateProperty, pg.AssignDate)); }
                    if (pg.Client != null) { uv.Add(MB.Update.Set(MEPatientProgram.ClientProperty, pg.Client)); }
                    if (pg.CompletedBy != null) { uv.Add(MB.Update.Set(MEPatientProgram.CompletedByProperty, pg.CompletedBy)); }
                    if (pg.ContractProgramId != null) { uv.Add(MB.Update.Set(MEPatientProgram.ContractProgramIdProperty, ObjectId.Parse(pg.ContractProgramId))); }
                    if (pg.DateCompleted != null) { uv.Add(MB.Update.Set(MEPatientProgram.CompletedOnProperty, pg.DateCompleted)); }
                    if (pg.Description != null) { uv.Add(MB.Update.Set(MEPatientProgram.DescriptionProperty, pg.Description)); }
                    if (pg.EligibilityEndDate != null) { uv.Add(MB.Update.Set(MEPatientProgram.EligibilityEndDateProperty, pg.EligibilityEndDate)); }
                    if (pg.EligibilityRequirements != null) { uv.Add(MB.Update.Set(MEPatientProgram.EligibilityRequirementsProperty, pg.EligibilityRequirements)); }
                    if (pg.EligibilityStartDate != null) { uv.Add(MB.Update.Set(MEPatientProgram.EligibilityStartDateProperty, pg.EligibilityStartDate)); }
                    if (pg.EndDate != null) { uv.Add(MB.Update.Set(MEPatientProgram.EndDateProperty, pg.EndDate)); }
                    if (pg.Name != null) { uv.Add(MB.Update.Set(MEPatientProgram.NameProperty, pg.Name)); }
                    if (pg.Next != null) { uv.Add(MB.Update.Set(MEPatientProgram.NextProperty, pg.Next)); }
                    if (pg.Previous != null) { uv.Add(MB.Update.Set(MEPatientProgram.PreviousProperty, pg.Previous)); }
                    if (pg.ShortName != null) { uv.Add(MB.Update.Set(MEPatientProgram.ShortNameProperty, pg.ShortName)); }
                    if (pg.SourceId != null) { uv.Add(MB.Update.Set(MEPatientProgram.SourceIdProperty, pg.SourceId)); }
                    if (pg.StartDate != null) { uv.Add(MB.Update.Set(MEPatientProgram.StartDateProperty, pg.StartDate)); }
                    if (pg.Version != null) { uv.Add(MB.Update.Set(MEPatientProgram.VersionProperty, pg.Version)); }
                    if (mods != null) { uv.Add(MB.Update.SetWrapped<List<MEModules>>(MEPatientProgram.ModulesProperty, mods)); }
                    if (pg.SpawnElement != null) { uv.Add(MB.Update.SetWrapped<List<MESpawnElement>>(MEPatientProgram.SpawnProperty, DTOUtils.GetSpawnElements(pg.SpawnElement))); }
                    if (pg.ObjectivesInfo != null) { uv.Add(MB.Update.SetWrapped<List<MEObjective>>(MEPatientProgram.ObjectivesInfoProperty, DTOUtils.GetObjectives(pg.ObjectivesInfo))); }

                    IMongoUpdate update = MB.Update.Combine(uv);
                    ctx.PatientPrograms.Collection.Update(q, update);
                }
                return pg;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:Update():" + ex.Message, ex.InnerException);
            }
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
