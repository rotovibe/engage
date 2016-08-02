using System;
using System.Collections.Generic;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Utils;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.Engage.Integrations.UOW
{
    public class PatientDataDomain : IDataDomain
    {
        protected readonly string DDPatientServiceUrl = ProcConstants.DdPatientServiceUrl;
        protected readonly string UpdatedBy = "5368ff2ad4332316288f3e3e";

        public object Update<T>(T patients, string contract, string ddServiceurl)
        {
            LoggerDomainEvent.Raise(new LogStatus { Message = "1) Sending update Patient DD request.", Type = LogType.Debug });
            string patientId = null;
            var userid = ProcConstants.UserId; // need to find a valid session id.
            try
            {
                IRestClient client = new JsonServiceClient { Timeout = TimeSpan.FromMinutes(50) }; //new TimeSpan( 28000000000) };
                //"/{Context}/{Version}/{ContractNumber}/Patient""
                var url =
                    Helper.BuildURL(
                        string.Format("{0}/{1}/{2}/{3}/Patient/", ddServiceurl, "NG", 1, contract), userid);

                var patientList = patients as List<PatientData>;

                patientList.ForEach(pt =>
                {
                    patientId = pt.ExternalRecordId;
                    pt.UpdatedByProperty = UpdatedBy;
                    pt.DisplayPatientSystemId = null; // set this manually
                    PutUpdatePatientDataResponse response = client.Put<PutUpdatePatientDataResponse>(url,
                        new PutUpdatePatientDataRequest
                        {
                            Context = "NG",
                            ContractNumber = contract,
                            PatientData = pt,
                            UserId = userid,
                            Insert = false,
                            InsertDuplicate = false,
                            Version = 1
                        });
                    //LogUtil.FormatOutputDebug<T>(response);
                    LoggerDomainEvent.Raise(new LogStatus {Message = "1) Success", Type = LogType.Debug});
                });

                return "Success!";
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientDataDomain:Update(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientDataDomain:Update(): " + ex.Message);
            }
        }

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

                LogUtil.FormatOutputDebug<T>(response);
                LoggerDomainEvent.Raise(new LogStatus { Message = "1) Success", Type = LogType.Debug });
                return response.Responses;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientDataDomain:Save(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientDataDomain:Save(): " + ex.Message);
            }
        }

        internal List<PatientData> GetAllPatientIds(string contract, string ddUrl)
        {
            LoggerDomainEvent.Raise(new LogStatus {Message = "1) Sending get all Patient DD request.", Type = LogType.Debug});

            var userid = ProcConstants.UserId; // need to find a valid session id.
            try
            {
                IRestClient client = new JsonServiceClient {Timeout = TimeSpan.FromMinutes(50)}; //new TimeSpan( 28000000000) };
                //"/{Context}/{Version}/{ContractNumber}/Patients"
                var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patients/", ddUrl, "NG", 1, contract), userid);

                GetAllPatientsDataResponse response = client.Get<GetAllPatientsDataResponse>(url);
                LoggerDomainEvent.Raise(new LogStatus {Message = "1) Success", Type = LogType.Debug});

                return response.PatientsData;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientDataDomain:GetAllPatientIds(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientDataDomain:GetAllPatientIds(): " + ex.Message);
            }
        }

        internal List<PatientData> getAllPatientsById(List<string> PatientIds, string ddUrl, string contract)
        {
            LoggerDomainEvent.Raise(new LogStatus { Message = "1) Sending get all Patient DD request.", Type = LogType.Debug });
            List<PatientData> patients = new List<PatientData>();

            var userid = ProcConstants.UserId; // need to find a valid session id.
            try
            {
                PatientIds.ForEach(pid =>
                {

                    IRestClient client = new JsonServiceClient {Timeout = TimeSpan.FromMinutes(50)}; //new TimeSpan( 28000000000) };
                    //"/{Context}/{Version}/{ContractNumber}/patient/{PatientID}
                    var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Patient/{4}", ddUrl, "NG", 1, contract, pid), userid);

                    GetPatientDataResponse response = client.Get<GetPatientDataResponse>(url);
                    patients.Add(response.Patient);
                });
                LoggerDomainEvent.Raise(new LogStatus { Message = "1) Success", Type = LogType.Debug });
                return patients;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientDataDomain:Save(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientDataDomain:Save(): " + ex.Message);
            }
        }
    }
}
