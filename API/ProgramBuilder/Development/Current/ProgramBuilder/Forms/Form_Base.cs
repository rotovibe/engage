using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Module.DTO;
using Phytel.API.DataDomain.Action.DTO;
using ProgramBuilder.Forms;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProgramBuilder.UserControls;
using System.Net.Http;
using System.Net.Http.Headers;
using Phytel.API.Interface;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Configuration;
using Phytel.API.DataDomain.Step.DTO;

namespace ProgramBuilder
{
    public class Form_Base : Form
    {
        //TODO
        protected String contractNumber = "InHealth001";
        protected double version = 1.0;
        protected String context = "NG";
        protected Uri requestUri;
                
        const string _userid = "5325db73d6a4850adc047035"; //this is a dummy id used to access the collection; not used for security, just validity
        const string _TEXTSTEP = "step/text";
        const string _YESNO = "step/yesno";

        object requestdata;
        object responsedata;


        protected HttpClient GetHttpClient(DataDomainTypes datadomain, string datadomainName = "")
        {
            requestUri = new Uri(string.Format("{0}/{1}/{2}/{3}/{4}?userid={5}",
                                                    ConfigurationManager.AppSettings["urlhost"].ToString() + "/" + datadomain.ToString(),
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    String.IsNullOrEmpty(datadomainName)?datadomain.ToString():datadomainName,
                                                    _userid));            
            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Host = requestUri.Host;

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        protected object GetData(DataDomainTypes datadomain)
        {
            return GetData(datadomain, "");
        }

        protected object GetData(DataDomainTypes datadomain, string datadomainName)
        {
            GetDTOs(datadomain, datadomainName);
            HttpClient dataClient = GetHttpClient(datadomain, datadomainName);

            //IDomainResponse responsedata;
            
            DataContractJsonSerializer dataJsonSer = new DataContractJsonSerializer(requestdata.GetType());
            MemoryStream dataMs = new MemoryStream();
            dataJsonSer.WriteObject(dataMs, requestdata);
            dataMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader dataSr = new StreamReader(dataMs);
            StringContent dataContent = new StringContent(dataSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var dataResponse = dataClient.GetStringAsync(requestUri);
            var dataResponseContent = dataResponse.Result;

            string dataResponseString = dataResponseContent;
            //IDomainResponse responsedata = null;

            using (var modesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(dataResponseString)))
            {
                var dataSerializer = new DataContractJsonSerializer(responsedata.GetType());
                responsedata = (IDomainResponse)dataSerializer.ReadObject(modesMsResponse);
            }

            return responsedata;
        }

        private void GetDTOs(DataDomainTypes datadomain, string datadomainName)
        {
            IDataDomainRequest req = null;

            switch (datadomain)
            {
                case DataDomainTypes.Module:
                    req = new GetAllModulesRequest();
                    responsedata = new GetAllModulesResponse();
                    break;
                
                case DataDomainTypes.Action:
                    req = new GetAllActionsDataRequest();
                    responsedata = new GetAllActionsDataResponse();
                    break;

                case DataDomainTypes.Step:
                    switch (datadomainName.ToLower())
	                {
                        case _TEXTSTEP:
                                req = new GetAllTextStepDataRequest();
                                responsedata = new GetAllTextStepDataResponse();
                            break;

                        case _YESNO:
                                req = new GetAllYesNoStepDataRequest();
                                responsedata = new GetAllYesNoStepDataResponse();
                            break;

	                }

                    break;
            }

             req.Version = version;
             req.Context = context;
             req.ContractNumber = contractNumber;

             requestdata = req;
        }

    }

    public enum DataDomainTypes
    {
        Module,
        Action,
        Step
    }
    
}
