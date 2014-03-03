using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using Phytel.API.Common.CustomObjects;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.CareMember.DTO;
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
        static int colCMan = 38;
        //static int colSSN = 39;


        private List<IdNamePair> modesLookUp = new List<IdNamePair>();
        private List<CommTypeData> typesLookUp = new List<CommTypeData>();
        private List<StateData> statesLookUp = new List<StateData>();
        private List<TimeZoneData> zonesLookUp = new List<TimeZoneData>();
        private TimeZoneData zoneDefault = new TimeZoneData();

        //TODO: Move to app config
        private string version = "v1";
        private string context = "NG";
        private string contractNumber = "InHealth001";
        private Guid userId = Guid.Parse("BB241C64-A0FF-4E01-BA5F-4246EF50780E");
        private string contactTypeId = "530cd571d433231ed4ba969b";

                
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
            try
            {
                Dictionary<string, string> dictionarySucceed = new Dictionary<string, string>();
                Dictionary<string, string> dictionaryFail = new Dictionary<string, string>();

                foreach (ListViewItem lvi in listView1.CheckedItems)
                {

                    PutPatientDataRequest patientRequest = new PutPatientDataRequest
                    {
                        FirstName = lvi.SubItems[colFirstN].Text,
                        LastName = lvi.SubItems[colLastN].Text,
                        MiddleName = lvi.SubItems[colMiddleN].Text,
                        Suffix = lvi.SubItems[colSuff].Text,
                        PreferredName = lvi.SubItems[colPrefN].Text,
                        Gender = lvi.SubItems[colGen].Text,
                        DOB = lvi.SubItems[colDB].Text,
                        Context = context,
                        ContractNumber = contractNumber,
                        Version = version,
                        //SSN = lvi.SubItems[colSSN].Text,
                    };
                    PutPatientDataResponse responsePatient = putPatientServiceCall(patientRequest);

                    //PatientSystem
                    PutPatientSystemDataRequest patSysRequest = new PutPatientSystemDataRequest
                    {
                        PatientID = responsePatient.Id,
                        SystemID = lvi.SubItems[colSID].Text,
                        SystemName = lvi.SubItems[colSysN].Text,
                        Version = responsePatient.Version,
                        Context = patientRequest.Context,
                        ContractNumber = patientRequest.ContractNumber
                    };
                    PutPatientSystemDataResponse responsePatientPS = putPatientSystemServiceCall(patSysRequest);

                    //Update Patient with DisplayPatientSystemId
                    PutUpdatePatientDataRequest updatePatientRequest = new PutUpdatePatientDataRequest
                    {
                        Id = responsePatient.Id.ToString(),
                        FirstName = patientRequest.FirstName,
                        LastName = patientRequest.LastName,
                        DisplayPatientSystemId = responsePatientPS.PatientSystemId.ToString(),
                        Priority = 0,
                        Context = patientRequest.Context,
                        ContractNumber = patientRequest.ContractNumber,
                        Version = patientRequest.Version,
                        UserId = userId.ToString()
                    };

                    PutUpdatePatientDataResponse updateResponsePatient = putUpdatePatientServiceCall(updatePatientRequest, responsePatient.Id.ToString());



                    //Contact
                    LoadLookUps();

                    //timezone
                    TimeZoneData tZone = new TimeZoneData();
                    if (lvi.SubItems[colTimeZ].Text != null)
                    {
                        foreach (TimeZoneData t in zonesLookUp)
                        {
                            string[] zones = t.Name.Split(" ".ToCharArray());
                            if (lvi.SubItems[colTimeZ].Text == zones[0])
                            {
                                tZone.Id = t.Id;
                            }
                        }
                    }
                    else
                    {
                        tZone.Id = zoneDefault.Id;
                    }

                    List<CommModeData> modes = new List<CommModeData>();
                    List<PhoneData> phones = new List<PhoneData>();
                    List<AddressData> addresses = new List<AddressData>();
                    List<EmailData> emails = new List<EmailData>();

                    //modes
                    if (modesLookUp != null && modesLookUp.Count > 0)
                    {
                        foreach (IdNamePair l in modesLookUp)
                        {
                            modes.Add(new CommModeData { ModeId = l.Id, OptOut = false, Preferred = false });
                        }
                    }


                    //phones
                    if (lvi.SubItems[colPh1].Text != "")
                    {
                        PhoneData phone1 = new PhoneData
                        {
                            Number = Convert.ToInt64(lvi.SubItems[colPh1].Text),
                            OptOut = false
                        };

                        if (lvi.SubItems[colPh1Pref].Text == "True")
                        {
                            phone1.PhonePreferred = true;
                        }
                        else
                            phone1.PhonePreferred = false;

                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (lvi.SubItems[colPh1Type].Text == c.Name)
                            {
                                phone1.TypeId = c.Id;
                            }
                        }
                        phones.Add(phone1);
                    }

                    if (lvi.SubItems[colPh2].Text != "")
                    {
                        PhoneData phone2 = new PhoneData
                        {
                            Number = Convert.ToInt64(lvi.SubItems[colPh2].Text),
                            OptOut = false
                        };

                        if (lvi.SubItems[colPh2Pref].Text == "True")
                        {
                            phone2.PhonePreferred = true;
                        }
                        else
                            phone2.PhonePreferred = false;

                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (lvi.SubItems[colPh2Type].Text == c.Name)
                            {
                                phone2.TypeId = c.Id;
                            }
                        }
                        phones.Add(phone2);
                    }

                    //emails
                    if (lvi.SubItems[colEm1].Text != "")
                    {
                        EmailData email1 = new EmailData
                        {
                            Text = lvi.SubItems[colEm1].Text,
                            OptOut = false,
                        };

                        if (lvi.SubItems[colEm1Pref].Text == "True")
                        {
                            email1.Preferred = true;
                        }
                        else
                            email1.Preferred = false;

                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (lvi.SubItems[colEm1Type].Text == c.Name)
                            {
                                email1.TypeId = c.Id;
                            }
                        }
                        emails.Add(email1);
                    }

                    if (lvi.SubItems[colEm2].Text != "")
                    {
                        EmailData email2 = new EmailData
                        {
                            Text = lvi.SubItems[colEm2].Text,
                            OptOut = false,
                        };

                        if (lvi.SubItems[colEm2Pref].Text == "True")
                        {
                            email2.Preferred = true;
                        }
                        else
                            email2.Preferred = false;

                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (lvi.SubItems[colEm2Type].Text == c.Name)
                            {
                                email2.TypeId = c.Id;
                            }
                        }
                        emails.Add(email2);
                    }

                    //addresses
                    if (!(string.IsNullOrEmpty(lvi.SubItems[colAdd1L1].Text)))
                    {
                        AddressData add1 = new AddressData
                        {
                            Line1 = lvi.SubItems[colAdd1L1].Text,
                            Line2 = lvi.SubItems[colAdd1L2].Text,
                            Line3 = lvi.SubItems[colAdd1L3].Text,
                            City = lvi.SubItems[colAdd1City].Text,
                            PostalCode = lvi.SubItems[colAdd1Zip].Text,
                            OptOut = false
                        };

                        if (lvi.SubItems[colAdd1Pref].Text == "True")
                        {
                            add1.Preferred = true;
                        }
                        else
                            add1.Preferred = false;

                        foreach (StateData st in statesLookUp)
                        {
                            if (st.Name == lvi.SubItems[colAdd1St].Text)
                            {
                                add1.StateId = st.Id;
                            }
                        }
                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (lvi.SubItems[colAdd1Type].Text == c.Name)
                            {
                                add1.TypeId = c.Id;
                            }
                        }
                        addresses.Add(add1);
                    }

                    if (!(string.IsNullOrEmpty(lvi.SubItems[colAdd2L1].Text)))
                    {
                        AddressData add2 = new AddressData
                        {
                            Line1 = lvi.SubItems[colAdd2L1].Text,
                            Line2 = lvi.SubItems[colAdd2L2].Text,
                            Line3 = lvi.SubItems[colAdd2L3].Text,
                            City = lvi.SubItems[colAdd2City].Text,
                            PostalCode = lvi.SubItems[colAdd2Zip].Text,
                            OptOut = false
                        };

                        if (lvi.SubItems[colAdd2Pref].Text == "True")
                        {
                            add2.Preferred = true;
                        }
                        else
                            add2.Preferred = false;

                        foreach (StateData st in statesLookUp)
                        {
                            if (st.Name == lvi.SubItems[colAdd2St].Text)
                            {
                                add2.StateId = st.Id;
                            }
                        }
                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (lvi.SubItems[colAdd2Type].Text == c.Name)
                            {
                                add2.TypeId = c.Id;
                            }
                        }
                        addresses.Add(add2);
                    }

                    //contact
                    PutContactDataRequest contactRequest = new PutContactDataRequest
                    {
                        PatientId = responsePatient.Id,
                        Modes = modes,
                        TimeZoneId = tZone.Id,
                        Phones = phones,
                        Emails = emails,
                        Addresses = addresses,
                        Version = patientRequest.Version,
                        Context = patientRequest.Context,
                        ContractNumber = patientRequest.ContractNumber
                    };

                    PutContactDataResponse responseContact = putContactServiceCall(contactRequest, responsePatient.Id.ToString());

                    Guid userIdResponse = getUserId(lvi.SubItems[colCMan].Text);
                    GetContactByUserIdDataResponse contactByUserIdResponse = getContactByUserIdServiceCall(userIdResponse.ToString());

                    CareMemberData careMember = new CareMemberData
                    {
                        PatientId = responsePatient.Id.ToString(),
                        ContactId = contactByUserIdResponse.Contact.ContactId,
                        TypeId = contactTypeId,
                        Primary = true,
                    };

                    PutCareMemberDataRequest careMemberRequest = new PutCareMemberDataRequest
                    {
                        PatientId = responsePatient.Id.ToString(),
                        UserId = userId.ToString(),
                        CareMember = careMember
                    };
                    PutCareMemberDataResponse responseCareMember = putCareMemberServiceCall(careMemberRequest, responsePatient.Id.ToString());


                    if (responsePatient.Id != null && responseCareMember.Id != null && responsePatient.Status == null)
                    {
                        dictionarySucceed.Add(patientRequest.FirstName + " " + patientRequest.LastName, responsePatient.Id);
                        int n = listView1.CheckedItems.IndexOf(lvi);
                        listView1.CheckedItems[n].Remove();
                    }
                    else
                    {
                        dictionaryFail.Add(patientRequest.FirstName + " " + patientRequest.LastName, responsePatient.Status.ToString());
                        int n = listView1.CheckedItems.IndexOf(lvi);
                        listView1.CheckedItems[n].BackColor = Color.Red;
                        listView1.CheckedItems[n].Checked = false;
                    }

                }

                string listSucceed = null;
                foreach (var pairSucceed in dictionarySucceed)
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadLookUps()
        {
            //modes
            Uri modesUri = new Uri(string.Format("{0}/{1}/{2}/{3}/commmodes",
                                                    "http://localhost:8888/LookUp",
                                                    context,
                                                    version,
                                                    contractNumber));
            HttpClient modesClient = new HttpClient();
            modesClient.DefaultRequestHeaders.Host = modesUri.Host;
            modesClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            GetAllCommModesDataRequest modesRequest = new GetAllCommModesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = contractNumber
            };

            DataContractJsonSerializer modesJsonSer = new DataContractJsonSerializer(typeof(GetAllCommModesDataRequest));
            MemoryStream modesMs = new MemoryStream();
            modesJsonSer.WriteObject(modesMs, modesRequest);
            modesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader modesSr = new StreamReader(modesMs);
            StringContent modesContent = new StringContent(modesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var modesResponse = modesClient.GetStringAsync(modesUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetAllCommModesDataResponse responseModes = null;

            using (var modesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(modesResponseString)))
            {
                var modesSerializer = new DataContractJsonSerializer(typeof(GetAllCommModesDataResponse));
                responseModes = (GetAllCommModesDataResponse)modesSerializer.ReadObject(modesMsResponse);
            }
            modesLookUp = responseModes.CommModes;

            //types
            Uri typesUri = new Uri(string.Format("{0}/{1}/{2}/{3}/commtypes",
                                                    "http://localhost:8888/LookUp",
                                                    context,
                                                    version,
                                                    contractNumber));
            HttpClient typesClient = new HttpClient();
            typesClient.DefaultRequestHeaders.Host = typesUri.Host;
            typesClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            GetAllCommTypesDataRequest typesRequest = new GetAllCommTypesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = contractNumber
            };

            DataContractJsonSerializer typesJsonSer = new DataContractJsonSerializer(typeof(GetAllCommTypesDataRequest));
            MemoryStream typesMs = new MemoryStream();
            typesJsonSer.WriteObject(typesMs, typesRequest);
            typesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader typesSr = new StreamReader(typesMs);
            StringContent typesContent = new StringContent(typesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var typesResponse = typesClient.GetStringAsync(typesUri);
            var typesResponseContent = typesResponse.Result;

            string typesResponseString = typesResponseContent;
            GetAllCommTypesDataResponse responseTypes = null;

            using (var typesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(typesResponseString)))
            {
                var typesSerializer = new DataContractJsonSerializer(typeof(GetAllCommTypesDataResponse));
                responseTypes = (GetAllCommTypesDataResponse)typesSerializer.ReadObject(typesMsResponse);
            }
            typesLookUp = responseTypes.CommTypes;

            //states
            Uri statesUri = new Uri(string.Format("{0}/{1}/{2}/{3}/states",
                                                    "http://localhost:8888/LookUp",
                                                    context,
                                                    version,
                                                    contractNumber));
            HttpClient statesClient = new HttpClient();
            statesClient.DefaultRequestHeaders.Host = statesUri.Host;
            statesClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            GetAllStatesDataRequest statesRequest = new GetAllStatesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = contractNumber
            };

            DataContractJsonSerializer statesJsonSer = new DataContractJsonSerializer(typeof(GetAllStatesDataRequest));
            MemoryStream statesMs = new MemoryStream();
            statesJsonSer.WriteObject(statesMs, statesRequest);
            statesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader statesSr = new StreamReader(statesMs);
            StringContent statesContent = new StringContent(statesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var statesResponse = statesClient.GetStringAsync(statesUri);
            var statesResponseContent = statesResponse.Result;

            string statesResponseString = statesResponseContent;
            GetAllStatesDataResponse responseStates = null;

            using (var statesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(statesResponseString)))
            {
                var statesSerializer = new DataContractJsonSerializer(typeof(GetAllStatesDataResponse));
                responseStates = (GetAllStatesDataResponse)statesSerializer.ReadObject(statesMsResponse);
            }
            statesLookUp = responseStates.States;

            //timezones
            Uri zonesUri = new Uri(string.Format("{0}/{1}/{2}/{3}/timeZones",
                                                    "http://localhost:8888/LookUp",
                                                    context,
                                                    version,
                                                    contractNumber));
            HttpClient zonesClient = new HttpClient();
            zonesClient.DefaultRequestHeaders.Host = zonesUri.Host;
            zonesClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            GetAllTimeZonesDataRequest zonesRequest = new GetAllTimeZonesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = contractNumber
            };

            DataContractJsonSerializer zonesJsonSer = new DataContractJsonSerializer(typeof(GetAllTimeZonesDataRequest));
            MemoryStream zonesMs = new MemoryStream();
            zonesJsonSer.WriteObject(zonesMs, zonesRequest);
            zonesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader zonesSr = new StreamReader(zonesMs);
            StringContent zonesContent = new StringContent(zonesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var zonesResponse = zonesClient.GetStringAsync(zonesUri);
            var zonesResponseContent = zonesResponse.Result;

            string zonesResponseString = zonesResponseContent;
            GetAllTimeZonesDataResponse responseZones = null;

            using (var zonesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(zonesResponseString)))
            {
                var zonesSerializer = new DataContractJsonSerializer(typeof(GetAllTimeZonesDataResponse));
                responseZones = (GetAllTimeZonesDataResponse)zonesSerializer.ReadObject(zonesMsResponse);
            }
            zonesLookUp = responseZones.TimeZones;

            //default timezone
            Uri zoneUri = new Uri(string.Format("{0}/{1}/{2}/{3}/TimeZone/Default",
                                                    "http://localhost:8888/LookUp",
                                                    context,
                                                    version,
                                                    contractNumber));
            HttpClient zoneClient = new HttpClient();
            zoneClient.DefaultRequestHeaders.Host = zoneUri.Host;
            zoneClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            GetTimeZoneDataRequest zoneRequest = new GetTimeZoneDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = contractNumber
            };

            DataContractJsonSerializer zoneJsonSer = new DataContractJsonSerializer(typeof(GetTimeZoneDataRequest));
            MemoryStream zoneMs = new MemoryStream();
            zoneJsonSer.WriteObject(zoneMs, zoneRequest);
            zoneMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader zoneSr = new StreamReader(zoneMs);
            StringContent zoneContent = new StringContent(zoneSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var zoneResponse = zoneClient.GetStringAsync(zoneUri);
            var zoneResponseContent = zoneResponse.Result;

            string zoneResponseString = zoneResponseContent;
            GetTimeZoneDataResponse responseZone = null;

            using (var zoneMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(zoneResponseString)))
            {
                var zoneSerializer = new DataContractJsonSerializer(typeof(GetTimeZoneDataResponse));
                responseZone = (GetTimeZoneDataResponse)zoneSerializer.ReadObject(zoneMsResponse);
            }
            zoneDefault = responseZone.TimeZone;
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
            button2.Text = "Close";
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

        private Guid getUserId(string userName)
        {
            Guid returnId = Guid.Empty;

            string sql = string.Format("Select userId from [User] where UserName = '{0}'",
                                        userName);
            DataSet ds = Phytel.Services.SQLDataService.Instance.ExecuteSQL("Phytel", false, sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                returnId = Guid.Parse(ds.Tables[0].Rows[0]["userId"].ToString());
            }
            return returnId;
        }

        private PutPatientDataResponse putPatientServiceCall(PutPatientDataRequest putPatientRequest)
        {
            //Patient
            Uri theUri = new Uri(string.Format("{0}/{1}/{2}/{3}/patient",
                                                 "http://localhost:8888/Patient",
                                                 context,
                                                 version,
                                                 contractNumber));
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Host = theUri.Host;

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(PutPatientDataRequest));

            // use the serializer to write the object to a MemoryStream 
            MemoryStream ms = new MemoryStream();
            jsonSer.WriteObject(ms, putPatientRequest);
            ms.Position = 0;


            //use a Stream reader to construct the StringContent (Json) 
            StreamReader sr = new StreamReader(ms);

            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var response = client.PutAsync(theUri, theContent);
            var responseContent = response.Result.Content;

            string responseString = responseContent.ReadAsStringAsync().Result;
            PutPatientDataResponse responsePatient = null;

            using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
            {
                var serializer = new DataContractJsonSerializer(typeof(PutPatientDataResponse));
                responsePatient = (PutPatientDataResponse)serializer.ReadObject(msResponse);
            }

            return responsePatient;
        }

        private PutPatientSystemDataResponse putPatientSystemServiceCall(PutPatientSystemDataRequest putPatSysRequest)
        {
            //PatientSystem
            Uri theUriPS = new Uri(string.Format("{0}/{1}/{2}/{3}/patientsystem",
                                                   "http://localhost:8888/PatientSystem",
                                                   context,
                                                   version,
                                                   contractNumber));
            HttpClient clientPS = new HttpClient();
            clientPS.DefaultRequestHeaders.Host = theUriPS.Host;
            clientPS.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            DataContractJsonSerializer jsonSerPS = new DataContractJsonSerializer(typeof(PutPatientSystemDataRequest));

            // use the serializer to write the object to a MemoryStream 
            MemoryStream msPS = new MemoryStream();
            jsonSerPS.WriteObject(msPS, putPatSysRequest);
            msPS.Position = 0;


            //use a Stream reader to construct the StringContent (Json) 
            StreamReader srPS = new StreamReader(msPS);

            StringContent theContentPS = new StringContent(srPS.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var responsePS = clientPS.PutAsync(theUriPS, theContentPS);
            var responseContentPS = responsePS.Result.Content;

            string responseStringPS = responseContentPS.ReadAsStringAsync().Result;
            PutPatientSystemDataResponse responsePatientPS = null;

            using (var msResponsePS = new MemoryStream(Encoding.Unicode.GetBytes(responseStringPS)))
            {
                var serializerPS = new DataContractJsonSerializer(typeof(PutPatientSystemDataResponse));
                responsePatientPS = (PutPatientSystemDataResponse)serializerPS.ReadObject(msResponsePS);
            }

            return responsePatientPS;
        }

        private PutUpdatePatientDataResponse putUpdatePatientServiceCall(PutUpdatePatientDataRequest putUpdatePatient, string patientId)
        {
            Uri updateUri = new Uri(string.Format("{0}/{1}/{2}/{3}/patient/{4}",
                                                        "http://localhost:8888/Patient",
                                                        context,
                                                        version,
                                                        contractNumber,
                                                        patientId));
            HttpClient updateClient = new HttpClient();
            updateClient.DefaultRequestHeaders.Host = updateUri.Host;

            // Add an Accept header for JSON format.
            updateClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PutUpdatePatientDataResponse updateResponsePatient = null;

            DataContractJsonSerializer updateJsonSer = new DataContractJsonSerializer(typeof(PutUpdatePatientDataRequest));

            // use the serializer to write the object to a MemoryStream 
            MemoryStream updateMs = new MemoryStream();
            updateJsonSer.WriteObject(updateMs, putUpdatePatient);
            updateMs.Position = 0;


            //use a Stream reader to construct the StringContent (Json) 
            StreamReader updateSr = new StreamReader(updateMs);

            StringContent updateContent = new StringContent(updateSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var updateResponse = updateClient.PutAsync(updateUri, updateContent);
            var updateResponseContent = updateResponse.Result.Content;

            string updateResponseString = updateResponseContent.ReadAsStringAsync().Result;


            using (var updateMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(updateResponseString)))
            {
                var updateSerializer = new DataContractJsonSerializer(typeof(PutUpdatePatientDataResponse));
                updateResponsePatient = (PutUpdatePatientDataResponse)updateSerializer.ReadObject(updateMsResponse);
            }

            return updateResponsePatient;
        }
        
        private PutContactDataResponse putContactServiceCall(PutContactDataRequest putContactRequest, string patientId)
        {
            Uri contactUri = new Uri(string.Format("{0}/{1}/{2}/{3}/patient/contact/{4}",
                                            "http://localhost:8888/Contact",
                                            context,
                                            version,
                                            contractNumber,
                                            patientId));
            HttpClient contactClient = new HttpClient();
            contactClient.DefaultRequestHeaders.Host = contactUri.Host;
            contactClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            DataContractJsonSerializer contactJsonSer = new DataContractJsonSerializer(typeof(PutContactDataRequest));
            MemoryStream contactMs = new MemoryStream();
            contactJsonSer.WriteObject(contactMs, putContactRequest);
            contactMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader contactSr = new StreamReader(contactMs);
            StringContent contactContent = new StringContent(contactSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var contactResponse = contactClient.PutAsync(contactUri, contactContent);
            var contactResponseContent = contactResponse.Result.Content;

            string contactResponseString = contactResponseContent.ReadAsStringAsync().Result;
            PutContactDataResponse responseContact = null;

            using (var contactMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(contactResponseString)))
            {
                var contactSerializer = new DataContractJsonSerializer(typeof(PutContactDataResponse));
                responseContact = (PutContactDataResponse)contactSerializer.ReadObject(contactMsResponse);
            }

            return responseContact;
        }

        private PutCareMemberDataResponse putCareMemberServiceCall(PutCareMemberDataRequest putCareMemberRequest, string patientId)
        {
            //Patient
            Uri careMemberUri = new Uri(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/CareMember/Insert",
                                                 "http://localhost:8888/CareMember",
                                                 context,
                                                 version,
                                                 contractNumber,
                                                 patientId));
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Host = careMemberUri.Host;

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(PutCareMemberDataRequest));

            // use the serializer to write the object to a MemoryStream 
            MemoryStream ms = new MemoryStream();
            jsonSer.WriteObject(ms, putCareMemberRequest);
            ms.Position = 0;


            //use a Stream reader to construct the StringContent (Json) 
            StreamReader sr = new StreamReader(ms);

            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var response = client.PutAsync(careMemberUri, theContent);
            var responseContent = response.Result.Content;

            string responseString = responseContent.ReadAsStringAsync().Result;
            PutCareMemberDataResponse responseCareMember = null;

            using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
            {
                var serializer = new DataContractJsonSerializer(typeof(PutCareMemberDataResponse));
                responseCareMember = (PutCareMemberDataResponse)serializer.ReadObject(msResponse);
            }

            return responseCareMember;
        }

        private GetContactByUserIdDataResponse getContactByUserIdServiceCall(string userId)
        {
            Uri getContactUri = new Uri(string.Format("{0}/{1}/{2}/{3}/Contact/User/{4}",
                                                    "http://localhost:8888/Contact",
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    userId));
            HttpClient getContactClient = new HttpClient();
            getContactClient.DefaultRequestHeaders.Host = getContactUri.Host;
            getContactClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            GetContactByUserIdDataRequest getContactRequest = new GetContactByUserIdDataRequest
            {
                UserId = userId,
                Context = context,
                Version = version,
                ContractNumber = contractNumber
            };

            DataContractJsonSerializer getContactJsonSer = new DataContractJsonSerializer(typeof(GetContactByUserIdDataRequest));
            MemoryStream getContactMs = new MemoryStream();
            getContactJsonSer.WriteObject(getContactMs, getContactRequest);
            getContactMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader getContactSr = new StreamReader(getContactMs);
            StringContent getContactContent = new StringContent(getContactSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var getContactResponse = getContactClient.GetStringAsync(getContactUri);
            var getContactResponseContent = getContactResponse.Result;

            string getContactResponseString = getContactResponseContent;
            GetContactByUserIdDataResponse responseContact = null;

            using (var getContactMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(getContactResponseString)))
            {
                var getContactSerializer = new DataContractJsonSerializer(typeof(GetContactByUserIdDataResponse));
                responseContact = (GetContactByUserIdDataResponse)getContactSerializer.ReadObject(getContactMsResponse);
            }
            return responseContact;
        }
    }
}
