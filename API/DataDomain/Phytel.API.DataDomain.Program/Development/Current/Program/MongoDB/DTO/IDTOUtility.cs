﻿using System;
using MongoDB.Bson;
using Phytel.API.DataDomain.Program.DTO;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Program.MongoDB.DTO
{
    public interface IDTOUtility
    {
        bool CanInsertPatientProgram(List<MEPatientProgram> pp);
        List<Module> CloneAppDomainModules(List<ModuleDetail> prg, string userId);
        MEPatientProgram CreateInitialMEPatientProgram(PutProgramToPatientRequest request, MEProgram cp, List<ObjectId> sil);
        List<MEPatientProgram> FindExistingpatientProgram(PutProgramToPatientRequest request);
        ActionsDetail GetAction(string contract, string userId, Action a);
        List<Action> GetActionElements(List<ActionsDetail> list, string userId);
        List<ActionsDetail> GetActions(List<Action> list, string contract, string userId);
        List<Module> GetClonedModules(List<Module> list, string contractNumber, string userId, List<ObjectId> sil);
        List<ModuleDetail> GetModules(List<Module> list, string contractNumber, string userId);
        List<Objective> GetObjectives(System.Collections.Generic.List<ObjectiveInfoData> list);
        List<ObjectiveInfoData> GetObjectives(List<Objective> list);
        MEProgram GetProgramForDeepCopy(PutProgramToPatientRequest request);
        List<MEResponse> GetProgramResponseslist(List<ObjectId> idl, MEProgram cp, PutProgramToPatientRequest request);
        List<ResponseDetail> GetResponses(Step step, string contract, string userId);
        List<SpawnElementDetail> GetResponseSpawnElement(List<SpawnElement> mESpawnElement);
        List<SpawnElementDetail> GetSpawnElement(PlanElement a);
        List<SpawnElement> GetSpawnElements(List<SpawnElementDetail> list);
        List<SpawnElementDetail> GetSpawnElements(List<SpawnElement> list);
        List<ObjectId> GetStepIds(MEPatientProgram mepp);
        GetStepResponseListResponse GetStepResponses(string stepId, string contractNumber, bool? service, string userId);
        List<MEPatientProgramResponse> GetStepResponses(ObjectId stepId, string contractNumber, string userId);
        List<StepsDetail> GetSteps(List<Step> list, string contract, string userId);
        void HydrateResponsesInProgram(MEProgram prog, List<MEResponse> responseList, string usrId);
        ProgramAttributeData InitializeElementAttributes(ProgramInfo p);
        List<MEPatientProgramResponse> InitializePatientProgramAssignment(PutProgramToPatientRequest request, MEPatientProgram nmePP);
        void InitializeProgramAttributes(PutProgramToPatientRequest request, PutProgramToPatientResponse response);
        ObjectId? ParseObjectId(string p);
        void RecurseAndReplaceIds(List<Module> mods, Dictionary<ObjectId, ObjectId> IdsList);
        void RecurseAndSaveResponseObjects(MEPatientProgram prog, string contractNumber, string userId);
        List<MEPatientProgramResponse> RecurseAndStoreResponseObjects(MEPatientProgram prog, string contractNumber, string userId);
        ProgramInfo SaveNewPatientProgram(PutProgramToPatientRequest request, MEPatientProgram nmePP);
        bool SavePatientProgramResponses(List<MEPatientProgramResponse> pprs, PutProgramToPatientRequest request);
    }
}
