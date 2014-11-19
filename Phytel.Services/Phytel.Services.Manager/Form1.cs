using Phytel.Services.API;
using Phytel.Services.Mongo;
using Phytel.Services.Security;
using Phytel.Services.SQLServer;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace PhytelServicesManager
{
    public partial class Form1 : Form
    {
        XmlDocument _configDoc = new XmlDocument();
        XmlNode _currentConnection = null;
        DataProtector protector = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);

        bool _somethingChanged = false;
        bool _changingRows = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory(DataProtector.GetConfigurationPath());

            gbSQL.Visible = false;
            gbMongo.Visible = false;
            gbAPI.Visible = false;

            gbSQL.Top = gbMongo.Top = gbAPI.Top = lstConnections.Top;
            gbSQL.Left = gbMongo.Left = gbAPI.Left = (lstConnections.Left + lstConnections.Width + 10);

            InitializeConfiguration();

            lblSQLVersion.Text = GetVersion("SQL");
            lblMongoVersion.Text = GetVersion("Mongo");
            lblAPIVersion.Text = GetVersion("API");

            _somethingChanged = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string GetVersion(string type)
        {
            Assembly sqlAssembly = System.Reflection.Assembly.LoadFrom("Phytel.Services.SQLServer.dll");
            Assembly mongoAssembly = System.Reflection.Assembly.LoadFrom("Phytel.Services.Mongo.dll");
            Assembly apiAssembly = System.Reflection.Assembly.LoadFrom("Phytel.Services.API.dll");
            FileVersionInfo fileVersionInfo = null;

            switch(type.ToUpper())
            {
                case "SQL":
                    fileVersionInfo = FileVersionInfo.GetVersionInfo(sqlAssembly.Location);
                    break;
                case "MONGO":
                    fileVersionInfo = FileVersionInfo.GetVersionInfo(mongoAssembly.Location);
                    break;
                case "API":
                    fileVersionInfo = FileVersionInfo.GetVersionInfo(apiAssembly.Location);
                    break;
            }

            if (fileVersionInfo != null)
                return fileVersionInfo.ProductVersion;
            else
                return "Unknown";
        }

        private void InitializeConfiguration()
        {
            string configFile = DataProtector.GetConfigurationPathAndFile();
            if (File.Exists(configFile) == false)
                BuildDefaultFile();

            _configDoc.Load(configFile);

            lstConnections.Items.Clear();

            LoadDatabaseNodes();
            LoadAPINodes();

            if (lstConnections.Items.Count > 0)
                lstConnections.Items[0].Selected = true;
        }

        private void LoadDatabaseNodes()
        {
            bool hasMongoAdmin = false;

            XmlNodeList dbs = _configDoc.SelectNodes("/configuration/database");
            foreach (XmlNode db in dbs)
            {
                ListViewItem lvi = new ListViewItem(db.Attributes.GetNamedItem("name").Value);
                lvi.SubItems.Add(db.Attributes.GetNamedItem("type").Value);

                lstConnections.Items.Add(lvi);

                if (lvi.Text.Trim().ToUpper() == "MONGOADMIN")
                    hasMongoAdmin = true;
            }

            if (hasMongoAdmin == false)
            {
                //create the mongo admin connection
                lstConnections.Items.Add(CreateNewDatabase("MongoAdmin", "Mongo"));
                SaveConfig(false);
            }
        }

        private void LoadAPINodes()
        {
            XmlNodeList apis = _configDoc.SelectNodes("/configuration/api");

            foreach (XmlNode api in apis)
            {
                ListViewItem lvi = new ListViewItem(api.Attributes.GetNamedItem("name").Value);
                lvi.SubItems.Add("URL");

                lstConnections.Items.Add(lvi);
            }
        }

        private void lstConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                _changingRows = true;
                _currentConnection = null;

                string connType = string.Empty;
                string connName = string.Empty;
                string configName = string.Empty;

                gbSQL.Visible = false;
                gbMongo.Visible = false;
                gbAPI.Visible = false;

                if (lstConnections.SelectedItems.Count > 0)
                {
                    connType = lstConnections.SelectedItems[0].SubItems[1].Text;
                    connName = lstConnections.SelectedItems[0].SubItems[0].Text;

                    configName = (connType == "URL" ? "api" : "database");

                    _currentConnection = _configDoc.SelectSingleNode(string.Format("/configuration/{0}[@name='{1}'][@type='{2}']", configName, connName, connType));

                    switch (connType.Trim().ToUpper())
                    {
                        case "MONGO":
                            LoadMongoConnection();
                            gbMongo.Visible = true;
                            gbMongo.BringToFront();
                            break;
                        case "SQL":
                            LoadSQLConnection();
                            gbSQL.Visible = true;
                            gbSQL.BringToFront();
                            break;
                        case "URL":
                            LoadAPIConnection();
                            gbAPI.Visible = true;
                            gbAPI.BringToFront();
                            break;
                        default:
                            MessageBox.Show("Invalid Connection Type");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _changingRows = false;
            }
        }

        private void LoadMongoConnection()
        {
            txtMongoConnName.Text = GetDatabaseAttrValue(_currentConnection, "name");
            txtMongoServerName.Text = GetConnectionValue(_currentConnection, "server");
            txtMongoDatabase.Text = GetConnectionValue(_currentConnection, "databasename");
            txtMongoUserName.Text = GetConnectionValue(_currentConnection, "username");

            string pwd = GetConnectionValue(_currentConnection, "password");

            if (pwd != string.Empty)
                pwd = protector.Decrypt(pwd);

            txtMongoPassword.Text = pwd;
            txtMongoReplSetName.Text = GetConnectionValue(_currentConnection, "replicationsetname");

            txtMongoOptions.Text = GetConnectionValue(_currentConnection, "options");

            bool isMongoAdmin = (txtMongoConnName.Text.ToUpper() == "MONGOADMIN");

            txtMongoConnName.Enabled =
                txtMongoServerName.Enabled =
                txtMongoDatabase.Enabled =
                txtMongoReplSetName.Enabled =
                txtMongoOptions.Enabled =
                btnTest.Enabled = !isMongoAdmin;
        }

        private void LoadSQLConnection()
        {
            txtSQLConnName.Text = GetDatabaseAttrValue(_currentConnection, "name");
            txtSQLServerName.Text = GetConnectionValue(_currentConnection, "server");
            txtSQLDatabase.Text = GetConnectionValue(_currentConnection, "databasename");
            chkSQLTrusted.Checked = (GetConnectionValue(_currentConnection, "trusted") == "True");
            txtSQLUserName.Text = GetConnectionValue(_currentConnection, "username");
            
            string pwd = GetConnectionValue(_currentConnection, "password");

            if (pwd != string.Empty)
                pwd = protector.Decrypt(pwd);

            txtSQLPassword.Text = pwd;
            txtSQLOptions.Text = GetConnectionValue(_currentConnection, "options");

            txtSQLPassword.Enabled = txtSQLUserName.Enabled = (!chkSQLTrusted.Checked);

            btnTest.Enabled = true;
        }

        private void LoadAPIConnection()
        {
            txtAPIConnName.Text = GetDatabaseAttrValue(_currentConnection, "name");
            txtAPIURL.Text = GetConnectionValue(_currentConnection, "url");

            btnTest.Enabled = true;
        }

        private void SetConnectionValue(XmlNode connNode, string nodeName, string nodeValue)
        {
            if (_changingRows)
                return;

            XmlNode node = GetConnectionNode(connNode, nodeName);
            if (node != null)
            {
                node.InnerText = nodeValue;
            }
            else
            {
                XmlElement newNode = _configDoc.CreateElement(nodeName);
                newNode.InnerText = nodeValue;
                connNode.AppendChild(newNode);
            }

            _somethingChanged = true;
        }

        private XmlNode GetConnectionNode(XmlNode connNode, string nodeName)
        {
            XmlNode returnVal = null;
            try
            {
                returnVal = connNode.SelectSingleNode(nodeName);
            }
            catch { }

            if (returnVal == null)
            {
                try
                {
                    returnVal = connNode.Attributes.GetNamedItem(nodeName);
                }
                catch { }
            }
            return returnVal;
        }

        private string GetConnectionValue(XmlNode connNode, string nodeName)
        {
            string returnVal = string.Empty;
            try
            {
                returnVal = connNode.SelectSingleNode(nodeName).InnerText;
            }
            catch { }

            return returnVal;
        }

        private string GetDatabaseAttrValue(XmlNode connNode, string nodeName)
        {
            string returnVal = string.Empty;
            
            try
            {
                returnVal = connNode.Attributes.GetNamedItem(nodeName).Value;
            }
            catch { }

            return returnVal;
        }

        private void BuildDefaultFile()
        {
            string fileInfo = @"<?xml version='1.0' encoding='Windows-1252'?><configuration></configuration>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fileInfo);
            doc.Save(DataProtector.GetConfigurationPathAndFile());

            string serverConfigInfo = @"<?xml version='1.0' encoding='utf-8'?><configuration><entropy>E0E63090-85B7-49e9-BA9C-F49DE2DB3291</entropy></configuration>";
            string serverConfigFilePath = string.Format(@"{0}\{1}", DataProtector.GetConfigurationPath(), "server.config");
            doc = new XmlDocument();
            doc.LoadXml(serverConfigInfo);
            doc.Save(serverConfigFilePath);

            _somethingChanged = true;
        }

        private ListViewItem CreateNewDatabase(string name, string type)
        {
            XmlNode newDatabaseNode = _configDoc.CreateElement("database");

            XmlAttribute attrName = _configDoc.CreateAttribute("name");
            attrName.Value = name;
            newDatabaseNode.Attributes.SetNamedItem(attrName);

            XmlAttribute attrType = _configDoc.CreateAttribute("type");
            attrType.Value = type;
            newDatabaseNode.Attributes.SetNamedItem(attrType);

            string server = (name.ToUpper() == "MONGOADMIN" && type.ToUpper() == "MONGO" ? "127.0.0.1" : string.Empty);
            string dbName = (name.ToUpper() == "MONGOADMIN" && type.ToUpper() == "MONGO" ? "admin" : string.Empty);
            string user = string.Empty;
            string pwd = string.Empty;
            string replSetName = string.Empty;
            string options = string.Empty;

            string innerXml = @"<server>{0}</server>" +
                                "<databasename>{1}</databasename>" +
                                "<username>{2}</username>" +
                                "<password>{3}</password>" +
                                "<trusted>False</trusted>" +
                                "<domainprefix></domainprefix>" +
                                "<replicationsetname>{4}</replicationsetname>" +
                                "<options>{5}</options>";

            innerXml = string.Format(innerXml, server, name, user, pwd, replSetName, options);

            newDatabaseNode.InnerXml = innerXml;

            _configDoc.SelectSingleNode("/configuration").AppendChild(newDatabaseNode);

            ListViewItem lvi = new ListViewItem(name);
            lvi.SubItems.Add(type);

            _somethingChanged = true;
            return lvi;
        }

        private ListViewItem CreateNewURL(string name)
        {
            XmlNode newAPINode = _configDoc.CreateElement("api");

            XmlAttribute attrName = _configDoc.CreateAttribute("name");
            attrName.Value = name;
            newAPINode.Attributes.SetNamedItem(attrName);

            XmlAttribute attrType = _configDoc.CreateAttribute("type");
            attrType.Value = "URL";
            newAPINode.Attributes.SetNamedItem(attrType);

            string innerXml = @"<url />";

            newAPINode.InnerXml = innerXml;

            _configDoc.SelectSingleNode("/configuration").AppendChild(newAPINode);

            ListViewItem lvi = new ListViewItem(name);
            lvi.SubItems.Add("URL");

            _somethingChanged = true;
            return lvi;
        }

        private void txtConnName_TextChanged(object sender, EventArgs e)
        {
            SetConnectionValue(_currentConnection, "name", ((TextBox)sender).Text);
            lstConnections.SelectedItems[0].SubItems[0].Text = ((TextBox)sender).Text;
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            SetConnectionValue(_currentConnection, "server", ((TextBox)sender).Text);
        }

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {
            SetConnectionValue(_currentConnection, "databasename", ((TextBox)sender).Text);
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            SetConnectionValue(_currentConnection, "username", ((TextBox)sender).Text);
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            SetConnectionValue(_currentConnection, "password", protector.Encrypt(((TextBox)sender).Text));
        }

        private void txtReplSetName_TextChanged(object sender, EventArgs e)
        {
            SetConnectionValue(_currentConnection, "replicationsetname", ((TextBox)sender).Text);
        }

        private void txtOptions_TextChanged(object sender, EventArgs e)
        {
            SetConnectionValue(_currentConnection, "options", ((TextBox)sender).Text);
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmNew frm = new frmNew();
            ListViewItem lvi = null;

            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string connType = frm.ConnectionType.ToUpper();
                switch (connType)
                {
                    case "MONGO":
                    case "SQL":
                        XmlNodeList databases = _configDoc.SelectNodes("/configuration/database");
                        foreach (XmlNode node in databases)
                        {
                            if (node.Attributes["name"].Value == frm.ConnectionName && node.Attributes["type"].Value == frm.ConnectionType)
                            {
                                MessageBox.Show("You cannot add a connection of the same name and connection type!");
                                return;
                            }
                        }

                        lvi = CreateNewDatabase(frm.ConnectionName, frm.ConnectionType);
                        lstConnections.Items.Add(lvi);
                        break;
                    case "API":
                        XmlNodeList apis = _configDoc.SelectNodes("/configuration/api");
                        foreach (XmlNode node in apis)
                        {
                            if (node.Attributes["name"].Value == frm.ConnectionName)
                            {
                                MessageBox.Show("You cannot add a connection of the same name!");
                                return;
                            }
                        }

                        lvi = CreateNewURL(frm.ConnectionName);
                        lstConnections.Items.Add(lvi);
                        break;
                }
                lstConnections.SelectedItems.Clear();
                lvi.Selected = true;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _configDoc.SelectSingleNode("/configuration").RemoveChild(_currentConnection);
            _somethingChanged = true;

            lstConnections.SelectedItems[0].Remove();

            if (lstConnections.Items.Count > 0)
                lstConnections.Items[0].Selected = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SaveConfig(true) == System.Windows.Forms.DialogResult.Cancel)
                e.Cancel = true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (SaveConfig(true) == System.Windows.Forms.DialogResult.Cancel)
                return;

            try
            {
                string connType = lstConnections.SelectedItems[0].SubItems[1].Text.ToUpper();

                switch (connType)
                {
                    case "MONGO":
                        MongoService.Instance.GetDatabase(txtMongoConnName.Text, false).FindAllUsers();
                        break;
                    case "SQL":
                        SQLDataService.Instance.ExecuteSQL(txtSQLConnName.Text, false, "Select Top 1 1 From sysobjects");
                        break;
                    case "URL":
                        string url = APIService.Instance.GetURL(txtAPIConnName.Text);

                        TestConnection(url);
                        break;
                }
                MessageBox.Show("Connection Succeeded!", "Phytel Services", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failed: " + ex.Message, "Phytel Services", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TestConnection(string url)
        {
            try
            {
                //modes
                Uri testUri = new Uri(url);

                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Host = testUri.Host;

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var getTask = client.GetStringAsync(testUri);
                var content = getTask.Result;

                string contentString = content;
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private DialogResult SaveConfig(bool ask)
        {
            DialogResult result = System.Windows.Forms.DialogResult.No;
            if (_somethingChanged)
            {
                if (ask)
                    result = MessageBox.Show("Changes have been made!  Save Changes Now?", "Phytel Services Configuration", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                else
                    result = System.Windows.Forms.DialogResult.Yes;

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    _configDoc.Save(DataProtector.GetConfigurationPathAndFile());
                    _somethingChanged = false;
                }
            }
            return result;
        }

        private void chkTrustedConnection_CheckedChanged(object sender, EventArgs e)
        {
            SetConnectionValue(_currentConnection, "trusted", chkSQLTrusted.Checked.ToString());

            txtSQLPassword.Enabled = txtSQLUserName.Enabled = (!chkSQLTrusted.Checked);
        }

        private void txtDomainPrefix_TextChanged(object sender, EventArgs e)
        {
            SetConnectionValue(_currentConnection, "domainprefix", ((TextBox)sender).Text);
        }

        private void txtAPIURL_TextChanged(object sender, EventArgs e)
        {
            SetConnectionValue(_currentConnection, "url", ((TextBox)sender).Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveConfig(false);
        }
    }
}
