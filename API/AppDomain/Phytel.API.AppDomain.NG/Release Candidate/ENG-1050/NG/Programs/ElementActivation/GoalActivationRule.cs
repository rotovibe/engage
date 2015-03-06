using System;
using System.Collections.Generic;
using AutoMapper;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Goal;
using Phytel.API.AppDomain.NG.DTO.Scheduling;
using Phytel.API.AppDomain.NG.PlanCOR;
using Phytel.API.DataDomain.PatientGoal.DTO;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.AppDomain.NG.Programs.ElementActivation
{
    public class GoalActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 301;
        private const int _alertType = 300;
        public int ElementType{ get { return _elementType; } }
        public IEndpointUtils EndpointUtil { get; set; }
        public IGoalsEndpointUtils GoalsEndpointUtil { get; set; }
        public IPlanElementUtils PlanUtils { get; set; }

        public GoalActivationRule()
        {
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override SpawnType Execute(string userId, PlanElementEventArg arg, SpawnElement pe, ProgramAttributeData pad)
        {
            try
            {
                Goal goalTemp = null;
                PatientGoal patientGoal = null;
                PatientGoal newPGoal = null;
                ToDoData todo = null;

                try
                {
                    // get template Goal from Goal endpoint
                    goalTemp = EndpointUtil.GetGoalById(pe.ElementId, userId, arg.DomainRequest);
                }
                catch(Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }

                try
                {
                    // get patient Goal from template id
                    // this will only return patientgoals that are open or notmet state
                    patientGoal = EndpointUtil.GetOpenNotMetPatientGoalByTemplateId(pe.ElementId, arg.PatientId, userId, arg.DomainRequest);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }

                try
                {
                    //Open = 1, Met = 2, NotMet =3, Abandoned =4
                    if (patientGoal == null) // || (patientGoal.StatusId == 2 || patientGoal.StatusId == 4))
                    {
                        newPGoal = Mapper.Map<PatientGoal>(goalTemp);
                        newPGoal.ProgramIds = new List<string> {arg.Program.Id};
                        newPGoal.PatientId = arg.PatientId;
                        newPGoal.TemplateId = goalTemp.Id;
                        newPGoal.StartDate = PlanUtils.HandleDueDate(goalTemp.StartDateRange);
                        newPGoal.TargetDate = PlanUtils.HandleDueDate(goalTemp.TargetDateRange);
                        newPGoal.StatusId = 1;

                        try
                        {
                            // initialize patientgoal and get id
                            var iPG = GoalsEndpointUtil.GetInitialGoalRequest(new GetInitializeGoalRequest
                            {
                                Context = "NG",
                                ContractNumber = arg.DomainRequest.ContractNumber,
                                PatientId = arg.PatientId,
                                Token = arg.DomainRequest.Token,
                                UserId = arg.DomainRequest.UserId,
                                Version = arg.DomainRequest.Version
                            });

                            // update patientgoal
                            if (iPG == null)
                                throw new ArgumentException("Failed to Initialize patient goal");

                            newPGoal.Id = iPG.Id;

                            GoalsEndpointUtil.PostUpdateGoalRequest(new PostPatientGoalRequest
                            {
                                ContractNumber = arg.DomainRequest.ContractNumber,
                                Goal = newPGoal,
                                PatientGoalId = iPG.Id,
                                PatientId = arg.PatientId,
                                Token = arg.DomainRequest.Token,
                                UserId = arg.DomainRequest.UserId,
                                Version = arg.DomainRequest.Version
                            });
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, ex.InnerException);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("PatientGoal Hydration Error." + ex.Message);
                }


                var spawnType = new SpawnType {Type = _alertType.ToString(), Tag = new List<object>{newPGoal}};
                return spawnType;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:ToDoActivationRule:Execute()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
