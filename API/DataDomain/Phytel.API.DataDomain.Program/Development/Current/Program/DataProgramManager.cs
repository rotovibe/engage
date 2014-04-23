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
        private List<ObjectId> sIL = new List<ObjectId>();

        public GetProgramResponse GetProgramByID(GetProgramRequest request)
        {
            try
            {
            GetProgramResponse programResponse = new GetProgramResponse();
            DTO.Program result;

            IProgramRepository<GetProgramResponse> repo = ProgramRepositoryFactory<GetProgramResponse>.GetProgramRepository(request.ContractNumber, request.Context, request.UserId);

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

                IProgramRepository<GetProgramResponse> repo = ProgramRepositoryFactory<GetProgramResponse>.GetProgramRepository(request.ContractNumber, request.Context, request.UserId);

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

            IProgramRepository<GetAllActiveProgramsResponse> repo =
                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetAllActiveProgramsResponse>.GetContractProgramRepository(request.ContractNumber, request.Context, request.UserId);

            result = repo.GetActiveProgramsInfoList(request);
            response.Programs = result;

            return response;
        }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetAllActiveContractPrograms()::" + ex.Message, ex.InnerException);
            }
        }

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
                List<MEPatientProgram> pp = DTOUtils.FindExistingpatientProgram(request);

                if (!DTOUtils.CanInsertPatientProgram(pp))
                {
                    response.Outcome.Result = 0;
                    response.Outcome.Reason = pp[0].Name + " is already assigned or reassignment is not allowed";
                }
                else
                {                    
                    MEProgram cp = DTOUtils.GetProgramForDeepCopy(request);

                    List<ObjectId> stepIdList = new List<ObjectId>();
                    List<MEResponse> responseList = DTOUtils.GetProgramResponseslist(stepIdList, cp,request);
                    DTOUtils.HydrateResponsesInProgram(cp, responseList, request.UserId);
                    MEPatientProgram nmePP = DTOUtils.CreateInitialMEPatientProgram(request, cp, stepIdList);
                    List<MEPatientProgramResponse> pprs = DTOUtils.InitializePatientProgramAssignment(request, nmePP);

                    ProgramInfo pi = DTOUtils.SaveNewPatientProgram(request, nmePP);
                    
                    if (pi != null)
                    {
                        response.program = pi;
                        DTOUtils.SavePatientProgramResponses(pprs, request);
                        DTOUtils.InitializeProgramAttributes(request, response);
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

                IProgramRepository<PutProgramActionProcessingResponse> patProgRepo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramActionProcessingResponse>
                    .GetPatientProgramRepository(request.ContractNumber, request.Context, request.UserId);

                response.program = (ProgramDetail)patProgRepo.Update((object)request);
                response.program.Attributes = GetProgramAttributes(response.program.Id, request.ContractNumber, request.Context, request.UserId);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:PutProgramActionUpdate()::" + ex.Message, ex.InnerException);
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

        private bool IsContractProgramAssignable(PutProgramToPatientRequest p)
        {
            bool result = false;
            try
            {
                IProgramRepository<ContractProgram> contractProgRepo =
                                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<ContractProgram>
                                .GetContractProgramRepository(p.ContractNumber, p.Context, p.UserId);

                ContractProgram c = contractProgRepo.FindByID(p.ContractProgramId) as ContractProgram;

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
                IProgramRepository<PutProgramToPatientResponse> contractProgRepo =
                                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramToPatientResponse>
                                .GetContractProgramRepository(request.ContractNumber, request.Context, request.UserId);

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

        public GetProgramDetailsSummaryResponse GetPatientProgramDetailsById(GetProgramDetailsSummaryRequest request)
        {
            try
            {
                GetProgramDetailsSummaryResponse response = new GetProgramDetailsSummaryResponse();

                IProgramRepository<GetProgramDetailsSummaryResponse> repo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetProgramDetailsSummaryResponse>
                    .GetPatientProgramRepository(request.ContractNumber, request.Context, request.UserId);

                MEPatientProgram mepp = repo.FindByID(request.ProgramId) as MEPatientProgram;

                response.Program = new ProgramDetail
                {
                    Id = mepp.Id.ToString(),
                    Client = mepp.Client != null ? mepp.Client.ToString() : null,
                    ContractProgramId = mepp.ContractProgramId.ToString(),
                    Description = mepp.Description,
                    Name = mepp.Name,
                    PatientId = mepp.PatientId.ToString(),
                    ProgramState = (int)mepp.ProgramState,
                    ShortName = mepp.ShortName,
                    StartDate = mepp.StartDate,
                    Status = (int)mepp.Status,
                    Version = mepp.Version,
                    EndDate = mepp.EndDate,
                    Completed = mepp.Completed,
                    Enabled = mepp.Enabled,
                    Next = mepp.Next != null ? mepp.Next.ToString() : string.Empty,
                    Order = mepp.Order,
                    Previous = mepp.Previous != null ? mepp.Previous.ToString() : string.Empty,
                    SourceId = mepp.SourceId.ToString(),
                    AssignBy = mepp.AssignedBy,
                    AssignDate = mepp.AssignedOn,
                    ElementState = (int)mepp.State,
                    CompletedBy = mepp.CompletedBy,
                    DateCompleted = mepp.DateCompleted,
                    EligibilityEndDate = mepp.EligibilityEndDate,
                    EligibilityStartDate = mepp.EligibilityStartDate,
                    EligibilityRequirements = mepp.EligibilityRequirements,
                    ObjectivesInfo = DTOUtils.GetObjectives(mepp.Objectives),
                    SpawnElement = DTOUtils.GetSpawnElement(mepp),
                    Modules = DTOUtils.GetModules(mepp.Modules, request.ContractNumber, request.UserId)
                };

                // load program attributes
                ProgramAttributeData pad = GetProgramAttributes(mepp.Id.ToString(), request.ContractNumber, request.Context, request.UserId);
                response.Program.Attributes = pad;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetPatientProgramDetailsById()::" + ex.Message, ex.InnerException);
            }
        }

        public ProgramAttributeData GetProgramAttributes(string objectId, string contract, string context, string userid)
        {
            try
            {
                ProgramAttributeData pad = null;
                IProgramRepository<GetProgramAttributeResponse> repo = 
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetProgramAttributeResponse>
                    .GetProgramAttributesRepository(contract, context, userid);

                MEProgramAttribute pa = repo.FindByPlanElementID(objectId) as MEProgramAttribute;
                if (pa != null)
                {
                    pad = new ProgramAttributeData
                    {
                        AssignedBy = pa.AssignedBy.ToString(),
                        AssignedTo = pa.AssignedTo.ToString(),
                        AssignedOn = pa.AssignedOn,
                        AuthoredBy = pa.AuthoredBy,
                        Completed = (int)pa.Completed,
                        CompletedBy = pa.CompletedBy,
                        DateCompleted = pa.DateCompleted,
                        DidNotEnrollReason = pa.DidNotEnrollReason,
                        Eligibility = (int)pa.Eligibility,
                        AttrEndDate = pa.EndDate,
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
                        AttrStartDate = pa.StartDate,
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

                IProgramRepository<GetPatientProgramsResponse> repo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetPatientProgramsResponse>
                    .GetPatientProgramRepository(request.ContractNumber, request.Context, request.UserId);

                ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

                // PatientID
                SelectExpression patientSelectExpression = new SelectExpression();
                patientSelectExpression.FieldName = MEPatientProgram.PatientIdProperty;
                patientSelectExpression.Type = SelectExpressionType.EQ;
                patientSelectExpression.Value = request.PatientId;
                patientSelectExpression.ExpressionOrder = 1;
                patientSelectExpression.GroupID = 1;
                selectExpressions.Add(patientSelectExpression);

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
            catch(Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetPatientPrograms()::" + ex.Message, ex.InnerException);
            }
        }

        public PutUpdateResponseResponse PutUpdateResponse(PutUpdateResponseRequest r)
        {
            try
            {
            PutUpdateResponseResponse result = new PutUpdateResponseResponse();
            IProgramRepository<PutUpdateResponseResponse> responseRepo =
                            Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutUpdateResponseResponse>
                            .GetPatientProgramStepResponseRepository(r.ContractNumber, r.Context, r.UserId);

            MEPatientProgramResponse meres = new MEPatientProgramResponse(r.UserId)
            {
                Id = ObjectId.Parse(r.ResponseDetail.Id),
                NextStepId = ObjectId.Parse(r.ResponseDetail.NextStepId),
                Nominal = r.ResponseDetail.Nominal,
                Spawn = ParseSpawnElements(r.ResponseDetail.SpawnElement),
                Required = r.ResponseDetail.Required,
                Order = r.ResponseDetail.Order,
                StepId = ObjectId.Parse(r.ResponseDetail.StepId),
                Text = r.ResponseDetail.Text,
                Value = r.ResponseDetail.Value,
                Selected = r.ResponseDetail.Selected,
                DeleteFlag = r.ResponseDetail.Delete,
                LastUpdatedOn = System.DateTime.UtcNow,
                UpdatedBy = ObjectId.Parse(r.UserId)
            };

            result.Result = (bool)responseRepo.Update(meres);

            return result;
        }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:PutUpdateResponse()::" + ex.Message, ex.InnerException);
            }
        }

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

            IProgramRepository<GetStepResponseResponse> repo = ProgramRepositoryFactory<GetStepResponseResponse>.GetPatientProgramStepResponseRepository(request.ContractNumber, request.Context, request.UserId);
            
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

            IProgramRepository<GetProgramAttributeResponse> repo = 
                ProgramRepositoryFactory<GetProgramAttributeResponse>.GetProgramAttributesRepository(request.ContractNumber, request.Context, request.UserId);
            
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

        public PutUpdateProgramAttributesResponse PutUpdateProgramAttributes(PutUpdateProgramAttributesRequest request)
        {
            try
            {
            PutUpdateProgramAttributesResponse result = new PutUpdateProgramAttributesResponse();
            IProgramRepository<PutUpdateProgramAttributesResponse> responseRepo =
                            Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutUpdateProgramAttributesResponse>
                            .GetProgramAttributesRepository(request.ContractNumber, request.Context, request.UserId);

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

            IProgramRepository<PutProgramAttributesResponse> progAttr =
                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramAttributesResponse>
                .GetProgramAttributesRepository(request.ContractNumber, request.Context, request.UserId);

            bool resp = (bool)progAttr.Insert((object)request.ProgramAttributes);
            response.Result = resp;

            return response;
        }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:InsertProgramAttributes()::" + ex.Message, ex.InnerException);
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

        public GetPatientActionDetailsDataResponse GetActionDetails(GetPatientActionDetailsDataRequest request)
        {
            try
            {
                GetPatientActionDetailsDataResponse response = new GetPatientActionDetailsDataResponse();

                IProgramRepository<GetProgramDetailsSummaryResponse> repo = Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetProgramDetailsSummaryResponse>.GetPatientProgramRepository(request.ContractNumber, request.Context, request.UserId);

                MEPatientProgram mepp = repo.FindByID(request.PatientProgramId) as MEPatientProgram;

                Module meModule = mepp.Modules.Where(m => m.Id == ObjectId.Parse(request.PatientModuleId)).FirstOrDefault();
                if (meModule != null)
                {
                    MongoDB.DTO.Action meAction = meModule.Actions.Where(a => a.Id == ObjectId.Parse(request.PatientActionId)).FirstOrDefault();  
                    if(meAction != null)
                    {
                        response.ActionData = DTOUtils.GetAction(request.ContractNumber, request.UserId, meAction);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetActionDetails()::" + ex.Message, ex.InnerException);
            }
        }
    }
}   
