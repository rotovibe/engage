using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.MongoDB.DTO;

namespace Phytel.Data.ETL.BulkCopy
{
    public class ReadPlanElementsSeries : ReadSeries
    {
        protected readonly Dictionary<string, int> stepIdList;
        protected readonly Dictionary<string, int> userIdList;
        protected readonly Dictionary<string, int> patientIdList;
        protected readonly Dictionary<string, int> observationIdList;
        public string Contract { get; set; }

        public ReadPlanElementsSeries(string contract)
        {
            Contract = contract;
            stepIdList = Utils.GetStepIdList(contract);
            userIdList = Utils.GetUserIdList(contract);
            patientIdList = Utils.GetPatientIdList(contract);
            observationIdList = Utils.GetObservationIdsList(contract);
        }

        public IEnumerable<EStepResponse> ReadEStepResponseSeries(List<MEPatientProgramResponse> list)
        {
            try
            {
                var enumerable = new ConcurrentBag<EStepResponse>();

                Parallel.ForEach(list, r =>
                {
                    var stepIds = stepIdList;
                    var userIds = userIdList;

                    enumerable.Add(new EStepResponse
                    {
                        MongoId = r.Id.ToString(),
                        MongoStepSourceId = r.StepSourceId.ToString(),
                        MongoActionId = r.ActionId.ToString(),
                        ActionId = 0, //get action id, // int
                        Delete = r.DeleteFlag.ToString(),
                        LastUpdatedOn = r.LastUpdatedOn,
                        MongoNextStepId = r.NextStepId == null ? string.Empty : r.NextStepId.ToString(),
                        NextStepId = GetId(stepIds, r.NextStepId.ToString()), // int
                        Nominal = r.Nominal.ToString(),
                        Order = r.Order.ToString(),
                        Text = r.Text ?? string.Empty,
                        Value = r.Value ?? string.Empty,
                        Required = r.Required.ToString(),
                        Selected = r.Selected.ToString(),
                        MongoRecordCreatedBy = r.RecordCreatedBy.ToString(),
                        RecordCreatedBy = GetId(userIds, r.RecordCreatedBy.ToString()), // int
                        RecordCreatedOn = r.RecordCreatedOn,
                        MongoStepId = r.StepId == null ? string.Empty : r.StepId.ToString(),
                        StepId = GetId(stepIds, r.StepId.ToString()), // int 
                        StepSourceId = 000000000000000000000000, //int
                        MongoUpdatedBy = r.UpdatedBy == null ? string.Empty : r.UpdatedBy.ToString(),
                        UpdatedBy = GetId(userIds, r.UpdatedBy.ToString()), // int
                        TTLDate = r.TTLDate ?? null,
                        Version = Convert.ToInt32(r.Version) // int
                    });
                });

                return enumerable;
            }
            catch (Exception ex)
            {
                throw new Exception("ReadEStepResponseSeries()" + ex.Message + ex.StackTrace);
            }
        }

