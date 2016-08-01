using MongoDB.Bson;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Program.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Program.Test.Stubs
{
    public class StubDTOUtility : IDTOUtility
    {
        public IProgramRepositoryFactory Factory { get; set; }
        public List<MEPatientProgramResponse> ResponsesBag { get; set; }

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

        public List<DTO.ActionsDetail> GetActions(List<MongoDB.DTO.Action> list, IDataDomainRequest request, Module mod)
        {
            throw new NotImplementedException();
        }

        public List<Module> GetClonedModules(ObjectId? abyId, List<Module> list, IDataDomainRequest request, List<ObjectId> sil)
        {
            throw new NotImplementedException();
        }

        public List<DTO.ModuleDetail> GetModules(List<Module> list, string contractProgId,  IDataDomainRequest request)
        {
            List<DTO.ModuleDetail> modules = new List<DTO.ModuleDetail>();

            list.ForEach(m =>
            {
                modules.Add(new DTO.ModuleDetail
                {
                    Id = m.Id.ToString(),
                    Name = m.Name,
                    Description = m.Description,
                    SourceId = m.SourceId.ToString(),
                    AttrStartDate = m.AttributeStartDate,
                    AttrEndDate = m.AttributeEndDate,
                    AssignDate = m.AssignedOn,
                    AssignTo = m.AssignedTo.ToString(),
                    AssignBy = m.AssignedBy.ToString(),
                    Objectives = this.GetObjectivesData(m.Objectives),
                     Actions = GetActions(m.Actions)
                });
            });
            return modules;
        }

        private List<DTO.ActionsDetail> GetActions(List<MongoDB.DTO.Action> list)
        {
            List<DTO.ActionsDetail> acts = new List<DTO.ActionsDetail>();
            list.ForEach(a =>
            {
                acts.Add(new DTO.ActionsDetail
                {
                    Description = a.Description,
                    AttrStartDate = a.AttributeStartDate,
                    AttrEndDate = a.AttributeEndDate,
                    AssignTo = a.AssignedTo.ToString(),
                    AssignBy = a.AssignedBy.ToString(),
                    AssignDate = a.AssignedOn,
                    Id = a.Id.ToString(),
                    SourceId = a.SourceId.ToString(),
                    Objectives = this.GetObjectivesData(a.Objectives)
                });
            });
            return acts;
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

        public void InitializePatientProgramAssignment(DTO.PutProgramToPatientRequest request, MEPatientProgram nmePP)
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


        public MEProgram GetLimitedProgramDetails(string objectId, Interface.IDataDomainRequest request)
        {
            MEProgram mep = new MEProgram(request.UserId)
            {
                AuthoredBy = "123456789012345678901234",
                TemplateName = "template stub name",
                TemplateVersion = "1.0",
                ProgramVersion = "1.0",
                ProgramVersionUpdatedOn = System.DateTime.UtcNow,
                Objectives = new List<Objective> { new Objective { Id = ObjectId.GenerateNewId(), Status = Status.Active, Units = "lbs", Value = "134" } }
            };
            return mep;
        }

        public List<DTO.ObjectiveInfoData> GetObjectivesData(List<Objective> sobjs)
        {
            List<DTO.ObjectiveInfoData> objs = new List<DTO.ObjectiveInfoData>();
            sobjs.ForEach(o =>
            {
                objs.Add(new DTO.ObjectiveInfoData { Id = o.Id.ToString(), Status = (int)o.Status,  Unit = o.Units, Value = o.Value });
            });

            return objs;
        }


        public List<Module> GetTemplateModulesList(string contractProgramId, string contractNumber, string userId)
        {
            throw new NotImplementedException();
        }


        public List<Objective> GetTemplateObjectives(ObjectId sourceId, Module mod)
        {
            throw new NotImplementedException();
        }


        public string GetCareManagerValueByRule(DTO.PutProgramToPatientRequest request, MEProgram cp)
        {
            throw new NotImplementedException();
        }

    }
}
