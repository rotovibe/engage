using Phytel.API.DataDomain.Patient;
using Phytel.API.DataDomain.Patient.DTO;

namespace Phytel.API.DataDomain.Patient.Service
{
    public class PatientService : ServiceStack.ServiceInterface.Service
    {
        public PatientResponse Post(PatientRequest request)
        {
            PatientResponse response = PatientDataManager.GetPatientByID(request);
            response.Version = request.Version;
            return response;
        }

        public PatientResponse Get(PatientRequest request)
        {
            PatientResponse response = PatientDataManager.GetPatientByID(request);
            response.Version = request.Version;
            return response;
        }

        public PatientListResponse Post(PatientListRequest request)
        {
            PatientListResponse response = PatientDataManager.GetPatientList(request);
            response.Version = request.Version;
            return response;
        }

        public PatientDetailsResponse Post(PatientDetailsRequest request)
        {
            PatientDetailsResponse response = PatientDataManager.GetPatientDetailsList(request);
            response.Version = request.Version;
            return response;
        }
    }
}