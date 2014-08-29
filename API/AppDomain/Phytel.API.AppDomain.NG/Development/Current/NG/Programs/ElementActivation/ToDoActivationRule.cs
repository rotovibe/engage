using System;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public class ToDoActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 201;
        private const string _alertType = "ToDo";
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
            HandleToDoTemplateRegistration(arg, pe);
            return _alertType;
        }

        private void HandleToDoTemplateRegistration(PlanElementEventArg e, SpawnElement rse)
        {
            try
            {
                // get template todo from schedule endpoint
                var todoTemp = EndpointUtil.GetScheduleToDoById(rse.ElementId, e.UserId);

                var todo = new ToDoData
                {
                    AssignedToId = e.UserId,
                    CreatedById = e.UserId,
                    Title = todoTemp.Title,
                    CategoryId = todoTemp.CategoryId,
                    StatusId = todoTemp.StatusId,
                    Description = todoTemp.Description,
                    PriorityId = todoTemp.PriorityId
                };

                // register new todo
                var result = EndpointUtil.PutInsertToDo(todo, e.UserId);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:StepPlanProcessor:HandlePatientProblemRegistration()::" + ex.Message,
                    ex.InnerException);
            }
        }
    }
}