        public IEnumerable<EProgram> ReadEProgramSeries(List<MEPatientProgram> programs)
        {
            try
            {
                var enumerable = new ConcurrentBag<EProgram>();
                Parallel.ForEach(programs, r =>
                {
                    var stepIds = stepIdList;
                    var userIds = userIdList;
                    var patientIds = patientIdList;

                    enumerable.Add(new EProgram
                    {
                        AssignedBy = r.AssignedBy == null ? 0 : GetId(userIds, r.AssignedBy.ToString()),
                        AssignedToId = r.AssignedTo == null ? 0 : GetId(userIds, r.AssignedTo.ToString()),
                        CompletedBy = r.CompletedBy == null ? 0 : GetId(userIds, r.CompletedBy.ToString()),
                        UpdatedBy = r.UpdatedBy == null ? 0 : GetId(userIds, r.UpdatedBy.ToString()),
                        PatientId = r.PatientId == null ? 0 : GetId(patientIds, r.PatientId.ToString()),
                        RecordCreatedBy = r.RecordCreatedBy == null ? 0 : GetId(userIds, r.RecordCreatedBy.ToString()),
                        MongoAssignedBy = r.AssignedBy == null ? string.Empty : r.AssignedBy.ToString(),
                        AssignedOn = r.AssignedOn ?? null,
                        MongoAssignedToId = r.AssignedTo == null ? string.Empty : r.AssignedTo.ToString(),
                        AttributeEndDate = r.AttributeEndDate ?? null,
                        AttributeStartDate = r.AttributeStartDate ?? null,
                        Completed = r.Completed.ToString(),
                        MongoCompletedBy = r.CompletedBy == null ? string.Empty : r.CompletedBy.ToString(),
                        ContractProgramId = r.ContractProgramId.ToString(),
                        DateCompleted = r.DateCompleted ?? null,
                        Delete = r.DeleteFlag.ToString(),
                        Description = r.Description,
                        EligibilityEndDate = r.EligibilityEndDate ?? null,
                        EligibilityReason = "",
                        EligibilityRequirements = r.EligibilityRequirements,
                        EligibilityStartDate = r.EligibilityStartDate ?? null,
                        Eligible = "",
                        Enabled = r.Enabled.ToString(),
                        EndDate = r.EndDate ?? null,
                        LastUpdatedOn = r.LastUpdatedOn ?? null,
                        MongoId = r.Id.ToString(),
                        MongoPatientId = r.PatientId == null ? string.Empty : r.PatientId.ToString(),
                        MongoRecordCreatedBy = r.RecordCreatedBy == null ? string.Empty : r.RecordCreatedBy.ToString(),
                        MongoUpdatedBy = r.UpdatedBy == null ? string.Empty : r.UpdatedBy.ToString(),
                        Name = r.Name,
                        Order = r.Order.ToString(),
                        RecordCreatedOn = GetNullableDate(r.RecordCreatedOn),
                        ShortName = r.ShortName,
                        SourceId = r.SourceId.ToString(),
                        StartDate = r.StartDate ?? null,
                        State = r.State.ToString(),
                        StateUpdatedOn = r.StateUpdatedOn ?? null,
                        Status = r.Status.ToString(),
                        TTLDate = r.TTLDate ?? null,
                        Version = r.Version.ToString()
                    });
                });

                return enumerable;
            }
            catch (Exception ex)
            {
                throw new Exception("ReadEProgramSeries()" + ex.Message + ex.StackTrace);
            }
        }

        public IEnumerable<EModule> ReadEModuleSeries(List<Module> modules, List<MEPatientProgram> programs)
        {
            try
            {
                var enumerable = new ConcurrentBag<EModule>();
                var programids = Utils.GetProgramIdList(Contract);

                Parallel.ForEach(modules, r =>
                {
                    var userIds = userIdList;
                    var patientIds = patientIdList;
                    MEPatientProgram pp;

                    try
                    {
                        pp = programs.Find(p => p.Id == r.ProgramId);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            "Error in trying to find the program id in the programs list. ProgramId : " + r.ProgramId +
                            ", " + ex.Message + ex.StackTrace);
                    }

                    enumerable.Add(new EModule
                    {
                        MongoNext = r.Next == null ? string.Empty : r.Next.ToString(),
                        MongoPrevious = r.Previous == null ? string.Empty : r.Previous.ToString(),
                        MongoProgramId = r.ProgramId == null ? string.Empty : r.ProgramId.ToString(),
                        PatientProgramId = programids[r.ProgramId.ToString()],
                        AssignedBy = r.AssignedBy == null ? 0 : GetId(userIds, r.AssignedBy.ToString()),
                        AssignedTo = r.AssignedTo == null ? 0 : GetId(userIds, r.AssignedTo.ToString()),
                        CompletedBy = r.CompletedBy == null ? 0 : GetId(userIds, r.CompletedBy.ToString()),
                        RecordCreatedBy = pp.RecordCreatedBy.ToString(),
                        MongoAssignedBy = r.AssignedBy == null ? string.Empty : r.AssignedBy.ToString(),
                        AssignedOn = r.AssignedOn ?? null,
                        MongoAssignedTo = r.AssignedTo == null ? string.Empty : r.AssignedTo.ToString(),
                        AttributeEndDate = r.AttributeEndDate ?? null,
                        AttributeStartDate = r.AttributeStartDate ?? null,
                        Completed = r.Completed.ToString(),
                        MongoCompletedBy = r.CompletedBy == null ? string.Empty : r.CompletedBy.ToString(),
                        DateCompleted = r.DateCompleted ?? null,
                        Delete = pp.DeleteFlag.ToString(),
                        Description = r.Description,
                        EligibilityEndDate = r.EligibilityEndDate ?? null,
                        EligibilityRequirements = r.EligibilityRequirements,
                        EligibilityStartDate = r.EligibilityStartDate ?? null,
                        Eligible = "",
                        Enabled = r.Enabled.ToString(),
                        LastupdatedOn = pp.LastUpdatedOn ?? null,
                        MongoId = r.Id.ToString(),
                        Name = r.Name,
                        Order = r.Order.ToString(),
                        RecordCreatedOn = GetNullableDate(pp.RecordCreatedOn),
                        SourceId = r.SourceId.ToString(),
                        State = r.State.ToString(),
                        StateUpdatedOn = r.StateUpdatedOn ?? null,
                        Status = r.Status.ToString(),
                        TTLDate = pp.TTLDate ?? null
                    });
                });

                return enumerable;
            }
            catch (Exception ex)
            {
                throw new Exception("ReadEModuleSeries()" + ex.Message + ex.StackTrace);
            }
        }

