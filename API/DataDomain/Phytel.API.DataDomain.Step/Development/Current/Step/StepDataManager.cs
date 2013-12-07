using Phytel.API.DataDomain.Step.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Step;

namespace Phytel.API.DataDomain.Step
{
    public static class StepDataManager
    {

        private static readonly string YESNO = "yesno";
        private static readonly string TEXT = "text";
        
        public static GetYesNoDataResponse GetStepByID(GetYesNoDataRequest request)
        {
            GetYesNoDataResponse result = new GetYesNoDataResponse();

            IStepRepository<GetYesNoDataResponse> repo = StepRepositoryFactory<GetYesNoDataResponse>.GetStepRepository(request.ContractNumber, request.Context, YESNO);
            result = repo.FindByID(request.YesNoID) as GetYesNoDataResponse;

            return (result != null ? result : new GetYesNoDataResponse());
        }

        //public static GetTextDataResponse GetStepByID(GetTextDataRequest request)
        //{
        //    GetTextDataResponse result = new GetTextDataResponse();

        //    IStepRepository<GetTextDataResponse> repo = StepRepositoryFactory<GetTextDataResponse>.GetStepRepository(request.ContractNumber, request.Context, TEXT);
        //    result = repo.FindByID(request.StepID) as GetTextDataResponse;

        //    return (result != null ? result : new GetTextDataResponse());
        //}
    }
}   
