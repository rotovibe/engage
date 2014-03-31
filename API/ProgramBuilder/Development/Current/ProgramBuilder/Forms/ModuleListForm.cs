using Phytel.API.DataDomain.Module.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramBuilder
{
    public partial class ModuleListForm : Form
    {
        private List<Module> loadModules = new List<Module>();
        //private double version = double.Parse(ConfigurationManager.AppSettings.Get("version"));
        //private string context = ConfigurationManager.AppSettings.Get("context");
        string _headerUserId = string.Empty;

        //TODO
        private String contractNumber = "InHealth001";
        private double version = 1.0;
        private String context = "NG";
        

        public ModuleListForm()
        {
            InitializeComponent();
            //GetAllModulesResponse modulesResponse = GetAllModulesRequestServiceCall();
            //MessageBox.Show(modulesResponse.Modules.ToString());
        }

        public GetAllModulesResponse GetAllModulesRequestServiceCall()
        {
            string _userid = "5325db73d6a4850adc047035"; //this is a dummy id used to access the collection; not used for security, just validity
                        
            Uri modulesUri = new Uri(string.Format("{0}/{1}/{2}/{3}/Module?userid={4}",
                                                    ConfigurationManager.AppSettings["urlhost"].ToString() + "/module",
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    _userid));
            HttpClient modulesClient = GetHttpClient(modulesUri);

            GetAllModulesRequest modulesRequest = new GetAllModulesRequest
            {
                Version = version,
                Context = context,
                ContractNumber = contractNumber,
                UserId = "5325db73d6a4850adc047035"
            };

            DataContractJsonSerializer modulesJsonSer = new DataContractJsonSerializer(typeof(GetAllModulesRequest));
            MemoryStream modulesMs = new MemoryStream();
            modulesJsonSer.WriteObject(modulesMs, modulesRequest);
            modulesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader modulesSr = new StreamReader(modulesMs);
            StringContent modulesContent = new StringContent(modulesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var modulesResponse = modulesClient.GetStringAsync(modulesUri);
            var modulesResponseContent = modulesResponse.Result;

            string modulesResponseString = modulesResponseContent;
            GetAllModulesResponse responseModules = null;

            using (var modesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(modulesResponseString)))
            {
                var modulesSerializer = new DataContractJsonSerializer(typeof(GetAllModulesResponse));
                responseModules = (GetAllModulesResponse)modulesSerializer.ReadObject(modesMsResponse);
            }

            return responseModules;
        }

        private HttpClient GetHttpClient(Uri uri)
        {            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Host = uri.Host;

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void ModuleListForm_Load(object sender, EventArgs e)
        {
            List<Module> list = GetAllModulesRequestServiceCall().Modules;            

            foreach (Module module in list.OrderBy(x => x.Name))
            {
                moduleListView.Items.Add(new ListViewItem(module.Name, module.Id));
            }
        }

        private void addModuleButton_Click(object sender, EventArgs e)
        {
            NewModuleForm newModule = new NewModuleForm();
            newModule.ShowDialog();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
