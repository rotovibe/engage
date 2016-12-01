using Phytel.API.DataDomain.Program.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Program;
using Phytel.API.Interface;
using System.Collections.Generic;
using System;
using ServiceStack.ServiceInterface.ServiceModel;
using Phytel.API.DataDomain.Patient.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Configuration;
using Phytel.API.DataDomain.Program.MongoDB.DTO;
using System.Linq;
using MongoDB.Bson;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Program
{
    public class ProgramDataManager : IProgramDataManager
    {
        public IDTOUtility DTOUtility { get; set; }
        public IProgramRepositoryFactory Factory { get; set; }

        #region PUTS
        public PutProgramToPatientResponse PutPatientToProgram(PutProgramToPatientRequest request)
        {
            try
            {
                PutProgramToPatientResponse response = new PutProgramToPatientResponse();
                response.Outcome = new Phytel.API.DataDomain.Program.DTO.Outcome();

                #region validation calls
                if (!IsValidPatientId(request))
                {
                    return FormatExceptionResponse(response, "Patient does not exist or has an invalid id.", "500");
                }

                if (!IsValidContractProgramId(request))
                {
                    return FormatExceptionResponse(response, "ContractProgram does not exist or has an invalid identifier.", "500");
                }

                if (!IsContractProgramAssignable(request))
                {
                    return FormatExceptionResponse(response, "ContractProgram is not currently active.", "500");
                }
                #endregion

                /**********************************/
                List<MEPatientProgram> pp = DTOUtility.FindExistingpatientProgram(request);

                if (!DTOUtility.CanInsertPatientProgram(pp))
                {
                    response.Outcome.Result = 0;
                    response.Outcome.Reason = pp[0].Name + " is already assigned or reassignment is not allowed";
                }
                else
                {
                    MEProgram cp = DTOUtility.GetProgramForDeepCopy(request);

                    var stepIdList = new List<ObjectId>();
                    List<MEResponse> responseList = DTOUtility.GetProgramResponseslist(stepIdList, cp, request);
                    DTOUtils.HydrateResponsesInProgram(cp, responseList, request.UserId);
                    MEPatientProgram nmePp = DTOUtility.CreateInitialMEPatientProgram(request, cp, stepIdList);
                    DTOUtility.InitializePatientProgramAssignment(request, nmePp);
                    List<MEPatientProgramResponse> pprs = DTOUtils.ExtractMEPatientProgramResponses(nmePp, request.ContractNumber, request.UserId);
                    ProgramInfo pi = DTOUtility.SaveNewPatientProgram(request, nmePp);

                    if (pi != null)
                    {
                        response.program = pi;
                        DTOUtility.SavePatientProgramResponses(pprs, request);
                        DTOUtility.InitializeProgramAttributes(request, response);
                    }

                    response.Outcome.Result = 1;
                    response.Outcome.Reason = "Successfully assigned this program for the individual";
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:PutPatientToProgram()::" + ex.Message, ex.InnerException);
            }
        }

        public PutProgramActionProcessingResponse PutProgramActionUpdate(PutProgramActionProcessingRequest request)
        {
            try
            {
                PutProgramActionProcessingResponse response = new PutProgramActionProcessingResponse();

                IProgramRepository patProgRepo =
                    Factory.GetRepository(request, RepositoryType.PatientProgram);

                response.program = (ProgramDetail)patProgRepo.Update((object)request);
                response.program.Attributes = GetProgramAttributes(response.program.Id, request);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:PutProgramActionUpdate()::" + ex.Message, ex.InnerException);
            }
        }

        public PutInsertResponseResponse PutInsertResponse(PutInsertResponseRequest request)
        {
            try
            {
                PutInsertResponseResponse result = new PutInsertResponseResponse();
                IProgramRepository responseRepo = Factory.GetRepository(request, RepositoryType.PatientProgramResponse);

                foreach (ResponseDetail rd in request.ResponseDetails)
                {
                    MEPatientProgramResponse meres = new MEPatientProgramResponse(request.UserId)
                    {
                        Id = ObjectId.Parse(rd.Id),
                        NextStepId = ObjectId.Parse(rd.NextStepId),
                        Nominal = rd.Nominal,
                        Spawn = ParseSpawnElements(rd.SpawnElement),
                        Required = rd.Required,
                        Order = rd.Order,
                        StepId = ObjectId.Parse(rd.StepId),
                        Text = rd.Text,
                        Value = rd.Value,
                        Selected = rd.Selected,
                        DeleteFlag = rd.Delete,
                        UpdatedBy = ObjectId.Parse(request.UserId),
                        RecordCreatedBy = ObjectId.Parse(request.UserId),
                        RecordCreatedOn = DateTime.UtcNow
                    };

                    //result.Result = (bool) 
                    responseRepo.Insert(meres);
                    result.Result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:PutInsertResponse()::" + ex.Message, ex.InnerException);
            }
        }

        public PutUpdateResponseResponse PutUpdateResponse(PutUpdateResponseRequest request)
        {
            try
            {
                PutUpdateResponseResponse result = new PutUpdateResponseResponse();
                IProgramRepository responseRepo = Factory.GetRepository(request, RepositoryType.PatientProgramResponse);

                foreach (ResponseDetail rd in request.ResponseDetails)
                {
                    MEPatientProgramResponse meres = new MEPatientProgramResponse(request.UserId)
                    {
                        Id = ObjectId.Parse(rd.Id),
                        NextStepId = ObjectId.Parse(rd.NextStepId),
                        Nominal = rd.Nominal,
                        Spawn = ParseSpawnElements(rd.SpawnElement),
                        Required = rd.Required,
                        Order = rd.Order,
                        StepId = ObjectId.Parse(rd.StepId),
                        Text = rd.Text,
                        Value = rd.Value,
                        Selected = rd.Selected,
                        DeleteFlag = rd.Delete,
                        LastUpdatedOn = System.DateTime.UtcNow,
                        UpdatedBy = ObjectId.Parse(request.UserId)
                    };

                    //result.Result = (bool) 
                    responseRepo.Update(meres);
                    result.Result = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:PutUpdateResponse()::" + ex.Message, ex.InnerException);
            }
        }

        public PutUpdateProgramAttributesResponse PutUpdateProgramAttributes(PutUpdateProgramAttributesRequest request)
        {
            try
            {
                PutUpdateProgramAttributesResponse result = new PutUpdateProgramAttributesResponse();
                IProgramRepository responseRepo = Factory.GetRepository(request, RepositoryType.PatientProgramAttribute);//.GetProgramAttributesRepository(request);

                ProgramAttributeData pa = request.ProgramAttributes;
                result.Result = (bool)responseRepo.Update(pa);

                return result; 
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:PutUpdateProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        public PutProgramAttributesResponse InsertProgramAttributes(PutProgramAttributesRequest request)
        {
            try
            {
                PutProgramAttributesResponse response = new PutProgramAttributesResponse();

                IProgramRepository progAttr = Factory.GetRepository(request, RepositoryType.PatientProgramAttribute);//.GetProgramAttributesRepository(request);

                bool resp = (bool)progAttr.Insert((object)request.ProgramAttributes);
                response.Result = resp;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:InsertProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        #endregion

        #region GETS
        public GetProgramDetailsSummaryResponse GetPatientProgramDetailsById(GetProgramDetailsSummaryRequest request)
        {
            try
            {
                GetProgramDetailsSummaryResponse response = new GetProgramDetailsSummaryResponse();

                IProgramRepository repo = Factory.GetRepository(request, RepositoryType.PatientProgram);
                MEPatientProgram mepp = repo.FindByID(request.ProgramId) as MEPatientProgram;
                
                IProgramRepository respRepo = Factory.GetRepository(request, RepositoryType.PatientProgramResponse);
                var stepIds = mepp.Modules.SelectMany(m => m.Actions.SelectMany(a => a.Steps.Select(s => s.Id))).ToList();
                DTOUtility.ResponsesBag = respRepo.Find(stepIds).Cast<MEPatientProgramResponse>().ToList();

                response.Program = new ProgramDetail
                {
                    Id = mepp.Id.ToString(),
                    Client = mepp.Client != null ? mepp.Client.ToString() : null,
                    ContractProgramId = mepp.ContractProgramId.ToString(),
                    Description = mepp.Description,
                    Name = mepp.Name,
                    PatientId = mepp.PatientId.ToString(),
                    //ProgramState = (int)mepp.ProgramState, depricated - Use Element state instead.
                    ShortName = mepp.ShortName,
                    StartDate = mepp.StartDate,
                    EndDate = mepp.EndDate,
                    AttrStartDate = mepp.AttributeStartDate,
                    AttrEndDate = mepp.AttributeEndDate,
                    Status = (int)mepp.Status,
                    Version = mepp.Version,
                    Completed = mepp.Completed,
                    Enabled = mepp.Enabled,
                    Next = mepp.Next != null ? mepp.Next.ToString() : string.Empty,
                    Order = mepp.Order,
                    Previous = mepp.Previous != null ? mepp.Previous.ToString() : string.Empty,
                    SourceId = mepp.SourceId.ToString(),
                    AssignBy = mepp.AssignedBy.ToString(),
                    AssignDate = mepp.AssignedOn,
                    AssignTo = mepp.AssignedTo.ToString(),
                    ElementState = (int)mepp.State,
                    StateUpdatedOn = mepp.StateUpdatedOn,
                    CompletedBy = mepp.CompletedBy,
                    DateCompleted = mepp.DateCompleted,
                    EligibilityEndDate = mepp.EligibilityEndDate,
                    EligibilityStartDate = mepp.EligibilityStartDate,
                    EligibilityRequirements = mepp.EligibilityRequirements,
                    AuthoredBy = mepp.AuthoredBy,
                    //ObjectivesData = DTOUtils.GetObjectives(mepp.Objectives),
                    SpawnElement = DTOUtility.GetSpawnElement(mepp),
                    Modules = DTOUtility.GetModules(mepp.Modules, mepp.ContractProgramId.ToString(), request)
                };

                // load program attributes
                ProgramAttributeData pad = GetProgramAttributes(mepp.Id.ToString(), request);
                response.Program.Attributes = pad;

                // Get the fields from Program collection.
                MEProgram meProgram = DTOUtility.GetLimitedProgramDetails(mepp.SourceId.ToString(), request);

                if (meProgram != null)
                {
                    response.Program.AuthoredBy = meProgram.AuthoredBy;
                    response.Program.TemplateName = meProgram.TemplateName;
                    response.Program.TemplateVersion = meProgram.TemplateVersion;
                    response.Program.ProgramVersion = meProgram.ProgramVersion;
                    response.Program.ProgramVersionUpdatedOn = meProgram.ProgramVersionUpdatedOn;
                    response.Program.ObjectivesData = DTOUtility.GetObjectivesData(meProgram.Objectives);
                }
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetPatientProgramDetailsById()::" + ex.Message, ex.InnerException);
            }
            finally
            {
                DTOUtility.ResponsesBag = null;
            }
        }

        public GetProgramResponse GetProgramByID(GetProgramRequest request)
        {
            try
            {
                GetProgramResponse programResponse = new GetProgramResponse();
                DTO.Program result;

                IProgramRepository repo = Factory.GetRepository(request, RepositoryType.Program);

                result = repo.FindByID(request.ProgramID) as DTO.Program;

                programResponse.Program = result;
                return (programResponse != null ? programResponse : new GetProgramResponse());
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetProgramByID()::" + ex.Message, ex.InnerException);
            }
        }

        public GetProgramByNameResponse GetProgramByName(GetProgramByNameRequest request)
        {
            try
            {
                GetProgramByNameResponse programResponse = new GetProgramByNameResponse();
                DTO.Program result;

                IProgramRepository repo = Factory.GetRepository(request, RepositoryType.Program);

                result = repo.FindByName(request.ProgramName) as DTO.Program;

                programResponse.Program = result;
                return (programResponse != null ? programResponse : new GetProgramByNameResponse());
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetProgramByName()::" + ex.Message, ex.InnerException);
            }
        }

        public GetAllActiveProgramsResponse GetAllActiveContractPrograms(GetAllActiveProgramsRequest request)
        {
            try
            {
                GetAllActiveProgramsResponse response = new GetAllActiveProgramsResponse();
                List<ProgramInfo> result;

                IProgramRepository repo = Factory.GetRepository(request, RepositoryType.ContractProgram);

                result = repo.GetActiveProgramsInfoList(request);
                response.Programs = result;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetAllActiveContractPrograms()::" + ex.Message, ex.InnerException);
            }
        }

        public ProgramAttributeData GetProgramAttributes(string objectId, IDataDomainRequest request)
        {
            try
            {
                ProgramAttributeData pad = null;
                IProgramRepository repo = Factory.GetRepository(request, RepositoryType.PatientProgramAttribute);

                MEProgramAttribute pa = repo.FindByPlanElementID(objectId) as MEProgramAttribute;
                if (pa != null)
                {
                    pad = new ProgramAttributeData
                    {
                        //  AssignedBy = pa.AssignedBy.ToString(), Sprint 12
                        //  AssignedTo = pa.AssignedTo.ToString(), Sprint 12
                        // AssignedOn = pa.AssignedOn, Sprint 12
                        Completed = (int)pa.Completed,
                        CompletedBy = pa.CompletedBy,
                        DateCompleted = pa.DateCompleted,
                        DidNotEnrollReason = pa.DidNotEnrollReason,
                        Eligibility = (int)pa.Eligibility,
                        //  AttrEndDate = pa.EndDate, , Sprint 12
                        Enrollment = (int)pa.Enrollment,
                        GraduatedFlag = (int)pa.GraduatedFlag,
                        Id = pa.Id.ToString(),
                        IneligibleReason = pa.IneligibleReason,
                        Locked = (int)pa.Locked,
                        OptOut = pa.OptOut,
                        OverrideReason = pa.OverrideReason,
                        PlanElementId = pa.PlanElementId.ToString(),
                        Population = pa.Population,
                        RemovedReason = pa.RemovedReason,
                        // AttrStartDate = pa.StartDate, Sprint 12
                        Status = (int)pa.Status
                    };
                }
                return pad;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        public GetPatientProgramsDataResponse GetPatientPrograms(GetPatientProgramsDataRequest request)
        {
            try
            {
                var response = new GetPatientProgramsDataResponse();
                var repo = Factory.GetRepository(request, RepositoryType.PatientProgram);
                var patientPrograms = repo.FindByPatientId(request.PatientId).Cast<MEPatientProgram>().ToList();

                if (patientPrograms != null)
                {
                    var lpi = new List<ProgramInfo>();
                    patientPrograms.ForEach(pd => lpi.Add(new ProgramInfo
                    {
                        Id = Convert.ToString(pd.Id),
                        Name = pd.Name,
                        PatientId = Convert.ToString(pd.PatientId),
                        ProgramSourceId = (pd.SourceId == null) ? null : pd.SourceId.ToString(),
                        ShortName = pd.ShortName,
                        Status = (int)pd.Status,
                        ElementState = (int)pd.State,
                        AttrEndDate = pd.AttributeEndDate,
                        StateUpdatedOnDate = pd.StateUpdatedOn,
                        AssignedOnDate = pd.AssignedOn
                            })
                        );
                        response.programs = lpi;
                    }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetPatientPrograms()::" + ex.Message, ex.InnerException);
            }
        }

        public GetStepResponseListResponse GetStepResponse(GetStepResponseListRequest request)
        {
            try
            {
                GetStepResponseListResponse response = null;
                response = DTOUtils.GetStepResponses(request.StepId, request.ContractNumber, true, request.UserId);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetStepResponse()::" + ex.Message, ex.InnerException);
            }
        }

        public GetStepResponseResponse GetStepResponse(GetStepResponseRequest request)
        {
            try
            {
                GetStepResponseResponse response = new GetStepResponseResponse();

                IProgramRepository repo = Factory.GetRepository(request, RepositoryType.PatientProgramResponse); //.GetPatientProgramStepResponseRepository(request);

                MEPatientProgramResponse result = repo.FindByID(request.ResponseId) as MEPatientProgramResponse;

                if (result != null)
                {
                    response.StepResponse = new StepResponse
                    {
                        Id = result.Id.ToString(),
                        NextStepId = result.NextStepId.ToString(),
                        Nominal = result.Nominal,
                        Order = result.Order,
                        Required = result.Required,
                        Spawn = DTOUtils.GetResponseSpawnElement(result.Spawn),
                        StepId = result.StepId.ToString(),
                        Text = result.Text,
                        Value = result.Value
                    };
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetStepResponse()::" + ex.Message, ex.InnerException);
            }
        }

        public GetProgramAttributeResponse GetProgramAttributes(GetProgramAttributeRequest request)
        {
            try
            {
                GetProgramAttributeResponse response = new GetProgramAttributeResponse();
                ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

                // PlanElementId
                SelectExpression patientSelectExpression = new SelectExpression();
                patientSelectExpression.FieldName = MEProgramAttribute.PlanElementIdProperty;
                patientSelectExpression.Type = SelectExpressionType.EQ;
                patientSelectExpression.Value = request.PlanElementId;
                patientSelectExpression.ExpressionOrder = 1;
                patientSelectExpression.GroupID = 1;
                selectExpressions.Add(patientSelectExpression);

                APIExpression apiExpression = new APIExpression();
                apiExpression.Expressions = selectExpressions;

                IProgramRepository repo = Factory.GetRepository(request, RepositoryType.PatientProgramAttribute);//.GetProgramAttributesRepository(request);

                Tuple<string, IEnumerable<object>> result = repo.Select(apiExpression);

                if (result != null)
                {
                    List<ProgramAttributeData> pds = result.Item2.Cast<ProgramAttributeData>().ToList();
                    if (pds.Count > 0)
                    {
                        response.ProgramAttribute = pds.FirstOrDefault();
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        public GetPatientActionDetailsDataResponse GetActionDetails(GetPatientActionDetailsDataRequest request)
        {
            try
            {
                GetPatientActionDetailsDataResponse response = new GetPatientActionDetailsDataResponse();

                IProgramRepository repo = Factory.GetRepository(request, RepositoryType.PatientProgram);//.GetPatientProgramRepository(request);
                MEPatientProgram mepp = repo.FindByID(request.PatientProgramId) as MEPatientProgram;

                IProgramRepository respRepo = Factory.GetRepository(request, RepositoryType.PatientProgramResponse);
                var stepIds = mepp.Modules.SelectMany(m => m.Actions.SelectMany(a => a.Steps.Select(s => s.Id))).ToList();
                DTOUtility.ResponsesBag = respRepo.Find(stepIds).Cast<MEPatientProgramResponse>().ToList();

                Module meModule = mepp.Modules.Where(m => m.Id == ObjectId.Parse(request.PatientModuleId)).FirstOrDefault();
                if (meModule != null)
                {
                    MongoDB.DTO.Action meAction = meModule.Actions.Where(a => a.Id == ObjectId.Parse(request.PatientActionId)).FirstOrDefault();
                    if (meAction != null)
                    {
                        List<Module> tMods = DTOUtility.GetTemplateModulesList(mepp.SourceId.ToString(), request.ContractNumber, request.UserId);
                        Module tmodule = tMods.Find(tm => tm.SourceId == meModule.SourceId);
                        if (tmodule != null)
                        {
                            meAction.Objectives = DTOUtility.GetTemplateObjectives(meAction.SourceId, tmodule);
                        }
                        response.ActionData = DTOUtility.GetAction(request.ContractNumber, request.UserId, meAction);
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                DTOUtility.ResponsesBag = null;
                throw new Exception("DD:DataProgramManager:GetActionDetails()::" + ex.Message, ex.InnerException);
            }
            finally
            {
                DTOUtility.ResponsesBag = null;
            }
        } 
        #endregion

        #region DELETES
        public DeletePatientProgramByPatientIdDataResponse DeletePatientProgramByPatientId(DeletePatientProgramByPatientIdDataRequest request)
        {
            DeletePatientProgramByPatientIdDataResponse response = null;
            bool success = false;
            try
            {
                response = new DeletePatientProgramByPatientIdDataResponse();
                IProgramRepository ppRepo = Factory.GetRepository(request, RepositoryType.PatientProgram);

                List<MEPatientProgram> meppList = ppRepo.FindByPatientId(request.PatientId) as List<MEPatientProgram>;
                List<DeletedPatientProgram> deletedPatientPrograms = null;
                
                if (meppList != null && meppList.Count > 0)
                {
                    deletedPatientPrograms = new List<DeletedPatientProgram>();
                    meppList.ForEach(mePP =>
                    {
                        DeletePatientProgramDataRequest req = new DeletePatientProgramDataRequest { 
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        DeletedPatientProgram deletedPatientProgram = null;
                        if(delete(mePP, req, ppRepo, out deletedPatientProgram))
                        {
                            deletedPatientPrograms.Add(deletedPatientProgram);
                            success = true;
                        }
                    });
                    response.DeletedPatientPrograms = deletedPatientPrograms;
                }
                else
                {
                    success = true;
                }
                response.Success = success;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }


        public DeletePatientProgramDataResponse DeletePatientProgram(DeletePatientProgramDataRequest request)
        {
            DeletePatientProgramDataResponse response = null;
            bool success = false;
            try
            {
                response = new DeletePatientProgramDataResponse();
                IProgramRepository ppRepo = Factory.GetRepository(request, RepositoryType.PatientProgram);

                MEPatientProgram mePP = ppRepo.FindByID(request.Id) as MEPatientProgram;
                DeletedPatientProgram deletedPatientProgram = null;

                if (mePP != null)
                {
                    if (delete(mePP, request, ppRepo, out deletedPatientProgram))
                        success = true;
                }
                else
                {
                    success = true;
                }
                response.DeletedPatientProgram = deletedPatientProgram;
                response.Success = success;
                return response;
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
        }

        private bool delete(MEPatientProgram mePP, DeletePatientProgramDataRequest request, IProgramRepository ppRepo, out DeletedPatientProgram deletedProgram)
        {
            DeletedPatientProgram deletedPatientProgram = null;
            List<string> deletedResponsesIds = null;
            bool success = false;
            try
            {
                if (mePP != null)
                {
                    IProgramRepository ppAttributesRepo = Factory.GetRepository(request, RepositoryType.PatientProgramAttribute);
                    IProgramRepository ppResponsesRepo = Factory.GetRepository(request, RepositoryType.PatientProgramResponse);

                    #region PatientProgram
                    request.Id = mePP.Id.ToString();
                    ppRepo.Delete(request);
                    deletedPatientProgram = new DeletedPatientProgram { Id = request.Id };
                    success = true;
                    deletedResponsesIds = new List<string>();
                    #endregion

                    #region PPAttributes
                    MEProgramAttribute pa = ppAttributesRepo.FindByPlanElementID(request.Id) as MEProgramAttribute;
                    if (pa != null)
                    {
                        DeletePatientProgramAttributesDataRequest deletePPAttrDataRequest = new DeletePatientProgramAttributesDataRequest
                        {
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            Id = pa.Id.ToString(),
                            UserId = request.UserId,
                            Version = request.Version
                        };
                        ppAttributesRepo.Delete(deletePPAttrDataRequest);
                        deletedPatientProgram.PatientProgramAttributeId = deletePPAttrDataRequest.Id;
                        success = true;
                    }
                    #endregion

                    #region PPResponses
                    List<Module> modules = mePP.Modules;
                    if (modules != null && modules.Count > 0)
                    {
                        modules.ForEach(m =>
                        {
                            List<MongoDB.DTO.Action> actions = m.Actions;
                            if (actions != null && actions.Count > 0)
                            {
                                actions.ForEach(a =>
                                {
                                    List<Step> steps = a.Steps;
                                    if (steps != null && steps.Count > 0)
                                    {
                                        steps.ForEach(s =>
                                        {
                                            List<MEPatientProgramResponse> meResponses = ppResponsesRepo.FindByStepId(s.Id.ToString()) as List<MEPatientProgramResponse>;
                                            if (meResponses != null && meResponses.Count > 0)
                                            {
                                                meResponses.ForEach(r =>
                                                {
                                                    DeletePatientProgramResponsesDataRequest deletePPResponsesRequest = new DeletePatientProgramResponsesDataRequest
                                                    {
                                                        Context = request.Context,
                                                        ContractNumber = request.ContractNumber,
                                                        Id = r.Id.ToString(),
                                                        UserId = request.UserId,
                                                        Version = request.Version
                                                    };
                                                    ppResponsesRepo.Delete(deletePPResponsesRequest);
                                                    deletedResponsesIds.Add(deletePPResponsesRequest.Id);
                                                    success = true;
                                                    deletedPatientProgram.PatientProgramResponsesIds = deletedResponsesIds;
                                                });
                                            }
                                        });

                                    }
                                });
                            }
                        });
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                success = false;
                string aseProcessID = ConfigurationManager.AppSettings.Get("ASEProcessID") ?? "0";
                Common.Helper.LogException(int.Parse(aseProcessID), ex);
            }
            deletedProgram = deletedPatientProgram;
            return success;
        }
        #endregion

        #region UNDODELETE
        public UndoDeletePatientProgramDataResponse UndoDeletePatientPrograms(UndoDeletePatientProgramDataRequest request)
        {
            UndoDeletePatientProgramDataResponse response = null;
            try
            {
                response = new UndoDeletePatientProgramDataResponse();
                IProgramRepository ppRepo = Factory.GetRepository(request, RepositoryType.PatientProgram);
                IProgramRepository ppAttributesRepo = Factory.GetRepository(request, RepositoryType.PatientProgramAttribute);
                IProgramRepository ppResponsesRepo = Factory.GetRepository(request, RepositoryType.PatientProgramResponse);
                if (request.Ids != null && request.Ids.Count > 0)
                {
                    List<DeletedPatientProgram> PatientProgramIds = request.Ids;
                    PatientProgramIds.ForEach(u =>
                    {
                        #region PatientProgram
                        if(u.Id != null)
                        {
                            request.PatientProgramId = u.Id;
                            ppRepo.UndoDelete(request);
                        }
                        #endregion

                        #region PPAttributes
                        if (u.PatientProgramAttributeId != null)
                        {
                            UndoDeletePatientProgramAttributesDataRequest attrRequest = new UndoDeletePatientProgramAttributesDataRequest
                            {
                                Context = request.Context,
                                ContractNumber = request.ContractNumber,
                                PatientProgramAttributeId = u.PatientProgramAttributeId,
                                UserId = request.UserId,
                                Version = request.Version
                            };
                            ppAttributesRepo.UndoDelete(attrRequest);
                        }
                        #endregion

                        #region PPResponses
                        List<string> responseIds = u.PatientProgramResponsesIds;
                        if (responseIds != null && responseIds.Count > 0)
                        {
                            responseIds.ForEach(r =>
                            {
                                UndoDeletePatientProgramResponsesDataRequest respRequest = new UndoDeletePatientProgramResponsesDataRequest 
                                { 
                                    Context = request.Context,
                                    ContractNumber = request.ContractNumber,
                                    PatientProgramResponseId = r,
                                    UserId = request.UserId,
                                    Version = request.Version
                                };
                                ppResponsesRepo.UndoDelete(respRequest);
                            });
                        }
                        #endregion
                    });
                }
                response.Success = true;
                return response;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region private methods
        private List<SpawnElement> ParseSpawnElements(List<SpawnElementDetail> list)
        {
            List<SpawnElement> mespn = new List<SpawnElement>();
            try
            {
                if (list != null)
                {
                    list.ForEach(s =>
                    {
                        mespn.Add(new SpawnElement
                        {
                            SpawnId = (s.ElementId != null) ? ObjectId.Parse(s.ElementId) : ObjectId.Parse("000000000000000000000000"),
                            Tag = s.Tag,
                            Type = (SpawnElementTypeCode)s.ElementType
                        });
                    });
                }
                return mespn;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:ParseSpawnElements()::" + ex.Message, ex.InnerException);
            }
        }

        private PutProgramToPatientResponse FormatExceptionResponse(PutProgramToPatientResponse response, string reason, string errorcode)
        {
            try
            {
                response.Status = new ResponseStatus(errorcode, reason);
                response.Outcome = new Phytel.API.DataDomain.Program.DTO.Outcome() { Reason = reason, Result = 0 };
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:FormatExceptionResponse()::" + ex.Message, ex.InnerException);
            }
        }

        private bool IsContractProgramAssignable(PutProgramToPatientRequest request)
        {
            bool result = false;
            try
            {
                IProgramRepository contractProgRepo = Factory.GetRepository(request, RepositoryType.ContractProgram);

                ContractProgram c = contractProgRepo.FindByID(request.ContractProgramId) as ContractProgram;

                if (c != null)
                {
                    if (c.Status == 1 && c.Delete != true)
                        result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:IsContractProgramAssignable" + ex.Message, ex.InnerException);
            }
        }

        private bool IsValidPatientId(PutProgramToPatientRequest request)
        {
            bool result = false;
            try
            {
                string path = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
                string contractNumber = request.ContractNumber;
                string context = request.Context;
                double version = request.Version;
                IRestClient client = new JsonServiceClient();

                string url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                    path,
                    context,
                    version,
                    contractNumber,
                    request.PatientId), request.UserId);

                GetPatientDataResponse response = client.Get<GetPatientDataResponse>(url);

                if (response.Patient != null)
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:IsValidPatientId()::" + ex.Message, ex.InnerException);
            }
        }

        private bool IsValidContractProgramId(PutProgramToPatientRequest request)
        {
            bool result = false;
            try
            {
                IProgramRepository contractProgRepo = Factory.GetRepository(request, RepositoryType.ContractProgram);

                object contractProgram = contractProgRepo.FindByID(request.ContractProgramId);
                if (contractProgram != null)
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:IsValidContractProgramId()::" + ex.Message, ex.InnerException);
            }
        }

        private List<SpawnElement> CreateSpawn(List<SpawnElementDetail> list)
        {
            try
            {
                List<SpawnElement> se = new List<SpawnElement>();
                if (list != null)
                {
                    list.ForEach(s =>
                    {
                        se.Add(new SpawnElement
                        {
                            SpawnId = (s.ElementId != null) ? ObjectId.Parse(s.ElementId) : ObjectId.Parse("000000000000000000000000"),
                            Tag = s.Tag,
                            Type = (SpawnElementTypeCode)s.ElementType
                        });
                    });
                }

                return se;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:CreateSpawn()::" + ex.Message, ex.InnerException);
            }
        } 
        #endregion
    }
}   
