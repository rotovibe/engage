using System.Collections.Generic;
using System.Configuration;

using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.Common;

namespace Phytel.Engage.Integrations.UOW
{
    public class EndPointUtil : IEndpointUtil
    {
        protected readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemUrl"];
        protected readonly string DDPatientServiceUrl = ConfigurationManager.AppSettings["DDPatientServiceUrl"];

        public List<HttpObjectResponse<PatientData>> SavePatientInfo<T>(T patients, string contract)
        {
            var userid = "5602f0f384ac071c989477cf"; // need to find a valid session id.
            IRestClient client = new JsonServiceClient();
            //"/{Context}/{Version}/{ContractNumber}/Batch/Patients"
            var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Batch/Patients/", DDPatientServiceUrl, "NG", 1, contract), "userid");

            InsertBatchPatientsDataResponse response = client.Post<InsertBatchPatientsDataResponse>(url,
                new InsertBatchPatientsDataRequest
                {
                    Context = "NG",
                    ContractNumber = contract,
                    PatientsData = patients as List<PatientData>,
                    UserId = "userid",
                    Version = 1
                });

            return response.Responses;
        }

        public List<HttpObjectResponse<PatientSystemData>> SaveSystemPatientInfo(List<PatientSystemData> patientSystems)
        {
            return null;
        }
    }
}
