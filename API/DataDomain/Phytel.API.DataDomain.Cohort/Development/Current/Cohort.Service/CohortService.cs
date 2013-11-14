using Phytel.API.DataDomain.Cohort;
using Phytel.API.DataDomain.Cohort.DTO;

namespace Phytel.API.DataDomain.Cohort.Service
{
    public class CohortService : ServiceStack.ServiceInterface.Service
    {
        public CohortResponse Post(CohortRequest request)
        {
            CohortResponse response = CohortDataManager.GetCohortByID(request);
            response.Version = request.Version;
            return response;
        }

        public CohortResponse Get(CohortRequest request)
        {
            CohortResponse response = CohortDataManager.GetCohortByID(request);
            response.Version = request.Version;
            return response;
        }

        public CohortListResponse Post(CohortListRequest request)
        {
            CohortListResponse response = CohortDataManager.GetCohortList(request);
            response.Version = request.Version;
            return response;
        }
    }
}