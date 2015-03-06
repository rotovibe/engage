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
    public class InterventionActivationRule : ElementActivationRule, IElementActivationRule
    {
        private const int _elementType = 401;
        private const int _alertType = 400;
        public int ElementType{ get { return _elementType; } }
        public IEndpointUtils EndpointUtil { get; set; }
        public IGoalsEndpointUtils GoalsEndpointUtil { get; set; }
        public IPlanElementUtils PlanUtils { get; set; }

        public InterventionActivationRule()
        {
            if (AppHostBase.Instance != null)
                AppHostBase.Instance.Container.AutoWire(this);
        }

        public override SpawnType Execute(string userId, PlanElementEventArg arg, SpawnElement pe, ProgramAttributeData pad)
        {
            try
            {
                Intervention interventionTemplate = null;
                Goal goalTemplate = null;
                PatientGoal patientGoal = null;
                PatientGoal newPGoal = null;
                PatientIntervention existingPatientIntervention = null;

                try
                {
                    // get template intervention 
                    interventionTemplate = EndpointUtil.GetInterventionById(pe.ElementId, userId, arg.DomainRequest);
                }
                catch (Exception ex)
                {
                    throw new Exception("AD:InterventionActivationRule:GetInterventionById()::" + ex.Message, ex.InnerException);
                }

                try
                {
                    // get template goal 
                    goalTemplate = EndpointUtil.GetGoalById(interventionTemplate.TemplateGoalId, userId,
                        arg.DomainRequest);
                }
                catch (Exception ex)
                {
                    throw new Exception("AD:InterventionActivationRule:GetGoalById()::" + ex.Message, ex.InnerException);
                }

                try
                {
                    // find if patientgoal exists
                    patientGoal = EndpointUtil.GetOpenNotMetPatientGoalByTemplateId(goalTemplate.Id, arg.PatientId,
                        userId, arg.DomainRequest);
                }
                catch (Exception ex)
                {
                    throw new Exception("AD:InterventionActivationRule:GetOpenNotMetPatientGoalByTemplateId()::" + ex.Message, ex.InnerException);
                }

                if (patientGoal != null)
                {
                    try
                    {
                        // find if patientintervention exists
                        existingPatientIntervention =
                            EndpointUtil.GetOpenNotMetPatientInterventionByTemplateId(patientGoal.Id,
                                interventionTemplate.Id, arg.PatientId, userId, arg.DomainRequest);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            "AD:InterventionActivationRule:GetOpenNotMetPatientInterventionByTemplateId()::" +
                            ex.Message, ex.InnerException);
                    }
                }

                PatientIntervention pIntr = existingPatientIntervention;
                PatientGoal pGoal = patientGoal;
                List<object> items = null;
                if (InsertInterventionAllowed(pIntr))
                {
                    // check to see that goal exists
                    if (InsertPatientGoalAllowed(patientGoal))
                    {
                        // 1) insert patient goal
                        pGoal = PlanUtils.InsertPatientGoal(arg, goalTemplate);
                    }

                    pIntr = PlanUtils.InsertPatientIntervention(arg, pGoal, interventionTemplate);

                    items = CreateItemsBag(pIntr, pGoal);
                }

                var spawnType = new SpawnType {Type = _alertType.ToString(), Tag = items};
                return spawnType;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:InterventionActivationRule:Execute()::" + ex.Message, ex.InnerException);
            }
        }

        private static List<object> CreateItemsBag(PatientIntervention pIntr, PatientGoal pGoal)
        {
            try
            {
                var items = new List<object>();
                if (pIntr != null)
                {
                    if (!items.Contains(pIntr))
                        items.Add(pIntr);
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
                throw new Exception("AD:InterventionActivationRule:CreateItemsBag()::" + ex.Message, ex.InnerException);
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
                
                //if (patientGoal.StatusId == 2 || patientGoal.StatusId == 4)
                //{
                //    return true;
                //}

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:InterventionActivationRule:InsertPatientGoalAllowed()::" + ex.Message, ex.InnerException);
            }
        }

        private bool InsertInterventionAllowed(PatientIntervention patientIntr)
        {
            try
            {
                if (patientIntr == null)
                {
                    return true;
                }
                
                //if (patientIntr.StatusId == 2 || patientIntr.StatusId == 4)
                //{
                //    return true;
                //}

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("AD:InterventionActivationRule:InsertInterventionAllowed()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
