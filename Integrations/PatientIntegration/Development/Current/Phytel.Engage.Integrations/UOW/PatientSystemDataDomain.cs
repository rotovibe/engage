using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.Common;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Utils;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.Engage.Integrations.UOW
{
    public class PatientSystemDataDomain : IDataDomain
    {
        protected readonly string DDPatientSystemUrl = ProcConstants.DdPatientSystemUrl; 
        //ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["DDPatientSystemUrl"].Value;
            //ConfigurationManager.AppSettings[""];

        //List<HttpObjectResponse<PatientSystemData>>
        public object Save<T>(T patientSystems, string contract)
        {
            LoggerDomainEvent.Raise(new LogStatus { Message = "2) Sending insert patientSystem DD request.", Type = LogType.Debug });
            var l = patientSystems as List<PatientSystemData>;
            if (l != null)
                LogUtil.LogExternalRecordId("Save", l.Cast<IAppData>().ToList());

            var userid = ProcConstants.UserId; // need to find a valid session id.

            try
            {
                IRestClient client = new JsonServiceClient();
                var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Batch/PatientSystems", DDPatientSystemUrl, "NG", 1, contract), userid);

                var request = new InsertBatchPatientSystemsDataRequest
                {
                    PatientSystemsData = patientSystems as List<PatientSystemData>,
                    Context = "NG",
                    ContractNumber = contract,
                    UserId = userid,
                    Version = 1
                };

                InsertBatchPatientSystemsDataResponse dataDomainResponse = client.Post<InsertBatchPatientSystemsDataResponse>(url, (object)request );

                //new Helpers().SerializeObject<List<PatientSystemData>>(request.PatientSystemsData, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PatientsSystemExample.txt");
                //var lPsd = Helpers.DeserializeObject<List<PatientSystemData>>(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PatientsSystemExample.txt");

                var result = dataDomainResponse.Responses;
                LoggerDomainEvent.Raise(new LogStatus { Message = "2) Success", Type = LogType.Debug });
                return result;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientSystemDataDomain:Save(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientSystemDataDomain:Save(): " + ex.Message);
            }
        }


        public object Update<T>(T patients, string contract, string ddServiceurl)
        {
            throw new NotImplementedException();
        }
    }
}
