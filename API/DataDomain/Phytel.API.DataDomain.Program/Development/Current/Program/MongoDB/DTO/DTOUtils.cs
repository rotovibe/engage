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
        public static List<Modules> SetValidModules(List<Modules> list)
        {
            try
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
                            AssignBy = m.AssignBy,
                            AssignDate = m.AssignDate,
                            ElementState = m.ElementState,
                            CompletedBy = m.CompletedBy,
                            DateCompleted = m.DateCompleted,
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
                                    AssignBy = ai.AssignBy,
                                    AssignDate = ai.AssignDate,
                                    ElementState = ai.ElementState,
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
                                        AssignBy = b.AssignBy,
                                        AssignDate = b.AssignDate,
                                        ElementState = b.ElementState
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

        public static void RecurseAndReplaceIds(List<Modules> mods)
        {
            Dictionary<ObjectId, ObjectId> IdsList = new Dictionary<ObjectId, ObjectId>();
            try
            {
                foreach (Modules md in mods)
                {
                    md.Id = RegisterIds(IdsList, md.Id);
                    if (md.Actions != null)
                    {
                        foreach (ActionsInfo a in md.Actions)
                        {
                            a.Id = RegisterIds(IdsList, a.Id);
                            a.ModuleId = md.Id;
                            if (a.Steps != null)
                            {
                                foreach (StepsInfo s in a.Steps)
                                {
                                    s.Id = RegisterIds(IdsList, s.Id);
                                    s.ActionId = a.Id;
                                    if (s.Responses != null)
                                    {
                                        foreach (ResponseInfo r in s.Responses)
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

        private static void ScanAndReplaceIdReferences(Dictionary<ObjectId, ObjectId> IdsList, List<Modules> mods)
        {
            try
            {
                foreach (KeyValuePair<ObjectId, ObjectId> kv in IdsList)
                {
                    foreach (Modules md in mods)
                    {
                        ReplaceNextAndPreviousIds(kv, md);
                        ReplaceSpawnIdReferences(kv, md);
                        if (md.Actions != null)
                        {
                            foreach (ActionsInfo a in md.Actions)
                            {
                                ReplaceNextAndPreviousIds(kv, a);
                                ReplaceSpawnIdReferences(kv, a);
                                if (a.Steps != null)
                                {
                                    foreach (StepsInfo s in a.Steps)
                                    {
                                        ReplaceNextAndPreviousIds(kv, s);
                                        ReplaceSpawnIdReferences(kv, s);
                                        ReplaceSelectedResponseId(kv, s);
                                        if (s.Responses != null)
                                        {
                                            foreach (ResponseInfo r in s.Responses)
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

                                                if (r.SpawnElement != null)
                                                {
                                                    if (r.SpawnElement.SpawnId != null)
                                                    {
                                                        if (r.SpawnElement.SpawnId.Equals(kv.Key))
                                                        {
                                                            r.SpawnElement.SpawnId = kv.Value;
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

        private static void ReplaceSelectedResponseId(KeyValuePair<ObjectId, ObjectId> kv, StepsInfo s)
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
                            Type = s.ElementType
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

        internal static List<ActionsInfo> GetActionElements(List<Program.DTO.ActionsDetail> list)
        {
            try
            {
                List<ActionsInfo> acts = null;
                if (list != null)
                {
                    acts = new List<ActionsInfo>();

                    list.ForEach(a =>
                    {
                        acts.Add(
                        new ActionsInfo
                        {
                            Id = ObjectId.Parse(a.Id),
                            ModuleId = ObjectId.Parse(a.ModuleId),
                            Steps = GetStepsInfo(a.Steps),
                            AssignBy = a.AssignBy,
                            AssignDate = a.AssignDate,
                            Completed = a.Completed,
                            CompletedBy = a.CompletedBy,
                            DateCompleted = a.DateCompleted,
                            Description = a.Description,
                            ElementState = (ElementState)a.ElementState,
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

        private static List<StepsInfo> GetStepsInfo(List<Program.DTO.StepsDetail> list)
        {
            try
            {
                List<StepsInfo> steps = null;
                if (list != null)
                {
                    steps = new List<StepsInfo>();

                    list.ForEach(st =>
                    {
                        steps.Add(
                        new StepsInfo
                        {
                            Id = ObjectId.Parse(st.Id),
                            ActionId = ObjectId.Parse(st.ActionId),
                            AssignBy = st.AssignBy,
                            AssignDate = st.AssignDate,
                            Completed = st.Completed,
                            CompletedBy = st.CompletedBy,
                            ControlType = st.ControlType,
                            DateCompleted = st.DateCompleted,
                            Description = st.Description,
                            ElementState = (ElementState)st.ElementState,
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

        private static List<ResponseInfo> GetResponses(List<Program.DTO.ResponseDetail> list)
        {
            try
            {
                List<ResponseInfo> rs = null;
                if (list != null)
                {
                    rs = new List<ResponseInfo>();

                    list.ForEach(r =>
                        {
                            rs.Add(
                                new ResponseInfo
                                {
                                    StepId = ObjectId.Parse(r.StepId),
                                    NextStepId = ObjectId.Parse(r.NextStepId),
                                    Id = ObjectId.Parse(r.Id),
                                    Nominal = r.Nominal,
                                    Order = r.Order,
                                    Required = r.Required,
                                    Text = r.Text,
                                    Value = r.Value,
                                    SpawnElement = GetSPawnElement(r.SpawnElement)
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

        private static MESpawnElement GetSPawnElement(SpawnElementDetail s)
        {
            try
            {
                MESpawnElement sp = null;
                if (s != null)
                {
                    sp = new MESpawnElement
                    {
                        SpawnId = ObjectId.Parse(s.ElementId),
                        Type = s.ElementType
                    };
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
                        ElementId = s.SpawnId.ToString()
                    };
                }
                return sp;
            }
            catch (Exception ex)
            {
                throw new Exception("DataDomain:GetSpawnElement():" + ex.Message, ex.InnerException);
            }
        }

        public static List<ObjectivesInfo> GetObjectives(List<Program.DTO.ObjectivesDetail> list)
        {
            try
            {
                List<ObjectivesInfo> objs = null;
                if (list != null)
                {
                    objs = new List<ObjectivesInfo>();

                    list.ForEach(o =>
                    {
                        objs.Add(new ObjectivesInfo
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

        public static List<Modules> CloneAppDomainModules(List<ModuleDetail> prg)
        {
            try
            {
                List<Modules> mods = null;
                if (prg != null)
                {
                    mods = new List<Modules>();
                    prg.ForEach(m =>
                    {
                        mods.Add(
                        new Modules
                        {
                            Id = ObjectId.Parse(m.Id),
                            DateCompleted = m.DateCompleted,
                            Next = m.Next,
                            Previous = m.Previous,
                            Spawn = DTOUtils.GetSpawnElements(m.SpawnElement),
                            Actions = DTOUtils.GetActionElements(m.Actions),
                            AssignBy = m.AssignBy,
                            AssignDate = m.AssignDate,
                            Completed = m.Completed,
                            CompletedBy = m.CompletedBy,
                            Description = m.Description,
                            ElementState = (ElementState)m.ElementState,
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
    }
}
