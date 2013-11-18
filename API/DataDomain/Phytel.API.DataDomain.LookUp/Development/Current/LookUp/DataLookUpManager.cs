using Phytel.API.DataDomain.LookUp.DTO;
using System.Collections.Generic;
using System.Linq;

namespace Phytel.API.DataDomain.LookUp
{
    public static class LookUpDataManager
    {
        private static readonly string PROBLEMLOOKUP = "problemlookup";

        public static ProblemResponse GetProblemByID(GetProblemRequest request)
        {
            ProblemResponse response = new ProblemResponse();

            ILookUpRepository<ProblemResponse> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<ProblemResponse>.GetLookUpRepository(request.ContractNumber, request.Context, PROBLEMLOOKUP);
            response = repo.FindByID(request.ProblemID) as ProblemResponse;
            return response;
        }

        public static ProblemsResponse FindProblems(FindProblemsRequest request)
        {
            ProblemsResponse response = new ProblemsResponse();

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
