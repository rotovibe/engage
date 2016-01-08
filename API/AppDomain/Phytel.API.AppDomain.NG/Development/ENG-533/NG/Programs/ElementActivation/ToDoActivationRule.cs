using System;
using System.Linq;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Scheduling;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public class ToDoActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 201;
        private const int _alertType = 200;
        public int ElementType{ get { return _elementType; } }
        public IEndpointUtils EndpointUtil { get; set; }
        public IPlanElementUtils PlanUtils { get; set; }

        public ToDoActivationRule()
        {
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override SpawnType Execute(string userId, PlanElementEventArg arg, SpawnElement pe, ProgramAttributeData pad)
        {
            try
            {
                Schedule todoTemp = null;
                ToDoData todo = null;

                if (!PatientToDoExists(arg, pe.ElementId))
                {
                    try
                    {
                        // get template todo from schedule endpoint
                        todoTemp = EndpointUtil.GetScheduleToDoById(pe.ElementId, userId, arg.DomainRequest);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }

                    var prog = new System.Collections.Generic.List<string>();
                    if (arg.Program != null)
                        prog.Add(arg.Program.Id);

                    string patientId = null;
                    if (arg.Program != null)
                        patientId = arg.Program.PatientId;

                    try
                    {
                        todo = new ToDoData
                        {
                            SourceId = todoTemp.Id,
                            Title = todoTemp.Title,
                            CategoryId = todoTemp.CategoryId,
                            CreatedById = userId,
                            StatusId = todoTemp.StatusId,
                            Description = todoTemp.Description,
                            PriorityId = todoTemp.PriorityId,
                            DueDate = HandleDueDate(todoTemp.DueDateRange),
                            StartTime = todoTemp.StartTime,
                            Duration = todoTemp.Duration,
                            PatientId = patientId,
                            ProgramIds = prog,
                            CreatedOn = DateTime.UtcNow
                        };

                        SetDefaultAssignment(userId, todoTemp, todo);

                        // modified for ENG-709
                        if (todo.StatusId == 2 || todo.StatusId == 4)
                        {
                            todo.ClosedDate = DateTime.UtcNow;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("ToDoData Hydration Error." + ex.Message);
                    }

                    try
                    {
                        // register new todo
                        var result = EndpointUtil.PutInsertToDo(todo, arg.UserId, arg.DomainRequest);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex.InnerException);
                    }
                }
                var spawnType = new SpawnType {Type = _alertType.ToString()};
                return spawnType;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ToDoActivationRule:Execute()::" + ex.Message, ex.InnerException);
            }
        }

        public bool PatientToDoExists(PlanElementEventArg arg, string sourceId)
        {
            try
            {
                var todos = EndpointUtil.GetPatientToDos(arg.PatientId, arg.UserId, arg.DomainRequest);
                if (todos == null) return false;
                var existing = todos.FirstOrDefault(r => r.SourceId == sourceId && (r.StatusId == 1 || r.StatusId == 3)); // open = 1, NotMet = 3 status
                if (existing == null) return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ToDoActivationRule:PatientToDoExists()::" + ex.Message, ex.InnerException);
            }
        }

        public void SetDefaultAssignment(string userId, Schedule todoTemp, ToDoData todo)
        {
            try
            {
                // eng-1069
                if (!todoTemp.DefaultAssignment) return;
                todo.AssignedToId = userId;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ToDoActivationRule:SetDefaultAssignment()::" + ex.Message, ex.InnerException);
            }
        }

        public DateTime? HandleDueDate(int? days)
        {
            try
            {
                DateTime? dueDate = null;
                if (days == null) return dueDate;

                if (days > -1)
                {
                    //var calcDate 
                    var nDt = DateTime.UtcNow.AddDays(days.Value);
                    dueDate = new DateTime(nDt.Year, nDt.Month,
                        nDt.Day, 12, 0, 0);
                    //dueDate = TimeZoneInfo.ConvertTimeToUtc(calcDate);
                }

                return dueDate;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ToDoActivationRule:HandleDueDate()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
