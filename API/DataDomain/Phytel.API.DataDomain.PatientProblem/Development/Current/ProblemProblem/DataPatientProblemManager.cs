using Phytel.API.DataDomain.PatientProblem.DTO;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Phytel.API.DataDomain.PatientProblem
{
    public static class PatientProblemDataManager
    {
        public static List<PatientProblemResponse> GetProblemsByPatientID(PatientProblemRequest request)
        {
            List<PatientProblemResponse> result = new List<PatientProblemResponse>();

            IPatientProblemRepository<PatientProblemResponse> repo = Phytel.API.DataDomain.PatientProblem.PatientProblemRepositoryFactory<PatientProblemResponse>.GetPatientProblemRepository(request.ContractNumber, request.Context);
            
            List <MEPatientProblem > mePatientProblem = null;//repo.Select(request.PatientID) as MEPatientProblem;

            foreach(MEPatientProblem p in mePatientProblem)
            {
                PatientProblemResponse patientProblem = new PatientProblemResponse
                {
                    DisplayName = p.DisplayName,
                    PatientID = p.PatientID,
                    ProblemID = p.Id.ToString()
                };
                result.Add(patientProblem);
            }
            return result;
        }
    }
}   
