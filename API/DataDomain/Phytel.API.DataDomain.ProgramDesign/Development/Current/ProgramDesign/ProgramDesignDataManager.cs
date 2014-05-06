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
    public static class ProgramDesignDataManager
    {
        private static readonly string yesnostep = "yesno";
        private static readonly string textstep = "text";

        public static GetActionDataResponse GetActionByID(GetActionDataRequest request)
        {
            GetActionDataResponse result = new GetActionDataResponse();

            IProgramDesignRepository<GetActionDataResponse> repo = ProgramDesignRepositoryFactory<GetActionDataResponse>.GetActionRepository(request.ContractNumber, request.Context, request.UserId);

            result = repo.FindByID(request.ActionID) as GetActionDataResponse;

            return (result != null ? result : new GetActionDataResponse());
        }

        public static GetAllActionsDataResponse GetActionsList(GetAllActionsDataRequest request)
        {
            GetAllActionsDataResponse result = new GetAllActionsDataResponse();

            MongoActionRepository<GetAllActionsDataResponse> repo = new MongoActionRepository<GetAllActionsDataResponse>(request.ContractNumber);
            repo.UserId = request.UserId;

            result = repo.SelectAll(request.Version, Status.Active);

            return result;
        }

        public static GetAllActiveProgramsResponse GetAllActiveContractPrograms(GetAllActiveProgramsRequest request)
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

        public static GetAllModulesResponse GetModuleList(GetAllModulesRequest request)
        {
            GetAllModulesResponse result = new GetAllModulesResponse();

            MongoModuleRepository<GetAllModulesResponse> repo = new MongoModuleRepository<GetAllModulesResponse>(request.ContractNumber);
            repo.UserId = request.UserId;

            result = repo.SelectAll(request.Version, Status.Active);

            return result;
        }

        public static GetAllProgramDesignsResponse GetProgramDesignList(GetAllProgramDesignsRequest request)
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

        public static GetAllTextStepDataResponse GetAllTextSteps(GetAllTextStepDataRequest request)
        {
            GetAllTextStepDataResponse result = new GetAllTextStepDataResponse();

            IProgramDesignRepository<GetAllTextStepDataResponse> repo = ProgramDesignRepositoryFactory<GetAllTextStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, "text");
            repo.UserId = request.UserId;
            result = repo.SelectAll() as GetAllTextStepDataResponse;

            return result;
        }

        public static GetAllYesNoStepDataResponse GetAllYesNoSteps(GetAllYesNoStepDataRequest request)
        {
            GetAllYesNoStepDataResponse result = new GetAllYesNoStepDataResponse();

            IProgramDesignRepository<GetAllYesNoStepDataResponse> repo = ProgramDesignRepositoryFactory<GetAllYesNoStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, yesnostep);
            repo.UserId = request.UserId;
            result = repo.SelectAll() as GetAllYesNoStepDataResponse;

            return result;
        }

        public static GetModuleResponse GetModuleByID(GetModuleRequest request)
        {
            GetModuleResponse result = new GetModuleResponse();

            IProgramDesignRepository<GetModuleResponse> repo = ProgramDesignRepositoryFactory<GetModuleResponse>.GetModuleRepository(request.ContractNumber, request.Context, request.UserId);

            var module = repo.FindByID(request.ModuleID);
            result.Module = module as DTO.Module;

            return (result != null ? result : new GetModuleResponse());
        }

        public static GetProgramByNameResponse GetProgramByName(GetProgramByNameRequest request)
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

        public static GetProgramDesignResponse GetProgramDesignByID(GetProgramDesignRequest request)
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

        public static GetProgramResponse GetProgramByID(GetProgramRequest request)
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

        public static GetTextStepDataResponse GetTextStepByID(GetTextStepDataRequest request)
        {
            GetTextStepDataResponse result = new GetTextStepDataResponse();

            IProgramDesignRepository<GetTextStepDataResponse> repo = ProgramDesignRepositoryFactory<GetTextStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, textstep);
            repo.UserId = request.UserId;
            result = repo.FindByID(request.TextStepID) as GetTextStepDataResponse;

            return (result != null ? result : new GetTextStepDataResponse());
        }

        public static GetYesNoStepDataResponse GetYesNoStepByID(GetYesNoStepDataRequest request)
        {
            GetYesNoStepDataResponse result = new GetYesNoStepDataResponse();

            IProgramDesignRepository<GetYesNoStepDataResponse> repo = ProgramDesignRepositoryFactory<GetYesNoStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, yesnostep);
            repo.UserId = request.UserId;
            result = repo.FindByID(request.YesNoStepID) as GetYesNoStepDataResponse;

            return (result != null ? result : new GetYesNoStepDataResponse());
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
                throw new Exception("DD:ProgramDesignDataManager:ParseSpawnElements()::" + ex.Message, ex.InnerException);
            }
        }

        public static PutProgramDataResponse InsertProgram(PutProgramDataRequest request)
        {
            IProgramDesignRepository<PutProgramDataRequest> repo =
                ProgramDesignRepositoryFactory<PutProgramDataRequest>.GetProgramRepository(request.ContractNumber, request.Context, request.UserId);
            PutProgramDataResponse result = repo.Insert(request) as PutProgramDataResponse;
            return result;
        }

        public static PutUpdateProgramDataResponse UpdateProgram(PutUpdateProgramDataRequest request)
        {
            IProgramDesignRepository<PutUpdateProgramDataRequest> repo =
                ProgramDesignRepositoryFactory<PutUpdateProgramDataRequest>.GetProgramRepository(request.ContractNumber, request.Context, request.UserId);
            PutUpdateProgramDataResponse result = repo.Update(request) as PutUpdateProgramDataResponse;
            return result;
        }

        public static PutModuleDataResponse InsertModule(PutModuleDataRequest request)
        {
            IProgramDesignRepository<PutModuleDataRequest> repo = 
                ProgramDesignRepositoryFactory<PutModuleDataRequest>.GetModuleRepository(request.ContractNumber, request.Context, request.UserId);

            PutModuleDataResponse result = repo.Insert(request) as PutModuleDataResponse;
            return result;
        }

        public static PutUpdateModuleDataResponse UpdateModule(PutUpdateModuleDataRequest request)
        {
            IProgramDesignRepository<PutUpdateModuleDataRequest> repo =
                ProgramDesignRepositoryFactory<PutUpdateModuleDataRequest>.GetModuleRepository(request.ContractNumber, request.Context, request.UserId);
            PutUpdateModuleDataResponse result = repo.Update(request) as PutUpdateModuleDataResponse;
            return result;
        }

        public static PutActionDataResponse InsertAction(PutActionDataRequest request)
        {
            IProgramDesignRepository<PutActionDataRequest> repo =
                ProgramDesignRepositoryFactory<PutActionDataRequest>.GetActionRepository(request.ContractNumber, request.Context, request.UserId);

            PutActionDataResponse result = repo.Insert(request) as PutActionDataResponse;
            return result;
        }

        public static PutUpdateActionDataResponse UpdateAction(PutUpdateActionDataRequest request)
        {
            IProgramDesignRepository<PutUpdateActionDataRequest> repo =
                ProgramDesignRepositoryFactory<PutUpdateActionDataRequest>.GetActionRepository(request.ContractNumber, request.Context, request.UserId);
            PutUpdateActionDataResponse result = repo.Update(request) as PutUpdateActionDataResponse;
            return result;
        }

        public static PutTextStepDataResponse InsertTextStep(PutTextStepDataRequest request)
        {
            IProgramDesignRepository<PutTextStepDataRequest> repo =
                ProgramDesignRepositoryFactory<PutTextStepDataRequest>.GetStepRepository(request.ContractNumber, request.Context, "text");
            PutTextStepDataResponse result = repo.Insert(request) as PutTextStepDataResponse;
            return result;
        }

        public static PutUpdateTextStepDataResponse UpdateTextStep(PutUpdateTextStepDataRequest request)
        {
            IProgramDesignRepository<PutUpdateTextStepDataRequest> repo =
                ProgramDesignRepositoryFactory<PutUpdateTextStepDataRequest>.GetStepRepository(request.ContractNumber, request.Context, "text");
            PutUpdateTextStepDataResponse result = repo.Update(request) as PutUpdateTextStepDataResponse;
            return result;
        }

        public static PutYesNoStepDataResponse InsertYesNoStep(PutYesNoStepDataRequest request)
        {
            IProgramDesignRepository<PutYesNoStepDataRequest> repo =
                ProgramDesignRepositoryFactory<PutYesNoStepDataRequest>.GetStepRepository(request.ContractNumber, request.Context, "yesno");
            PutYesNoStepDataResponse result = repo.Insert(request) as PutYesNoStepDataResponse;
            return result;
        }

        public static PutUpdateYesNoStepDataResponse UpdateYesNoStep(PutUpdateYesNoStepDataRequest request)
        {
            IProgramDesignRepository<PutUpdateYesNoStepDataRequest> repo =
                ProgramDesignRepositoryFactory<PutUpdateYesNoStepDataRequest>.GetStepRepository(request.ContractNumber, request.Context, "yesno");
            PutUpdateYesNoStepDataResponse result = repo.Update(request) as PutUpdateYesNoStepDataResponse;
            return result;
        }

        public static DeleteProgramDataResponse DeleteProgram(DeleteProgramDataRequest request)
        {
            try
            {
                DeleteProgramDataResponse result = new DeleteProgramDataResponse();

                IProgramDesignRepository<DeleteProgramDataResponse> repo = ProgramDesignRepositoryFactory<DeleteProgramDataResponse>.GetProgramRepository(request.ContractNumber, request.Context, request.UserId);

                repo.Delete(request);

                result.Deleted = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteModuleDataResponse DeleteModule(DeleteModuleDataRequest request)
        {
            DeleteModuleDataResponse result = new DeleteModuleDataResponse();
            result.Deleted = false;
                   
            try
            {
                IProgramDesignRepository<DeleteModuleDataResponse> repo = ProgramDesignRepositoryFactory<DeleteModuleDataResponse>.GetModuleRepository(request.ContractNumber, request.Context, request.UserId);

                repo.Delete(request);

                result.Deleted = true;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public static DeleteActionDataResponse DeleteAction(DeleteActionDataRequest request)
        {
            try
            {
                DeleteActionDataResponse result = new DeleteActionDataResponse();

                IProgramDesignRepository<DeleteActionDataResponse> repo = ProgramDesignRepositoryFactory<DeleteActionDataResponse>.GetActionRepository(request.ContractNumber, request.Context, request.UserId);

                repo.Delete(request);

                result.Deleted = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteTextStepDataResponse DeleteTextStep(DeleteTextStepDataRequest request)
        {
            try
            {
                DeleteTextStepDataResponse result = new DeleteTextStepDataResponse();

                IProgramDesignRepository<DeleteTextStepDataResponse> repo = ProgramDesignRepositoryFactory<DeleteTextStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, "text");

                repo.Delete(request);

                result.Deleted = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DeleteYesNoStepDataResponse DeleteYesNoStep(DeleteYesNoStepDataRequest request)
        {
            try
            {
                DeleteYesNoStepDataResponse result = new DeleteYesNoStepDataResponse();

                IProgramDesignRepository<DeleteYesNoStepDataResponse> repo = ProgramDesignRepositoryFactory<DeleteYesNoStepDataResponse>.GetStepRepository(request.ContractNumber, request.Context, "yesno");

                repo.Delete(request);

                result.Deleted = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}   
