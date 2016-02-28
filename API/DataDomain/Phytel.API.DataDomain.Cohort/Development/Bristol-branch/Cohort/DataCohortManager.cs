using Phytel.API.DataDomain.Cohort.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Cohort;
using System.Linq;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.Cohort
{
    public static class DataCohortManager
    {
        public static GetCohortDataResponse GetCohortByID(GetCohortDataRequest request)
        {
            GetCohortDataResponse response = new GetCohortDataResponse();

            ICohortRepository<GetCohortDataResponse> repo = CohortRepositoryFactory<GetCohortDataResponse>.GetCohortRepository(request.ContractNumber, request.Context, request.UserId);

            response = repo.FindByID(request.CohortID) as GetCohortDataResponse;

            return response;
        }

        public static GetAllCohortsDataResponse GetCohorts(GetAllCohortsDataRequest request)
        {
            GetAllCohortsDataResponse response = new GetAllCohortsDataResponse();

            ICohortRepository<API.DataDomain.Cohort.DTO.CohortData> repo = CohortRepositoryFactory<API.DataDomain.Cohort.DTO.CohortData>.GetCohortRepository(request.ContractNumber, request.Context, request.UserId);

            List<API.DataDomain.Cohort.DTO.CohortData> cohorts = repo.SelectAll() as List<API.DataDomain.Cohort.DTO.CohortData>;

            if (cohorts != null)
            {
                response.Cohorts = cohorts.ToList();
            }
            return response;
        }
    }
}   
