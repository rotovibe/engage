using Phytel.API.DataDomain.Cohort;
using Phytel.API.DataDomain.Cohort.DTO;

namespace Phytel.API.DataDomain.Cohort.Service
{
    public class CohortService : ServiceStack.ServiceInterface.Service
    {
        public CohortResponse Get(GetCohortRequest request)
        {
            CohortResponse response = CohortDataManager.GetCohortByID(request);
            response.Version = request.Version;
            return response;
        }

        public CohortsResponse Get(GetAllCohortRequest request)
        {
            CohortsResponse response = CohortDataManager.GetCohorts(request);
            response.Version = request.Version;
            return response;
        }
    }
}