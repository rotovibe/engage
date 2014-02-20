using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using Phytel.API.DataDomain.Patient.DTO;
using System.Runtime.Serialization.Json;

namespace NightingaleImport
{

    public partial class Form1 : Form
    {
        string filename;
        static int colFirstN = 0;
        static int colLastN = 1;
        static int colMiddleN = 2;
        static int colSuff = 3;
        static int colPrefN = 4;
        static int colGen = 5;
        static int colDB = 6;
        static int colSID = 7;
        static int colSysN = 8;
        static int colTimeZ = 9;
        static int colPh1 = 10;
        static int colPh1Pref = 11;
        static int colPh1Type = 12;
        static int colPh2 = 13;
        static int colPh2Pref = 14;
        static int colPh2Type = 15;
        static int colEm1 = 16;
        static int colEm1Pref = 17;
        static int colEm1Type = 18;
        static int colEm2 = 19;
        static int colEm2Pref = 20;
        static int colEm2Type = 21;
        static int colAdd1L1 = 22;
        static int colAdd1L2 = 23;
        static int colAdd1L3 = 24;
        static int colAdd1City = 25;
        static int colAdd1St = 26;
        static int colAdd1Zip = 27;
        static int colAdd1Pref = 28;
        static int colAdd1Type = 29;
        static int colAdd2L1 = 30;
        static int colAdd2L2 = 31;
        static int colAdd2L3 = 32;
        static int colAdd2City = 33;
        static int colAdd2St = 34;
        static int colAdd2Zip = 35;
        static int colAdd2Pref = 36;
        static int colAdd2Type = 37;
        static int colSSN = 38;


                
        public Form1()
        {
            InitializeComponent();
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);

        }

