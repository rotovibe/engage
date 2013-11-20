using Phytel.API.DataDomain.Cohort.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Cohort;
using System.Linq;

namespace Phytel.API.DataDomain.Cohort
{
    public static class DataCohortManager
    {
        public static GetCohortResponse GetCohortByID(GetCohortRequest request)
        {
            GetCohortResponse response = new GetCohortResponse();

            ICohortRepository<GetCohortResponse> repo = CohortRepositoryFactory<GetCohortResponse>.GetCohortRepository(request.ContractNumber, request.Context);
            response = repo.FindByID(request.CohortID) as GetCohortResponse;

            return response;
        }

        public static GetAllCohortsResponse GetCohorts(GetAllCohortsRequest request)
        {
            GetAllCohortsResponse response = new GetAllCohortsResponse();

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
