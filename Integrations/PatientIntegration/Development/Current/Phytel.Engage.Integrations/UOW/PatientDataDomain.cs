using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
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
            var userid = "5602f0f384ac071c989477cf"; // need to find a valid session id.
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

                //Helpers.SerializeObject<List<PatientData>>(patients as List<PatientData>, System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PatientsExample.txt");
                //var lPsd = Helpers.DeserializeObject<List<PatientSystemData>>(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PatientsSystemExample.txt");

                LoggerDomainEvent.Raise(new LogStatus { Message = "1) Success", Type = LogType.Debug });
                return response.Responses;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientDataDomain:Save(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientDataDomain:Save(): " + ex.Message);
            }
        }
    }
}
