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
                response.Outcome = new Outcome();

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
                    response.Outcome.Reason = "Successfully assigned this program for the patient";
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

        public PutUpdateResponseResponse PutUpdateResponse(PutUpdateResponseRequest request)
        {
            try
            {
                PutUpdateResponseResponse result = new PutUpdateResponseResponse();
                IProgramRepository responseRepo = Factory.GetRepository(request, RepositoryType.PatientProgramResponse);

                MEPatientProgramResponse meres = new MEPatientProgramResponse(request.UserId)
                {
                    Id = ObjectId.Parse(request.ResponseDetail.Id),
                    NextStepId = ObjectId.Parse(request.ResponseDetail.NextStepId),
                    Nominal = request.ResponseDetail.Nominal,
                    Spawn = ParseSpawnElements(request.ResponseDetail.SpawnElement),
                    Required = request.ResponseDetail.Required,
                    Order = request.ResponseDetail.Order,
                    StepId = ObjectId.Parse(request.ResponseDetail.StepId),
                    Text = request.ResponseDetail.Text,
                    Value = request.ResponseDetail.Value,
                    Selected = request.ResponseDetail.Selected,
                    DeleteFlag = request.ResponseDetail.Delete,
                    LastUpdatedOn = System.DateTime.UtcNow,
                    UpdatedBy = ObjectId.Parse(request.UserId)
                };

                result.Result = (bool)responseRepo.Update(meres);

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
                    Modules = DTOUtility.GetModules(mepp.Modules, mepp.ContractProgramId.ToString(), request.ContractNumber, request.UserId)
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

        public GetPatientProgramsResponse GetPatientPrograms(GetPatientProgramsRequest request)
        {
            try
            {
                GetPatientProgramsResponse response = new GetPatientProgramsResponse(); ;

                IProgramRepository repo = Factory.GetRepository(request, RepositoryType.PatientProgram);

                ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

                // PatientID
                SelectExpression patientSelectExpression = new SelectExpression();
                patientSelectExpression.FieldName = MEPatientProgram.PatientIdProperty;
                patientSelectExpression.Type = SelectExpressionType.EQ;
                patientSelectExpression.Value = request.PatientId;
                patientSelectExpression.ExpressionOrder = 1;
                patientSelectExpression.GroupID = 1;
                selectExpressions.Add(patientSelectExpression);

                //DeleteFlag
                SelectExpression deleteFlagSelectExpression = new SelectExpression();
                deleteFlagSelectExpression.FieldName = MEPatientProgram.DeleteFlagProperty;
                deleteFlagSelectExpression.Type = SelectExpressionType.EQ;
                deleteFlagSelectExpression.Value = false;
                deleteFlagSelectExpression.ExpressionOrder = 2;
                deleteFlagSelectExpression.GroupID = 1;
                selectExpressions.Add(deleteFlagSelectExpression);

                APIExpression apiExpression = new APIExpression();
                apiExpression.Expressions = selectExpressions;

                Tuple<string, IEnumerable<object>> patientPrograms = repo.Select(apiExpression);

                if (patientPrograms != null)
                {
                    List<ProgramDetail> pds = patientPrograms.Item2.Cast<ProgramDetail>().ToList();
                    if (pds.Count > 0)
                    {

                        List<ProgramInfo> lpi = new List<ProgramInfo>();
                        pds.ForEach(pd => lpi.Add(new ProgramInfo
                            {
                                Id = pd.Id,
                                Name = pd.Name,
                                PatientId = pd.PatientId,
                                ProgramState = pd.ProgramState,
                                ShortName = pd.ShortName,
                                Status = pd.Status,
                                ElementState = pd.ElementState
                            })
                        );
                        response.programs = lpi;
                    }
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
                throw new Exception("DD:DataProgramManager:GetActionDetails()::" + ex.Message, ex.InnerException);
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
                IProgramRepository ppAttributesRepo = Factory.GetRepository(request, RepositoryType.PatientProgramAttribute);
                IProgramRepository ppResponsesRepo = Factory.GetRepository(request, RepositoryType.PatientProgramResponse);
                List<MEPatientProgram> meppList = ppRepo.FindByPatientId(request.PatientId) as List<MEPatientProgram>;
                List<DeletedPatientProgram> deletedPatientPrograms = null;
                List<string> deletedResponsesIds = null;
                if (meppList != null && meppList.Count > 0)
                {
                    meppList.ForEach(u =>
                    {
                        #region PatientProgram
                        request.Id = u.Id.ToString();
                        ppRepo.Delete(request);
                        DeletedPatientProgram deletedPatientProgram = new DeletedPatientProgram { PatientProgramId = request.Id };
                        deletedResponsesIds = new List<string>();
                        success = true;
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
                        List<Module> modules = u.Modules;
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
                                                        });
                                                    }
                                                });

                                            }
                                        });
                                    }
                                });
                        }
                        #endregion

                        deletedPatientProgram.PatientProgramResponsesIds = deletedResponsesIds;
                        deletedPatientPrograms.Add(deletedPatientProgram);
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
                response.Outcome = new Outcome() { Reason = reason, Result = 0 };
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
