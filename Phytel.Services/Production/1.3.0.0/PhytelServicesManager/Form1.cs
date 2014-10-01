using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using Phytel.Services;

namespace PhytelServicesManager
{
    public partial class Form1 : Form
    {
        XmlDocument _configDoc = new XmlDocument();
        XmlNode _currentDatabase = null;
        DataProtector protector = new DataProtector(Phytel.Services.DataProtector.Store.USE_MACHINE_STORE);

        bool _somethingChanged = false;
        bool _changingRows = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory(Phytel.Services.DataProtector.GetConfigurationPath());

            InitializeConfiguration();

            lblVersion.Text = "Version: " + Application.ProductVersion;
            _somethingChanged = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void InitializeConfiguration()
        {
            bool hasMongoAdmin = false;

            string configFile = Phytel.Services.DataProtector.GetConfigurationPathAndFile();
            if (File.Exists(configFile) == false)
                BuildDefaultFile();

            _configDoc.Load(configFile);

            XmlNodeList dbs = _configDoc.SelectNodes("/configuration/database");
            lstConnections.Items.Clear();
            foreach (XmlNode db in dbs)
            {
                ListViewItem lvi = new ListViewItem(db.Attributes.GetNamedItem("name").Value);
                lvi.SubItems.Add(db.Attributes.GetNamedItem("type").Value);

                lstConnections.Items.Add(lvi);

                if (lvi.Text == "MongoAdmin")
                    hasMongoAdmin = true;
            }

            if (hasMongoAdmin == false)
            {
                //create the mongo admin connection
                lstConnections.Items.Add(CreateNewDatabase("MongoAdmin", "Mongo"));
                SaveConfig(false);
            }

            if (lstConnections.Items.Count > 0)
                lstConnections.Items[0].Selected = true;
        }

        private void lstConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            _changingRows = true;
            _currentDatabase = null;

            if (lstConnections.SelectedItems.Count > 0)
            {
                _currentDatabase = _configDoc.SelectSingleNode(string.Format("/configuration/database[@name='{0}'][@type='{1}']", 
                    lstConnections.SelectedItems[0].SubItems[0].Text, lstConnections.SelectedItems[0].SubItems[1].Text));

                txtConnectionName.Text = GetDatabaseAttrValue(_currentDatabase, "name");
                lblConnectionType.Text = GetDatabaseAttrValue(_currentDatabase, "type");
                txtDatabase.Text = GetDatabaseValue(_currentDatabase, "databasename");
                txtMongoOptions.Text = GetDatabaseValue(_currentDatabase, "options");
                txtMongoReplSetName.Text = GetDatabaseValue(_currentDatabase, "replicationsetname");
                txtServer.Text = GetDatabaseValue(_currentDatabase, "server");
                txtUserName.Text = GetDatabaseValue(_currentDatabase, "username");
                txtDomainPrefix.Text = GetDatabaseValue(_currentDatabase, "domainprefix");

                string pwd = GetDatabaseValue(_currentDatabase, "password");

                if (pwd != string.Empty)
                    pwd = protector.Decrypt(pwd);

                txtPassword.Text = pwd;
                
                chkTrustedConnection.Checked = (GetDatabaseValue(_currentDatabase, "trusted") == "True");

                txtPassword.Enabled =
                    txtUserName.Enabled = (!chkTrustedConnection.Checked);

                txtDomainPrefix.Enabled = (chkTrustedConnection.Checked && lblConnectionType.Text.ToUpper() == "MONGO");

                bool isMongoAdmin = (txtConnectionName.Text.ToUpper() == "MONGOADMIN" && lblConnectionType.Text.ToUpper() == "MONGO");

                txtConnectionName.Enabled =
                    txtDatabase.Enabled =
                    txtMongoOptions.Enabled =
                    txtMongoReplSetName.Enabled =
                    txtServer.Enabled = 
                    btnTest.Enabled = 
                    chkTrustedConnection.Enabled = !isMongoAdmin;

                txtMongoReplSetName.Enabled = !isMongoAdmin && (lblConnectionType.Text.ToUpper() == "MONGO");
            }
            _changingRows = false;
        }

        private void SetDatabaseValue(XmlNode database, string nodeName, string nodeValue)
        {
            if (_changingRows)
                return;

            XmlNode node = GetDatabaseNode(database, nodeName);
            if (node != null)
            {
                node.InnerText = nodeValue;
            }
            else
            {
                XmlElement newNode = _configDoc.CreateElement(nodeName);
                newNode.InnerText = nodeValue;
                database.AppendChild(newNode);
            }

            _somethingChanged = true;
        }

        private XmlNode GetDatabaseNode(XmlNode database, string nodeName)
        {
            XmlNode returnVal = null;
            try
            {
                returnVal = database.SelectSingleNode(nodeName);
            }
            catch { }

            if (returnVal == null)
            {
                try
                {
                    returnVal = database.Attributes.GetNamedItem(nodeName);
                }
                catch { }
            }
            return returnVal;
        }

