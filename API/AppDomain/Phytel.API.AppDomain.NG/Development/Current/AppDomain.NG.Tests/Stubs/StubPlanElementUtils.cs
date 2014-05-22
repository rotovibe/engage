using Phytel.API.AppDomain.NG.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.AppDomain.NG.Test.Stubs
{
    public class StubPlanElementUtils : IPlanElementUtils
    {
        public event ProcessedElementInUtilEventHandlers _processedElementEvent;

        public DTO.PlanElement ActivatePlanElement(string p, DTO.Program program)
        {
            throw new NotImplementedException();
        }

        public DTO.Program CloneProgram(DTO.Program pr)
        {
            throw new NotImplementedException();
        }

        public void DisableCompleteButtonForAction(List<DTO.Step> list)
        {
            throw new NotImplementedException();
        }

        public T FindElementById<T>(List<T> list, string id)
        {
            return list[0];
        }

        public void FindIdInActions(Program program, string p, DTO.Module m)
        {
            throw new NotImplementedException();
        }

        public void FindIdInSteps(Program program, string p, DTO.Actions a)
        {
            throw new NotImplementedException();
        }

        public Phytel.API.DataDomain.Patient.DTO.CohortPatientViewData GetCohortPatientViewRecord(string patientId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.Actions GetProcessingAction(List<DTO.Module> list, string actionId)
        {
            throw new NotImplementedException();
        }

        public void HydratePlanElementLists(List<object> ProcessedElements, DTO.PostProcessActionResponse response)
        {
            throw new NotImplementedException();
        }

        public DTO.PlanElement InitializePlanElementSettings(DTO.PlanElement pe, DTO.PlanElement p)
        {
            throw new NotImplementedException();
        }

        public bool IsActionInitial(DTO.Program p)
        {
            return true;
        }

        public bool IsProgramCompleted(DTO.Program p, string userId)
        {
            throw new NotImplementedException();
        }

        public bool ModifyProgramAttributePropertiesForUpdate(Phytel.API.DataDomain.Program.DTO.ProgramAttributeData pAtt, Phytel.API.DataDomain.Program.DTO.ProgramAttributeData _pAtt)
        {
            throw new NotImplementedException();
        }

        public void OnProcessIdEvent(object pe)
        {
            throw new NotImplementedException();
        }

        public void RegisterCohortPatientViewProblemToPatient(string problemId, string patientId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public bool ResponseSpawnAllowed(DTO.Step s, DTO.Response r)
        {
            throw new NotImplementedException();
        }

        public void SaveReportingAttributes(Phytel.API.DataDomain.Program.DTO.ProgramAttributeData _programAttributes, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public bool SetCompletionStatus<T>(List<T> list)
        {
            throw new NotImplementedException();
        }

        public void SetElementEnabledState(string p, DTO.Program program)
        {
            throw new NotImplementedException();
        }

        public void SetEnabledState<T>(List<T> list, T x)
        {
            throw new NotImplementedException();
        }

        public void SetEnabledStatusByPrevious<T>(List<T> actions)
        {
            throw new NotImplementedException();
        }

        public void SetInitialProperties(Program program, DTO.IPlanElement m)
        {
            throw new NotImplementedException();
        }

        public void SetProgramAttributes(DTO.SpawnElement r, DTO.Program program, string userId, Phytel.API.DataDomain.Program.DTO.ProgramAttributeData progAttr)
        {
            throw new NotImplementedException();
        }

        public void SetProgramInformation(Phytel.API.DataDomain.Program.DTO.ProgramAttributeData _programAttributes, DTO.Program p)
        {
            throw new NotImplementedException();
        }

        public void SetStartDateForProgramAttributes(string programId, Interface.IAppDomainRequest request)
        {
            throw new NotImplementedException();
        }

        public void SpawnElementsInList(List<DTO.SpawnElement> list, DTO.Program program, string userId, Phytel.API.DataDomain.Program.DTO.ProgramAttributeData progAttr)
        {
            throw new NotImplementedException();
        }


        public DTO.ProgramAttribute GetAttributes(DataDomain.Program.DTO.ProgramAttributeData programAttributeData)
        {
            throw new NotImplementedException();
        }



        public Module CloneModule(Module md)
        {
            throw new NotImplementedException();
        }


        public void SetEnabledState<T>(List<T> list, T x, string assignToId)
        {
            throw new NotImplementedException();
        }

        public void SetEnabledStatusByPrevious<T>(List<T> actions, string assignToId)
        {
            throw new NotImplementedException();
        }


        public PlanElement InitializePlanElementSettings(PlanElement pe, PlanElement p, Program program)
        {
            throw new NotImplementedException();
        }
    }
}
