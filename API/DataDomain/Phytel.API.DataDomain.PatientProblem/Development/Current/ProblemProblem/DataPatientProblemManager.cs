using Phytel.API.DataDomain.PatientProblem.DTO;
using System.Data.SqlClient;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;
using System.Linq;

namespace Phytel.API.DataDomain.PatientProblem
{
    public static class DataPatientProblemManager
    {
        public static PatientProblemsResponse GetProblemsByPatientID(PatientProblemRequest request)
        {

            PatientProblemsResponse response = new PatientProblemsResponse();

            IPatientProblemRepository<Problem> repo = Phytel.API.DataDomain.PatientProblem.PatientProblemRepositoryFactory<Problem>.GetPatientProblemRepository(request.ContractNumber, request.Context);
            
            APIExpression apiExpression = new APIExpression();
            // expressionID will be used as a unique cacheKey for caching the query.
            string expressionID = Guid.NewGuid().ToString();
            apiExpression.ExpressionID = expressionID;

            ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

            SelectExpression patientSelectExpression = new SelectExpression();
            patientSelectExpression.FieldName = MEPatientProblem.PatientIDProperty;
            patientSelectExpression.Type = SelectExpressionType.EQ;
            patientSelectExpression.Value = request.PatientID;
            patientSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
            patientSelectExpression.ExpressionOrder = 1;
            patientSelectExpression.GroupID = 1;
            selectExpressions.Add(patientSelectExpression);

            SelectExpression statusSelectExpression = new SelectExpression();
            statusSelectExpression.FieldName = MEPatientProblem.StatusProperty;
            statusSelectExpression.Type = SelectExpressionType.EQ;
            statusSelectExpression.Value = (int)request.Status;
            statusSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
            statusSelectExpression.ExpressionOrder = 2;
            statusSelectExpression.GroupID = 1;
            selectExpressions.Add(statusSelectExpression);

            SelectExpression categorySelectExpression = new SelectExpression();
            categorySelectExpression.FieldName = MEPatientProblem.CategoryProperty;
            categorySelectExpression.Type = SelectExpressionType.EQ;
            categorySelectExpression.Value = (int)request.Category;
            categorySelectExpression.ExpressionOrder = 3;
            categorySelectExpression.GroupID = 1;
            selectExpressions.Add(categorySelectExpression);

            apiExpression.Expressions = selectExpressions;

            Tuple<string, IQueryable<Problem>> problems = repo.Select(apiExpression);

            if (problems != null)
            {
                response.PatientProblems = problems.Item2.ToList();
            }
            return response;
        }
    }
}   