        private string GetDatabaseValue(XmlNode database, string nodeName)
        {
            string returnVal = string.Empty;
            try
            {
                returnVal = database.SelectSingleNode(nodeName).InnerText;
            }
            catch { }

            return returnVal;
        }

        private string GetDatabaseAttrValue(XmlNode database, string nodeName)
        {
            string returnVal = string.Empty;
            
            try
            {
                returnVal = database.Attributes.GetNamedItem(nodeName).Value;
            }
            catch { }

            return returnVal;
        }

        private void BuildDefaultFile()
        {
            string fileInfo = @"<?xml version='1.0' encoding='Windows-1252'?><configuration></configuration>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fileInfo);
            doc.Save(Phytel.Services.DataProtector.GetConfigurationPathAndFile());

            string serverConfigInfo = @"<?xml version='1.0' encoding='utf-8'?><configuration><entropy>E0E63090-85B7-49e9-BA9C-F49DE2DB3291</entropy></configuration>";
            string serverConfigFilePath = string.Format(@"{0}\{1}", Phytel.Services.DataProtector.GetConfigurationPath(), "server.config");
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

        private void txtConnectionName_TextChanged(object sender, EventArgs e)
        {
            SetDatabaseValue(_currentDatabase, "name", txtConnectionName.Text);
            lstConnections.SelectedItems[0].SubItems[0].Text = txtConnectionName.Text;
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            SetDatabaseValue(_currentDatabase, "server", txtServer.Text);
        }

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {
            SetDatabaseValue(_currentDatabase, "databasename", txtDatabase.Text);
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            SetDatabaseValue(_currentDatabase, "username", txtUserName.Text);
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            SetDatabaseValue(_currentDatabase, "password", protector.Encrypt(txtPassword.Text));
        }

        private void txtMongoReplSetName_TextChanged(object sender, EventArgs e)
        {
            SetDatabaseValue(_currentDatabase, "replicationsetname", txtMongoReplSetName.Text);
        }

        private void txtMongoOptions_TextChanged(object sender, EventArgs e)
        {
            SetDatabaseValue(_currentDatabase, "options", txtMongoOptions.Text);
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmNew frm = new frmNew();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                XmlNodeList databases = _configDoc.SelectNodes("/configuration/database");
                foreach (XmlNode node in databases)
                {
                    if (node.Attributes["name"].Value == frm.ConnectionName && node.Attributes["type"].Value == frm.ConnectionType)
                    {
                        MessageBox.Show("You cannot add a connection of the same name and connection type!");
                        return;
                    }
                }

                ListViewItem lvi = CreateNewDatabase(frm.ConnectionName, frm.ConnectionType);
                lstConnections.Items.Add(lvi);

                lstConnections.SelectedItems.Clear();

                lvi.Selected = true;
            }
        }

        private void rdoSQL_CheckedChanged(object sender, EventArgs e)
        {
            txtMongoOptions.Enabled = txtMongoReplSetName.Enabled = false;
            lstConnections.SelectedItems[0].SubItems[1].Text = "SQL";
        }

        private void rdoMongo_CheckedChanged(object sender, EventArgs e)
        {
            txtMongoOptions.Enabled = txtMongoReplSetName.Enabled = true;
            lstConnections.SelectedItems[0].SubItems[1].Text = "Mongo";

            if (txtMongoReplSetName.Text == string.Empty)
                txtMongoReplSetName.Text = "27017";
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _configDoc.SelectSingleNode("/configuration").RemoveChild(_currentDatabase);
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
                if (lstConnections.SelectedItems[0].SubItems[1].Text == "Mongo")
                    MongoService.Instance.GetDatabase(txtConnectionName.Text, false).FindAllUsers();
                else
                    SQLDataService.Instance.ExecuteSQL(txtConnectionName.Text, false, "Select Top 1 1 From sysobjects");

                MessageBox.Show("Connection Succeeded!", "Phytel Services", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failed: " + ex.Message, "Phytel Services", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    _configDoc.Save(Phytel.Services.DataProtector.GetConfigurationPathAndFile());
                    _somethingChanged = false;
                }
            }
            return result;
        }

        private void chkTrustedConnection_CheckedChanged(object sender, EventArgs e)
        {
            SetDatabaseValue(_currentDatabase, "trusted", chkTrustedConnection.Checked.ToString());

            txtPassword.Enabled = 
                txtUserName.Enabled = (!chkTrustedConnection.Checked);

            txtDomainPrefix.Enabled = (chkTrustedConnection.Checked && lblConnectionType.Text == "Mongo");

            if (txtDomainPrefix.Enabled && txtDomainPrefix.Text.Trim() == string.Empty)
                txtDomainPrefix.Text = "phytel.com";
        }

        private void txtDomainPrefix_TextChanged(object sender, EventArgs e)
        {
            SetDatabaseValue(_currentDatabase, "domainprefix", txtDomainPrefix.Text);
        }
    }
}
