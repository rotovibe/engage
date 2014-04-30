using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace DataProtectorWinApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            Phytel.Framework.Data.DataProtector protector = null;

            switch (cboStore.SelectedItem.ToString())
            {
                case "MACHINE":
                    protector = new Phytel.Framework.Data.DataProtector(Phytel.Framework.Data.DataProtector.Store.USE_MACHINE_STORE);
                    break;
                case "USER":
                    protector = new Phytel.Framework.Data.DataProtector(Phytel.Framework.Data.DataProtector.Store.USE_USER_STORE);
                    break;
                case "SIMPLE":
                    protector = new Phytel.Framework.Data.DataProtector(Phytel.Framework.Data.DataProtector.Store.USE_SIMPLE_STORE);
                    break;
            }
            this.txtDecrypt.Text = protector.Encrypt(txtEncrypt.Text, txtConfig.Text, Phytel.Framework.Data.DataProtector.EntropyType.CONFIGPATH);
        }

        public string GetEntropyKey(string configPath)
        {
            string entropy = "";

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configPath + "\\server.config");

                XmlNode node = doc.SelectSingleNode("//configuration/entropy");

                if ((node != null))
                {
                    entropy = node.InnerText;
                }
            }
            catch (System.Exception ex)
            {
                string msg = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name + System.Environment.NewLine;
                MessageBox.Show(msg + ex.Message);
            }

            return entropy;
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            Phytel.Framework.Data.DataProtector protector = null;

            switch (cboStore.SelectedItem.ToString())
            {
                case "MACHINE":
                    protector = new Phytel.Framework.Data.DataProtector(Phytel.Framework.Data.DataProtector.Store.USE_MACHINE_STORE);
                    break;
                case "USER":
                    protector = new Phytel.Framework.Data.DataProtector(Phytel.Framework.Data.DataProtector.Store.USE_USER_STORE);
                    break;
                case "SIMPLE":
                    protector = new Phytel.Framework.Data.DataProtector(Phytel.Framework.Data.DataProtector.Store.USE_SIMPLE_STORE);
                    break;
            }
            this.txtEncrypt.Text = protector.Decrypt(txtDecrypt.Text, txtConfig.Text, Phytel.Framework.Data.DataProtector.EntropyType.CONFIGPATH);
        } 
    }
}