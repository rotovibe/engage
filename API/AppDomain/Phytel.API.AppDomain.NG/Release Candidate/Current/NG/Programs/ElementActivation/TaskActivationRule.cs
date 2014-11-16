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
    public class TaskActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 501;
        private const int _alertType = 500;
        public int ElementType{ get { return _elementType; } }
        public IEndpointUtils EndpointUtil { get; set; }
        public IGoalsEndpointUtils GoalsEndpointUtil { get; set; }
        public IPlanElementUtils PlanUtils { get; set; }

        public TaskActivationRule()
        {
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override SpawnType Execute(string userId, PlanElementEventArg arg, SpawnElement pe, ProgramAttributeData pad)
        {
            try
            {
                #region

                //Goal goalTemp = null;
                //Intervention interventionTemp = null;
                //PatientIntervention patientInt = null;
                //PatientIntervention newPInt = null;
                //ToDoData todo = null;

                //try
                //{
                //    // get patientintervention template by id
                //    interventionTemp = EndpointUtil.GetGoalById(pe.ElementId, userId, arg.DomainRequest);
                //}
                //catch(Exception ex)
                //{
                //    throw new ArgumentException(ex.Message);
                //}

                //try
                //{
                //    // get template Goal from Goal endpoint
                //    patientGoal = EndpointUtil.GetOpenNotMetPatientGoalByTemplateId(pe.ElementId, arg.PatientId, userId, arg.DomainRequest);
                //}
                //catch (Exception ex)
                //{
                //    throw new ArgumentException(ex.Message);
                //}

                //try
                //{
                //    //Open = 1, Met = 2, NotMet =3, Abandoned =4
                //    if (patientGoal == null || (patientGoal.StatusId == 2 || patientGoal.StatusId == 4))
                //    {
                //        newPGoal = Mapper.Map<PatientGoal>(goalTemp);
                //        newPGoal.ProgramIds = new List<string> {arg.Program.Id};
                //        newPGoal.PatientId = arg.PatientId;
                //        newPGoal.StatusId = 1;

                //        try
                //        {
                //            // register new patientobservation
                //            // initialize patientgoal and get id
                //            var iPG = GoalsEndpointUtil.GetInitialGoalRequest(new GetInitializeGoalRequest
                //            {
                //                Context = "NG",
                //                ContractNumber = arg.DomainRequest.ContractNumber,
                //                PatientId = arg.PatientId,
                //                Token = arg.DomainRequest.Token,
                //                UserId = arg.DomainRequest.UserId,
                //                Version = arg.DomainRequest.Version
                //            });

                //            // update patientgoal
                //            if (iPG == null)
                //                throw new ArgumentException("Failed to Initialize patient goal");

                //            newPGoal.Id = iPG.Id;

                //            GoalsEndpointUtil.PostUpdateGoalRequest(new PostPatientGoalRequest
                //            {
                //                ContractNumber = arg.DomainRequest.ContractNumber,
                //                Goal = newPGoal,
                //                PatientGoalId = iPG.Id,
                //                PatientId = arg.PatientId,
                //                Token = arg.DomainRequest.Token,
                //                UserId = arg.DomainRequest.UserId,
                //                Version = arg.DomainRequest.Version
                //            });
                //        }
                //        catch (Exception ex)
                //        {
                //            throw new Exception(ex.Message, ex.InnerException);
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw new ArgumentException("PatientGoal Hydration Error." + ex.Message);
                //}

                #endregion

                Task taskTemplate = null;
                Goal goalTemplate = null;
                PatientGoal patientGoal = null;
                PatientGoal newPGoal = null;
                PatientTask existingPatientTask = null;

                // get template intervention 
                taskTemplate = EndpointUtil.GetTaskById(pe.ElementId, userId, arg.DomainRequest);

                // get template goal 
                goalTemplate = EndpointUtil.GetGoalById(taskTemplate.TemplateGoalId, userId, arg.DomainRequest);

                // find if patientgoal exists
                patientGoal = EndpointUtil.GetOpenNotMetPatientGoalByTemplateId(goalTemplate.Id, arg.PatientId, userId, arg.DomainRequest);

                if (patientGoal != null)
                {
                    // find if patientintervention exists
                    existingPatientTask = EndpointUtil.GetOpenNotMetPatientTaskByTemplateId(patientGoal.Id,
                        taskTemplate.Id, arg.PatientId, userId, arg.DomainRequest);
                }

                PatientTask pTsk = existingPatientTask;
                PatientGoal pGoal = patientGoal;
                List<object> items = null;
                if (InsertTaskAllowed(existingPatientTask))
                {
                    // check to see that goal exists
                    if (InsertPatientGoalAllowed(patientGoal))
                    {
                        // 1) insert patient goal
                        pGoal = PlanUtils.InsertPatientGoal(arg, goalTemplate);
                    }
                    // insert patient intervention
                    pTsk = PlanUtils.InsertPatientTask(arg, pGoal, taskTemplate);
                    items = CreateItemsBag(pTsk, pGoal);
                }

                var spawnType = new SpawnType {Type = _alertType.ToString(), Tag = items};
                return spawnType;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:TaskActivationRule:Execute()::" + ex.Message, ex.InnerException);
            }
        }

        private static List<object> CreateItemsBag(PatientTask pTsk, PatientGoal pGoal)
        {
            try
            {
                var items = new List<object>();
                if (pTsk != null)
                {
                    if (!items.Contains(pTsk))
                        items.Add(pTsk);
                }

                if (pGoal != null)
                {
                    if (!items.Contains(pGoal))
                        items.Add(pGoal);
                }
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:TaskActivationRule:CreateItemsBag()::" + ex.Message, ex.InnerException);
            }
        }

        private bool InsertPatientGoalAllowed(PatientGoal patientGoal)
        {
            try
            {
                if (patientGoal == null)
                {
                    return true;
                }
                //else if (patientGoal.StatusId == 2 || patientGoal.StatusId == 4)
                //{
                //    return true;
                //}

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:TaskActivationRule:InsertPatientGoalAllowed()::" + ex.Message, ex.InnerException);
            }
        }

        private bool InsertTaskAllowed(PatientTask patientTask)
        {
            try
            {
                if (patientTask == null)
                {
                    return true;
                }
                //else if (patientTask.StatusId == 2 || patientTask.StatusId == 4)
                //{
                //    return true;
                //}

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:TaskActivationRule:InsertInterventionAllowed()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
