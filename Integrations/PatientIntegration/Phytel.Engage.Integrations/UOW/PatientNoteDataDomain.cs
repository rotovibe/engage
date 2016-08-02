using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.Common;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.PatientNote.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Utils;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.Engage.Integrations.UOW
{
    public class PatientNoteDataDomain : IDataDomain
    {
        private readonly string _ddPatientNotesServiceUrl = ProcConstants.DdPatientNoteUrl; //ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["DDPatientNoteUrl"].Value; //ConfigurationManager.AppSettings[""];

        public object Save<T>(T patientNotes, string contract)
        {
            LoggerDomainEvent.Raise(new LogStatus { Message = "5) Sending insert PatientNotes DD request.", Type = LogType.Debug });
            var l = patientNotes as List<PatientNoteData>;
            if (l != null)
                LogUtil.LogExternalRecordId("Save", l.Cast<IAppData>().ToList());

            var userid = ProcConstants.UserId; // need to find a valid session id.
            try
            {
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Batch/PatientNotes", "POST")]
                var url =
                    Helper.BuildURL(
                        string.Format("{0}/{1}/{2}/{3}/Batch/PatientNotes/", _ddPatientNotesServiceUrl, "NG", 1,
                            contract), userid);

                InsertBatchPatientNotesDataResponse response = client.Post<InsertBatchPatientNotesDataResponse>(url,
                    new InsertBatchPatientNotesDataRequest
                    {
                        Context = "NG",
                        ContractNumber = contract,
                        PatientNotesData = patientNotes as List<PatientNoteData>,
                        UserId = userid,
                        Version = 1
                    });

                //new Helpers().SerializeObject<List<PatientNoteData>>(patientNotes as List<PatientNoteData>, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PatientNotesExample.txt");
                //var lPsd = Helpers.DeserializeObject<List<PatientSystemData>>(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\PatientsSystemExample.txt");

                LoggerDomainEvent.Raise(new LogStatus { Message = "5) Success", Type = LogType.Debug });
                return response.Responses;
            }
            catch (Exception ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "PatientNoteDataDomain:Save(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("PatientNoteDataDomain:Save(): " + ex.Message);
            }
        }


        public object Update<T>(T patients, string contract, string ddServiceUrl)
        {
            throw new NotImplementedException();
        }
    }
}
