using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.Engage.Integrations.UOW
{
    public class PatientDataDomain : IDataDomain
    {
        protected readonly string DDPatientServiceUrl = ProcConstants.DdPatientServiceUrl; //ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["DDPatientServiceUrl"].Value; //ConfigurationManager.AppSettings[""];

        public object Save<T>(T patients, string contract)
        {
            LoggerDomainEvent.Raise(new LogStatus { Message = "1) Sending insert Patient DD request.", Type = LogType.Debug });
            var userid = ProcConstants.UserId; // need to find a valid session id.
            try
            {
                IRestClient client = new JsonServiceClient {Timeout = TimeSpan.FromMinutes(50) }; //new TimeSpan( 28000000000) };
                //"/{Context}/{Version}/{ContractNumber}/Batch/Patients"
                var url =
                    Helper.BuildURL(
                        string.Format("{0}/{1}/{2}/{3}/Batch/Patients/", DDPatientServiceUrl, "NG", 1, contract), userid);

                InsertBatchPatientsDataResponse response = client.Post<InsertBatchPatientsDataResponse>(url,
                    new InsertBatchPatientsDataRequest
                    {
                        Context = "NG",
                        ContractNumber = contract,
                        PatientsData = patients as List<PatientData>,
                        UserId = userid,
                        Version = 1
                    });

                FormatOutputDebug<T>(response);
                LoggerDomainEvent.Raise(new LogStatus { Message = "1) Success", Type = LogType.Debug });
                return response.Responses;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientDataDomain:Save(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientDataDomain:Save(): " + ex.Message);
            }
        }

        private static void FormatOutputDebug<T>(InsertBatchPatientsDataResponse response)
        {
            //Helpers.SerializeObject<List<PatientData>>(patients as List<PatientData>, System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PatientsExample.txt");
            //var lPsd = Helpers.DeserializeObject<List<PatientSystemData>>(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PatientsSystemExample.txt");
            StringBuilder sb = new StringBuilder();
            response.Responses.ForEach(r =>
            {
                sb.Append("Id: " + r.Body.Id + " ,");
                sb.Append("ExternalRecordId: " + r.Body.ExternalRecordId + " ,");
                sb.Append("EngageId: " + r.Body.EngagePatientSystemValue + " |");
            });

            LoggerDomainEvent.Raise(new LogStatus {Message = "patient save result: " + sb.ToString(), Type = LogType.Debug});
        }
    }
}
