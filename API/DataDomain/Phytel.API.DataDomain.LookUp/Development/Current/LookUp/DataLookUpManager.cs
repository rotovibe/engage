using Phytel.API.DataDomain.LookUp.DTO;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.Interface;
using System;

namespace Phytel.API.DataDomain.LookUp
{
    public static class LookUpDataManager
    {
        private static readonly string PROBLEMLOOKUP = "problemlookup";

        public static GetProblemResponse GetPatientProblem(GetProblemRequest request)
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

        public static SearchProblemResponse SearchProblem(SearchProblemRequest request)
        {
            SearchProblemResponse response = new SearchProblemResponse();

            ILookUpRepository<Problem> repo = Phytel.API.DataDomain.LookUp.LookUpRepositoryFactory<Problem>.GetLookUpRepository(request.ContractNumber, request.Context, PROBLEMLOOKUP);

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

            Tuple<string, IQueryable<Problem>> problems = repo.Select(apiExpression);

            if (problems != null)
            {
                response.Problems = problems.Item2.ToList();
            }
            return response;
        }



    }
}   