        public IEnumerable<EAction> ReadEActionSeries(List<Phytel.API.DataDomain.Program.MongoDB.DTO.Action> actions,
            List<Module> modules, List<MEPatientProgram> programs)
        {
            try
            {
                var enumerable = new ConcurrentBag<EAction>();
                var moduleIds = Utils.GetModuleIdList(Contract);
                var moduleSourceIds = Utils.GetModuleIdBySourceIdList(Contract);

                Parallel.ForEach(actions, r =>
                {
                    var userIds = userIdList;
                    var patientIds = patientIdList;

                    //var mod = modules.Find(m => m.Id == r.ModuleId) ?? modules.Find(m => m.SourceId == r.ModuleId);
                    //var pp = programs.Find(p => p.Id == mod.ProgramId);

                    //if (pp == null)
                    //    throw new ArgumentException("Could not find program element");

                    enumerable.Add(new EAction
                    {
                        ArchivedDate = r.ArchivedDate,
                        MongoModuleId = r.ModuleId.ToString(),
                        PatientProgramModuleId = GetModuleId(r.ModuleId, moduleIds, moduleSourceIds),
                        Archived = r.Archived == null ? null : r.Archived.ToString(),
                        MongoArchiveOriginId = r.ArchiveOriginId,
                        //LastUpdatedOn = pp.LastUpdatedOn ?? null,
                        //MongoRecordCreatedBy = pp.RecordCreatedBy == null ? string.Empty : pp.RecordCreatedBy.ToString(),
                        //RecordCreatedBy = GetId(userIds, pp.RecordCreatedBy.ToString()),
                        //MongoUpdatedBy = pp.UpdatedBy == null ? null : pp.UpdatedBy.ToString(),
                        //UpdatedBy = pp.UpdatedBy == null ? 0 : GetId(userIds, pp.UpdatedBy.ToString()),
                        //version = pp.Version == null ? null : pp.Version.ToString(),
                        //Delete = pp.DeleteFlag.ToString(),
                        //RecordCreatedOn = GetNullableDate(pp.RecordCreatedOn),
                        //TTLDate = pp.TTLDate ?? null,
                        //Delete = r.DeleteFlag.ToString(), //add later
                        Next = 0,
                        Previous = 0,
                        MongoNext = r.Next == null ? string.Empty : r.Next.ToString(),
                        MongoPrevious = r.Previous == null ? string.Empty : r.Previous.ToString(),
                        AssignedBy = r.AssignedBy == null ? 0 : GetId(userIds, r.AssignedBy.ToString()),
                        AssignedTo = r.AssignedTo == null ? 0 : GetId(userIds, r.AssignedTo.ToString()),
                        CompletedBy = r.CompletedBy == null ? 0 : GetId(userIds, r.CompletedBy.ToString()),
                        MongoAssignedBy = r.AssignedBy == null ? string.Empty : r.AssignedBy.ToString(),
                        AssignedOn = r.AssignedOn ?? null,
                        MongoAssignedTo = r.AssignedTo == null ? string.Empty : r.AssignedTo.ToString(),
                        AttributeEndDate = r.AttributeEndDate ?? null,
                        AttributeStartDate = r.AttributeStartDate ?? null,
                        Completed = r.Completed.ToString(),
                        MongoCompletedBy = r.CompletedBy == null ? string.Empty : r.CompletedBy.ToString(),
                        DateCompleted = r.DateCompleted ?? null,
                        Description = r.Description,
                        EligibilityEndDate = r.EligibilityEndDate ?? null,
                        EligibilityRequirements = r.EligibilityRequirements,
                        EligibilityStartDate = r.EligibilityStartDate ?? null,
                        Eligible = "",
                        Enabled = r.Enabled.ToString(),
                        MongoId = r.Id.ToString(),
                        Name = r.Name,
                        Order = r.Order.ToString(),
                        SourceId = r.SourceId.ToString(),
                        State = r.State.ToString(),
                        StateUpdatedOn = r.StateUpdatedOn ?? null,
                        Status = r.Status.ToString()
                    });
                });

                return enumerable;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("ReadEActionSeries()", ex);
            }
        }

