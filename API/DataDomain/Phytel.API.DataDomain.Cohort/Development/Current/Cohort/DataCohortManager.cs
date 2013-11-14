using Phytel.API.DataDomain.Cohort.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Cohort;

namespace Phytel.API.DataDomain.Cohort
{
    public static class CohortDataManager
    {
        public static CohortResponse GetCohortByID(CohortRequest request)
        {
            CohortResponse result = new CohortResponse();

            ICohortRepository<CohortResponse> repo = CohortRepositoryFactory<CohortResponse>.GetCohortRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.CohortID) as CohortResponse;
            
            return (result != null ? result : new CohortResponse());
        }

        public static CohortListResponse GetCohortList(CohortListRequest request)
        {
            CohortListResponse result = new CohortListResponse();

            ICohortRepository<CohortListResponse> repo = CohortRepositoryFactory<CohortListResponse>.GetCohortRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
