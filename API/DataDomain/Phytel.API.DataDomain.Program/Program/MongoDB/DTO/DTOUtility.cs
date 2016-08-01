using MongoDB.Bson;
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
    public class DTOUtility : IDTOUtility
    {
        public IProgramRepositoryFactory Factory { get; set; }
        public List<MEPatientProgramResponse> ResponsesBag { get; set; }

        public MEPatientProgram CreateInitialMEPatientProgram(PutProgramToPatientRequest request, MEProgram cp, List<ObjectId> sil)
        {
            try
            {
                // logic to check for assignto params from program template
                var res = GetCareManagerValueByRule(request, cp);
                ObjectId? cmId = null;
                if (res != null) cmId = ObjectId.Parse(res); 

                MEPatientProgram patientProgDoc = new MEPatientProgram(request.UserId)
                {
                    PatientId = ObjectId.Parse(request.PatientId),
                    //AuthoredBy = cp.AuthoredBy,
                    Client = cp.Client,
                    //ProgramState = ProgramState.NotStarted  depricated - Use Element state instead.
                    State = ElementState.NotStarted,
                    StateUpdatedOn = DateTime.UtcNow,
                    AttributeStartDate = null,
                    AttributeEndDate = null,
                    AssignedBy = request.UserId != null ? ObjectId.Parse(request.UserId): ObjectId.Empty,
                    AssignedOn = DateTime.UtcNow,
                    AssignedTo = cmId,
                    StartDate = cp.StartDate,
                    EndDate = cp.EndDate,
                    DateCompleted = cp.DateCompleted,
                    ContractProgramId = cp.Id,
                    DeleteFlag = cp.DeleteFlag,
                    Description = cp.Description,
                    LastUpdatedOn = DateTime.UtcNow, // utc time
                    Name = cp.Name,
                    CompletedBy = cp.CompletedBy,
                    SourceId = cp.Id,
                    ShortName = cp.ShortName,
                    Status = cp.Status,
                    Version = cp.Version,
                    Spawn = cp.Spawn,
                    Completed = cp.Completed,
                    Enabled = cp.Enabled,
                    Next = cp.Next,
                    Order = cp.Order,
                    Previous = cp.Previous,
                    Modules = GetClonedModules(cmId, cp.Modules, request, sil),
                    EligibilityEndDate = cp.EligibilityEndDate,
                    EligibilityStartDate = cp.EligibilityStartDate,
                    EligibilityRequirements = cp.EligibilityRequirements,
                    //Objectives = cp.Objectives
                    //,UpdatedBy = ObjectId.Parse(request.UserId)
                };
                if (!string.IsNullOrEmpty(request.UserId))
                {
                    patientProgDoc.UpdatedBy = ObjectId.Parse(request.UserId);
                }
                return patientProgDoc;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:CreateInitialMEPatientProgram()::" + ex.Message, ex.InnerException);
            }
        }

        public string GetCareManagerValueByRule(PutProgramToPatientRequest request, MEProgram cp)
        {
            string cmId = null;
            try
            {
                switch (cp.AssignToType)
                {
                    case AssignToType.PCM:
                        {
                            cmId = request.CareManagerId ?? null;
                            break;
                        }
                    case AssignToType.Unassigned:
                        {
                            break;
                        }
                    default:
                        {
                            cmId = request.CareManagerId ?? null;
                            break;
                        }
                }
                return cmId;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetCareManagerValueByRule()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<Module> GetClonedModules(ObjectId? cmid, List<Module> list, IDataDomainRequest request, List<ObjectId> sil)
        {
            try
            {
                List<Module> mods = new List<Module>();

                if (list != null && list.Count > 0)
                {
                    foreach (Module m in list)
                    {
                        if (m.Status == Status.Active)
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
                                State = ElementState.NotStarted,
                                CompletedBy = m.CompletedBy,
                                DateCompleted = m.DateCompleted,
                                Actions = GetClonedActions(cmid, m.Actions, request.ContractNumber, request.UserId, sil, m.Enabled)
                            };

                            mod.Objectives = null;
                            if (mod.Enabled)
                            {
                                mod.AssignedBy = ObjectId.Parse(Constants.SystemContactId);
                                mod.AssignedOn = System.DateTime.UtcNow;
                                mod.AssignedTo = cmid;
                                mod.StateUpdatedOn = DateTime.UtcNow;
                            }
                            mods.Add(mod);
                        }
                    }
                }
                return mods;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:SetValidModules()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<Action> GetClonedActions(ObjectId? cmid, List<Action> list, string contractNumber, string userId, List<ObjectId> sil, bool pEnabled)
        {
            try
            {
                List<Action> actions = new List<Action>();
                if (list == null || list.Count <= 0) return actions;

                foreach (Action ai in list)
                {
                    if (ai.Status == Status.Active)
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
                            AssignedOn = ai.AssignedOn,
                            State = ElementState.NotStarted,
                            Steps = GetClonedSteps(contractNumber, userId, ai, sil),
                            DeleteFlag = ai.DeleteFlag
                        };
                        ac.Objectives = null;

                        if (ac.Enabled && pEnabled)
                        {
                            ac.AssignedBy = ObjectId.Parse(Constants.SystemContactId); // NIGHT-876
                            ac.AssignedOn = System.DateTime.UtcNow; // NIGHT-835
                            ac.AssignedTo = cmid; // NIGHT-877
                            ac.StateUpdatedOn = DateTime.UtcNow; //NIGHT-952
                        }

                        actions.Add(ac);
                    }
                }
                return actions;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetClonedActions()::" + ex.Message, ex.InnerException);
            }
        }

        private  List<Step> GetClonedSteps(string contractNumber, string userId, Action ai, List<ObjectId> sil)
        {
            try
            {
                List<Step> steps = new List<Step>();
                if (ai.Steps == null) return steps;

                //Parallel.ForEach(ai.Steps, b =>
                foreach (Step b in ai.Steps.Where(b => b.Status == Status.Active))
                {
                    sil.Add(b.Id);
                    steps.Add(new Step()
                    {
                        Status = b.Status,
                        Description = b.Description,
                        Id = b.Id,
                        ActionId = b.ActionId,
                        Notes = b.Notes,
                        Question = b.Question,
                        Title = b.Title,
                        Text = b.Text,
                        StepTypeId = b.StepTypeId,
                        Responses = b.Responses,
                        //Responses = GetContractStepResponses(b.Id, contractNumber, userId),
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
                    });
                }
                //);

                return steps;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetStepsByParallel()::" + ex.Message, ex.InnerException);
            }

            //return ai.Steps.Where(s => s.Status == Common.Status.Active).Select(b => new Step()
            //{
            //    Status = b.Status,
            //    Description = b.Description,
            //    Id = b.Id,
            //    ActionId = b.ActionId,
            //    Notes = b.Notes,
            //    Question = b.Question,
            //    Title = b.Title,
            //    Text = b.Text,
            //    StepTypeId = b.StepTypeId,
            //    Responses = GetContractStepResponses(b.Id, contractNumber, userId),
            //    Completed = b.Completed,
            //    ControlType = b.ControlType,
            //    Enabled = b.Enabled,
            //    Header = b.Header,
            //    Next = b.Next,
            //    Order = b.Order,
            //    Previous = b.Previous,
            //    SelectedResponseId = b.SelectedResponseId,
            //    Spawn = b.Spawn,
            //    SourceId = b.SourceId,
            //    AssignedBy = b.AssignedBy,
            //    AssignedOn = b.AssignedOn,
            //    State = b.State
            //}).ToList();
        }

        //private static List<MEPatientProgramResponse> GetContractStepResponses(ObjectId stepId, string contractNumber, string userId)
        //{
        //    List<MEResponse> responseList = null;
        //    List<MEPatientProgramResponse> ppresponseList = new List<MEPatientProgramResponse>();
        //    try
        //    {
        //        IProgramRepository repo =
        //            Factory.GetStepResponseRepository(contractNumber, "NG", userId);

        //        List<MEResponse> stepResponses = (List<MEResponse>)repo.Find(stepId.ToString());

        //        if (stepResponses != null)
        //        {
        //            responseList = stepResponses;
        //            responseList.ForEach(rs =>
        //            {
        //                ppresponseList.Add(new MEPatientProgramResponse(userId)
        //                {
        //                    Id = rs.Id,
        //                    Value = rs.Value,
        //                    Text = rs.Text,
        //                    StepId = rs.StepId,
        //                    Spawn = rs.Spawn,
        //                    Required = rs.Required,
        //                    Order = rs.Order,
        //                    Nominal = rs.Nominal,
        //                    NextStepId = rs.NextStepId
        //                });
        //            });
        //        }

        //        return ppresponseList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DTOUtils:GetContractStepResponses()::" + ex.Message, ex.InnerException);
        //    }
        //}

        public  GetStepResponseListResponse GetStepResponses(string stepId, string contractNumber, bool? service, string userId)
        {
            GetStepResponseListResponse StepResponseResponse = new GetStepResponseListResponse(); 
            List<MEResponse> responseList = null;
            List<StepResponse> returnResponseList = new List<StepResponse>();
            try
            {
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest { ContractNumber = contractNumber, Context = "NG", UserId = userId };
                IProgramRepository repo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.Response);

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
                throw new Exception("DD:DTOUtils:GetStepResponses()::" + ex.Message, ex.InnerException);
            }
        }

        public  void RecurseAndReplaceIds(List<Module> mods, Dictionary<ObjectId, ObjectId> IdsList)
        {
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
                throw new Exception("DD:DTOUtils:RecurseAndReplaceIds()::" + ex.Message, ex.InnerException);
            }
        }

        private  void ScanAndReplaceIdReferences(Dictionary<ObjectId, ObjectId> IdsList, List<Module> mods)
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
                throw new Exception("DD:DTOUtils:ScanAndReplaceIdReferences()::" + ex.Message, ex.InnerException);
            }
        }

        private  void ReplaceNextAndPreviousIds(KeyValuePair<ObjectId, ObjectId> kv, PlanElement md)
        {
            try
            {
                if (md.Previous != null)
                {
                    if (md.Previous.Equals(kv.Key))
                    {
                        md.Previous = kv.Value;
                    }
                }
                if (md.Next != null)
                {
                    if (md.Next.Equals(kv.Key))
                    {
                        md.Next = kv.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:ReplaceNextAndPreviousIds()::" + ex.Message, ex.InnerException);
            }
        }

        private  void ReplaceSpawnIdReferences(KeyValuePair<ObjectId, ObjectId> kv, PlanElement pln)
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
                throw new Exception("DD:DTOUtils:ReplaceSpawnIdReferences()::" + ex.Message, ex.InnerException);
            }
        }

        private  void ReplaceSelectedResponseId(KeyValuePair<ObjectId, ObjectId> kv, Step s)
        {
            try
            {
                if (s.SelectedResponseId != null)
                {
                    // need to cast to non nullable type to compare
                    ObjectId sObjId = (ObjectId)s.SelectedResponseId;

                    if (sObjId.Equals(kv.Key))
                    {
                        s.SelectedResponseId = kv.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:ReplaceSelectedResponseId()::" + ex.Message, ex.InnerException);
            }
        }

        private  ObjectId RegisterIds(Dictionary<ObjectId, ObjectId> list, ObjectId id)
        {
            try
            {
                ObjectId old = id;
                ObjectId newId = ObjectId.GenerateNewId();
                if (!list.ContainsKey(id))
                {
                    list.Add(id, newId);
                }
                return newId;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:RegisterIds()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<SpawnElement> GetSpawnElements(List<Program.DTO.SpawnElementDetail> list)
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
                throw new Exception("DD:DTOUtils:GetSpawnElements()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<SpawnElementDetail> GetSpawnElements(List<SpawnElement> list)
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
                throw new Exception("DD:DTOUtils:GetSpawnElements()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<Action> GetActionElements(List<Program.DTO.ActionsDetail> list, string userId)
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
                            Steps = GetStepsInfo(a.Steps, userId),
                            AssignedBy = ParseObjectId(a.AssignBy),
                            AssignedOn = a.AssignDate,
                            Completed = a.Completed,
                            CompletedBy = a.CompletedBy,
                            DateCompleted = a.DateCompleted,
                            Description = a.Description,
                            State = (ElementState)a.ElementState,
                            Enabled = a.Enabled,
                            Name = a.Name,
                            Next = ParseObjectId(a.Next),
                            Objectives = GetObjectives(a.Objectives),
                            Order = a.Order,
                            Previous = ParseObjectId(a.Previous),
                            SourceId = ObjectId.Parse(a.SourceId),
                            Spawn = GetSpawnElements(a.SpawnElement),
                            Status = (Status)a.Status,
                            DeleteFlag = a.DeleteFlag
                        });
                    });
                }
                return acts;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetActionElements()::" + ex.Message, ex.InnerException);
            }
        }

        private  List<Step> GetStepsInfo(List<Program.DTO.StepsDetail> list, string userId)
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
                            AssignedBy = ParseObjectId(st.AssignBy),
                            AssignedOn = st.AssignDate,
                            Completed = st.Completed,
                            CompletedBy = st.CompletedBy,
                            ControlType = (ControlType)st.ControlType,
                            DateCompleted = st.DateCompleted,
                            Description = st.Description,
                            State = (ElementState)st.ElementState,
                            Enabled = st.Enabled,
                            Header = st.Header,
                            IncludeTime = st.IncludeTime,
                            Next = ParseObjectId(st.Next),
                            Notes = st.Notes,
                            Order = st.Order,
                            Previous = ParseObjectId(st.Previous),
                            Question = st.Question,
                            SelectedResponseId = DTOUtils.ParseObjectId(st.SelectedResponseId),
                            SelectType = (SelectType)st.SelectType,
                            SourceId = ObjectId.Parse(st.SourceId),
                            Status = (Status)st.Status,
                            StepTypeId = st.StepTypeId,
                            Text = st.Text,
                            Title = st.Title,
                            Spawn = GetSpawnElements(st.SpawnElement),
                            Responses = GetResponses(st.Responses, userId)
                        });
                    });
                }
                return steps;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetStepsInfo()::" + ex.Message, ex.InnerException);
            }
        }

        public  ObjectId? ParseObjectId(string p)
        {
            try
            {
                ObjectId? obj = null;
                if (!String.IsNullOrEmpty(p))
                {
                    obj = ObjectId.Parse(p);
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:ParseObjectId()::" + ex.Message, ex.InnerException);
            }
        }

        private  List<MEPatientProgramResponse> GetResponses(List<Program.DTO.ResponseDetail> list, string userId)
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
                                new MEPatientProgramResponse(userId)
                                {
                                    StepId = ObjectId.Parse(r.StepId),
                                    NextStepId = ObjectId.Parse(r.NextStepId),
                                    Id = ObjectId.Parse(r.Id),
                                    Nominal = r.Nominal,
                                    Order = r.Order,
                                    Required = r.Required,
                                    Text = r.Text,
                                    Value = r.Value,
                                    Spawn = GetSPawnElement(r.SpawnElement),
                                    DeleteFlag = r.Delete
                                });
                        });
                }
                return rs;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetResponses()::" + ex.Message, ex.InnerException);
            }
        }

        private  List<SpawnElement> GetSPawnElement(List<SpawnElementDetail> s)
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
                throw new Exception("DD:DTOUtils:GetSpawnElement()::" + ex.Message, ex.InnerException);
            }
        }

        private  SpawnElementDetail GetSPawnElement(SpawnElement s)
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
                throw new Exception("DD:DTOUtils:GetSpawnElement()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<Objective> GetObjectives(List<Program.DTO.ObjectiveInfoData> list)
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
                            Units = o.Unit,
                            Value = o.Value
                        });
                    });
                }
                return objs;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetObjectives()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<Module> CloneAppDomainModules(List<ModuleDetail> prg, string userId)
        {
            try
            {
                List<Module> mods = null;
                if (prg != null)
                {
                    mods = new List<Module>();
                    prg.ForEach(m => mods.Add(
                        new Module
                        {
                            Id = ObjectId.Parse(m.Id),
                            DateCompleted = m.DateCompleted,
                            Next = ParseObjectId(m.Next),
                            Previous = ParseObjectId(m.Previous),
                            Spawn = DTOUtils.GetSpawnElements(m.SpawnElement),
                            Actions = DTOUtils.GetActionElements(m.Actions, userId),
                            AssignedBy = ParseObjectId(m.AssignBy),
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
                        }));
                }
                return mods;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:CloneAppDomainModules()::" + ex.Message, ex.InnerException);
            }
        }


        public  List<ModuleDetail> GetModules(List<Module> list, string contractProgramId, IDataDomainRequest request)
        {
            try
            {
                List<Module> pMods = GetTemplateModulesList(contractProgramId, request.ContractNumber, request.UserId);

                List<ModuleDetail> mods = new List<ModuleDetail>();
                list.ForEach(m => mods.Add(new ModuleDetail
                {
                    Id = m.Id.ToString(),
                    ProgramId = m.ProgramId.ToString(),
                    Description = m.Description,
                    Name = m.Name,
                    Status = (int)m.Status,
                    Completed = m.Completed,
                    Enabled = m.Enabled,
                    Next = m.Next != null ? m.Next.ToString() : string.Empty,
                    Previous = m.Previous != null ? m.Previous.ToString() : string.Empty,
                    Order = m.Order,
                    SpawnElement = GetSpawnElement(m),
                    SourceId = m.SourceId.ToString(),
                    AssignBy = m.AssignedBy.ToString(),
                    AssignDate = m.AssignedOn,
                    AssignTo = m.AssignedTo != null ? m.AssignedTo.ToString() : null,
                    AttrEndDate = m.AttributeEndDate,
                    AttrStartDate = m.AttributeStartDate,
                    ElementState = (int)m.State,
                    StateUpdatedOn =  m.StateUpdatedOn,
                    CompletedBy = m.CompletedBy,
                    DateCompleted = m.DateCompleted,
                    Objectives = GetObjectivesForModule(pMods, m.SourceId),
                    Actions = GetActions(m.Actions, request, pMods.Find(pm => pm.SourceId == m.SourceId))
                }));
                return mods;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtility:GetModules()::" + ex.Message, ex.InnerException);
            }
        }

        public List<Module> GetTemplateModulesList(string contractProgramId, string contractNumber, string userId)
        {
            try
            {
                DataDomainRequest request = new DataDomainRequest { ContractNumber = contractNumber, UserId = userId };
                IProgramRepository programRepo = Factory.GetRepository(request, RepositoryType.Program);
                List<Module> pMods = (List<Module>)programRepo.GetProgramModules(ObjectId.Parse(contractProgramId));
                return pMods;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtility:GetTemplateModulesList()::" + ex.Message, ex.InnerException);
            }
        }

        public List<ObjectiveInfoData> GetObjectivesForModule(List<Module> mods, ObjectId sourceModId)
        {
            try
            {
                List<ObjectiveInfoData> oid = new List<ObjectiveInfoData>();
                if (mods != null)
                {
                    Module mod = mods.Find(m => m.SourceId == sourceModId);
                    if (mod != null && mod.Objectives != null)
                    {
                        mod.Objectives.ForEach(o =>
                        {
                            if (o.Status == Status.Active)
                            {
                                oid.Add(new ObjectiveInfoData
                                {
                                    Id = o.Id.ToString(),
                                    Status = (int)o.Status,
                                    Unit = o.Units,
                                    Value = o.Value
                                });
                            }
                        });
                    }
                }
                return oid;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtility:GetObjectivesForModule()::" + ex.Message, ex.InnerException);
            }
        }

        public List<ObjectiveInfoData> GetFromProgramObjectives(string pid, IDataDomainRequest request)
        {
            try
            {
                List<ObjectiveInfoData> odata = new List<ObjectiveInfoData>();
                MEProgram mep = GetLimitedProgramDetails(pid, request);
                odata = this.GetObjectivesData(mep.Objectives);
                return odata;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtility:GetFromProgramObjectives()::" + ex.Message, ex.InnerException);
            }
        }

        public MEProgram GetLimitedProgramDetails(string objectId, IDataDomainRequest request)
        {
            IProgramRepository programRepo = Factory.GetRepository(request, RepositoryType.Program);
            MEProgram meProgram = programRepo.GetLimitedProgramFields(objectId) as MEProgram;
            return meProgram;
        }

        public List<ObjectiveInfoData> GetObjectivesData(List<Objective> sobjs)
        {
            try
            {
                List<ObjectiveInfoData> objs = null;
                if (sobjs != null && sobjs.Count > 0)
                {
                    objs =
                    sobjs.Where(x => x.Status == Status.Active)
                    .Select(o => new ObjectiveInfoData
                    {
                        Id = o.Id.ToString(),
                        Status = (int)o.Status,
                        Unit = o.Units,
                        Value = o.Value
                    }).ToList();
                }
                return objs;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetObjectivesData()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<ObjectiveInfoData> GetObjectives(List<Objective> list)
        {
            try
            {
                List<ObjectiveInfoData> objs = new List<ObjectiveInfoData>();
                if (list != null)
                {
                    list.Where(ob => ob.Status == Status.Active).ToList().ForEach(o => objs.Add(new ObjectiveInfoData
                    {
                        Id = o.Id.ToString(),
                        Value = o.Value,
                        Status = (int)o.Status,
                        Unit = o.Units
                    }));
                }
                return objs;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetObjectives()::" + ex.Message, ex.InnerException);
            }
        }

        public List<ActionsDetail> GetActions(List<Action> list, IDataDomainRequest request, Module mod)
        {
            try
            {
                List<ActionsDetail> acts = new List<ActionsDetail>();
                list.ForEach(a =>
                {
                    a.Objectives = GetTemplateObjectives(a.SourceId, mod);
                    acts.Add(GetAction(request.Context, request.UserId, a));
                });
                return acts;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetActions()::" + ex.Message, ex.InnerException);
            }
        }

        public List<Objective> GetTemplateObjectives(ObjectId sourceId, Module mod)
        {
            try
            {
                List<Objective> objs = new List<Objective>();

                List<Action> templateActions = null;
                if (mod != null)
                {
                    if (mod.Actions != null)
                        templateActions = (List<Action>)mod.Actions;
                }

                if (templateActions != null)
                {
                    Action act = templateActions.Find(ta => ta.SourceId == sourceId);
                    if (act != null)
                        objs = act.Objectives;
                }

                return objs;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetTemplateObjectives()::" + ex.Message, ex.InnerException);
            }
        }

        public  ActionsDetail GetAction(string contract, string userId, Action a)
        {
            ActionsDetail actionDetail = null;
            try
            {
                if (a != null)
                {
                    actionDetail =  new ActionsDetail
                    {
                        CompletedBy = a.CompletedBy,
                        Description = a.Description,
                        Id = a.Id.ToString(),
                        ModuleId = a.ModuleId != null ? a.ModuleId.ToString() : null,
                        Name = a.Name,
                        Status = (int)a.Status,
                        Completed = a.Completed,
                        Enabled = a.Enabled,
                        Next = a.Next != null ? a.Next.ToString() : string.Empty,
                        Previous = a.Previous != null ? a.Previous.ToString() : string.Empty,
                        Order = a.Order,
                        SpawnElement = GetSpawnElement(a),
                        SourceId = a.SourceId != null ? a.SourceId.ToString() : null,
                        AssignBy = a.AssignedBy != null ? a.AssignedBy.ToString(): null,
                        AssignTo = a.AssignedTo != null ? a.AssignedTo.ToString() : null,
                        AssignDate = a.AssignedOn,
                        AttrEndDate = a.AttributeEndDate,
                        AttrStartDate = a.AttributeStartDate,
                        ElementState = (int)a.State,
                        StateUpdatedOn = a.StateUpdatedOn,
                        DateCompleted = a.DateCompleted,
                        Objectives = GetObjectives(a.Objectives),
                        Steps = GetSteps(a.Steps, contract, userId),
                        Archived = a.Archived,
                        ArchivedDate = a.ArchivedDate,
                        ArchiveOriginId = a.ArchiveOriginId,
                        DeleteFlag = a.DeleteFlag
                    };
                }
            }
            catch 
            {
                throw;
            }
            return actionDetail;
        }

        public List<StepsDetail> GetSteps(List<Step> list, string contract, string userId)
        {
            try
            {
                List<StepsDetail> steps = new List<StepsDetail>();
                if (list == null)
                    return steps;

                list.ForEach(s =>
                {
                    steps.Add(
                        new StepsDetail
                        {
                            Description = s.Description,
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
                            Next = s.Next != null ? s.Next.ToString() : string.Empty,
                            Previous = s.Previous != null ? s.Previous.ToString() : string.Empty,
                            Order = s.Order,
                            ControlType = (int)s.ControlType,
                            Header = s.Header,
                            SelectedResponseId = s.SelectedResponseId.ToString(),
                            IncludeTime = s.IncludeTime,
                            SelectType = (int)s.SelectType,
                            AssignBy = s.AssignedBy.ToString(),
                            AssignDate = s.AssignedOn,
                            ElementState = (int)s.State,
                            CompletedBy = s.CompletedBy,
                            DateCompleted = s.DateCompleted,
                            Responses = GetResponses(s, contract, userId),
                            SpawnElement = GetSpawnElement(s)
                        });
                }
                );
                return steps;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetSteps()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<SpawnElementDetail> GetSpawnElement(PlanElement a)
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
                throw new Exception("DD:DTOUtils:GetSpawnElement()::" + ex.Message, ex.InnerException);
            }
        }

        public List<ResponseDetail> GetResponses(Step step, string contract, string userId)
        {
            try
            {
                List<MEPatientProgramResponse> meresp = step.Responses;
                List<ResponseDetail> resp = null;

                if (meresp == null || meresp.Count == 0)
                {
                    try
                    {
                    //meresp = GetStepResponses(step.Id, contract, userId);
                        if (ResponsesBag == null) throw new ArgumentException("ResponseBag is null");
                    meresp = ResponsesBag.Where(r => r.StepId == step.Id).ToList();
                }
                    catch (Exception ex)
                    {
                        throw new Exception("DD:DTOUtils:GetResponses()::meresp where clause error" + ex.Message,
                            ex.InnerException);
                    }
                }

                try
                {
                resp = meresp.Select(x => new ResponseDetail
                {
                        Id = Convert.ToString(x.Id),
                        NextStepId = Convert.ToString(x.NextStepId),
                    Nominal = x.Nominal,
                    Order = x.Order,
                    Required = x.Required,
                        StepId = Convert.ToString(x.StepId),
                    Text = x.Text,
                    Value = x.Value,
                    SpawnElement = GetResponseSpawnElement(x.Spawn)
                }).ToList();
                }
                catch(Exception ex)
                {
                    throw new Exception("DD:DTOUtils:GetResponses()::Linq Expression error" + ex.Message, ex.InnerException);
                }

                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetResponses()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<SpawnElementDetail> GetResponseSpawnElement(List<SpawnElement> mESpawnElement)
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
                throw new ArgumentException("DD:DTOUtils:GetResponseSpawnElement()::" + ex.Message, ex.InnerException);
            }
        }

        public  void RecurseAndSaveResponseObjects(MEPatientProgram prog, string contractNumber, string userId)
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
                                    r.DeleteFlag = true;
                                    r.ActionId = a.Id;
                                    success = SaveResponseToDocument(r, contractNumber, userId);
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
                throw new ArgumentException("DD:DTOUtils:RecurseAndSaveResponseObjects()::" + ex.Message, ex.InnerException);
            }
        }

        public  bool SavePatientProgramResponses(List<MEPatientProgramResponse> pprs, PutProgramToPatientRequest request)
        {
            try
            {
                IProgramRepository repo =
                    new ProgramRepositoryFactory().GetRepository(request, RepositoryType.PatientProgramResponse);//.GetPatientProgramStepResponseRepository(request);

                bool result = (bool)repo.InsertAsBatch(pprs);
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DD:DTOUtils:SavePatientProgramResponses()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<MEPatientProgramResponse> RecurseAndStoreResponseObjects(MEPatientProgram prog, string contractNumber, string userId)
        {
            try
            {
                List<MEPatientProgramResponse> ppr = new List<MEPatientProgramResponse>();
                foreach (Module m in prog.Modules)
                {
                    foreach (Action a in m.Actions)
                    {
                        foreach (Step s in a.Steps)
                        {
                            foreach (MEPatientProgramResponse r in s.Responses)
                            {
                                r.DeleteFlag = false;
                                r.RecordCreatedBy = ObjectId.Parse(userId);
                                r.RecordCreatedOn = DateTime.UtcNow;

                                if (!ppr.Contains(r))
                                    ppr.Add(r);
                            }
                        }
                    }
                }
                return ppr;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DD:DTOUtils:RecurseAndStoreResponseObjects()::" + ex.Message, ex.InnerException);
            }
        }

        private  bool SaveResponseToDocument(MEPatientProgramResponse r, string contractNumber, string userId)
        {
            bool result = false;
            try
            {
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest { ContractNumber = contractNumber, Context = "NG", UserId = userId };
                IProgramRepository repo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.PatientProgramResponse);//.GetPatientProgramStepResponseRepository(new GetPatientProgramsRequest { ContractNumber = contractNumber, Context = "NG", UserId = userId });

                result = (Boolean)repo.Insert(r);
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DD:DTOUtils:SaveResponseToDocument()::" + ex.Message, ex.InnerException);
            }
        }

        public  ProgramAttributeData InitializeElementAttributes(ProgramInfo p)
        {
            try
            {
                ProgramAttributeData pa = new ProgramAttributeData();
                pa.PlanElementId = p.Id;
                pa.Status = p.Status;
                //pa.AttrStartDate = System.DateTime.UtcNow;
                //pa.AttrEndDate = null;
                pa.Eligibility = 3;
                pa.Enrollment = 2;
                pa.GraduatedFlag = 1;
                pa.OptOut = false;
                //pa.EligibilityOverride = 1;
                return pa;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DD:DTOUtils:InitializeElementAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        public  void InitializeProgramAttributes(PutProgramToPatientRequest request, PutProgramToPatientResponse response)
        {
            try
            {
                // create program attribute insertion
                ProgramAttributeData attr = InitializeElementAttributes(response.program);

                IProgramRepository attrRepo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.PatientProgramAttribute);//.GetProgramAttributesRepository(request);

                attrRepo.Insert(attr);
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:InitializeProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        public  bool CanInsertPatientProgram(List<MEPatientProgram> pp)
        {
            try
            {
                bool result = true;
                if (pp == null)
                {
                    result = true;
                }
                else if (pp.Count >= 1)
                {
                    foreach (MEPatientProgram p in pp)
                    {
                        if ((p.State != ElementState.Completed) && (p.State != ElementState.Closed))
                        {
                            result = false;
                            break;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtility:CanInsertPatientProgram()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<MEPatientProgram> FindExistingpatientProgram(PutProgramToPatientRequest request)
        {
            try
            {
                IProgramRepository pRepo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.PatientProgram);//.GetPatientProgramRepository(request);

                List<MEPatientProgram> pp = pRepo.FindByEntityExistsID(request.PatientId, request.ContractProgramId) as List<MEPatientProgram>;
                return pp;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtility:FindExistingpatientProgram()::" + ex.Message, ex.InnerException);
            }
        }

        public  MEProgram GetProgramForDeepCopy(PutProgramToPatientRequest request)
        {
            try
            {
                IProgramRepository pgRepo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.Program);//.GetProgramRepository(request);
                MEProgram cp = (MEProgram)pgRepo.FindByID(request.ContractProgramId.ToString());
                return cp;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtility:GetProgramForDeepCopy()::" + ex.Message, ex.InnerException);
            }
        }

        public  ProgramInfo SaveNewPatientProgram(PutProgramToPatientRequest request, MEPatientProgram nmePP)
        {
            try
            {
                // give this the formatted patient program ready for saving
                IProgramRepository patProgRepo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.PatientProgram);//.GetPatientProgramRepository(request);

                ProgramInfo pi = (ProgramInfo)patProgRepo.Insert((object)nmePP);
                return pi;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:SaveNewPatientProgram()::" + ex.Message, ex.InnerException);
            }
        }

        public  void InitializePatientProgramAssignment(PutProgramToPatientRequest request, MEPatientProgram nmePP)
        {
            try
            {
                // update to new ids and their references
                Dictionary<ObjectId, ObjectId> IdsList = new Dictionary<ObjectId, ObjectId>();
                DTOUtils.RecurseAndReplaceIds(nmePP.Modules, IdsList);
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:InitializePatientProgramAssignment()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<MEResponse> GetProgramResponseslist(List<ObjectId> idl, MEProgram cp, PutProgramToPatientRequest request)
        {
            try
            {
                List<MEResponse> list = null;
                IProgramRepository respRepo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.Response);//.GetStepResponseRepository(request);

                list = respRepo.Find(idl) as List<MEResponse>;
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetProgramResponseslist()::" + ex.Message, ex.InnerException);
            }
        }

        public  void HydrateResponsesInProgram(MEProgram prog, List<MEResponse> responseList, string usrId)
        {
            try
            {
                foreach (Module m in prog.Modules)
                {
                    foreach (Action a in m.Actions)
                    {
                        foreach (Step s in a.Steps)
                        {
                            List<MEResponse> resps = responseList.FindAll(st => st.StepId == s.Id);
                            List<MEPatientProgramResponse> mrp = new List<MEPatientProgramResponse>();
                            resps.ForEach(rs =>
                            {
                                mrp.Add(
                                new MEPatientProgramResponse(usrId)
                                {
                                    Id = rs.Id,
                                    StepId = rs.StepId,
                                    StepSourceId = rs.StepSourceId,
                                    NextStepId = rs.NextStepId,
                                    Nominal = rs.Nominal,
                                    Order = rs.Order,
                                    Required = rs.Required,
                                    Selected = rs.Selected,
                                    Text = rs.Text,
                                    Spawn = rs.Spawn,
                                    Value = rs.Value,
                                    Version = rs.Version
                                });
                            });
                            s.Responses = mrp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DD:DTOUtils:HydrateResponsesInProgram()::" + ex.Message, ex.InnerException);
            }
        }

        public  List<ObjectId> GetStepIds(MEPatientProgram mepp)
        {
            throw new NotImplementedException();
        }

    }
}
