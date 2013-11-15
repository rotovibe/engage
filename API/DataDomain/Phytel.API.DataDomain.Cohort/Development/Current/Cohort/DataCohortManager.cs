using Phytel.API.DataDomain.Cohort.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Cohort;

namespace Phytel.API.DataDomain.Cohort
{
    public static class CohortDataManager
    {
        public static CohortResponse GetCohortByID(GetCohortRequest request)
        {
            CohortResponse result = new CohortResponse();

            ICohortRepository<CohortResponse> repo = CohortRepositoryFactory<CohortResponse>.GetCohortRepository(request.ContractNumber, request.Context);
            result = repo.FindByID(request.CohortID) as CohortResponse;
            
            return (result != null ? result : new CohortResponse());
        }

        public static CohortsResponse GetCohorts(GetAllCohortRequest request)
        {
            CohortsResponse result = new CohortsResponse();

            ICohortRepository<CohortsResponse> repo = CohortRepositoryFactory<CohortsResponse>.GetCohortRepository(request.ContractNumber, request.Context);

            return result;
        }
    }
}   
