using System;
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

        public override object Execute(string userId, PlanElementEventArg arg, SpawnElement pe, ProgramAttributeData pad)
        {
            try
            {
                Schedule todoTemp = null;
                ToDoData todo = null;

                try
                {
                    // get template todo from schedule endpoint
                    todoTemp = EndpointUtil.GetScheduleToDoById(pe.ElementId, userId);
                }
                catch(Exception ex)
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
                        AssignedToId = userId,
                        CreatedById = userId,
                        SourceId = todoTemp.Id,
                        Title = todoTemp.Title,
                        CategoryId = todoTemp.CategoryId,
                        StatusId = todoTemp.StatusId,
                        Description = todoTemp.Description,
                        PriorityId = todoTemp.PriorityId,
                        DueDate = HandleDueDate(todoTemp.DueDateRange),
                        PatientId = patientId,
                        ProgramIds = prog,
                        CreatedOn = DateTime.UtcNow
                    };
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("ToDoData Hydration Error." + ex.Message);
                }

                try
                {
                    // register new todo
                    var result = EndpointUtil.PutInsertToDo(todo, arg.UserId);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }

                return _alertType;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ToDoActivationRule:Execute()::" + ex.Message, ex.InnerException);
            }
        }

        public DateTime? HandleDueDate(int? days)
        {
            try
            {
                DateTime? dueDate = null;
                if (days == null) return dueDate;

                if (days > 0)
                {
                    dueDate = DateTime.UtcNow.AddDays(days.Value);
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
