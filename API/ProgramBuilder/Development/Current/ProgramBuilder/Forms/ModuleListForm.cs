using Phytel.API.DataDomain.Module.DTO;
using MongoDB.Bson;
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
        public List<Module> listModules = new List<Module>();
        string _headerUserId = string.Empty;

        public ModuleListForm()
        {
            InitializeComponent();
        }

        public GetAllModulesResponse GetAllModulesRequestServiceCall()
        {
            try
            {
                return GetData(DataDomainTypes.Module) as GetAllModulesResponse;
            }
            catch (Exception ex)
            {
               //TODO
                return null;
            }
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

            NewModuleForm newModuleForm = new NewModuleForm();
            newModuleForm.ShowDialog();
            if(newModuleForm.DialogResult.Equals(DialogResult.OK))
            {
                Module newModule = new Module()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = newModuleForm.nmTextBox.Text,
                    Description = newModuleForm.descTextBox.Text,
                    //Objectives =
                    //Status = stsNumericUpDwn.Value.ToString(),
                    //Version = 
                };
                ListViewItem lvi = moduleListView.Items.Add(new ListViewItem(newModule.Name, newModule.Id));
                listModules.Add(newModule);
                lvi.Checked = true;
            }
            
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }


    }
}
