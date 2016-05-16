using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Phytel.API.Common;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.Engage.Integrations.DomainEvents;
using Phytel.Engage.Integrations.DTO;
using Phytel.Engage.Integrations.Utils;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace Phytel.Engage.Integrations.UOW
{
    public class ContactDataDomain : IDataDomain
    {
        private readonly string _ddContactServiceUrl = ProcConstants.DdContactServiceUrl; //ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location).AppSettings.Settings["DDContactServiceUrl"].Value; 
        //ConfigurationManager.AppSettings[""];

        public object Save<T>(T list, string contract)
        {
            LoggerDomainEvent.Raise(new LogStatus { Message = "4) Sending insert Contact DD request.", Type = LogType.Debug });
            var l = list as List<ContactData>;
            if (l != null)
                LogUtil.LogExternalRecordId("Save", l.Cast<IAppData>().ToList());

            InsertBatchContactDataResponse response = null;
            try
            {
                List<HttpObjectResponse<ContactData>> resp = null;
                var userid = ProcConstants.UserId; // need to find a valid session id.
                IRestClient client = new JsonServiceClient();
                //[Route("/{Context}/{Version}/{ContractNumber}/Batch/Contacts", "POST")]
                var url =
                    Helper.BuildURL(
                        string.Format("{0}/{1}/{2}/{3}/Batch/Contacts/", _ddContactServiceUrl, "NG", 1, contract),
                        userid);

                try
                {
                    response = client.Post<InsertBatchContactDataResponse>(url,
                        new InsertBatchContactDataRequest
                        {
                            Context = "NG",
                            ContractNumber = contract,
                            ContactsData = list as List<ContactData>,
                            UserId = userid,
                            Version = 1
                        });
                    resp = response.Responses;
                }
                catch (Exception ex)
                {
                    if (response != null)
                    {
                        var status = response.Status;
                    }
                    throw ex;
                }

                LoggerDomainEvent.Raise(new LogStatus { Message = "4) Success", Type = LogType.Debug });
                return resp;
            }
            catch (WebServiceException ex)
            {
                LoggerDomainEvent.Raise(new LogStatus { Message = "ContactDataDomain:Save(): " + ex.Message, Type = LogType.Error });
                throw new ArgumentException("ContactDataDomain:Save(): " + ex.Message);
            }
        }


        public object Update<T>(T patients, string contract, string ddServiceUrl)
        {
            throw new NotImplementedException();
        }
    }
}
