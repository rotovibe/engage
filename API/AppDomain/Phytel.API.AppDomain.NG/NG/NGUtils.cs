using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.DataDomain.Program.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact.DTO;
using ServiceStack.Common.Extensions;

namespace Phytel.API.AppDomain.NG
{
    public static partial class NGUtils
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
                    AssignBy = p.AssignById,
                    AssignDate = p.AssignDate,
                    AssignTo = p.AssignToId,
                    AttrStartDate = p.AttrStartDate,
                    AttrEndDate = p.AttrEndDate,
                    Client = p.Client,
                    Completed = p.Completed,
                    CompletedBy = p.CompletedBy,
                    ContractProgramId = p.ContractProgramId,
                    DateCompleted = p.DateCompleted,
                    Description = p.Description,
                    ElementState = p.ElementState,
                    StateUpdatedOn = p.StateUpdatedOn,
                    EligibilityEndDate = p.EligibilityEndDate,
                    EligibilityRequirements = p.EligibilityRequirements,
                    EligibilityStartDate = p.EligibilityStartDate,
                    Enabled = p.Enabled,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    AuthoredBy = p.AuthoredBy,
                    TemplateName = p.TemplateName,
                    TemplateVersion = p.TemplateVersion,
                    ProgramVersion = p.ProgramVersion,
                    ProgramVersionUpdatedOn = p.ProgramVersionUpdatedOn,
                    Id = p.Id,
                    Modules = GetADModules(p.Modules),
                    Name = p.Name,
                    Next = p.Next,
                    ObjectivesData = GetADObjectives(p.Objectives),
                    Order = p.Order,
                    PatientId = p.PatientId,
                    Previous = p.Previous,
                    ProgramState = p.ProgramState,
                    ShortName = p.ShortName,
                    SourceId = p.SourceId,
                    SpawnElement = GetADSpawnElements(p.SpawnElement),
                    Status = p.Status,
                    Text = p.Text,
                    Version = p.Version
                };

                return pD;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:FormatProgramDetail()::" + ex.Message, ex.InnerException);
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
                    AssignById = p.AssignBy,
                    AssignDate = p.AssignDate,
                    AssignToId = p.AssignTo,
                    Client = p.Client,
                    Completed = p.Completed,
                    CompletedBy = p.CompletedBy,
                    ContractProgramId = p.ContractProgramId,
                    DateCompleted = p.DateCompleted,
                    Description = p.Description,
                    ElementState = p.ElementState,
                    StateUpdatedOn = p.StateUpdatedOn,
                    EligibilityEndDate = p.EligibilityEndDate,
                    EligibilityRequirements = p.EligibilityRequirements,
                    EligibilityStartDate = p.EligibilityStartDate,
                    Enabled = p.Enabled,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    AttrEndDate = p.AttrEndDate,
                    AttrStartDate = p.AttrStartDate,
                    AuthoredBy = p.AuthoredBy,
                    TemplateName = p.TemplateName,
                    TemplateVersion = p.TemplateVersion,
                    ProgramVersion = p.ProgramVersion,
                    ProgramVersionUpdatedOn = p.ProgramVersionUpdatedOn,
                    Id = p.Id,
                    Modules = GetADModules(p.Modules),
                    Name = p.Name,
                    Next = p.Next,
                    Objectives = GetADObjectives(p.ObjectivesData),
                    Order = p.Order,
                    PatientId = p.PatientId,
                    Previous = p.Previous,
                    ProgramState = p.ProgramState,
                    ShortName = p.ShortName,
                    SourceId = p.SourceId,
                    SpawnElement = GetADSpawnElements(p.SpawnElement),
                    Status = p.Status,
                    Text = p.Text,
                    Version = p.Version,
                    Archived = p.Archived,
                    ArchivedDate = p.ArchivedDate,
                    ArchiveOriginId = p.ArchiveOriginId
                };

                return pD;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:FormatProgramDetail()::" + ex.Message, ex.InnerException);
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
                throw new Exception("AD:GetADSpawnElements()::" + ex.Message, ex.InnerException);
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
                throw new Exception("AD:GetADSpawnElements()::" + ex.Message, ex.InnerException);
            }
        }

        public static List<ObjectiveInfoData> GetADObjectives(List<ObjectiveInfo> list)
        {
            try
            {
                List<ObjectiveInfoData> od = new List<ObjectiveInfoData>();
                if (list != null)
                {
                    list.ForEach(o =>
                    {
                        ObjectiveInfoData odi = new ObjectiveInfoData
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
                throw new Exception("AD:GetADObjectives()::" + ex.Message, ex.InnerException);
            }
        }

        public static List<ObjectiveInfo> GetADObjectives(List<ObjectiveInfoData> list)
        {
            try
            {
                List<ObjectiveInfo> od = new List<ObjectiveInfo>();
                if (list != null)
                {
                    list.ForEach(o =>
                    {
                        ObjectiveInfo odi = new ObjectiveInfo
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
                throw new Exception("AD:GetADObjectives()::" + ex.Message, ex.InnerException);
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
                            AssignBy = m.AssignById,
                            AssignTo = m.AssignToId,
                            AssignDate = m.AssignDate,
                            AttrEndDate = m.AttrEndDate, // night 919
                            AttrStartDate = m.AttrStartDate, // night 919
                            Completed = m.Completed,
                            CompletedBy = m.CompletedBy,
                            DateCompleted = m.DateCompleted,
                            Description = m.Description,
                            ElementState = m.ElementState,
                            StateUpdatedOn = m.StateUpdatedOn,
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
                throw new Exception("AD:GetADModules()::" + ex.Message, ex.InnerException);
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
                            AssignById = m.AssignBy,
                            AssignDate = m.AssignDate,
                            AssignToId = m.AssignTo,
                            AttrStartDate = m.AttrEndDate,
                            AttrEndDate = m.AttrEndDate,
                            Completed = m.Completed,
                            CompletedBy = m.CompletedBy,
                            DateCompleted = m.DateCompleted,
                            Description = m.Description,
                            ElementState = m.ElementState,
                            StateUpdatedOn = m.StateUpdatedOn,
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
                            Text = m.Text,
                            Archived = m.Archived,
                            ArchivedDate = m.ArchivedDate,
                            ArchiveOriginId = m.ArchiveOriginId
                        };
                        md.Add(mdi);
                    });
                }
                return md;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetADModules()::" + ex.Message, ex.InnerException);
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
                            AssignBy = a.AssignById,
                            AssignDate = a.AssignDate,
                            AssignTo = a.AssignToId,
                            Completed = a.Completed,
                            CompletedBy = a.CompletedBy,
                            DateCompleted = a.DateCompleted,
                            Description = a.Description,
                            ElementState = a.ElementState,
                            StateUpdatedOn = a.StateUpdatedOn,
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
                            Text = a.Text,
                            Archived = a.Archived,
                            ArchivedDate = a.ArchivedDate,
                            ArchiveOriginId = a.ArchiveOriginId,
                            DeleteFlag = a.DeleteFlag
                        };
                        ad.Add(adi);
                    });
                }
                return ad;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetADActions()::" + ex.Message, ex.InnerException);
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
                            AssignById = a.AssignBy,
                            AssignToId = a.AssignTo,
                            AssignDate = a.AssignDate,
                            Completed = a.Completed,
                            CompletedBy = a.CompletedBy,
                            DateCompleted = a.DateCompleted,
                            Description = a.Description,
                            ElementState = a.ElementState,
                            StateUpdatedOn = a.StateUpdatedOn,
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
                            Text = a.Text,
                            Archived = a.Archived,
                            ArchivedDate = a.ArchivedDate,
                            ArchiveOriginId = a.ArchiveOriginId,
                            DeleteFlag = a.DeleteFlag
                        };
                        ad.Add(adi);
                    });
                }
                return ad;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetADActions()::" + ex.Message, ex.InnerException);
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
                            AssignBy = s.AssignById,
                            AssignDate = s.AssignDate,
                            Completed = s.Completed,
                            CompletedBy = s.CompletedBy,
                            ControlType = s.ControlType,
                            DateCompleted = s.DateCompleted,
                            Description = s.Description,
                            ElementState = s.ElementState,
                            StateUpdatedOn = s.StateUpdatedOn,
                            Enabled = s.Enabled,
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
                throw new Exception("AD:GetADSteps()::" + ex.Message, ex.InnerException);
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
                            AssignById = s.AssignBy,
                            AssignDate = s.AssignDate,
                            Completed = s.Completed,
                            CompletedBy = s.CompletedBy,
                            ControlType = s.ControlType,
                            DateCompleted = s.DateCompleted,
                            Description = s.Description,
                            ElementState = s.ElementState,
                            StateUpdatedOn = s.StateUpdatedOn,
                            Enabled = s.Enabled,
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
                            Title = s.Title,
                            Archived = s.Archived,
                            ArchivedDate = s.ArchivedDate,
                            ArchiveOriginId = s.ArchiveOriginId
                        };
                        sd.Add(sdi);
                    });
                }
                return sd;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:GetADSteps()::" + ex.Message, ex.InnerException);
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
                throw new Exception("AD:GetADResponses()::" + ex.Message, ex.InnerException);
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
                throw new Exception("AD:GetDDSpawnElement()::" + ex.Message, ex.InnerException);
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
                throw new Exception("AD:GetADResponses()::" + ex.Message, ex.InnerException);
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
                throw new Exception("AD:GetADResponses()::" + ex.Message, ex.InnerException);
            }
        }

        public static Actions UpdateProgramAction(Actions ac, DTO.Program p)
        {
            DTO.Program pr = p;
            DTO.Actions act = null;
            pr.Modules.ForEach(m =>
            {
                foreach (Actions a in m.Actions)
                {
                    bool replaced = ReplaceAction(ac, m, a);
                    if (replaced)
                    {
                        act = a;
                        break;
                    }
                }
            });

            return act;
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

        public static List<DTO.Goal.Option> FormatOptions(Dictionary<int, string> dictionary)
        {
            var list = dictionary.ToList();

            var mlist = list.ConvertAll(c => new DTO.Goal.Option { Display = c.Value,  Value = c.Key}).ToList();
            return mlist;
        }

        public static void SetContactPreferredName(Contact contact)
        {
            var res = string.Empty;
            if (contact == null) throw new ArgumentNullException("The contact card is null");

            var preferred = contact.PreferredName;
            var firstName = contact.FirstName;
            if (string.IsNullOrEmpty(firstName))
            {
                firstName = "";
            }
            var lastName = contact.LastName;
            if (string.IsNullOrEmpty(lastName))
            {
                lastName = "";
            }
            if (!string.IsNullOrEmpty(preferred))
            {
                res = preferred;
                if (!preferred.ToLower().Contains(lastName.ToLower()) || lastName.IsEmpty())
                {
                    res = preferred + " " + lastName;
                }

            }
            else
            {
                res = firstName + " " + lastName;
            }
            contact.PreferredName = res;
        }
        
    }
}
