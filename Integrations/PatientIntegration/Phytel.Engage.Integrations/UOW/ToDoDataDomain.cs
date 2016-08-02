using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.Common;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Utils;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.Engage.Integrations.UOW
{
    public class ToDoDataDomain : IDataDomain
    {
        protected readonly string DDPatientToDoServiceUrl = ProcConstants.DdPatientToDoServiceUrl;

        public object Save<T>(T toDo, string contract)
        {
            LoggerDomainEvent.Raise(new LogStatus { Message = "1) Sending insert ToDo DD request.", Type = LogType.Debug });
            var l = toDo as List<ToDoData>;
            if (l != null)
                LogUtil.LogExternalRecordId("Save", l.Cast<IAppData>().ToList());

            var userid = ProcConstants.UserId; // need to find a valid session id.
            try
            {
                IRestClient client = new JsonServiceClient {Timeout = TimeSpan.FromMinutes(50) }; //new TimeSpan( 28000000000) };
                //"/{Context}/{Version}/{ContractNumber}/Batch/PatientToDos"
                var url = Helper.BuildURL(string.Format("{0}/{1}/{2}/{3}/Batch/PatientToDos/", DDPatientToDoServiceUrl, "NG", 1, contract), userid);

                InsertBatchPatientToDosDataResponse response = client.Post<InsertBatchPatientToDosDataResponse>(url,
                    new InsertBatchPatientToDosDataRequest
                    {
                        Context = "NG",
                        ContractNumber = contract,
                        PatientToDosData = toDo as List<ToDoData>,
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
                LoggerDomainEvent.Raise(new LogStatus { Message = "ToDoDataDomain:Save(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("ToDoDataDomain:Save(): " + ex.Message);
            }
        }


        public object Update<T>(T patients, string contract, string ddSerivceUrl)
        {
            throw new NotImplementedException();
        }
    }
}
