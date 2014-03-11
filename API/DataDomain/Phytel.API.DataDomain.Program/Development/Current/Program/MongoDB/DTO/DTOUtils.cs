﻿using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    public static class DTOUtils
    {
        public static MEPatientProgram CreateInitialMEPatientProgram(PutProgramToPatientRequest request, MEProgram cp)
        {
            MEPatientProgram patientProgDoc = new MEPatientProgram
            {
                PatientId = ObjectId.Parse(request.PatientId),
                //AuthoredBy = cp.AuthoredBy,
                Client = cp.Client,
                ProgramState = Common.ProgramState.NotStarted,
                State = Common.ElementState.NotStarted,
                AssignedBy = cp.AssignedBy,
                AssignedOn = cp.AssignedOn,
                StartDate = System.DateTime.UtcNow, // utc time
                EndDate = null,
                //GraduatedFlag = false,
                //Population = null,
                //OptOut = null,
                //DidNotEnrollReason = null,
                //DisEnrollReason = null,
                //Eligibility = Common.EligibilityStatus.Pending,
                //EligibilityStartDate = System.DateTime.UtcNow,
                //EligibilityEndDate = null,
                //EligibilityRequirements = cp.EligibilityRequirements,
                //Enrollment = Common.GenericStatus.Pending,
                //EligibilityOverride = Common.GenericSetting.No,
                DateCompleted = cp.DateCompleted,
                ContractProgramId = cp.Id,
                DeleteFlag = cp.DeleteFlag,
                Description = cp.Description,
                LastUpdatedOn = System.DateTime.UtcNow, // utc time
                //Locked = cp.Locked,
                Name = cp.Name,
                ObjectivesInfo = cp.ObjectivesInfo,
                CompletedBy = cp.CompletedBy,
                UpdatedBy = ObjectId.Parse(request.UserId),
                SourceId = cp.Id,
                ShortName = cp.ShortName,
                Status = cp.Status,
                Version = cp.Version,
                Spawn = cp.Spawn,
                Completed = cp.Completed,
                Enabled = cp.Enabled,
                ExtraElements = cp.ExtraElements,
                Next = cp.Next,
                Order = cp.Order,
                Previous = cp.Previous,
                Modules = DTOUtils.SetValidModules(cp.Modules, request.ContractNumber)
            };
            return patientProgDoc;
        }

        public static List<Module> SetValidModules(List<Module> list, string contractNumber)
        {
            try
            {
                List<Step> steps = new List<Step>();
                List<Action> acts = new List<Action>();
                List<Module> mods = new List<Module>();

                foreach (Module m in list)
                {
                    if (m.Status == Common.Status.Active)
                    {
                        Module mod = new Module()
                        {
                            Id = m.Id,
                            ProgramId = m.ProgramId,
                            Description = m.Description,
                            Name = m.Name,
                            Status = m.Status,
                            Objectives = m.Objectives,
                            Completed = m.Completed,
                            Enabled = m.Enabled,
                            Next = m.Next,
                            Order = m.Order,
                            Previous = m.Previous,
                            Spawn = m.Spawn,
                            SourceId = m.SourceId,
                            AssignedBy = m.AssignedBy,
                            AssignedOn = m.AssignedOn,
                            State = Common.ElementState.NotStarted,
                            CompletedBy = m.CompletedBy,
                            DateCompleted = m.DateCompleted,
                            //Objectives = m.Objectives.Where(a => a.Status == Common.Status.Active).Select(z => new ObjectivesInfo()
                            //{
                            //    Id = z.Id,
                            //    Status = z.Status,
                            //    Unit = z.Unit,
                            //    Value = z.Value
                            //}).ToList(),
                            Actions = new List<Action>()
                        };

                        foreach (Action ai in m.Actions)
                        {
                            if (ai.Status == Common.Status.Active)
                            {
                                Action ac = new Action()
                                {
                                    CompletedBy = ai.CompletedBy,
                                    Description = ai.Description,
                                    Id = ai.Id,
                                    ModuleId = ai.ModuleId,
                                    Name = ai.Name,
                                    Status = ai.Status,
                                    Objectives = ai.Objectives,
                                    Completed = ai.Completed,
                                    Enabled = ai.Enabled,
                                    Next = ai.Next,
                                    Order = ai.Order,
                                    Previous = ai.Previous,
                                    Spawn = ai.Spawn,
                                    SourceId = ai.SourceId,
                                    AssignedBy = ai.AssignedBy,
                                    AssignedOn = ai.AssignedOn,
                                    State = Common.ElementState.NotStarted,
                                    //Objectives = ai.Objectives.Where(r => r.Status == Common.Status.Active).Select(x => new ObjectivesInfo()
                                    //{
                                    //    Id = x.Id,
                                    //    Status = x.Status,
                                    //    Unit = x.Unit,
                                    //    Value = x.Value
                                    //}).ToList(),
                                    Steps = ai.Steps.Where(s => s.Status == Common.Status.Active).Select(b => new Step()
                                    {
                                        Status = b.Status,
                                        Description = b.Description,
                                        Ex = b.Ex,
                                        Id = b.Id,
                                        ActionId = b.ActionId,
                                        Notes = b.Notes,
                                        Question = b.Question,
                                        Title = b.Title,
                                        Text = b.Text,
                                        StepTypeId = b.StepTypeId,
                                        Responses = GetContractStepResponses(b.Id, contractNumber),
                                        Completed = b.Completed,
                                        ControlType = b.ControlType,
                                        Enabled = b.Enabled,
                                        Header = b.Header,
                                        Next = b.Next,
                                        Order = b.Order,
                                        Previous = b.Previous,
                                        SelectedResponseId = b.SelectedResponseId,
                                        Spawn = b.Spawn,
                                        SourceId = b.SourceId,
                                        AssignedBy = b.AssignedBy,
                                        AssignedOn = b.AssignedOn,
                                        State = b.State
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
            catch (Exception ex)
            {
                throw new Exception("DataDomain:SetValidModules():" + ex.Message, ex.InnerException);
            }
        }

        private static List<MEPatientProgramResponse> GetContractStepResponses(ObjectId stepId, string contractNumber)
        {
            List<MEResponse> responseList = null;
            List<MEPatientProgramResponse> ppresponseList = new List<MEPatientProgramResponse>();
            try
            {
                IProgramRepository<GetStepResponseListResponse> repo =
                    ProgramRepositoryFactory<GetStepResponseListResponse>.GetStepResponseRepository(contractNumber, "NG");

                ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

                SelectExpression stepResponseExpression = new SelectExpression();
                stepResponseExpression.FieldName = MEResponse.StepIdProperty;
                stepResponseExpression.Type = SelectExpressionType.EQ;
                stepResponseExpression.Value = stepId.ToString();
                stepResponseExpression.ExpressionOrder = 1;
                stepResponseExpression.GroupID = 1;
                selectExpressions.Add(stepResponseExpression);

                APIExpression apiExpression = new APIExpression();
                apiExpression.Expressions = selectExpressions;

                Tuple<string, IEnumerable<object>> stepResponses = repo.Select(apiExpression);

                if (stepResponses != null)
                {
                    responseList = stepResponses.Item2.Cast<MEResponse>().ToList();
                    responseList.ForEach(rs =>
                    {
                        ppresponseList.Add(new MEPatientProgramResponse
                        {
                            Id = rs.Id,
                            Value = rs.Value,
                            Text = rs.Text,
                            StepId = rs.StepId,
                            Spawn = rs.Spawn,
                            Required = rs.Required,
                            Order = rs.Order,
                            Nominal = rs.Nominal,
                            NextStepId = rs.NextStepId
                        });
                    });
                }

                return ppresponseList;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetContractStepResponses():" + ex.Message, ex.InnerException);
            }
        }

        private static List<MEPatientProgramResponse> GetStepResponses(ObjectId stepId, string contractNumber)
        {
            List<MEPatientProgramResponse> responseList = null;
            try
            {
                IProgramRepository<GetStepResponseListResponse> repo =
                    ProgramRepositoryFactory<GetStepResponseListResponse>.GetPatientProgramStepResponseRepository(contractNumber, "NG");

                ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

                SelectExpression stepResponseExpression = new SelectExpression();
                stepResponseExpression.FieldName = MEPatientProgramResponse.StepIdProperty;
                stepResponseExpression.Type = SelectExpressionType.EQ;
                stepResponseExpression.Value = stepId.ToString();
                stepResponseExpression.ExpressionOrder = 1;
                stepResponseExpression.GroupID = 1;
                selectExpressions.Add(stepResponseExpression);

                APIExpression apiExpression = new APIExpression();
                apiExpression.Expressions = selectExpressions;

                Tuple<string, IEnumerable<object>> stepResponses = repo.Select(apiExpression);

                if (stepResponses != null)
                {
                    responseList = stepResponses.Item2.Cast<MEPatientProgramResponse>().ToList();
                }

                return responseList;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetStepResponses():" + ex.Message, ex.InnerException);
            }
        }

        public static GetStepResponseListResponse GetStepResponses(string stepId, string contractNumber, bool? service)
        {
            GetStepResponseListResponse StepResponseResponse = new GetStepResponseListResponse(); 
            List<MEResponse> responseList = null;
            List<StepResponse> returnResponseList = new List<StepResponse>();
            try
            {
                IProgramRepository<GetStepResponseListResponse> repo =
                    ProgramRepositoryFactory<GetStepResponseListResponse>.GetStepResponseRepository(contractNumber, "NG");

                ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

                SelectExpression stepResponseExpression = new SelectExpression();
                stepResponseExpression.FieldName = MEResponse.StepIdProperty;
                stepResponseExpression.Type = SelectExpressionType.EQ;
                stepResponseExpression.Value = stepId;
                stepResponseExpression.ExpressionOrder = 1;
                stepResponseExpression.GroupID = 1;
                selectExpressions.Add(stepResponseExpression);

                APIExpression apiExpression = new APIExpression();
                apiExpression.Expressions = selectExpressions;

                Tuple<string, IEnumerable<object>> stepResponses = repo.Select(apiExpression);

                if (stepResponses != null)
                {
                    responseList = stepResponses.Item2.Cast<MEResponse>().ToList();
                    responseList.ForEach(r =>
                    {
                        returnResponseList.Add(new StepResponse
                        {
                            Id = r.Id.ToString(),
                            NextStepId = r.NextStepId.ToString(),
                            Nominal = r.Nominal,
                            Order = r.Order,
                            Required = r.Required,
                            Spawn = DTOUtils.GetSpawnElements(r.Spawn),
                            StepId = r.StepId.ToString(),
                            Text = r.Text,
                            Value = r.Value
                        });
                    });
                }

                StepResponseResponse.StepResponseList = returnResponseList;

                return StepResponseResponse;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetStepResponses():" + ex.Message, ex.InnerException);
            }
        }

        public static void RecurseAndReplaceIds(List<Module> mods)
        {
            Dictionary<ObjectId, ObjectId> IdsList = new Dictionary<ObjectId, ObjectId>();
            try
            {
                foreach (Module md in mods)
                {
                    md.Id = RegisterIds(IdsList, md.Id);
                    if (md.Actions != null)
                    {
                        foreach (Action a in md.Actions)
                        {
                            a.Id = RegisterIds(IdsList, a.Id);
                            a.ModuleId = md.Id;
                            if (a.Steps != null)
                            {
                                foreach (Step s in a.Steps)
                                {
                                    s.Id = RegisterIds(IdsList, s.Id);
                                    s.ActionId = a.Id;
                                    if (s.Responses != null)
                                    {
                                        foreach (MEPatientProgramResponse r in s.Responses)
                                        {
                                            r.Id = RegisterIds(IdsList, r.Id);
                                            r.StepId = s.Id;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                ScanAndReplaceIdReferences(IdsList, mods);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ScanAndReplaceIdReferences(Dictionary<ObjectId, ObjectId> IdsList, List<Module> mods)
        {
            try
            {
                foreach (KeyValuePair<ObjectId, ObjectId> kv in IdsList)
                {
                    foreach (Module md in mods)
                    {
                        ReplaceNextAndPreviousIds(kv, md);
                        ReplaceSpawnIdReferences(kv, md);
                        if (md.Actions != null)
                        {
                            foreach (Action a in md.Actions)
                            {
                                ReplaceNextAndPreviousIds(kv, a);
                                ReplaceSpawnIdReferences(kv, a);
                                if (a.Steps != null)
                                {
                                    foreach (Step s in a.Steps)
                                    {
                                        ReplaceNextAndPreviousIds(kv, s);
                                        ReplaceSpawnIdReferences(kv, s);
                                        ReplaceSelectedResponseId(kv, s);
                                        if (s.Responses != null)
                                        {
                                            foreach (MEPatientProgramResponse r in s.Responses)
                                            {
                                                if (r.Id.Equals(kv.Key))
                                                {
                                                    r.Id = kv.Value;
                                                }

                                                if (r.NextStepId != null)
                                                {
                                                    if (r.NextStepId.Equals(kv.Key))
                                                    {
                                                        r.NextStepId = kv.Value;
                                                    }
                                                }

                                                if (r.Spawn != null)
                                                {
                                                    foreach (SpawnElement sp in r.Spawn)
                                                    {
                                                        if (sp.SpawnId != null)
                                                        {
                                                            if (sp.SpawnId.Equals(kv.Key))
                                                            {
                                                                sp.SpawnId = kv.Value;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ReplaceNextAndPreviousIds(KeyValuePair<ObjectId, ObjectId> kv, PlanElement md)
        {
            try
            {
                if (md.Previous != null)
                {
                    if (md.Previous.Equals(kv.Key.ToString()))
                    {
                        md.Previous = kv.Value.ToString();
                    }
                }
                if (md.Next != null)
                {
                    if (md.Next.Equals(kv.Key.ToString()))
                    {
                        md.Next = kv.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ReplaceSpawnIdReferences(KeyValuePair<ObjectId, ObjectId> kv, PlanElement pln)
        {
            try
            {
                if (pln.Spawn != null)
                {
                    foreach (SpawnElement sp in pln.Spawn)
                    {
                        if (sp.SpawnId != null)
                        {
                            if (sp.SpawnId.Equals(kv.Key))
                            {
                                sp.SpawnId = kv.Value;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ReplaceSelectedResponseId(KeyValuePair<ObjectId, ObjectId> kv, Step s)
        {
            try
            {
                if (s.SelectedResponseId != null)
                {
                    if (s.SelectedResponseId.Equals(kv.Key))
                    {
                        s.SelectedResponseId = kv.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static ObjectId RegisterIds(Dictionary<ObjectId, ObjectId> list, ObjectId id)
        {
            ObjectId old = id;
            ObjectId newId = ObjectId.GenerateNewId();
            if (!list.ContainsKey(id))
            {
                list.Add(id, newId);
            }
            return newId;
        }

        internal static List<SpawnElement> GetSpawnElements(List<Program.DTO.SpawnElementDetail> list)
        {
            try
            {
                List<SpawnElement> spawnList = null;
                if (list != null)
                {
                    spawnList = new List<SpawnElement>();

                    list.ForEach(s =>
                    {
                        spawnList.Add(
                        new SpawnElement
                        {
                            SpawnId = ObjectId.Parse(s.ElementId),
                            Type = (SpawnElementTypeCode)s.ElementType,
                            Tag = s.Tag
                        });
                    });
                }
                return spawnList;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetSpawnElements():" + ex.Message, ex.InnerException);
            }
        }

        internal static List<SpawnElementDetail> GetSpawnElements(List<SpawnElement> list)
        {
            try
            {
                List<SpawnElementDetail> spawnList = null;
                if (list != null)
                {
                    spawnList = new List<SpawnElementDetail>();

                    list.ForEach(s =>
                    {
                        spawnList.Add(
                        new SpawnElementDetail
                        {
                            ElementId = s.SpawnId != null ? s.SpawnId.ToString() : null,
                            ElementType = (int)s.Type,
                            Tag = s.Tag
                        });
                    });
                }
                return spawnList;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetSpawnElements():" + ex.Message, ex.InnerException);
            }
        }

        internal static List<Action> GetActionElements(List<Program.DTO.ActionsDetail> list)
        {
            try
            {
                List<Action> acts = null;
                if (list != null)
                {
                    acts = new List<Action>();

                    list.ForEach(a =>
                    {
                        acts.Add(
                        new Action
                        {
                            Id = ObjectId.Parse(a.Id),
                            ModuleId = ObjectId.Parse(a.ModuleId),
                            Steps = GetStepsInfo(a.Steps),
                            AssignedBy = a.AssignBy,
                            AssignedOn = a.AssignDate,
                            Completed = a.Completed,
                            CompletedBy = a.CompletedBy,
                            DateCompleted = a.DateCompleted,
                            Description = a.Description,
                            State = (ElementState)a.ElementState,
                            Enabled = a.Enabled,
                            Name = a.Name,
                            Next = a.Next,
                            Objectives = GetObjectives(a.Objectives),
                            Order = a.Order,
                            Previous = a.Previous,
                            SourceId = ObjectId.Parse(a.SourceId),
                            Spawn = GetSpawnElements(a.SpawnElement),
                            Status = (Status)a.Status
                        });
                    });
                }
                return acts;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetActionElements():" + ex.Message, ex.InnerException);
            }
        }

        private static List<Step> GetStepsInfo(List<Program.DTO.StepsDetail> list)
        {
            try
            {
                List<Step> steps = null;
                if (list != null)
                {
                    steps = new List<Step>();

                    list.ForEach(st =>
                    {
                        steps.Add(
                        new Step
                        {
                            Id = ObjectId.Parse(st.Id),
                            ActionId = ObjectId.Parse(st.ActionId),
                            AssignedBy = st.AssignBy,
                            AssignedOn = st.AssignDate,
                            Completed = st.Completed,
                            CompletedBy = st.CompletedBy,
                            ControlType = (ControlType)st.ControlType,
                            DateCompleted = st.DateCompleted,
                            Description = st.Description,
                            State = (ElementState)st.ElementState,
                            Enabled = st.Enabled,
                            Ex = st.Ex,
                            Header = st.Header,
                            IncludeTime = st.IncludeTime,
                            Next = st.Next,
                            Notes = st.Notes,
                            Order = st.Order,
                            Previous = st.Previous,
                            Question = st.Question,
                            SelectedResponseId = DTOUtils.ParseObjectId(st.SelectedResponseId),
                            SelectType = (SelectType)st.SelectType,
                            SourceId = ObjectId.Parse(st.SourceId),
                            Status = (Status)st.Status,
                            StepTypeId = st.StepTypeId,
                            Text = st.Text,
                            Title = st.Title,
                            Spawn = GetSpawnElements(st.SpawnElement),
                            Responses = GetResponses(st.Responses)
                        });
                    });
                }
                return steps;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetStepsInfo():" + ex.Message, ex.InnerException);
            }
        }

        private static ObjectId? ParseObjectId(string p)
        {
            try
            {
                ObjectId? obj = null;
                if (!String.IsNullOrEmpty(p)) { ObjectId.Parse(p); }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:ParseObjectId():" + ex.Message, ex.InnerException);
            }
        }

        private static List<MEPatientProgramResponse> GetResponses(List<Program.DTO.ResponseDetail> list)
        {
            try
            {
                List<MEPatientProgramResponse> rs = null;
                if (list != null)
                {
                    rs = new List<MEPatientProgramResponse>();

                    list.ForEach(r =>
                        {
                            rs.Add(
                                new MEPatientProgramResponse
                                {
                                    StepId = ObjectId.Parse(r.StepId),
                                    NextStepId = ObjectId.Parse(r.NextStepId),
                                    Id = ObjectId.Parse(r.Id),
                                    Nominal = r.Nominal,
                                    Order = r.Order,
                                    Required = r.Required,
                                    Text = r.Text,
                                    Value = r.Value,
                                    Spawn = GetSPawnElement(r.SpawnElement)
                                });
                        });
                }
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetResponses():" + ex.Message, ex.InnerException);
            }
        }

        private static List<SpawnElement> GetSPawnElement(List<SpawnElementDetail> s)
        {
            try
            {
                List<SpawnElement> sp = new List<SpawnElement>();
                if (s != null)
                {
                    s.ForEach(sed =>
                    {
                        sp.Add(new SpawnElement
                        {
                            SpawnId =  sed.ElementId != null ? ObjectId.Parse(sed.ElementId) : ObjectId.Parse("000000000000000000000000"),
                            Type = (SpawnElementTypeCode)sed.ElementType,
                            Tag = sed.Tag
                        });
                    });
                }
                return sp;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetSpawnElement():" + ex.Message, ex.InnerException);
            }
        }

        private static SpawnElementDetail GetSPawnElement(SpawnElement s)
        {
            try
            {
                SpawnElementDetail sp = null;
                if (s != null)
                {
                    sp = new SpawnElementDetail
                    {
                        ElementType = (int)s.Type,
                        ElementId = s.SpawnId != null ? s.SpawnId.ToString() : null,
                        Tag = s.Tag
                    };
                }
                return sp;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetSpawnElement():" + ex.Message, ex.InnerException);
            }
        }

        public static List<Objective> GetObjectives(List<Program.DTO.ObjectivesDetail> list)
        {
            try
            {
                List<Objective> objs = null;
                if (list != null)
                {
                    objs = new List<Objective>();

                    list.ForEach(o =>
                    {
                        objs.Add(new Objective
                        {
                            Id = ObjectId.Parse(o.Id),
                            Status = (Status)o.Status,
                            Unit = o.Unit,
                            Value = o.Value
                        });
                    });
                }
                return objs;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetObjectives():" + ex.Message, ex.InnerException);
            }
        }

        public static List<Module> CloneAppDomainModules(List<ModuleDetail> prg)
        {
            try
            {
                List<Module> mods = null;
                if (prg != null)
                {
                    mods = new List<Module>();
                    prg.ForEach(m =>
                    {
                        mods.Add(
                        new Module
                        {
                            Id = ObjectId.Parse(m.Id),
                            DateCompleted = m.DateCompleted,
                            Next = m.Next,
                            Previous = m.Previous,
                            Spawn = DTOUtils.GetSpawnElements(m.SpawnElement),
                            Actions = DTOUtils.GetActionElements(m.Actions),
                            AssignedBy = m.AssignBy,
                            AssignedOn = m.AssignDate,
                            Completed = m.Completed,
                            CompletedBy = m.CompletedBy,
                            Description = m.Description,
                            State = (ElementState)m.ElementState,
                            Enabled = m.Enabled,
                            Name = m.Name,
                            Objectives = DTOUtils.GetObjectives(m.Objectives),
                            Order = m.Order,
                            ProgramId = ObjectId.Parse(m.ProgramId),
                            SourceId = ObjectId.Parse(m.SourceId),
                            Status = (Status)m.Status
                        });
                    });
                }
                return mods;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:CloneAppDomainModules():" + ex.Message, ex.InnerException);
            }
        }


        public static List<ModuleDetail> GetModules(List<Module> list, string contractNumber)
        {
            try
            {
                List<ModuleDetail> mods = new List<ModuleDetail>();
                list.ForEach(r => mods.Add(new ModuleDetail
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
                    SourceId = r.SourceId.ToString(),
                    AssignBy = r.AssignedBy,
                    AssignDate = r.AssignedOn,
                    ElementState = (int)r.State,
                    CompletedBy = r.CompletedBy,
                    DateCompleted = r.DateCompleted,
                    Objectives = GetObjectives(r.Objectives),
                    Actions = GetActions(r.Actions, contractNumber)
                }));
                return mods;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetModules():" + ex.Message, ex.InnerException);
            }
        }

        public static List<ObjectivesDetail> GetObjectives(List<Objective> list)
        {
            try
            {
                List<ObjectivesDetail> objs = new List<ObjectivesDetail>();
                if (list != null)
                {
                    list.ForEach(o => objs.Add(new ObjectivesDetail
                    {
                        Id = o.Id.ToString(),
                        Value = o.Value,
                        Status = (int)o.Status,
                        Unit = o.Unit
                    }));
                }
                return objs;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetObjectives():" + ex.Message, ex.InnerException);
            }
        }

        public static List<ActionsDetail> GetActions(List<Action> list, string contract)
        {
            try
            {
                List<ActionsDetail> acts = new List<ActionsDetail>();
                list.ForEach(a => acts.Add(new ActionsDetail
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
                    SourceId = a.SourceId.ToString(),
                    AssignBy = a.AssignedBy,
                    AssignDate = a.AssignedOn,
                    ElementState = (int)a.State,
                    DateCompleted = a.DateCompleted,
                    Objectives = GetObjectives(a.Objectives),
                    Steps = GetSteps(a.Steps, contract)
                }));
                return acts;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetActions():" + ex.Message, ex.InnerException);
            }
        }

        public static List<StepsDetail> GetSteps(List<Step> list, string contract)
        {
            try
            {
                List<StepsDetail> steps = new List<StepsDetail>();
                list.ForEach(s => steps.Add(
                    new StepsDetail
                    {
                        Description = s.Description,
                        Ex = s.Ex,
                        Id = s.Id.ToString(),
                        SourceId = s.SourceId.ToString(),
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
                        ControlType = (int)s.ControlType,
                        Header = s.Header,
                        SelectedResponseId = s.SelectedResponseId.ToString(),
                        IncludeTime = s.IncludeTime,
                        SelectType = (int)s.SelectType,
                        AssignBy = s.AssignedBy,
                        AssignDate = s.AssignedOn,
                        ElementState = (int)s.State,
                        CompletedBy = s.CompletedBy,
                        DateCompleted = s.DateCompleted,
                        Responses = GetResponses(s, contract),
                        SpawnElement = GetSpawnElement(s)
                    }));
                return steps;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetSteps():" + ex.Message, ex.InnerException);
            }
        }

        public static List<SpawnElementDetail> GetSpawnElement(PlanElement a)
        {
            try
            {
                List<SpawnElementDetail> spawn = new List<SpawnElementDetail>();

                if (a.Spawn != null)
                {
                    spawn = a.Spawn.Select(s => new SpawnElementDetail
                    {
                        ElementId = s.SpawnId != null ? s.SpawnId.ToString() : null,
                        ElementType = (int)s.Type,
                        Tag = s.Tag
                    }).ToList();
                }
                return spawn;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetSpawnElement():" + ex.Message, ex.InnerException);
            }
        }

        public static List<ResponseDetail> GetResponses(Step step, string contract)
        {
            try
            {
                List<MEPatientProgramResponse> meresp = step.Responses;
                List<ResponseDetail> resp = null;

                if (meresp == null || meresp.Count == 0)
                {
                    meresp = GetStepResponses(step.Id, contract);
                }

                resp = meresp.Select(x => new ResponseDetail
                {
                    Id = x.Id.ToString(),
                    NextStepId = x.NextStepId.ToString(),
                    Nominal = x.Nominal,
                    Order = x.Order,
                    Required = x.Required,
                    StepId = x.StepId.ToString(),
                    Text = x.Text,
                    Value = x.Value,
                    SpawnElement = GetResponseSpawnElement(x.Spawn)
                }).ToList<ResponseDetail>();

                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetResponses():" + ex.Message, ex.InnerException);
            }
        }

        public static List<SpawnElementDetail> GetResponseSpawnElement(List<SpawnElement> mESpawnElement)
        {
            try
            {
                List<SpawnElementDetail> sed = new List<SpawnElementDetail>();
                if (mESpawnElement != null)
                {
                    mESpawnElement.ForEach(se =>
                    {
                        sed.Add(new SpawnElementDetail
                        {
                            ElementId = se.SpawnId != null ? se.SpawnId.ToString(): null,
                            ElementType = (int)se.Type,
                            Tag = se.Tag
                        });
                    });
                }
                return sed;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DDomain:GetResponseSpawnElement()" + ex.Message, ex.InnerException);
            }
        }

        public static void RecurseAndSaveResponseObjects(MEPatientProgram prog, string contractNumber)
        {
            try
            {
                foreach (Module m in prog.Modules)
                {
                    foreach (Action a in m.Actions)
                    {
                        foreach (Step s in a.Steps)
                        {
                            bool success = false;
                            foreach (MEPatientProgramResponse r in s.Responses)
                            {
                                r.StepSourceId = s.SourceId;
                                success = SaveResponseToDocument(r, contractNumber);
                            }
                            if (success)
                            {
                                s.Responses = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DDomain:RecurseAndSaveResponseObjects()" + ex.Message, ex.InnerException);
            }
        }

        private static bool SaveResponseToDocument(MEPatientProgramResponse r, string contractNumber)
        {
            bool result = false;
            try
            {
                IProgramRepository<MEPatientProgramResponse> repo =
                    ProgramRepositoryFactory<MEPatientProgramResponse>.GetPatientProgramStepResponseRepository(contractNumber, "NG");

                result = (Boolean)repo.Insert(r);
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DDomain:SaveResponseToDocument()" + ex.Message, ex.InnerException);
            }
        }

        internal static ProgramAttribute InitializeElementAttributes(ProgramInfo p)
        {
            try
            {
                ProgramAttribute pa = new ProgramAttribute();
                pa.PlanElementId = p.Id;
                pa.Status = p.Status;
                pa.StartDate = System.DateTime.Now;
                pa.EndDate = null;
                pa.Eligibility = 3;
                pa.Enrollment = 2;
                pa.GraduatedFlag = 1;
                pa.OptOut = false;
                pa.EligibilityOverride = 1;
                return pa;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DDomain:InitializeElementAttributes()" + ex.Message, ex.InnerException);
            }
        }

        internal static void InitializeProgramAttributes(PutProgramToPatientRequest request, PutProgramToPatientResponse response)
        {
            try
            {
                // create program attribute insertion
                ProgramAttribute attr = DTOUtils.InitializeElementAttributes(response.program);

                IProgramRepository<PutProgramAttributesResponse> attrRepo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramAttributesResponse>
                    .GetProgramAttributesRepository(request.ContractNumber, request.Context);

                attrRepo.Insert(attr);
            }
            catch (Exception ex)
            {
                throw new Exception("DD: InitializeProgramAttributes()" + ex.Message, ex.InnerException);
            }
        }
    }
}
