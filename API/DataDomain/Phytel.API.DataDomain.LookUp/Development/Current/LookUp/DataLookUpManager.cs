using Phytel.API.DataDomain.LookUp.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Phytel.API.DataDomain.LookUp
{
    public static class LookUpDataManager
    {
        private static readonly string PROBLEMLOOKUP = "problemlookup";

        public static GetProblemResponse GetProblemByID(GetProblemRequest request)
        {
            GetProblemResponse response = new GetProblemResponse();

            ILookUpRepository<GetProblemResponse> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<GetProblemResponse>.GetLookUpRepository(request.ContractNumber, request.Context, PROBLEMLOOKUP);
            response = repo.FindByID(request.ProblemID) as GetProblemResponse;
            return response;
        }

        public static GetAllProblemResponse GetAllProblem(GetAllProblemRequest request)
        {
            GetAllProblemResponse response = new GetAllProblemResponse();

            ILookUpRepository<Problem> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<Problem>.GetLookUpRepository(request.ContractNumber, request.Context, PROBLEMLOOKUP);
            IQueryable<Problem> problems = repo.SelectAll() as IQueryable<Problem>;

            if (problems != null)
            {
                response.Problems = problems.ToList();
            }
            return response;
        }
    }
}   
