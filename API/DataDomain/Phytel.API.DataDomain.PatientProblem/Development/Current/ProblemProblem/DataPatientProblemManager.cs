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
            
            ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

            // PatientID
            SelectExpression patientSelectExpression = new SelectExpression();
            patientSelectExpression.FieldName = MEPatientProblem.PatientIDProperty;
            patientSelectExpression.Type = SelectExpressionType.EQ;
            patientSelectExpression.Value = request.PatientID;
            patientSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
            patientSelectExpression.ExpressionOrder = 1;
            patientSelectExpression.GroupID = 1;
            selectExpressions.Add(patientSelectExpression);

            // Status
            if (!string.IsNullOrEmpty(request.Status))
            {
                SelectExpression statusSelectExpression = new SelectExpression();
                statusSelectExpression.FieldName = MEPatientProblem.ActiveProperty;
                statusSelectExpression.Type = SelectExpressionType.EQ;
                statusSelectExpression.Value = request.Status;
                statusSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
                statusSelectExpression.ExpressionOrder = 2;
                statusSelectExpression.GroupID = 1;
                selectExpressions.Add(statusSelectExpression);
            }

            // DisplayCondition.
            // This is not passed through the request object. But user story demands that only conditions set to DisplayCondition == true should be displayed to the end user.
            SelectExpression displaySelectExpression = new SelectExpression();
            displaySelectExpression.FieldName = MEPatientProblem.FeaturedProperty;
            displaySelectExpression.Type = SelectExpressionType.EQ;
            displaySelectExpression.Value = true;
            displaySelectExpression.ExpressionOrder = 4;
            displaySelectExpression.GroupID = 1;
            selectExpressions.Add(displaySelectExpression);

            APIExpression apiExpression = new APIExpression();
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
