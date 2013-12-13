using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.Interface;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using DataDomain.LookUp.DTO;

namespace Phytel.API.DataDomain.PatientProblem
{
    public static class DataPatientProblemManager
    {
        #region endpoint addresses
        private static readonly string DDLookUpServiceUrl = ConfigurationManager.AppSettings["DDLookUpServiceUrl"];
        #endregion
        
        public static GetAllPatientProblemsDataResponse GetAllPatientProblem(GetAllPatientProblemsDataRequest request)
        {
            GetAllPatientProblemsDataResponse response = null;

            // Call LookUp data domain to fetch all active problems.
            IRestClient client = new JsonServiceClient();
            SearchProblemsDataResponse problemLookUpResponse = client.Post<SearchProblemsDataResponse>
                (string.Format("{0}/{1}/{2}/{3}/problems",
                   DDLookUpServiceUrl,
                    request.Context,
                    request.Version,
                    request.ContractNumber
                    ),
                new SearchProblemsDataRequest {  
                    Active = true,
                    Context = request.Context,
                    Version = request.Version,
                    ContractNumber = request.ContractNumber
                } as object);

            if (problemLookUpResponse != null)
            {
                List<string> activeProblemIDs = new List<string>();
                List<ProblemData> problems = problemLookUpResponse.Problems;
                foreach (ProblemData p in problems)
                {
                    activeProblemIDs.Add(p.ProblemID);
                }

          
                IPatientProblemRepository<PatientProblemData> repo = Phytel.API.DataDomain.PatientProblem.PatientProblemRepositoryFactory<PatientProblemData>.GetPatientProblemRepository(request.ContractNumber, request.Context);
            
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


                // Active = true.
                SelectExpression activeSelectExpression = new SelectExpression();
                activeSelectExpression.FieldName = MEPatientProblem.ActiveProperty;
                activeSelectExpression.Type = SelectExpressionType.EQ;
                activeSelectExpression.Value = true;
                activeSelectExpression.ExpressionOrder = 2;
                activeSelectExpression.GroupID = 1;
                selectExpressions.Add(activeSelectExpression);
            
                // DeleteFlag = false.
                // This is not passed through the request object. But user story demands that only Problems set to DeleteFlag == false should be displayed to the end user.
                SelectExpression deleteFlagSelectExpression = new SelectExpression();
                deleteFlagSelectExpression.FieldName = MEPatientProblem.DeleteFlagProperty;
                deleteFlagSelectExpression.Type = SelectExpressionType.EQ;
                deleteFlagSelectExpression.Value = false;
                deleteFlagSelectExpression.ExpressionOrder = 3;
                deleteFlagSelectExpression.GroupID = 1;
                selectExpressions.Add(deleteFlagSelectExpression);

                // Active problems
                SelectExpression problemIDsSelectExpression = new SelectExpression();
                problemIDsSelectExpression.FieldName = MEPatientProblem.ProblemIDProperty;
                problemIDsSelectExpression.Type = SelectExpressionType.IN;
                problemIDsSelectExpression.Value = activeProblemIDs;
                problemIDsSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
                problemIDsSelectExpression.ExpressionOrder = 4;
                problemIDsSelectExpression.GroupID = 1;
                selectExpressions.Add(problemIDsSelectExpression);

                // Primary and secondary problems
                List<int> levels = new List<int>();
                levels.Add(1);
                levels.Add(2);
                SelectExpression levelSelectExpression = new SelectExpression();
                levelSelectExpression.FieldName = MEPatientProblem.LevelProperty;
                levelSelectExpression.Type = SelectExpressionType.IN;
                levelSelectExpression.Value = levels;
                levelSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
                levelSelectExpression.ExpressionOrder = 5;
                levelSelectExpression.GroupID = 1;
                selectExpressions.Add(levelSelectExpression);

                APIExpression apiExpression = new APIExpression();
                apiExpression.Expressions = selectExpressions;

                Tuple<string, IQueryable<object>> patientProblems = repo.Select(apiExpression);

                if (patientProblems != null)
                {
                    response = new GetAllPatientProblemsDataResponse();
                    response.PatientProblems = patientProblems.Item2.Cast<PatientProblemData>().ToList();
                }
            }
            return response;
        }
    }
}   