        public void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            
        }

        private void button1_VisibleChanged(object sender, EventArgs e)
        {
            Browse.Text = "Browse";
        }

        private void textBox1_VisibleChanged(object sender, EventArgs e)
        {
            if (filename == null)
                textBox1.Text = "Choose a file...";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Uri theUri = new Uri("http://localhost:8888/Patient/NG/v1/InHealth001/patient");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Host = theUri.Host;

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            Dictionary<string, string> dictionarySucceed = new Dictionary<string, string>();
            Dictionary<string, string> dictionaryFail = new Dictionary<string, string>();

            foreach (ListViewItem lvi in listView1.CheckedItems)
            {
                
                PutPatientDataRequest request = new PutPatientDataRequest
                {
                    FirstName = lvi.SubItems[colFirstN].Text,
                    LastName = lvi.SubItems[colLastN].Text,
                    MiddleName = lvi.SubItems[colMiddleN].Text,
                    Suffix = lvi.SubItems[colSuff].Text,
                    PreferredName = lvi.SubItems[colPrefN].Text,
                    Gender = lvi.SubItems[colGen].Text,
                    DOB = lvi.SubItems[colDB].Text,
                    //SSN = lvi.SubItems[colSSN].Text,
                    SystemID = lvi.SubItems[colSID].Text,
                    SystemName = lvi.SubItems[colSysN].Text,
                    TimeZone = lvi.SubItems[colTimeZ].Text,
                    Phone1 = lvi.SubItems[colPh1].Text,
                    Phone1Type = lvi.SubItems[colPh1Type].Text,
                    Phone2 = lvi.SubItems[colPh2].Text,
                    Phone2Type = lvi.SubItems[colPh2Type].Text,
                    Email1 = lvi.SubItems[colEm1].Text,
                    Email1Type = lvi.SubItems[colEm1Type].Text,
                    Email2 = lvi.SubItems[colEm2].Text,
                    Email2Type = lvi.SubItems[colEm2Type].Text,
                    Address1Line1 = lvi.SubItems[colAdd1L1].Text,
                    Address1Line2 = lvi.SubItems[colAdd1L2].Text,
                    Address1Line3 = lvi.SubItems[colAdd1L3].Text,
                    Address1City = lvi.SubItems[colAdd1City].Text,
                    Address1State = lvi.SubItems[colAdd1St].Text,
                    Address1Zip = lvi.SubItems[colAdd1Zip].Text,
                    Address1Type = lvi.SubItems[colAdd1Type].Text,
                    Address2Line1 = lvi.SubItems[colAdd2L1].Text,
                    Address2Line2 = lvi.SubItems[colAdd2L2].Text,
                    Address2Line3 = lvi.SubItems[colAdd2L3].Text,
                    Address2City = lvi.SubItems[colAdd2City].Text,
                    Address2State = lvi.SubItems[colAdd2St].Text,
                    Address2Zip = lvi.SubItems[colAdd2Zip].Text,
                    Address2Type = lvi.SubItems[colAdd2Type].Text
                };

                if (lvi.SubItems[colPh1Pref].Text == "True")
                {
                    request.Phone1Preferred = true;
                }
                else
                    request.Phone1Preferred = false;

                if (lvi.SubItems[colPh2Pref].Text == "True")
                {
                    request.Phone2Preferred = true;
                }
                else
                    request.Phone2Preferred = false;

                if (lvi.SubItems[colEm1Pref].Text == "True")
                {
                    request.Email1Preferred = true;
                }
                else
                    request.Email1Preferred = false;

                if (lvi.SubItems[colEm2Pref].Text == "True")
                {
                    request.Email2Preferred = true;
                }
                else
                    request.Email2Preferred = false;

                if (lvi.SubItems[colAdd1Pref].Text == "True")
                {
                    request.Address1Preferred = true;
                }
                else
                    request.Address1Preferred = false;

                if (lvi.SubItems[colAdd2Pref].Text == "True")
                {
                    request.Address2Preferred = true;
                }
                else
                    request.Address2Preferred = false;

                
                
                DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(PutPatientDataRequest));

                // use the serializer to write the object to a MemoryStream 
                MemoryStream ms = new MemoryStream();
                jsonSer.WriteObject(ms, request);
                ms.Position = 0;

                //use a Stream reader to construct the StringContent (Json) 
                StreamReader sr = new StreamReader(ms);
                
                StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                //Post the data 
                var response = client.PutAsync(theUri, theContent);
                if(response.Result.IsSuccessStatusCode)
                {
                    var responseContent = response.Result.Content;

                    string responseString = responseContent.ReadAsStringAsync().Result;

                    PutPatientDataResponse responseObj = null;

                    using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(PutPatientDataResponse));
                        responseObj = (PutPatientDataResponse)serializer.ReadObject(msResponse);
                    }
                    dictionarySucceed.Add(request.FirstName + " " + request.LastName, responseObj.Id);
                    int n = listView1.CheckedItems.IndexOf(lvi);
                    listView1.CheckedItems[n].Remove();
                }
                else
                {
                    var responseContent = response.Result.Content;

                    string responseString = responseContent.ReadAsStringAsync().Result;
                    PutPatientDataResponse responseObj = null;

                    using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(PutPatientDataResponse));
                        responseObj = (PutPatientDataResponse)serializer.ReadObject(msResponse);
                    }
                    dictionaryFail.Add(request.FirstName + " " + request.LastName, responseObj.Status.Message);
                    int n = listView1.CheckedItems.IndexOf(lvi);
                    listView1.CheckedItems[n].BackColor = Color.Red;
                    listView1.CheckedItems[n].Checked = false;
                }
                
            }

            string listSucceed = null;
            foreach(var pairSucceed in dictionarySucceed)
            {
                listSucceed += pairSucceed + "\n";
            }

            string listFail = null;
            foreach (var pairFail in dictionaryFail)
            {
                listFail += pairFail + "\n";
            }

            MessageBox.Show(dictionarySucceed.Count() + " patient file(s) successfully imported:\n" + listSucceed + "\n"
                                + dictionaryFail.Count() + " patient file(s) failed to import: \n" + listFail);
        }

        private void button1_VisibleChanged_1(object sender, EventArgs e)
        {
            button1.Text = "Import";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //close Form1
            Application.Exit();
        }

        private void button2_VisibleChanged(object sender, EventArgs e)
        {
            button2.Text = "Cancel";
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (openFileDialog1.CheckFileExists)
            {
                filename = openFileDialog1.FileName;
                textBox1.Text = filename;
                string[] attributes;
                string[]filelines = File.ReadAllLines(filename);
                foreach (string line in filelines)
                {
                    attributes = line.Split(",".ToCharArray());
                    ListViewItem lvi = new ListViewItem(attributes[colFirstN]);
                    for (int i = 1; i < attributes.Count(); i++)
                    {
                        lvi.SubItems.Add(attributes[i]);
                    }
                   
                    listView1.Items.Add(lvi);
                }    
            }
        }


        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column);
            listView1.Sort();
        }

        class ListViewItemComparer : System.Collections.IComparer
        {
            private int col;
            public ListViewItemComparer()
            {
                col = 0;
            }
            public ListViewItemComparer(int column)
            {
                col = column;
            }
            public int Compare(object x, object y)
            {
                int returnVal = -1;
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                ((ListViewItem)y).SubItems[col].Text);
                return returnVal;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (checkBox1.Checked)
                {
                    listView1.Items[i].Checked = true;
                }
                else
                    listView1.Items[i].Checked = false;
            }

        }
    }
}
