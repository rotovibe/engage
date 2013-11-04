using Phytel.API.DataDomain.Patient;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.API.DataDomain.Patient.Service
{
    public class PatientService : ServiceStack.ServiceInterface.Service
    {
        public object Any(PatientDataRequest request)
        {
            PatientDataResponse vResponse;
            
            vResponse = PatientDataManager.LoginUser(request);

            return vResponse;
        }
    }
}