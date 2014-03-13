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
    public static class ProgramDataManager
    {
        public static GetProgramResponse GetProgramByID(GetProgramRequest request)
        {
            try
            {
                GetProgramResponse programResponse = new GetProgramResponse();
                DTO.Program result;

                IProgramRepository<GetProgramResponse> repo = ProgramRepositoryFactory<GetProgramResponse>.GetProgramRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;

                result = repo.FindByID(request.ProgramID) as DTO.Program;

                programResponse.Program = result;
                return (programResponse != null ? programResponse : new GetProgramResponse());
            }
            catch (Exception ex)
            {
                throw new Exception("DD:GetProgramByID()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetAllActiveProgramsResponse GetAllActiveContractPrograms(GetAllActiveProgramsRequest request)
        {
            try
            {
                GetAllActiveProgramsResponse response = new GetAllActiveProgramsResponse();
                List<ProgramInfo> result;

                IProgramRepository<GetAllActiveProgramsResponse> repo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetAllActiveProgramsResponse>.GetContractProgramRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;

                result = repo.GetActiveProgramsInfoList(request);
                response.Programs = result;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:GetAllActiveContractPrograms()::" + ex.Message, ex.InnerException);
            }
        }

        public static PutProgramToPatientResponse PutPatientToProgram(PutProgramToPatientRequest request)
        {
            try
            {
                PutProgramToPatientResponse response = new PutProgramToPatientResponse();

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

                IProgramRepository<PutProgramToPatientResponse> patProgRepo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramToPatientResponse>
                    .GetPatientProgramRepository(request.ContractNumber, request.Context);
                patProgRepo.UserId = request.UserId;

                object resp = patProgRepo.Insert((object)request);

                response = (PutProgramToPatientResponse)resp;

                if (response.program != null)
                {
                    // initialize attributes
                    DTOUtils.InitializeProgramAttributes(request, response);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PutPatientToProgram()::" + ex.Message, ex.InnerException);
            }
        }



        public static PutProgramActionProcessingResponse PutProgramActionUpdate(PutProgramActionProcessingRequest request)
        {
            try
            {
                PutProgramActionProcessingResponse response = new PutProgramActionProcessingResponse();

                IProgramRepository<PutProgramActionProcessingResponse> patProgRepo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramActionProcessingResponse>
                    .GetPatientProgramRepository(request.ContractNumber, request.Context);
                patProgRepo.UserId = request.UserId;

                response.program = (ProgramDetail)patProgRepo.Update((object)request);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PutProgramActionUpdate()::" + ex.Message, ex.InnerException);
            }
        }

        private static PutProgramToPatientResponse FormatExceptionResponse(PutProgramToPatientResponse response, string reason, string errorcode)
        {
            try
            {
                response.Status = new ResponseStatus(errorcode, reason);
                response.Outcome = new Outcome() { Reason = reason, Result = 0 };
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:FormatExceptionResponse()::" + ex.Message, ex.InnerException);
            }
        }

        private static bool IsContractProgramAssignable(PutProgramToPatientRequest p)
        {
            bool result = false;
            try
            {
                IProgramRepository<ContractProgram> contractProgRepo =
                                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<ContractProgram>
                                .GetContractProgramRepository(p.ContractNumber, p.Context);
                contractProgRepo.UserId = p.UserId;

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
                throw new Exception("ProgramDD:IsContractProgramAssignable" + ex.Message, ex.InnerException);
            }
        }

        private static bool IsValidPatientId(PutProgramToPatientRequest request)
        {
            bool result = false;
            try
            {
                string path = ConfigurationManager.AppSettings["DDPatientServiceUrl"];
                string contractNumber = request.ContractNumber;
                string context = request.Context;
                double version = request.Version;
                IRestClient client = Common.Helper.GetJsonServiceClient(request.UserId);

                GetPatientDataResponse response = client.Get<GetPatientDataResponse>(
                    string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                    path,
                    context,
                    version,
                    contractNumber,
                    request.PatientId));

                if (response.Patient != null)
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("ProgramDD:IsValidPatientId()::" + ex.Message, ex.InnerException);
            }
        }

        private static bool IsValidContractProgramId(PutProgramToPatientRequest request)
        {
            bool result = false;
            try
            {
                IProgramRepository<PutProgramToPatientResponse> contractProgRepo =
                                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramToPatientResponse>
                                .GetContractProgramRepository(request.ContractNumber, request.Context);
                contractProgRepo.UserId = request.UserId;

                object contractProgram = contractProgRepo.FindByID(request.ContractProgramId);
                if (contractProgram != null)
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("ProgramDD:IsValidContractProgramId()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetProgramDetailsSummaryResponse GetPatientProgramDetailsById(GetProgramDetailsSummaryRequest request)
        {
            try
            {
                GetProgramDetailsSummaryResponse response = new GetProgramDetailsSummaryResponse();

                IProgramRepository<GetProgramDetailsSummaryResponse> repo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetProgramDetailsSummaryResponse>
                    .GetPatientProgramRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;

                //response = as GetProgramDetailsSummaryResponse;

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
                    Next = mepp.Next,
                    Order = mepp.Order,
                    Previous = mepp.Previous,
                    SourceId = mepp.SourceId.ToString(),
                    AssignBy = mepp.AssignedBy,
                    AssignDate = mepp.AssignedOn,
                    ElementState = (int)mepp.State,
                    CompletedBy = mepp.CompletedBy,
                    DateCompleted = mepp.DateCompleted,
                    ObjectivesInfo = DTOUtils.GetObjectives(mepp.ObjectivesInfo),
                    SpawnElement = DTOUtils.GetSpawnElement(mepp),
                    Modules = DTOUtils.GetModules(mepp.Modules, request.ContractNumber)
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("ProgramDD:GetPatientProgramDetailsById()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetPatientProgramsResponse GetPatientPrograms(GetPatientProgramsRequest request)
        {
            try
            {
                GetPatientProgramsResponse response = new GetPatientProgramsResponse(); ;

                IProgramRepository<GetPatientProgramsResponse> repo =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetPatientProgramsResponse>
                    .GetPatientProgramRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;

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
                throw new Exception("DD:GetPatientPrograms()::" + ex.Message, ex.InnerException);
            }
        }

        public static PutUpdateResponseResponse PutUpdateResponse(PutUpdateResponseRequest r)
        {
            try
            {
                PutUpdateResponseResponse result = new PutUpdateResponseResponse();
                IProgramRepository<PutUpdateResponseResponse> responseRepo =
                                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutUpdateResponseResponse>
                                .GetPatientProgramStepResponseRepository(r.ContractNumber, r.Context);
                responseRepo.UserId = r.UserId;

                MEPatientProgramResponse meres = new MEPatientProgramResponse
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
                    DeleteFlag = r.ResponseDetail.Delete
                };

                result.Result = (bool)responseRepo.Update(meres);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PutUpdateResponse()::" + ex.Message, ex.InnerException);
            }
        }

        private static List<SpawnElement> ParseSpawnElements(List<SpawnElementDetail> list)
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
                throw new Exception("DD:ParseSpawnElements()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetStepResponseListResponse GetStepResponse(GetStepResponseListRequest request)
        {
            try
            {
                GetStepResponseListResponse response = null;
                response = DTOUtils.GetStepResponses(request.StepId, request.ContractNumber, true);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:GetStepResponse()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetStepResponseResponse GetStepResponse(GetStepResponseRequest request)
        {
            try
            {
                GetStepResponseResponse response = new GetStepResponseResponse();
                IProgramRepository<GetStepResponseResponse> repo = ProgramRepositoryFactory<GetStepResponseResponse>.GetPatientProgramStepResponseRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;

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
                throw new Exception("DD:GetStepResponse()::" + ex.Message, ex.InnerException);
            }
        }

        public static GetProgramAttributeResponse GetProgramAttributes(GetProgramAttributeRequest request)
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
                    ProgramRepositoryFactory<GetProgramAttributeResponse>.GetProgramAttributesRepository(request.ContractNumber, request.Context);
                repo.UserId = request.UserId;

                Tuple<string, IEnumerable<object>> result = repo.Select(apiExpression);

                if (result != null)
                {
                    List<ProgramAttribute> pds = result.Item2.Cast<ProgramAttribute>().ToList();
                    if (pds.Count > 0)
                    {
                        response.ProgramAttribute = pds.FirstOrDefault();
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:GetProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        public static PutUpdateProgramAttributesResponse PutUpdateProgramAttributes(PutUpdateProgramAttributesRequest request)
        {
            try
            {
                PutUpdateProgramAttributesResponse result = new PutUpdateProgramAttributesResponse();
                IProgramRepository<PutUpdateProgramAttributesResponse> responseRepo =
                                Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutUpdateProgramAttributesResponse>
                                .GetProgramAttributesRepository(request.ContractNumber, request.Context);
                responseRepo.UserId = request.UserId;

                ProgramAttribute pa = request.ProgramAttributes;
                result.Result = (bool)responseRepo.Update(pa);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:PutUpdateProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        public static PutProgramAttributesResponse InsertProgramAttributes(PutProgramAttributesRequest request)
        {
            try
            {
                PutProgramAttributesResponse response = new PutProgramAttributesResponse();

                IProgramRepository<PutProgramAttributesResponse> progAttr =
                    Phytel.API.DataDomain.Program.ProgramRepositoryFactory<PutProgramAttributesResponse>
                    .GetProgramAttributesRepository(request.ContractNumber, request.Context);
                progAttr.UserId = request.UserId;

                bool resp = (bool)progAttr.Insert((object)request.ProgramAttributes);
                response.Result = resp;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:InsertProgramAttributes()::" + ex.Message, ex.InnerException);
            }
        }

        private static List<SpawnElement> CreateSpawn(List<SpawnElementDetail> list)
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
                throw new Exception("DD:CreateSpawn()::" + ex.Message, ex.InnerException);
            }
        }
    }
}   
