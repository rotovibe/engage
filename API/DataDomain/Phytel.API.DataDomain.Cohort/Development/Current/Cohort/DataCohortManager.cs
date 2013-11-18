using Phytel.API.DataDomain.Cohort.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Cohort;
using System.Linq;

namespace Phytel.API.DataDomain.Cohort
{
    public static class DataCohortManager
    {
        public static CohortResponse GetCohortByID(GetCohortRequest request)
        {
            CohortResponse response = new CohortResponse();

            ICohortRepository<CohortResponse> repo = CohortRepositoryFactory<CohortResponse>.GetCohortRepository(request.ContractNumber, request.Context);
            response = repo.FindByID(request.CohortID) as CohortResponse;

            return response;
        }

        public static CohortsResponse GetCohorts(GetAllCohortRequest request)
        {
            CohortsResponse response = new CohortsResponse();

            ICohortRepository<API.DataDomain.Cohort.DTO.Cohort> repo = CohortRepositoryFactory<API.DataDomain.Cohort.DTO.Cohort>.GetCohortRepository(request.ContractNumber, request.Context);

            IQueryable<API.DataDomain.Cohort.DTO.Cohort> cohorts = repo.SelectAll() as IQueryable<API.DataDomain.Cohort.DTO.Cohort>;

            if (cohorts != null)
            {
                response.Cohorts = cohorts.ToList();
            }
            return response;
        }
    }
}   
