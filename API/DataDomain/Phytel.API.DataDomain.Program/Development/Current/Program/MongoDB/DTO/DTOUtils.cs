using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.DataDomain.Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    public static class DTOUtils
    {
        public static List<MEModules> SetValidModules(List<MEModules> list)
        {
            try
            {
                List<MEStep> steps = new List<MEStep>();
                List<MEAction> acts = new List<MEAction>();
                List<MEModules> mods = new List<MEModules>();

                foreach (MEModules m in list)
                {
                    if (m.Status == Common.Status.Active)
                    {
                        MEModules mod = new MEModules()
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
                            State = m.State,
                            CompletedBy = m.CompletedBy,
                            DateCompleted = m.DateCompleted,
                            //Objectives = m.Objectives.Where(a => a.Status == Common.Status.Active).Select(z => new ObjectivesInfo()
                            //{
                            //    Id = z.Id,
                            //    Status = z.Status,
                            //    Unit = z.Unit,
                            //    Value = z.Value
                            //}).ToList(),
                            Actions = new List<MEAction>()
                        };

                        foreach (MEAction ai in m.Actions)
                        {
                            if (ai.Status == Common.Status.Active)
                            {
                                MEAction ac = new MEAction()
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
                                    State = ai.State,
                                    //Objectives = ai.Objectives.Where(r => r.Status == Common.Status.Active).Select(x => new ObjectivesInfo()
                                    //{
                                    //    Id = x.Id,
                                    //    Status = x.Status,
                                    //    Unit = x.Unit,
                                    //    Value = x.Value
                                    //}).ToList(),
                                    Steps = ai.Steps.Where(s => s.Status == Common.Status.Active).Select(b => new MEStep()
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
                                        Responses = b.Responses,
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

        public static void RecurseAndReplaceIds(List<MEModules> mods)
        {
            Dictionary<ObjectId, ObjectId> IdsList = new Dictionary<ObjectId, ObjectId>();
            try
            {
                foreach (MEModules md in mods)
                {
                    md.Id = RegisterIds(IdsList, md.Id);
                    if (md.Actions != null)
                    {
                        foreach (MEAction a in md.Actions)
                        {
                            a.Id = RegisterIds(IdsList, a.Id);
                            a.ModuleId = md.Id;
                            if (a.Steps != null)
                            {
                                foreach (MEStep s in a.Steps)
                                {
                                    s.Id = RegisterIds(IdsList, s.Id);
                                    s.ActionId = a.Id;
                                    if (s.Responses != null)
                                    {
                                        foreach (MEResponse r in s.Responses)
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

        private static void ScanAndReplaceIdReferences(Dictionary<ObjectId, ObjectId> IdsList, List<MEModules> mods)
        {
            try
            {
                foreach (KeyValuePair<ObjectId, ObjectId> kv in IdsList)
                {
                    foreach (MEModules md in mods)
                    {
                        ReplaceNextAndPreviousIds(kv, md);
                        ReplaceSpawnIdReferences(kv, md);
                        if (md.Actions != null)
                        {
                            foreach (MEAction a in md.Actions)
                            {
                                ReplaceNextAndPreviousIds(kv, a);
                                ReplaceSpawnIdReferences(kv, a);
                                if (a.Steps != null)
                                {
                                    foreach (MEStep s in a.Steps)
                                    {
                                        ReplaceNextAndPreviousIds(kv, s);
                                        ReplaceSpawnIdReferences(kv, s);
                                        ReplaceSelectedResponseId(kv, s);
                                        if (s.Responses != null)
                                        {
                                            foreach (MEResponse r in s.Responses)
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
                                                    foreach (MESpawnElement sp in r.Spawn)
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

        private static void ReplaceNextAndPreviousIds(KeyValuePair<ObjectId, ObjectId> kv, MEPlanElement md)
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

        private static void ReplaceSpawnIdReferences(KeyValuePair<ObjectId, ObjectId> kv, MEPlanElement pln)
        {
            try
            {
                if (pln.Spawn != null)
                {
                    foreach (MESpawnElement sp in pln.Spawn)
                    {
                        if (sp.SpawnId.Equals(kv.Key))
                        {
                            sp.SpawnId = kv.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void ReplaceSelectedResponseId(KeyValuePair<ObjectId, ObjectId> kv, MEStep s)
        {
            try
            {
                if (s.SelectedResponseId != null)
                {
                    if (s.SelectedResponseId.Equals(kv.Key.ToString()))
                    {
                        s.SelectedResponseId = kv.Value.ToString();
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

        internal static List<MESpawnElement> GetSpawnElements(List<Program.DTO.SpawnElementDetail> list)
        {
            try
            {
                List<MESpawnElement> spawnList = null;
                if (list != null)
                {
                    spawnList = new List<MESpawnElement>();

                    list.ForEach(s =>
                    {
                        spawnList.Add(
                        new MESpawnElement
                        {
                            SpawnId = ObjectId.Parse(s.ElementId),
                            Type = s.ElementType,
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

        internal static List<MEAction> GetActionElements(List<Program.DTO.ActionsDetail> list)
        {
            try
            {
                List<MEAction> acts = null;
                if (list != null)
                {
                    acts = new List<MEAction>();

                    list.ForEach(a =>
                    {
                        acts.Add(
                        new MEAction
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
                            SourceId = a.SourceId,
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

        private static List<MEStep> GetStepsInfo(List<Program.DTO.StepsDetail> list)
        {
            try
            {
                List<MEStep> steps = null;
                if (list != null)
                {
                    steps = new List<MEStep>();

                    list.ForEach(st =>
                    {
                        steps.Add(
                        new MEStep
                        {
                            Id = ObjectId.Parse(st.Id),
                            ActionId = ObjectId.Parse(st.ActionId),
                            AssignedBy = st.AssignBy,
                            AssignedOn = st.AssignDate,
                            Completed = st.Completed,
                            CompletedBy = st.CompletedBy,
                            ControlType = st.ControlType,
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
                            SelectedResponseId = st.SelectedResponseId,
                            SelectType = st.SelectType,
                            SourceId = st.SourceId,
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

        private static List<MEResponse> GetResponses(List<Program.DTO.ResponseDetail> list)
        {
            try
            {
                List<MEResponse> rs = null;
                if (list != null)
                {
                    rs = new List<MEResponse>();

                    list.ForEach(r =>
                        {
                            rs.Add(
                                new MEResponse
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

        private static List<MESpawnElement> GetSPawnElement(List<SpawnElementDetail> s)
        {
            try
            {
                List<MESpawnElement> sp = new List<MESpawnElement>();
                if (s != null)
                {
                    s.ForEach(sed =>
                    {
                        sp.Add(new MESpawnElement
                        {
                            SpawnId =  sed.ElementId != null ? ObjectId.Parse(sed.ElementId) : ObjectId.Parse("000000000000000000000000"),
                            Type = sed.ElementType,
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

        private static SpawnElementDetail GetSPawnElement(MESpawnElement s)
        {
            try
            {
                SpawnElementDetail sp = null;
                if (s != null)
                {
                    sp = new SpawnElementDetail
                    {
                        ElementType = s.Type,
                        ElementId = s.SpawnId.ToString(),
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

        public static List<MEObjective> GetObjectives(List<Program.DTO.ObjectivesDetail> list)
        {
            try
            {
                List<MEObjective> objs = null;
                if (list != null)
                {
                    objs = new List<MEObjective>();

                    list.ForEach(o =>
                    {
                        objs.Add(new MEObjective
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

        public static List<MEModules> CloneAppDomainModules(List<ModuleDetail> prg)
        {
            try
            {
                List<MEModules> mods = null;
                if (prg != null)
                {
                    mods = new List<MEModules>();
                    prg.ForEach(m =>
                    {
                        mods.Add(
                        new MEModules
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
                            SourceId = m.SourceId,
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


        public static List<ModuleDetail> GetModules(List<MEModules> list)
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
                    SourceId = r.SourceId,
                    AssignBy = r.AssignedBy,
                    AssignDate = r.AssignedOn,
                    ElementState = (int)r.State,
                    CompletedBy = r.CompletedBy,
                    DateCompleted = r.DateCompleted,
                    Objectives = GetObjectives(r.Objectives),
                    Actions = GetActions(r.Actions)
                }));
                return mods;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetModules():" + ex.Message, ex.InnerException);
            }
        }

        public static List<ObjectivesDetail> GetObjectives(List<MEObjective> list)
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

        public static List<ActionsDetail> GetActions(List<MEAction> list)
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
                    SourceId = a.SourceId,
                    AssignBy = a.AssignedBy,
                    AssignDate = a.AssignedOn,
                    ElementState = (int)a.State,
                    DateCompleted = a.DateCompleted,
                    Objectives = GetObjectives(a.Objectives),
                    Steps = GetSteps(a.Steps)
                }));
                return acts;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetActions():" + ex.Message, ex.InnerException);
            }
        }

        public static List<StepsDetail> GetSteps(List<MEStep> list)
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
                        SourceId = s.SourceId,
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
                        IncludeTime = s.IncludeTime,
                        SelectType = s.SelectType,
                        AssignBy = s.AssignedBy,
                        AssignDate = s.AssignedOn,
                        ElementState = (int)s.State,
                        CompletedBy = s.CompletedBy,
                        DateCompleted = s.DateCompleted,
                        Responses = GetResponses(s),
                        SpawnElement = GetSpawnElement(s)
                    }));
                return steps;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetSteps():" + ex.Message, ex.InnerException);
            }
        }

        public static List<SpawnElementDetail> GetSpawnElement(MEPlanElement a)
        {
            try
            {
                List<SpawnElementDetail> spawn = new List<SpawnElementDetail>();

                if (a.Spawn != null)
                {
                    spawn = a.Spawn.Select(s => new SpawnElementDetail
                    {
                        ElementId = s.SpawnId.ToString(),
                        ElementType = s.Type,
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

        public static List<ResponseDetail> GetResponses(MEStep step)
        {
            try
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
                        StepId = x.StepId.ToString(),
                        Text = x.Text,
                        Value = x.Value,
                        SpawnElement = GetResponseSpawnElement(x.Spawn)
                    }).ToList<ResponseDetail>();
                }
                return resp;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetResponses():" + ex.Message, ex.InnerException);
            }
        }

        public static List<SpawnElementDetail> GetResponseSpawnElement(List<MESpawnElement> mESpawnElement)
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
                            ElementId = se.SpawnId.ToString(),
                            ElementType = se.Type,
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
    }
}
