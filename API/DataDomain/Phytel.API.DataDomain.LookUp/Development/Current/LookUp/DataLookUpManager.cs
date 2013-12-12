using Phytel.API.DataDomain.LookUp.DTO;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.Interface;
using System;
using DataDomain.LookUp.DTO;

namespace Phytel.API.DataDomain.LookUp
{
    public static class LookUpDataManager
    {
        public static GetProblemDataResponse GetProblem(GetProblemDataRequest request)
        {
            GetProblemDataResponse response = new GetProblemDataResponse();

            ILookUpRepository<GetProblemDataResponse> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<GetProblemDataResponse>.GetLookUpRepository(request.ContractNumber, request.Context);
            response = repo.FindProblemByID(request.ProblemID) as GetProblemDataResponse;
            return response;
        }

        public static GetAllProblemsDataResponse GetAllProblems(GetAllProblemsDataRequest request)
        {
            GetAllProblemsDataResponse response = new GetAllProblemsDataResponse();

            ILookUpRepository<ProblemData> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<ProblemData>.GetLookUpRepository(request.ContractNumber, request.Context);
            IQueryable<ProblemData> problems = repo.GetAllProblems() as IQueryable<ProblemData>;

            if (problems != null)
            {
                response.Problems = problems.ToList();
            }
            return response;
        }

        public static SearchProblemsDataResponse SearchProblem(SearchProblemsDataRequest request)
        {
            SearchProblemsDataResponse response = new SearchProblemsDataResponse();

            ILookUpRepository<ProblemData> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<ProblemData>.GetLookUpRepository(request.ContractNumber, request.Context);
            IQueryable<ProblemData> problems = repo.SearchProblem(request) as IQueryable<ProblemData>;

            if (problems != null)
            {
                response.Problems = problems.ToList();
            }
            return response;
        }



    }
}   
