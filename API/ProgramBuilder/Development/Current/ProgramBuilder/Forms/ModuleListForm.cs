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
    public partial class ModuleListForm : Form_Base
    {
        private List<Module> loadModules = new List<Module>();
        //private double version = double.Parse(ConfigurationManager.AppSettings.Get("version"));
        //private string context = ConfigurationManager.AppSettings.Get("context");
        string _headerUserId = string.Empty;

        public ModuleListForm()
        {
            InitializeComponent();
            //GetAllModulesResponse modulesResponse = GetAllModulesRequestServiceCall();
            //MessageBox.Show(modulesResponse.Modules.ToString());
        }

        public GetAllModulesResponse GetAllModulesRequestServiceCall()
        {
            try
            {
                return PullData(DataDomainTypes.Module) as GetAllModulesResponse;
            }
            catch (Exception ex)
            {
               //TODO
                return null;
            }
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

        public void addModule(Module newModule)
        {
            moduleListView.Items.Add(newModule.Name);
        }

    }
}
