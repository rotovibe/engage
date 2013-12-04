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
        private static readonly string PROBLEMLOOKUP = "problemlookup";

        public static GetProblemDataResponse GetProblem(GetProblemDataRequest request)
        {
            GetProblemDataResponse response = new GetProblemDataResponse();

            ILookUpRepository<GetProblemDataResponse> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<GetProblemDataResponse>.GetLookUpRepository(request.ContractNumber, request.Context, PROBLEMLOOKUP);
            response = repo.FindByID(request.ProblemID) as GetProblemDataResponse;
            return response;
        }

        public static GetAllProblemsDataResponse GetAllProblem(GetAllProblemsDataRequest request)
        {
            GetAllProblemsDataResponse response = new GetAllProblemsDataResponse();

            ILookUpRepository<ProblemData> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<ProblemData>.GetLookUpRepository(request.ContractNumber, request.Context, PROBLEMLOOKUP);
            IQueryable<ProblemData> problems = repo.SelectAll() as IQueryable<ProblemData>;

            if (problems != null)
            {
                response.Problems = problems.ToList();
            }
            return response;
        }

        public static SearchProblemsDataResponse SearchProblem(SearchProblemsDataRequest request)
        {
            SearchProblemsDataResponse response = new SearchProblemsDataResponse();

            ILookUpRepository<ProblemData> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<ProblemData>.GetLookUpRepository(request.ContractNumber, request.Context, PROBLEMLOOKUP);

            ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

            // Active property
            SelectExpression activeSelectExpression = new SelectExpression();
            activeSelectExpression.FieldName = MEProblem.ActiveProperty;
            activeSelectExpression.Type = SelectExpressionType.EQ;
            activeSelectExpression.Value = request.Active;
            activeSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
            activeSelectExpression.ExpressionOrder = 1;
            activeSelectExpression.GroupID = 1;
            selectExpressions.Add(activeSelectExpression);

            // Type
            if (!string.IsNullOrEmpty(request.Type))
            {
                SelectExpression categorySelectExpression = new SelectExpression();
                categorySelectExpression.FieldName = MEProblem.TypeProperty;
                categorySelectExpression.Type = SelectExpressionType.EQ;
                categorySelectExpression.Value = request.Type;
                categorySelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
                categorySelectExpression.ExpressionOrder = 2;
                categorySelectExpression.GroupID = 1;
                selectExpressions.Add(categorySelectExpression);
            }

            APIExpression apiExpression = new APIExpression();
            apiExpression.Expressions = selectExpressions;

            Tuple<string, IQueryable<ProblemData>> problems = repo.Select(apiExpression);

            if (problems != null)
            {
                response.Problems = problems.Item2.ToList();
            }
            return response;
        }



    }
}   
