using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.DataDomain.Program.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG
{
    public static class NGUtils
    {

        public static bool IsDateValid(string p)
        {
            DateTime date;
            bool result = false;
            if (DateTime.TryParse(p, out date))
            {
                result = true;
            }

            return result;
        }

        public static List<GetPatientResponse> PopulateCohortPatientStubData()
        {
            List<GetPatientResponse> pts = new List<GetPatientResponse>();
            pts.Add(new GetPatientResponse
            {
                Patient = new Patient
                {
                    DOB = "09/12/1993",
                    FirstName = "Oscar",
                    Gender = "M",
                    LastName = "DeLahoya",
                    MiddleName = "Denni",
                    PreferredName = "Punchy",
                    Suffix = "Mr."
                }
            });

            pts.Add(new GetPatientResponse
            {
                Patient = new Patient
                {
                    DOB = "02/16/1980",
                    FirstName = "Jenny",
                    Gender = "F",
                    LastName = "Greer",
                    MiddleName = "Irene",
                    PreferredName = "Jen",
                    Suffix = "Mr."
                }
            });

            return pts;
        }

        public static ProgramDetail FormatProgramDetail(DTO.Program program)
        {
            try
            {
                DTO.Program p = program;
                ProgramDetail pD = null;

                pD = new ProgramDetail
                {
                    AssignBy = p.AssignBy,
                    AssignDate = p.AssignDate,
                    Client = p.Client,
                    Completed = p.Completed,
                    CompletedBy = p.CompletedBy,
                    ContractProgramId = p.ContractProgramId,
                    DateCompleted = p.DateCompleted,
                    Description = p.Description,
                    ElementState = p.ElementState,
                    EligibilityEndDate = p.EligibilityEndDate,
                    EligibilityRequirements = p.EligibilityRequirements,
                    EligibilityStartDate = p.EligibilityStartDate,
                    Enabled = p.Enabled,
                    EndDate = p.EndDate,
                    Id = p.Id,
                    Modules = GetADModules(p.Modules),
                    Name = p.Name,
                    Next = p.Next,
                    ObjectivesInfo = GetADObjectives(p.ObjectivesInfo),
                    Order = p.Order,
                    PatientId = p.PatientId,
                    Previous = p.Previous,
                    ProgramState = p.ProgramState,
                    ShortName = p.ShortName,
                    SourceId = p.SourceId,
                    SpawnElement = GetADSpawnElements(p.SpawnElement),
                    StartDate = p.StartDate,
                    Status = p.Status,
                    Text = p.Text,
                    Version = p.Version
                };

                return pD;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:FormatProgramDetail():" + ex.Message, ex.InnerException);
            }
        }

        public static DTO.Program FormatProgramDetail(ProgramDetail program)
        {
            try
            {
                ProgramDetail p = program;
                DTO.Program pD = null;

                pD = new DTO.Program
                {
                    AssignBy = p.AssignBy,
                    AssignDate = p.AssignDate,
                    Client = p.Client,
                    Completed = p.Completed,
                    CompletedBy = p.CompletedBy,
                    ContractProgramId = p.ContractProgramId,
                    DateCompleted = p.DateCompleted,
                    Description = p.Description,
                    ElementState = p.ElementState,
                    EligibilityEndDate = p.EligibilityEndDate,
                    EligibilityRequirements = p.EligibilityRequirements,
                    EligibilityStartDate = p.EligibilityStartDate,
                    Enabled = p.Enabled,
                    EndDate = p.EndDate,
                    Id = p.Id,
                    Modules = GetADModules(p.Modules),
                    Name = p.Name,
                    Next = p.Next,
                    ObjectivesInfo = GetADObjectives(p.ObjectivesInfo),
                    Order = p.Order,
                    PatientId = p.PatientId,
                    Previous = p.Previous,
                    ProgramState = p.ProgramState,
                    ShortName = p.ShortName,
                    SourceId = p.SourceId,
                    SpawnElement = GetADSpawnElements(p.SpawnElement),
                    StartDate = p.StartDate,
                    Status = p.Status,
                    Text = p.Text,
                    Version = p.Version,
                    DisEnrollReason = p.DisEnrollReason,
                    DidNotEnrollReason = p.DidNotEnrollReason,
                    EligibilityOverride = p.EligibilityOverride,
                    Enrollment = p.Enrollment,
                    GraduatedFlag = p.GraduatedFlag,
                    IneligibleReason = p.IneligibleReason,
                    OptOut = p.OptOut,
                    OptOutDate = p.OptOutDate,
                    OptOutReason = p.OptOutReason,
                    OverrideReason = p.OverrideReason,
                    RemovedReason = p.RemovedReason
                };

                return pD;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:FormatProgramDetail():" + ex.Message, ex.InnerException);
            }
        }

        public static List<SpawnElementDetail> GetADSpawnElements(List<SpawnElement> list)
        {
            try
            {
                List<SpawnElementDetail> sd = new List<SpawnElementDetail>();
                if (list != null)
                {
                    list.ForEach(s =>
                    {
                        SpawnElementDetail sed = new SpawnElementDetail
                        {
                            ElementId = s.ElementId,
                            ElementType = s.ElementType,
                            Tag = s.Tag
                        };
                        sd.Add(sed);
                    });
                }
                return sd;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADSpawnElements():" + ex.Message, ex.InnerException);
            }
        }

        public static List<SpawnElement> GetADSpawnElements(List<SpawnElementDetail> list)
        {
            try
            {
                List<SpawnElement> sd = new List<SpawnElement>();
                if (list != null)
                {
                    list.ForEach(s =>
                    {
                        SpawnElement sed = new SpawnElement
                        {
                            ElementId = s.ElementId,
                            ElementType = s.ElementType,
                            Tag = s.Tag
                        };
                        sd.Add(sed);
                    });
                }
                return sd;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADSpawnElements():" + ex.Message, ex.InnerException);
            }
        }

        public static List<ObjectivesDetail> GetADObjectives(List<Objective> list)
        {
            try
            {
                List<ObjectivesDetail> od = new List<ObjectivesDetail>();
                if (list != null)
                {
                    list.ForEach(o =>
                    {
                        ObjectivesDetail odi = new ObjectivesDetail
                        {
                            Id = o.Id,
                            Status = o.Status,
                            Unit = o.Unit,
                            Value = o.Value
                        };
                        od.Add(odi);
                    });
                }
                return od;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADObjectives():" + ex.Message, ex.InnerException);
            }
        }

        public static List<Objective> GetADObjectives(List<ObjectivesDetail> list)
        {
            try
            {
                List<Objective> od = new List<Objective>();
                if (list != null)
                {
                    list.ForEach(o =>
                    {
                        Objective odi = new Objective
                        {
                            Id = o.Id,
                            Status = o.Status,
                            Unit = o.Unit,
                            Value = o.Value
                        };
                        od.Add(odi);
                    });
                }
                return od;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADObjectives():" + ex.Message, ex.InnerException);
            }
        }

        public static List<ModuleDetail> GetADModules(List<Module> list)
        {
            try
            {
                List<ModuleDetail> md = new List<ModuleDetail>();
                if (list != null)
                {
                    list.ForEach(m =>
                    {
                        ModuleDetail mdi = new ModuleDetail
                        {
                            Actions = GetADActions(m.Actions),
                            AssignBy = m.AssignBy,
                            AssignDate = m.AssignDate,
                            Completed = m.Completed,
                            CompletedBy = m.CompletedBy,
                            DateCompleted = m.DateCompleted,
                            Description = m.Description,
                            ElementState = m.ElementState,
                            Enabled = m.Enabled,
                            Id = m.Id,
                            Name = m.Name,
                            Next = m.Next,
                            Objectives = GetADObjectives(m.Objectives),
                            Order = m.Order,
                            Previous = m.Previous,
                            ProgramId = m.ProgramId,
                            SourceId = m.SourceId,
                            SpawnElement = GetADSpawnElements(m.SpawnElement),
                            Status = m.Status,
                            Text = m.Text
                        };
                        md.Add(mdi);
                    });
                }
                return md;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADModules():" + ex.Message, ex.InnerException);
            }
        }

        public static List<Module> GetADModules(List<ModuleDetail> list)
        {
            try
            {
                List<Module> md = new List<Module>();
                if (list != null)
                {
                    list.ForEach(m =>
                    {
                        Module mdi = new Module
                        {
                            Actions = GetADActions(m.Actions),
                            AssignBy = m.AssignBy,
                            AssignDate = m.AssignDate,
                            Completed = m.Completed,
                            CompletedBy = m.CompletedBy,
                            DateCompleted = m.DateCompleted,
                            Description = m.Description,
                            ElementState = m.ElementState,
                            Enabled = m.Enabled,
                            Id = m.Id,
                            Name = m.Name,
                            Next = m.Next,
                            Objectives = GetADObjectives(m.Objectives),
                            Order = m.Order,
                            Previous = m.Previous,
                            ProgramId = m.ProgramId,
                            SourceId = m.SourceId,
                            SpawnElement = GetADSpawnElements(m.SpawnElement),
                            Status = m.Status,
                            Text = m.Text
                        };
                        md.Add(mdi);
                    });
                }
                return md;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADModules():" + ex.Message, ex.InnerException);
            }
        }

        public static List<ActionsDetail> GetADActions(List<Actions> list)
        {
            try
            {
                List<ActionsDetail> ad = new List<ActionsDetail>();
                if (list != null)
                {
                    list.ForEach(a =>
                    {
                        ActionsDetail adi = new ActionsDetail
                        {
                            AssignBy = a.AssignBy,
                            AssignDate = a.AssignDate,
                            Completed = a.Completed,
                            CompletedBy = a.CompletedBy,
                            DateCompleted = a.DateCompleted,
                            Description = a.Description,
                            ElementState = a.ElementState,
                            Enabled = a.Enabled,
                            Id = a.Id,
                            ModuleId = a.ModuleId,
                            Name = a.Name,
                            Next = a.Next,
                            Objectives = GetADObjectives(a.Objectives),
                            Order = a.Order,
                            Previous = a.Previous,
                            SourceId = a.SourceId,
                            SpawnElement = GetADSpawnElements(a.SpawnElement),
                            Status = a.Status,
                            Steps = GetADSteps(a.Steps),
                            Text = a.Text
                        };
                        ad.Add(adi);
                    });
                }
                return ad;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADActions():" + ex.Message, ex.InnerException);
            }
        }

        public static List<Actions> GetADActions(List<ActionsDetail> list)
        {
            try
            {
                List<Actions> ad = new List<Actions>();
                if (list != null)
                {
                    list.ForEach(a =>
                    {
                        Actions adi = new Actions
                        {
                            AssignBy = a.AssignBy,
                            AssignDate = a.AssignDate,
                            Completed = a.Completed,
                            CompletedBy = a.CompletedBy,
                            DateCompleted = a.DateCompleted,
                            Description = a.Description,
                            ElementState = a.ElementState,
                            Enabled = a.Enabled,
                            Id = a.Id,
                            ModuleId = a.ModuleId,
                            Name = a.Name,
                            Next = a.Next,
                            Objectives = GetADObjectives(a.Objectives),
                            Order = a.Order,
                            Previous = a.Previous,
                            SourceId = a.SourceId,
                            SpawnElement = GetADSpawnElements(a.SpawnElement),
                            Status = a.Status,
                            Steps = GetADSteps(a.Steps),
                            Text = a.Text
                        };
                        ad.Add(adi);
                    });
                }
                return ad;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADActions():" + ex.Message, ex.InnerException);
            }
        }


        /// <summary>
        /// Converts Step collection into StepsDetail collection.
        /// </summary>
        /// <param name="list">Step list</param>
        /// <returns>StepsDetail list</returns>
        public static List<StepsDetail> GetADSteps(List<Step> list)
        {
            try
            {
                List<StepsDetail> sd = new List<StepsDetail>();
                if (list != null)
                {
                    list.ForEach(s =>
                    {
                        StepsDetail sdi = new StepsDetail
                        {
                            ActionId = s.ActionId,
                            AssignBy = s.AssignBy,
                            AssignDate = s.AssignDate,
                            Completed = s.Completed,
                            CompletedBy = s.CompletedBy,
                            ControlType = s.ControlType,
                            DateCompleted = s.DateCompleted,
                            Description = s.Description,
                            ElementState = s.ElementState,
                            Enabled = s.Enabled,
                            Ex = s.Ex,
                            Header = s.Header,
                            Id = s.Id,
                            IncludeTime = s.IncludeTime,
                            Next = s.Next,
                            Notes = s.Notes,
                            Order = s.Order,
                            Previous = s.Previous,
                            Question = s.Question,
                            //Responses = GetADResponses(s.Responses),
                            SelectedResponseId = s.SelectedResponseId,
                            SelectType = s.SelectType,
                            SourceId = s.SourceId,
                            SpawnElement = GetADSpawnElements(s.SpawnElement),
                            Status = s.Status,
                            StepTypeId = s.StepTypeId,
                            Text = s.Text,
                            Title = s.Title
                        };
                        sd.Add(sdi);
                    });
                }
                return sd;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADSteps():" + ex.Message, ex.InnerException);
            }
        }

        public static List<Step> GetADSteps(List<StepsDetail> list)
        {
            try
            {
                List<Step> sd = new List<Step>();
                if (list != null)
                {
                    list.ForEach(s =>
                    {
                        Step sdi = new Step
                        {
                            ActionId = s.ActionId,
                            AssignBy = s.AssignBy,
                            AssignDate = s.AssignDate,
                            Completed = s.Completed,
                            CompletedBy = s.CompletedBy,
                            ControlType = s.ControlType,
                            DateCompleted = s.DateCompleted,
                            Description = s.Description,
                            ElementState = s.ElementState,
                            Enabled = s.Enabled,
                            Ex = s.Ex,
                            Header = s.Header,
                            Id = s.Id,
                            IncludeTime = s.IncludeTime,
                            Next = s.Next,
                            Notes = s.Notes,
                            Order = s.Order,
                            Previous = s.Previous,
                            Question = s.Question,
                            Responses = GetADResponses(s.Responses),
                            SelectedResponseId = s.SelectedResponseId,
                            SelectType = s.SelectType,
                            SourceId = s.SourceId,
                            SpawnElement = GetADSpawnElements(s.SpawnElement),
                            Status = s.Status,
                            StepTypeId = s.StepTypeId,
                            Text = s.Text,
                            Title = s.Title
                        };
                        sd.Add(sdi);
                    });
                }
                return sd;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADSteps():" + ex.Message, ex.InnerException);
            }
        }

        public static List<ResponseDetail> GetADResponses(List<Response> list)
        {
            try
            {
                List<ResponseDetail> rd = new List<ResponseDetail>();
                if (list != null)
                {
                    list.ForEach(r =>
                    {
                        ResponseDetail rdi = new ResponseDetail
                        {
                            Id = r.Id,
                            NextStepId = r.NextStepId,
                            Nominal = r.Nominal,
                            Order = r.Order,
                            Required = r.Required,
                            StepId = r.StepId,
                            Text = r.Text,
                            Value = r.Value,
                            SpawnElement = GetDDSpawnElement(r.SpawnElement)
                        };
                        rd.Add(rdi);
                    });
                }
                return rd;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADResponses():" + ex.Message, ex.InnerException);
            }
        }

        public static List<SpawnElementDetail> GetDDSpawnElement(List<SpawnElement> s)
        {
            try
            {
                List<SpawnElementDetail> sed = new List<SpawnElementDetail>();
                if (s != null)
                {
                    s.ForEach(x =>
                    {
                        sed.Add(new SpawnElementDetail
                        {
                            ElementType = x.ElementType,
                            ElementId = x.ElementId,
                            Tag = x.Tag
                        });
                    });
                }
                return sed;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetDDSpawnElement():" + ex.Message, ex.InnerException);
            }
        }

        public static List<Response> GetADResponses(List<ResponseDetail> list)
        {
            try
            {
                List<Response> rd = new List<Response>();
                if (list != null)
                {
                    list.ForEach(r =>
                    {
                        Response rdi = new Response
                        {
                            Id = r.Id,
                            NextStepId = r.NextStepId,
                            Nominal = r.Nominal,
                            Order = r.Order,
                            Required = r.Required,
                            StepId = r.StepId,
                            Text = r.Text,
                            Value = r.Value,
                            SpawnElement = GetADSpawnElement(r.SpawnElement)
                        };
                        rd.Add(rdi);
                    });
                }
                return rd;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADResponses():" + ex.Message, ex.InnerException);
            }
        }

        public static List<SpawnElement> GetADSpawnElement(List<SpawnElementDetail> s)
        {
            try
            {
                List<SpawnElement> se = new List<SpawnElement>();
                if (s != null)
                {
                    s.ForEach(x =>{
                        se.Add(new SpawnElement
                        {
                            ElementId = x.ElementId,
                            ElementType = x.ElementType,
                            Tag = x.Tag
                        });
                    });
                }
                return se;
            }
            catch (Exception ex)
            {
                throw new Exception("AppDomain:GetADResponses():" + ex.Message, ex.InnerException);
            }
        }

        public static void UpdateProgramAction(Actions ac, DTO.Program p)
        {
            DTO.Program pr = p;
            pr.Modules.ForEach(m =>
            {
                foreach (Actions a in m.Actions)
                {
                    bool replaced = ReplaceAction(ac, m, a);
                    if (replaced)
                        break;
                }
                //m.Actions.ForEach(a =>
                //{
                //    bool replaced = ReplaceAction(ac, m, a);
                //    if (replaced)
                //        return;
                //});
            });
        }

        public static bool ReplaceAction(Actions ac, Module m, Actions a)
        {
            bool replaced = false;
            if (a.Id.Equals(ac.Id))
            {
                var i = m.Actions.FindIndex(x => x == a);
                m.Actions[i] = ac;
                replaced = true;
            }
            return replaced;
        }
    }
}
