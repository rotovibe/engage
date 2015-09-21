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
    public static class DTOUtils
    {
        //public static MEPatientProgram CreateInitialMEPatientProgram(PutProgramToPatientRequest request, MEProgram cp, List<ObjectId> sil)
        //{
        //    try
        //    {
        //        MEPatientProgram patientProgDoc = new MEPatientProgram(request.UserId)
        //        {
        //            PatientId = ObjectId.Parse(request.PatientId),
        //            //AuthoredBy = cp.AuthoredBy,
        //            Client = cp.Client,
        //            ProgramState = ProgramState.NotStarted,
        //            State = ElementState.NotStarted,
        //            AttributeStartDate = null,
        //            AttrEndDate = null,
        //            AssignedBy = null,
        //            AssignedOn = null,
        //            StartDate = cp.StartDate, 
        //            EndDate = cp.EndDate,
        //            DateCompleted = cp.DateCompleted,
        //            ContractProgramId = cp.Id,
        //            DeleteFlag = cp.DeleteFlag,
        //            Description = cp.Description,
        //            LastUpdatedOn = System.DateTime.UtcNow, // utc time
        //            Name = cp.Name,
        //            CompletedBy = cp.CompletedBy,
        //            SourceId = cp.Id,
        //            ShortName = cp.ShortName,
        //            Status = cp.Status,
        //            Version = cp.Version,
        //            Spawn = cp.Spawn,
        //            Completed = cp.Completed,
        //            Enabled = cp.Enabled,
        //            Next = cp.Next,
        //            Order = cp.Order,
        //            Previous = cp.Previous,
        //            Modules = DTOUtils.GetClonedModules(cp.Modules, request.ContractNumber, request.UserId, sil),
        //            EligibilityEndDate = cp.EligibilityEndDate,
        //            EligibilityStartDate = cp.EligibilityStartDate,
        //            EligibilityRequirements = cp.EligibilityRequirements,
        //            //Objectives = cp.Objectives
        //            //,UpdatedBy = ObjectId.Parse(request.UserId)
        //        };
        //        if (!string.IsNullOrEmpty(request.UserId))
        //        {
        //            patientProgDoc.UpdatedBy = ObjectId.Parse(request.UserId);
        //        }
        //        return patientProgDoc;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DTOUtils:CreateInitialMEPatientProgram()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //public static List<Module> GetClonedModules(List<Module> list, string contractNumber, string userId, List<ObjectId> sil)
        //{
        //    try
        //    {
        //        List<Module> mods = new List<Module>();

        //        //Parallel.ForEach(list, m =>
        //        foreach (Module m in list)
        //        {
        //            if (m.Status == Status.Active)
        //            {
        //                Module mod = new Module()
        //                {
        //                    Id = m.Id,
        //                    ProgramId = m.ProgramId,
        //                    Description = m.Description,
        //                    Name = m.Name,
        //                    Status = m.Status,
        //                    Objectives = m.Objectives,
        //                    Completed = m.Completed,
        //                    Enabled = m.Enabled,
        //                    Next = m.Next,
        //                    Order = m.Order,
        //                    Previous = m.Previous,
        //                    Spawn = m.Spawn,
        //                    SourceId = m.SourceId,
        //                    AssignedBy = m.AssignedBy,
        //                    AssignedOn = m.AssignedOn,
        //                    State = ElementState.NotStarted,
        //                    CompletedBy = m.CompletedBy,
        //                    DateCompleted = m.DateCompleted,
        //                    //Objectives = m.Objectives.Where(a => a.Status == Common.Status.Active).Select(z => new ObjectivesInfo()
        //                    //{
        //                    //    Id = z.Id,
        //                    //    Status = z.Status,
        //                    //    Unit = z.Unit,
        //                    //    Value = z.Value
        //                    //}).ToList(),
        //                    Actions = GetClonedActions(m.Actions, contractNumber, userId, sil)
        //                };
        //                    mods.Add(mod);
        //            }
        //        }
        //        //);

        //        return mods;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DTOUtils:GetClonedModules()::" + ex.Message, ex.InnerException);
        //    }
        //}

        private static List<Action> GetClonedActions(List<Action> list, string contractNumber, string userId, List<ObjectId> sil)
        {
            try
            {
                List<Action> actions = new List<Action>();
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
                            AssignedBy = ai.AssignedBy,
                            AssignedOn = ai.AssignedOn,
                            State = ElementState.NotStarted,
                            //Objectives = ai.Objectives.Where(r => r.Status == Common.Status.Active).Select(x => new ObjectivesInfo()
                            //{
                            //    Id = x.Id,
                            //    Status = x.Status,
                            //    Unit = x.Unit,
                            //    Value = x.Value
                            //}).ToList(),
                            Steps = GetClonedSteps(contractNumber, userId, ai, sil)
                        };
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

        private static List<Step> GetClonedSteps(string contractNumber, string userId, Action ai, List<ObjectId> sil)
        {
            try
            {
                List<Step> steps = new List<Step>();
                //Parallel.ForEach(ai.Steps, b =>
                foreach (Step b in ai.Steps)
                {
                    if (b.Status == Status.Active)
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

        private static List<MEPatientProgramResponse> GetStepResponses(ObjectId stepId, string contractNumber, string userId)
        {
            List<MEPatientProgramResponse> responseList = null;
            try
            {
                GetPatientProgramsDataRequest request = new GetPatientProgramsDataRequest { ContractNumber = contractNumber, Context = "NG", UserId = userId };
                IProgramRepository repo =
                    new ProgramRepositoryFactory().GetRepository(request, RepositoryType.PatientProgramResponse);

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
                throw new Exception("DD:DTOUtils:GetStepResponses()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetStepResponseListResponse GetStepResponses(string stepId, string contractNumber, bool? service, string userId)
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

        public static void RecurseAndReplaceIds(List<Module> mods, Dictionary<ObjectId, ObjectId> IdsList)
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
                throw new Exception("DD:DTOUtils:ScanAndReplaceIdReferences()::" + ex.Message, ex.InnerException);
            }
        }

        private static void ReplaceNextAndPreviousIds(KeyValuePair<ObjectId, ObjectId> kv, PlanElement md)
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
                throw new Exception("DD:DTOUtils:ReplaceSpawnIdReferences()::" + ex.Message, ex.InnerException);
            }
        }

        private static void ReplaceSelectedResponseId(KeyValuePair<ObjectId, ObjectId> kv, Step s)
        {
            try
            {
                if (s.SelectedResponseId != null)
                {
                    if (s.SelectedResponseId.ToString().Equals("52d241a31e601521285e97e8") && kv.Key.ToString().Equals("52d241a31e601521285e97e8"))
                    {
                        string test = "test";
                    }

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

        private static ObjectId RegisterIds(Dictionary<ObjectId, ObjectId> list, ObjectId id)
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
                throw new Exception("DD:DTOUtils:GetSpawnElements()::" + ex.Message, ex.InnerException);
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
                throw new Exception("DD:DTOUtils:GetSpawnElements()::" + ex.Message, ex.InnerException);
            }
        }

        public static List<Action> GetActionElements(List<Program.DTO.ActionsDetail> list, string userId)
        {
            try
            {
                List<Action> acts = null;
                if (list != null)
                {
                    acts = new List<Action>();

                    list.ForEach(a => acts.Add(
                        new Action
                        {
                            Id = ObjectId.Parse(a.Id),
                            ModuleId = ObjectId.Parse(a.ModuleId),
                            Steps = GetStepsInfo(a.Steps, userId),
                            AssignedBy = ParseObjectId(a.AssignBy),
                            AssignedOn = a.AssignDate,
                            AssignedTo = ParseObjectId(a.AssignTo),
                            Completed = a.Completed,
                            CompletedBy = a.CompletedBy,
                            DateCompleted = a.DateCompleted,
                            Description = a.Description,
                            State = (ElementState) a.ElementState,
                            StateUpdatedOn = a.StateUpdatedOn, // NIGHT-952
                            Enabled = a.Enabled,
                            Name = a.Name,
                            Next = ParseObjectId(a.Next),
                            Objectives = GetObjectives(a.Objectives),
                            Order = a.Order,
                            Previous = ParseObjectId(a.Previous),
                            SourceId = ObjectId.Parse(a.SourceId),
                            Spawn = GetSpawnElements(a.SpawnElement),
                            Status = (Status) a.Status,
                            Archived = a.Archived,
                            ArchivedDate = a.ArchivedDate,
                            ArchiveOriginId = a.ArchiveOriginId
                        }));
                }
                return acts;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetActionElements()::" + ex.Message, ex.InnerException);
            }
        }

        private static List<Step> GetStepsInfo(List<Program.DTO.StepsDetail> list, string userId)
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

        public static ObjectId? ParseObjectId(string p)
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

        private static List<MEPatientProgramResponse> GetResponses(List<Program.DTO.ResponseDetail> list, string userId)
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
                throw new Exception("DD:DTOUtils:GetSpawnElement()::" + ex.Message, ex.InnerException);
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
                throw new Exception("DD:DTOUtils:GetSpawnElement()::" + ex.Message, ex.InnerException);
            }
        }

        public static List<Objective> GetObjectives(List<Program.DTO.ObjectiveInfoData> list)
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

        public static List<Module> CloneAppDomainModules(List<ModuleDetail> prg, string userId)
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
                            AssignedTo = ParseObjectId(m.AssignTo),
                            StateUpdatedOn = m.StateUpdatedOn,
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


        public static List<ModuleDetail> GetModules(List<Module> list, string contractNumber, string userId)
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
                    Next = r.Next != null ? r.Next.ToString() : string.Empty,
                    Previous = r.Previous != null ? r.Previous.ToString() : string.Empty,
                    Order = r.Order,
                    SpawnElement = GetSpawnElement(r),
                    SourceId = r.SourceId.ToString(),
                    AssignBy = r.AssignedBy.ToString(),
                    AssignDate = r.AssignedOn,
                    ElementState = (int)r.State,
                    CompletedBy = r.CompletedBy,
                    DateCompleted = r.DateCompleted,
                    Objectives = GetObjectives(r.Objectives),
                    Actions = GetActions(r.Actions, contractNumber, userId)
                }));
                return mods;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetModules()::" + ex.Message, ex.InnerException);
            }
        }

        public static List<ObjectiveInfoData> GetObjectives(List<Objective> list)
        {
            try
            {
                List<ObjectiveInfoData> objs = new List<ObjectiveInfoData>();
                if (list != null)
                {
                    list.ForEach(o => objs.Add(new ObjectiveInfoData
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

        public static List<ActionsDetail> GetActions(List<Action> list, string contract, string userId)
        {
            try
            {
                List<ActionsDetail> acts = new List<ActionsDetail>();
                list.ForEach(a => acts.Add(GetAction(contract, userId, a)));
                return acts;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DTOUtils:GetActions()::" + ex.Message, ex.InnerException);
            }
        }

        public static ActionsDetail GetAction(string contract, string userId, Action a)
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
                        ModuleId = a.ModuleId.ToString(),
                        Name = a.Name,
                        Status = (int)a.Status,
                        Completed = a.Completed,
                        Enabled = a.Enabled,
                        Next = a.Next != null ? a.Next.ToString() : string.Empty,
                        Previous = a.Previous != null ? a.Previous.ToString() : string.Empty,
                        Order = a.Order,
                        SpawnElement = GetSpawnElement(a),
                        SourceId = a.SourceId.ToString(),
                        AssignBy = a.AssignedBy.ToString(),
                        AssignDate = a.AssignedOn,
                        ElementState = (int)a.State,
                        DateCompleted = a.DateCompleted,
                        Objectives = GetObjectives(a.Objectives),
                        Steps = GetSteps(a.Steps, contract, userId),
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

        public static List<StepsDetail> GetSteps(List<Step> list, string contract, string userId)
        {
            try
            {
                List<StepsDetail> steps = new List<StepsDetail>();
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
                throw new Exception("DD:DTOUtils:GetSpawnElement()::" + ex.Message, ex.InnerException);
            }
        }

        public static List<ResponseDetail> GetResponses(Step step, string contract, string userId)
        {
            try
            {
                List<MEPatientProgramResponse> meresp = step.Responses;
                List<ResponseDetail> resp = null;

                if (meresp == null || meresp.Count == 0)
                {
                    meresp = GetStepResponses(step.Id, contract, userId);
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
                throw new Exception("DD:DTOUtils:GetResponses()::" + ex.Message, ex.InnerException);
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
                throw new ArgumentException("DD:DTOUtils:GetResponseSpawnElement()::" + ex.Message, ex.InnerException);
            }
        }

        public static void RecurseAndSaveResponseObjects(MEPatientProgram prog, string contractNumber, string userId)
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

        //public static bool SavePatientProgramResponses(List<MEPatientProgramResponse> pprs, PutProgramToPatientRequest request)
        //{
        //    try
        //    {
        //        IProgramRepository repo =
        //            new ProgramRepositoryFactory().GetRepository(request, RepositoryType.PatientProgramResponse);//.GetPatientProgramStepResponseRepository(request);

        //        bool result = (bool)repo.InsertAsBatch(pprs);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException("DD:DTOUtils:SavePatientProgramResponses()::" + ex.Message, ex.InnerException);
        //    }
        //}

        public static List<MEPatientProgramResponse> ExtractMEPatientProgramResponses(MEPatientProgram prog, string contractNumber, string userId)
        {
            try
            {
                List<MEPatientProgramResponse> ppr = new List<MEPatientProgramResponse>();
                foreach (Module m in prog.Modules)
                {
                    if(m.Actions != null)
                    {
                        foreach (Action a in m.Actions)
                        {
                            if(a.Steps != null)
                            {
                                foreach (Step s in a.Steps)
                                {
                                    if(s.Responses != null)
                                    {
                                        foreach (MEPatientProgramResponse r in s.Responses)
                                        {
                                            r.DeleteFlag = false;
                                            r.RecordCreatedBy = ObjectId.Parse(userId);
                                            r.RecordCreatedOn = DateTime.UtcNow;

                                            if (!ppr.Contains(r))
                                                ppr.Add(r);
                                        }
                                        // Remove responses from PatientProgram collection.
                                        s.Responses = null;
                                    }
                                }
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

        private static bool SaveResponseToDocument(MEPatientProgramResponse r, string contractNumber, string userId)
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

        //internal static ProgramAttributeData InitializeElementAttributes(ProgramInfo p)
        //{
        //    try
        //    {
        //        ProgramAttributeData pa = new ProgramAttributeData();
        //        pa.PlanElementId = p.Id;
        //        pa.Status = p.Status;
        //        //pa.AttrStartDate = System.DateTime.UtcNow;
        //        //pa.AttrEndDate = null;
        //        pa.Eligibility = 3;
        //        pa.Enrollment = 2;
        //        pa.GraduatedFlag = 1;
        //        pa.OptOut = false;
        //        //pa.EligibilityOverride = 1;
        //        return pa;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ArgumentException("DD:DTOUtils:InitializeElementAttributes()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //internal static void InitializeProgramAttributes(PutProgramToPatientRequest request, PutProgramToPatientResponse response)
        //{
        //    try
        //    {
        //        // create program attribute insertion
        //        ProgramAttributeData attr = DTOUtils.InitializeElementAttributes(response.program);

        //        IProgramRepository attrRepo = new ProgramRepositoryFactory().GetRepository(request  , RepositoryType.PatientProgramAttribute);//.GetProgramAttributesRepository(request);

        //        attrRepo.Insert(attr);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DTOUtils:InitializeProgramAttributes()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //internal static bool CanInsertPatientProgram(List<MEPatientProgram> pp)
        //{
        //    try
        //    {
        //        bool result = true;
        //        if (pp == null)
        //        {
        //            result = true;
        //        }
        //        else if (pp.Count >= 1)
        //        {
        //            foreach (MEPatientProgram p in pp)
        //            {
        //                if (!p.State.Equals(ElementState.Removed) && !p.State.Equals(ElementState.Completed))
        //                {
        //                    result = false;
        //                    break;
        //                }
        //            }
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:PatientProgramRepository:CanInsertPatientProgram()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //internal static List<MEPatientProgram> FindExistingpatientProgram(PutProgramToPatientRequest request)
        //{
        //    try
        //    {
        //        IProgramRepository pRepo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.PatientProgram);//.GetPatientProgramRepository(request);

        //        List<MEPatientProgram> pp = pRepo.FindByEntityExistsID(request.PatientId, request.ContractProgramId) as List<MEPatientProgram>;
        //        return pp;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DataProgramManager:FindExistingpatientProgram()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //internal static MEProgram GetProgramForDeepCopy(PutProgramToPatientRequest request)
        //{
        //    try
        //    {
        //        IProgramRepository pgRepo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.Program);//.GetProgramRepository(request);
        //        MEProgram cp = (MEProgram)pgRepo.FindByID(request.ContractProgramId.ToString());
        //        return cp;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DataProgramManager:GetProgram()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //internal static ProgramInfo SaveNewPatientProgram(PutProgramToPatientRequest request, MEPatientProgram nmePP)
        //{
        //    try
        //    {
        //        // give this the formatted patient program ready for saving
        //        IProgramRepository patProgRepo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.PatientProgram);//.GetPatientProgramRepository(request);

        //        ProgramInfo pi = (ProgramInfo)patProgRepo.Insert((object)nmePP);
        //        return pi;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DataProgramManager:SaveNewPatientProgram()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //internal static List<MEPatientProgramResponse> InitializePatientProgramAssignment(PutProgramToPatientRequest request, MEPatientProgram nmePP)
        //{
        //    try
        //    {
        //        List<MEPatientProgramResponse> pprs = new List<MEPatientProgramResponse>();
        //        // update to new ids and their references
        //        Dictionary<ObjectId, ObjectId> IdsList = new Dictionary<ObjectId, ObjectId>();
        //        DTOUtils.RecurseAndReplaceIds(nmePP.Modules, IdsList);
        //        pprs = DTOUtils.RecurseAndStoreResponseObjects(nmePP, request.ContractNumber, request.UserId);
        //        //DTOUtils.RecurseAndSaveResponseObjects(nmePP, request.ContractNumber, request.UserId);
        //        return pprs;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DataProgramManager:InitializePatientProgramAssignment()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //internal static List<MEResponse> GetProgramResponseslist(List<ObjectId> idl, MEProgram cp, PutProgramToPatientRequest request)
        //{
        //    try
        //    {
        //        List<MEResponse> list = null;
        //        IProgramRepository respRepo = new ProgramRepositoryFactory().GetRepository(request, RepositoryType.Response);//.GetStepResponseRepository(request);

        //        list = respRepo.Find(idl) as List<MEResponse>;
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DataProgramManager:GetProgramResponseslist()::" + ex.Message, ex.InnerException);
        //    }
        //}

        internal static void HydrateResponsesInProgram(MEProgram prog, List<MEResponse> responseList, string usrId)
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

        internal static List<ObjectId> GetStepIds(MEPatientProgram mepp)
        {
            throw new NotImplementedException();
        }
    }
}
