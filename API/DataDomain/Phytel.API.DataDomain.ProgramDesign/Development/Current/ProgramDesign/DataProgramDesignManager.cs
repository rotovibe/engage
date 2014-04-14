using Phytel.API.DataDomain.ProgramDesign.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.ProgramDesign;
using System;
using Phytel.API.Common.Format;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public static class ProgramDesignDataManager
    {
        public static GetProgramDesignResponse GetProgramDesignByID(GetProgramDesignRequest request)
        {
            try
            {
                GetProgramDesignResponse result = new GetProgramDesignResponse();

                IProgramDesignRepository<GetProgramDesignResponse> repo = ProgramDesignRepositoryFactory<GetProgramDesignResponse>.GetProgramDesignRepository(request.ContractNumber, request.Context, request.UserId);
                repo.UserId = request.UserId;
                result = repo.FindByID(request.ProgramDesignID) as GetProgramDesignResponse;

                return (result != null ? result : new GetProgramDesignResponse());
            }
            catch (Exception ex)
            { 
                throw new Exception("ProgramDesignDD:GetProgramDesignByID()::" + ex.Message, ex.InnerException); 
            }
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
    }
}   
