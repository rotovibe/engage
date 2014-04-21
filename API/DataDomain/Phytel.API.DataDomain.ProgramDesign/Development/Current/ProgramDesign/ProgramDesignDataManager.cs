using MongoDB.Bson;
using Phytel.API.Common;
using Phytel.API.Common.Format;
using Phytel.API.DataDomain.ProgramDesign;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class ProgramDesignDataManager: IProgramDesignDataManager
    {
        private static readonly string yesnostep = "yesno";
        private static readonly string textstep = "text";

        public GetActionDataResponse GetActionByID(GetActionDataRequest request)
        {
            GetActionDataResponse result = new GetActionDataResponse();

            IProgramDesignRepository<GetActionDataResponse> repo = ProgramDesignRepositoryFactory<GetActionDataResponse>.GetActionRepository(request.ContractNumber, request.Context, request.UserId);

            result = repo.FindByID(request.ActionID) as GetActionDataResponse;

            return (result != null ? result : new GetActionDataResponse());
        }

        public GetAllActionsDataResponse GetActionsList(GetAllActionsDataRequest request)
        {
            GetAllActionsDataResponse result = new GetAllActionsDataResponse();

            MongoActionRepository<GetAllActionsDataResponse> repo = new MongoActionRepository<GetAllActionsDataResponse>(request.ContractNumber);
            repo.UserId = request.UserId;

            result = repo.SelectAll(request.Version, Common.Status.Active);

            return result;
        }

        public GetAllActiveProgramsResponse GetAllActiveContractPrograms(GetAllActiveProgramsRequest request)
        {
            try
            {
                GetAllActiveProgramsResponse response = new GetAllActiveProgramsResponse();
                List<ProgramInfo> result;

                IProgramDesignRepository<GetAllActiveProgramsResponse> repo =
                    ProgramDesignRepositoryFactory<GetAllActiveProgramsResponse>.GetContractProgramRepository(request.ContractNumber, request.Context, request.UserId);

                result = repo.GetActiveProgramsInfoList(request);
                response.Programs = result;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetAllActiveContractPrograms()::" + ex.Message, ex.InnerException);
            }
        }

        public GetAllModulesResponse GetModuleList(GetAllModulesRequest request)
        {
            GetAllModulesResponse result = new GetAllModulesResponse();

            MongoModuleRepository<GetAllModulesResponse> repo = new MongoModuleRepository<GetAllModulesResponse>(request.ContractNumber);
            repo.UserId = request.UserId;

            result = repo.SelectAll(request.Version, Common.Status.Active);

            return result;
        }

        public GetAllProgramDesignsResponse GetProgramDesignList(GetAllProgramDesignsRequest request)
        {
            try
            {
                GetAllProgramDesignsResponse result = new GetAllProgramDesignsResponse();

                IProgramDesignRepository<GetAllProgramDesignsResponse> repo = ProgramDesignRepositoryFactory<GetAllProgramDesignsResponse>.GetProgramDesignRepository(request.ContractNumber, request.Context, request.UserId);
                repo.UserId = request.UserId;
                result = repo.SelectAll() as GetAllProgramDesignsResponse;

                return (result != null ? result : new GetAllProgramDesignsResponse());
            }
            catch (Exception ex)
            {
                throw new Exception("ProgramDesignDD:GetProgramDesignList()::" + ex.Message, ex.InnerException);
            }
        }

        //public GetAllProgramsResponse

        public GetAllTextStepDataResponse GetAllTextSteps(GetAllTextStepDataRequest request)
        {
            GetAllTextStepDataResponse result = new GetAllTextStepDataResponse();

            IProgramDesignRepository<GetAllTextStepDataResponse> repo = ProgramDesignRepositoryFactory<GetAllTextStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, "text");
            repo.UserId = request.UserId;
            result = repo.SelectAll() as GetAllTextStepDataResponse;

            return result;
        }

        public GetAllYesNoStepDataResponse GetAllYesNoSteps(GetAllYesNoStepDataRequest request)
        {
            GetAllYesNoStepDataResponse result = new GetAllYesNoStepDataResponse();

            IProgramDesignRepository<GetAllYesNoStepDataResponse> repo = ProgramDesignRepositoryFactory<GetAllYesNoStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, yesnostep);
            repo.UserId = request.UserId;
            result = repo.SelectAll() as GetAllYesNoStepDataResponse;

            return result;
        }

        //public GetContractProgramResponse

        public GetModuleResponse GetModuleByID(GetModuleRequest request)
        {
            GetModuleResponse result = new GetModuleResponse();

            IProgramDesignRepository<GetModuleResponse> repo = ProgramDesignRepositoryFactory<GetModuleResponse>.GetModuleRepository(request.ContractNumber, request.Context, request.UserId);

            var module = repo.FindByID(request.ModuleID);
            result.Module = module as DTO.Module;

            return (result != null ? result : new GetModuleResponse());
        }

        //public GetProgramAttributeResponse GetProgramAttributes(GetProgramAttributeRequest request)
        //{
        //    try
        //    {
        //        GetProgramAttributeResponse response = new GetProgramAttributeResponse();
        //        ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

        //        // PlanElementId
        //        SelectExpression patientSelectExpression = new SelectExpression();
        //        patientSelectExpression.FieldName = MEProgramAttribute.PlanElementIdProperty;
        //        patientSelectExpression.Type = SelectExpressionType.EQ;
        //        patientSelectExpression.Value = request.PlanElementId;
        //        patientSelectExpression.ExpressionOrder = 1;
        //        patientSelectExpression.GroupID = 1;
        //        selectExpressions.Add(patientSelectExpression);

        //        APIExpression apiExpression = new APIExpression();
        //        apiExpression.Expressions = selectExpressions;

        //        IProgramDesignRepository<GetProgramAttributeResponse> repo =
        //            ProgramDesignRepositoryFactory<GetProgramAttributeResponse>.GetProgramAttributesRepository(request.ContractNumber, request.Context, request.UserId);

        //        Tuple<string, IEnumerable<object>> result = repo.Select(apiExpression);

        //        if (result != null)
        //        {
        //            List<ProgramAttribute> pds = result.Item2.Cast<ProgramAttribute>().ToList();
        //            if (pds.Count > 0)
        //            {
        //                response.ProgramAttribute = pds.FirstOrDefault();
        //            }
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DataProgramManager:GetProgramAttributes()::" + ex.Message, ex.InnerException);
        //    }
        //}

        public GetProgramByNameResponse GetProgramByName(GetProgramByNameRequest request)
        {
            try
            {
                GetProgramByNameResponse programResponse = new GetProgramByNameResponse();
                DTO.Program result;

                IProgramDesignRepository<GetProgramResponse> repo = ProgramDesignRepositoryFactory<GetProgramResponse>.GetProgramRepository(request.ContractNumber, request.Context, request.UserId);

                result = repo.FindByName(request.ProgramName) as DTO.Program;

                programResponse.Program = result;
                return (programResponse != null ? programResponse : new GetProgramByNameResponse());
            }
            catch (Exception ex)
            {
                throw new Exception("DD:DataProgramManager:GetProgramByName()::" + ex.Message, ex.InnerException);
            }
        }

        public GetProgramDesignResponse GetProgramDesignByID(GetProgramDesignRequest request)
        {
            try
            {
                GetProgramDesignResponse pDesignResponse = new GetProgramDesignResponse();
                DTO.ProgramDesign result;

                IProgramDesignRepository<GetProgramDesignResponse> repo = ProgramDesignRepositoryFactory<GetProgramDesignResponse>.GetProgramDesignRepository(request.ContractNumber, request.Context, request.UserId);
                repo.UserId = request.UserId;
                result = repo.FindByID(request.ProgramDesignID) as DTO.ProgramDesign;

                return (result != null ? pDesignResponse : new GetProgramDesignResponse());
            }
            catch (Exception ex)
            { 
                throw new Exception("ProgramDesignDD:GetProgramDesignByID()::" + ex.Message, ex.InnerException); 
            }
        }

        //public GetProgramDetailsSummaryResponse GetPatientProgramDetailsById(GetProgramDetailsSummaryRequest request)
        //{
        //    try
        //    {
        //        GetProgramDetailsSummaryResponse response = new GetProgramDetailsSummaryResponse();

        //        IProgramRepository<GetProgramDetailsSummaryResponse> repo =
        //            Phytel.API.DataDomain.Program.ProgramRepositoryFactory<GetProgramDetailsSummaryResponse>
        //            .GetPatientProgramRepository(request.ContractNumber, request.Context, request.UserId);

        //        MEPatientProgram mepp = repo.FindByID(request.ProgramId) as MEPatientProgram;

        //        response.Program = new ProgramDetail
        //        {
        //            Id = mepp.Id.ToString(),
        //            Client = mepp.Client != null ? mepp.Client.ToString() : null,
        //            ContractProgramId = mepp.ContractProgramId.ToString(),
        //            Description = mepp.Description,
        //            Name = mepp.Name,
        //            PatientId = mepp.PatientId.ToString(),
        //            ProgramState = (int)mepp.ProgramState,
        //            ShortName = mepp.ShortName,
        //            StartDate = mepp.StartDate,
        //            Status = (int)mepp.Status,
        //            Version = mepp.Version,
        //            EndDate = mepp.EndDate,
        //            Completed = mepp.Completed,
        //            Enabled = mepp.Enabled,
        //            Next = mepp.Next != null ? mepp.Next.ToString() : string.Empty,
        //            Order = mepp.Order,
        //            Previous = mepp.Previous != null ? mepp.Previous.ToString() : string.Empty,
        //            SourceId = mepp.SourceId.ToString(),
        //            AssignBy = mepp.AssignedBy,
        //            AssignDate = mepp.AssignedOn,
        //            ElementState = (int)mepp.State,
        //            CompletedBy = mepp.CompletedBy,
        //            DateCompleted = mepp.DateCompleted,
        //            ObjectivesInfo = DTOUtils.GetObjectives(mepp.Objectives),
        //            SpawnElement = DTOUtils.GetSpawnElement(mepp),
        //            Modules = DTOUtils.GetModules(mepp.Modules, request.ContractNumber, request.UserId)
        //        };

        //        // load responses

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DataProgramManager:GetPatientProgramDetailsById()::" + ex.Message, ex.InnerException);
        //    }
        //}

        public GetProgramResponse GetProgramByID(GetProgramRequest request)
        {
            try
            {
                GetProgramResponse programResponse = new GetProgramResponse();
                DTO.Program result;

                IProgramDesignRepository<GetProgramResponse> repo = ProgramDesignRepositoryFactory<GetProgramResponse>.GetProgramRepository(request.ContractNumber, request.Context, request.UserId);

                result = repo.FindByID(request.ProgramID) as DTO.Program;

                programResponse.Program = result;
                return (programResponse != null ? programResponse : new GetProgramResponse());
            }
            catch (Exception ex)
            {
                throw new Exception("DD:ProgramDesignDataManager:GetProgramByID()::" + ex.Message, ex.InnerException);
            }
        }

        //public GetStepResponseListResponse GetStepResponse(GetStepResponseListRequest request)
        //{
        //    try
        //    {
        //        GetStepResponseListResponse response = null;
        //        response = DTOUtils.GetStepResponses(request.StepId, request.ContractNumber, true, request.UserId);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:DataProgramManager:GetStepResponse()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //public GetStepResponseResponse GetStepResponse(GetStepResponseRequest request)
        //{
        //    try
        //    {
        //        GetStepResponseResponse response = new GetStepResponseResponse();

        //        IProgramDesignRepository<GetStepResponseResponse> repo = ProgramDesignRepositoryFactory<GetStepResponseResponse>.GetPatientProgramStepResponseRepository(request.ContractNumber, request.Context, request.UserId);

        //        MEPatientProgramResponse result = repo.FindByID(request.ResponseId) as MEPatientProgramResponse;

        //        if (result != null)
        //        {
        //            response.StepResponse = new StepResponse
        //            {
        //                Id = result.Id.ToString(),
        //                NextStepId = result.NextStepId.ToString(),
        //                Nominal = result.Nominal,
        //                Order = result.Order,
        //                Required = result.Required,
        //                Spawn = DTOUtils.GetResponseSpawnElement(result.Spawn),
        //                StepId = result.StepId.ToString(),
        //                Text = result.Text,
        //                Value = result.Value
        //            };
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("DD:ProgramDesignDataManager:GetStepResponse()::" + ex.Message, ex.InnerException);
        //    }
        //}

        public GetTextStepDataResponse GetTextStepByID(GetTextStepDataRequest request)
        {
            GetTextStepDataResponse result = new GetTextStepDataResponse();

            IProgramDesignRepository<GetTextStepDataResponse> repo = ProgramDesignRepositoryFactory<GetTextStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, textstep);
            repo.UserId = request.UserId;
            result = repo.FindByID(request.TextStepID) as GetTextStepDataResponse;

            return (result != null ? result : new GetTextStepDataResponse());
        }

        public GetYesNoStepDataResponse GetYesNoStepByID(GetYesNoStepDataRequest request)
        {
            GetYesNoStepDataResponse result = new GetYesNoStepDataResponse();

            IProgramDesignRepository<GetYesNoStepDataResponse> repo = ProgramDesignRepositoryFactory<GetYesNoStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, yesnostep);
            repo.UserId = request.UserId;
            result = repo.FindByID(request.YesNoStepID) as GetYesNoStepDataResponse;

            return (result != null ? result : new GetYesNoStepDataResponse());
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
                throw new Exception("DD:ProgramDesignDataManager:ParseSpawnElements()::" + ex.Message, ex.InnerException);
            }
        }

        //public PutProgramActionProcessingResponse

        //public PutProgramAttributesResponse

        //public PutUpdateProgramAttributesResponse

        //public PutUpdateResponseResponse
    }
}   
