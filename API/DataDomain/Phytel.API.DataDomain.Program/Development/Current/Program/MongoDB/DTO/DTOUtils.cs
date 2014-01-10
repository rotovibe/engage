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
                                    T = b.T,
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
                                    Spawn = b.Spawn
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
    }
}