        public IEnumerable<EStep> ReadEStepSeries(List<Phytel.API.DataDomain.Program.MongoDB.DTO.Step> steps,
            List<Module> modules, List<MEPatientProgram> programs)
        {
            try
            {
                var enumerable = new ConcurrentBag<EStep>();
                var actionIds = Utils.GetActionIdList(Contract);

                Parallel.ForEach(steps, r =>
                {
                    var userIds = userIdList;
                    var patientIds = patientIdList;

                    //var mod = modules.Find(m => m.Id == r.ModuleId) ?? modules.Find(m => m.SourceId == r.ModuleId);
                    //var pp = programs.Find(p => p.Id == mod.ProgramId);

                    //if (pp == null)
                    //    throw new ArgumentException("Could not find program element");

                    enumerable.Add(new EStep
                    {
                        //LastUpdatedOn = pp.LastUpdatedOn ?? null,
                        //MongoRecordCreatedBy = pp.RecordCreatedBy == null ? string.Empty : pp.RecordCreatedBy.ToString(),
                        //RecordCreatedBy = GetId(userIds, pp.RecordCreatedBy.ToString()),
                        //MongoUpdatedBy = pp.UpdatedBy == null ? null : pp.UpdatedBy.ToString(),
                        //UpdatedBy = pp.UpdatedBy == null ? 0 : GetId(userIds, pp.UpdatedBy.ToString()),
                        //Version = pp.Version == null ? null : pp.Version.ToString(),
                        //Delete = pp.DeleteFlag.ToString(),
                        //RecordCreatedOn = GetNullableDate(pp.RecordCreatedOn),
                        //TTLDate = pp.TTLDate ?? null,
                        ActionId = actionIds[r.ActionId.ToString()],
                        ControlType = r.ControlType.ToString(),
                        Header = r.Header,
                        IncludeTime = r.IncludeTime.ToString(),
                        MongoActionId = r.ActionId.ToString(),
                        Notes = r.Notes,
                        Question = r.Question,
                        SelectedResponseId = r.SelectedResponseId.ToString(),
                        SelectType = r.SelectType.ToString(),
                        StepTypeId = r.StepTypeId.ToString(),
                        Text = r.Text,
                        Title = r.Title,
                        Next = 0,
                        Previous = 0,
                        MongoNext = r.Next == null ? string.Empty : r.Next.ToString(),
                        MongoPrevious = r.Previous == null ? string.Empty : r.Previous.ToString(),
                        CompletedBy = r.CompletedBy == null ? 0 : GetId(userIds, r.CompletedBy.ToString()),
                        AttributeEndDate = r.AttributeEndDate ?? null,
                        AttributeStartDate = r.AttributeStartDate ?? null,
                        Completed = r.Completed.ToString(),
                        MongoCompletedBy = r.CompletedBy == null ? string.Empty : r.CompletedBy.ToString(),
                        DateCompleted = r.DateCompleted ?? null,
                        Description = r.Description,
                        EligibilityEndDate = r.EligibilityEndDate ?? null,
                        EligibilityRequirements = r.EligibilityRequirements,
                        EligibilityStartDate = r.EligibilityStartDate ?? null,
                        Eligible = "",
                        Enabled = r.Enabled.ToString(),
                        MongoId = r.Id.ToString(),
                        Order = r.Order.ToString(),
                        SourceId = r.SourceId.ToString(),
                        State = r.State.ToString(),
                        StateUpdatedOn = r.StateUpdatedOn ?? null,
                        Status = r.Status.ToString()
                    });
                });

                return enumerable;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("ReadStepSeries()" + ex.Message + ex.StackTrace, ex);
            }
        }

