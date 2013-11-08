using System.Collections.Generic;
using Phytel.API.DataDomain.PatientProblem;
using Phytel.API.DataDomain.PatientProblem.DTO;

namespace Phytel.API.DataDomain.PatientProblem.Service
{
    public class PatientProblemService : ServiceStack.ServiceInterface.Service
    {
        public List<PatientProblemResponse> Get(PatientProblemRequest request)
        {
            List<PatientProblemResponse> response = PatientProblemDataManager.GetProblemsByPatientID(request);
            return response;
        }
    }
}