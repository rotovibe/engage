using MongoDB.Bson;
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
    }
}
