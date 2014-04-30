using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Program.Test.Stubs
{
    public class StubDTOUtility : IDTOUtility
    {
        public IProgramRepositoryFactory Factory { get; set; }

        public bool CanInsertPatientProgram(List<MEPatientProgram> pp)
        {
            throw new NotImplementedException();
        }

        public List<Module> CloneAppDomainModules(List<DTO.ModuleDetail> prg, string userId)
        {
            throw new NotImplementedException();
        }

        public MEPatientProgram CreateInitialMEPatientProgram(DTO.PutProgramToPatientRequest request, MEProgram cp, List<global::MongoDB.Bson.ObjectId> sil)
        {
            throw new NotImplementedException();
        }

        public List<MEPatientProgram> FindExistingpatientProgram(DTO.PutProgramToPatientRequest request)
        {
            throw new NotImplementedException();
        }

        public DTO.ActionsDetail GetAction(string contract, string userId, MongoDB.DTO.Action a)
        {
            throw new NotImplementedException();
        }

        public List<MongoDB.DTO.Action> GetActionElements(List<DTO.ActionsDetail> list, string userId)
        {
            throw new NotImplementedException();
        }

        public List<DTO.ActionsDetail> GetActions(List<MongoDB.DTO.Action> list, string contract, string userId)
        {
            throw new NotImplementedException();
        }

        public List<Module> GetClonedModules(List<Module> list, string contractNumber, string userId, List<global::MongoDB.Bson.ObjectId> sil)
        {
            throw new NotImplementedException();
        }

        public List<DTO.ModuleDetail> GetModules(List<Module> list, string contractNumber, string userId)
        {
            List<DTO.ModuleDetail> modules = new List<DTO.ModuleDetail>();
            modules.Add(
                    new DTO.ModuleDetail
                    {
                        Id = "000000000000000000000000",
                        Name = "Test stub module 1",
                        Description = "BSHSI - Outreach & Enrollment",
                        SourceId = "532b5585a381168abe00042c",
                        Actions = new List<DTO.ActionsDetail>(){ 
                            new DTO.ActionsDetail{ Id = "000000000000000000000000", ElementState = 4, Name ="test action from stub", Text = "test action 1"} }
                    });
            return modules;
        }

        public List<Objective> GetObjectives(List<DTO.ObjectiveInfoData> list)
        {
            throw new NotImplementedException();
        }

        public List<DTO.ObjectiveInfoData> GetObjectives(List<Objective> list)
        {
            throw new NotImplementedException();
        }

        public MEProgram GetProgramForDeepCopy(DTO.PutProgramToPatientRequest request)
        {
            throw new NotImplementedException();
        }

        public List<MEResponse> GetProgramResponseslist(List<global::MongoDB.Bson.ObjectId> idl, MEProgram cp, DTO.PutProgramToPatientRequest request)
        {
            throw new NotImplementedException();
        }

        public List<DTO.ResponseDetail> GetResponses(Step step, string contract, string userId)
        {
            throw new NotImplementedException();
        }

        public List<DTO.SpawnElementDetail> GetResponseSpawnElement(List<SpawnElement> mESpawnElement)
        {
            throw new NotImplementedException();
        }

        public List<DTO.SpawnElementDetail> GetSpawnElement(PlanElement a)
        {
            return new List<DTO.SpawnElementDetail>();
        }

        public List<SpawnElement> GetSpawnElements(List<DTO.SpawnElementDetail> list)
        {
            throw new NotImplementedException();
        }

        public List<DTO.SpawnElementDetail> GetSpawnElements(List<SpawnElement> list)
        {
            throw new NotImplementedException();
        }

        public List<global::MongoDB.Bson.ObjectId> GetStepIds(MEPatientProgram mepp)
        {
            throw new NotImplementedException();
        }

        public DTO.GetStepResponseListResponse GetStepResponses(string stepId, string contractNumber, bool? service, string userId)
        {
            throw new NotImplementedException();
        }

        public List<MEPatientProgramResponse> GetStepResponses(global::MongoDB.Bson.ObjectId stepId, string contractNumber, string userId)
        {
            throw new NotImplementedException();
        }

        public List<DTO.StepsDetail> GetSteps(List<Step> list, string contract, string userId)
        {
            throw new NotImplementedException();
        }

        public void HydrateResponsesInProgram(MEProgram prog, List<MEResponse> responseList, string usrId)
        {
            throw new NotImplementedException();
        }

        public DTO.ProgramAttributeData InitializeElementAttributes(DTO.ProgramInfo p)
        {
            throw new NotImplementedException();
        }

        public List<MEPatientProgramResponse> InitializePatientProgramAssignment(DTO.PutProgramToPatientRequest request, MEPatientProgram nmePP)
        {
            throw new NotImplementedException();
        }

        public void InitializeProgramAttributes(DTO.PutProgramToPatientRequest request, DTO.PutProgramToPatientResponse response)
        {
            throw new NotImplementedException();
        }

        public global::MongoDB.Bson.ObjectId? ParseObjectId(string p)
        {
            throw new NotImplementedException();
        }

        public void RecurseAndReplaceIds(List<Module> mods, Dictionary<global::MongoDB.Bson.ObjectId, global::MongoDB.Bson.ObjectId> IdsList)
        {
            throw new NotImplementedException();
        }

        public void RecurseAndSaveResponseObjects(MEPatientProgram prog, string contractNumber, string userId)
        {
            throw new NotImplementedException();
        }

        public List<MEPatientProgramResponse> RecurseAndStoreResponseObjects(MEPatientProgram prog, string contractNumber, string userId)
        {
            throw new NotImplementedException();
        }

        public DTO.ProgramInfo SaveNewPatientProgram(DTO.PutProgramToPatientRequest request, MEPatientProgram nmePP)
        {
            throw new NotImplementedException();
        }

        public bool SavePatientProgramResponses(List<MEPatientProgramResponse> pprs, DTO.PutProgramToPatientRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