        public IEnumerable<EProgramAttribute> ReadEProgramAttributeSeries(ConcurrentBag<MEProgramAttribute> pAttr)
        {
            try
            {
                var enumerable = new ConcurrentBag<EProgramAttribute>();
                var actionIds = Utils.GetActionIdList(Contract);
                var progIds = Utils.GetProgramIdList(Contract);

                Parallel.ForEach(pAttr, r =>
                {
                    var userIds = userIdList;
                    var patientIds = patientIdList;

                    enumerable.Add(new EProgramAttribute
                    {
                        DidNotEnrollReason = r.DidNotEnrollReason,
                        Eligibility = r.Eligibility == null ? null : r.Eligibility.ToString(),
                        Enrollment = r.Enrollment == null ? null : r.Enrollment.ToString(),
                        GraduatedFlag = r.GraduatedFlag == null ? null : r.GraduatedFlag.ToString(),
                        InelligibleReason = r.IneligibleReason,
                        Lock = r.Locked == null ? null : r.Locked.ToString(),
                        MongoPlanElementId = r.PlanElementId.ToString(),
                        OptOut = r.OptOut.ToString(),
                        OverrideReason = r.OverrideReason,
                        //PlanElementId = progIds[r.PlanElementId.ToString()],
                        Population = r.Population,
                        RemovedReason = r.RemovedReason,
                        LastUpdatedOn = r.LastUpdatedOn ?? null,
                        MongoRecordCreatedBy = r.RecordCreatedBy == null ? string.Empty : r.RecordCreatedBy.ToString(),
                        RecordCreatedBy = GetId(userIds, r.RecordCreatedBy.ToString()),
                        MongoUpdatedBy = r.UpdatedBy == null ? null : r.UpdatedBy.ToString(),
                        UpdatedBy = r.UpdatedBy == null ? 0 : GetId(userIds, r.UpdatedBy.ToString()),
                        Version = r.Version == null ? null : r.Version.ToString(),
                        Delete = r.DeleteFlag.ToString(),
                        RecordCreatedOn = GetNullableDate(r.RecordCreatedOn),
                        TTLDate = r.TTLDate ?? null,
                        CompletedBy = r.CompletedBy == null ? 0 : GetId(userIds, r.CompletedBy.ToString()),
                        Completed = r.Completed.ToString(),
                        MongoCompletedBy = r.CompletedBy == null ? string.Empty : r.CompletedBy.ToString(),
                        DateCompleted = r.DateCompleted ?? null,
                        MongoId = r.Id.ToString(),
                        Status = r.Status.ToString()
                    });
                });

                return enumerable;
            }
            catch (Exception ex)
            {
                throw new Exception("ReadEProgramAttributeSeries()" + ex.Message + ex.StackTrace);
            }
        }


        private int GetModuleId(MongoDB.Bson.ObjectId objectId, Dictionary<string, int> moduleIds,
            Dictionary<string, int> moduleSourceIds)
        {
            try
            {
                var modId = 0;

                if (moduleIds.ContainsKey(objectId.ToString()))
                    modId = moduleIds[objectId.ToString()];
                else if (moduleSourceIds.ContainsKey(objectId.ToString()))
                    modId = moduleSourceIds[objectId.ToString()];

                return modId;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("GetModuleId(): id :" + objectId.ToString(), ex);
            }
        }
    }
}
