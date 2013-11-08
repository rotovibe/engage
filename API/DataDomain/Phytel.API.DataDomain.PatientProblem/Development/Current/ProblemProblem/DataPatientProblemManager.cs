using Phytel.API.DataDomain.PatientProblem.DTO;
using System.Data.SqlClient;
using System.Collections.Generic;
using Phytel.API.Interface;
using System;
using System.Linq;

namespace Phytel.API.DataDomain.PatientProblem
{
    public static class PatientProblemDataManager
    {
        public static PatientProblemsResponse GetProblemsByPatientID(PatientProblemRequest request)
        {

            PatientProblemsResponse response = new PatientProblemsResponse();

            IPatientProblemRepository<Problem> repo = Phytel.API.DataDomain.PatientProblem.PatientProblemRepositoryFactory<Problem>.GetPatientProblemRepository(request.ContractNumber, request.Context);
            
            APIExpression apiExpression = new APIExpression();

            SelectExpression patientSelectExpression = new SelectExpression();
            patientSelectExpression.FieldName = "PatientID";
            patientSelectExpression.Type = SelectExpressionType.EQ;
            patientSelectExpression.Value = request.PatientID;
            patientSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
            patientSelectExpression.ExpressionOrder = 1;

            SelectExpression statusSelectExpression = new SelectExpression();
            statusSelectExpression.FieldName = "Status";
            statusSelectExpression.Type = SelectExpressionType.EQ;
            statusSelectExpression.Value = request.Status;
            statusSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
            statusSelectExpression.ExpressionOrder = 2;

            SelectExpression categorySelectExpression = new SelectExpression();
            categorySelectExpression.FieldName = "Category";
            categorySelectExpression.Type = SelectExpressionType.EQ;
            categorySelectExpression.Value = request.Category;
            categorySelectExpression.ExpressionOrder = 3;


            Tuple<int, IQueryable<Problem>> problems = repo.Select(apiExpression);

            if (problems != null)
            {
                response.PatientProblems = problems.Item2.ToList();
            }
            return response;
        }
    }
}   
