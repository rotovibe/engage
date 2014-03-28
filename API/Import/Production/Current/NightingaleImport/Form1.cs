using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
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
using System.Windows.Forms;

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
        
        private List<IdNamePair> modesLookUp = new List<IdNamePair>();
        private List<CommTypeData> typesLookUp = new List<CommTypeData>();
        private List<StateData> statesLookUp = new List<StateData>();
        private List<TimeZoneData> zonesLookUp = new List<TimeZoneData>();
        private TimeZoneData zoneDefault = new TimeZoneData();
        private List<IdNamePair> careMemberLookUp = new List<IdNamePair>();

        private double version = double.Parse(ConfigurationManager.AppSettings.Get("version"));
        private string context = ConfigurationManager.AppSettings.Get("context");

        string _headerUserId = string.Empty;

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
                Guid sqlUserId = getUserId(txtContactID.Text);
                if (sqlUserId != Guid.Empty)
                {
                    GetContactByUserIdDataResponse contactUserResp = getContactByUserIdServiceCall(sqlUserId.ToString());

                    _headerUserId = contactUserResp.Contact.ContactId;
                    if (string.IsNullOrEmpty(_headerUserId))
                        throw new Exception("Invalid 'Admin User'");

                    Dictionary<string, string> dictionarySucceed = new Dictionary<string, string>();
                    Dictionary<string, string> dictionaryFail = new Dictionary<string, string>();

                    LoadLookUps();

                    //"Care Manager
                    string contactTypeId = careMemberLookUp.Where(x => x.Name == "Care Manager").FirstOrDefault().Id;

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
                            ContractNumber = txtContract.Text,
                            Version = version
                        };

                        lblStatus.Text = string.Format("Importing '{0} {1}'...", patientRequest.FirstName, patientRequest.LastName);
                        lblStatus.Refresh();

                        PutPatientDataResponse responsePatient = putPatientServiceCall(patientRequest);
                        if (responsePatient.Id == null)
                        {
                            throw new Exception("Patient import request failed.");
                        }

                        //PatientSystem
                        PutPatientSystemDataRequest patSysRequest = new PutPatientSystemDataRequest
                        {
                            PatientID = responsePatient.Id,
                            SystemID = lvi.SubItems[colSID].Text,
                            SystemName = lvi.SubItems[colSysN].Text,
                            DisplayLabel = "ID",
                            Version = responsePatient.Version,
                            Context = patientRequest.Context,
                            ContractNumber = patientRequest.ContractNumber
                        };
                        if (patSysRequest.SystemID != null && patSysRequest.SystemName != null)
                        {
                            PutPatientSystemDataResponse responsePatientPS = putPatientSystemServiceCall(patSysRequest);
                            if (responsePatientPS.PatientSystemId == null)
                            {
                                throw new Exception("Patient System import request failed.");
                            }
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
                                Version = patientRequest.Version
                            };

                            PutUpdatePatientDataResponse updateResponsePatient = putUpdatePatientServiceCall(updatePatientRequest, responsePatient.Id.ToString());
                            if (updatePatientRequest.Id == null)
                            {
                                throw new Exception("Patient was not successfully updated with Patient System ID");
                            }
                        }

                        //Contact

                        //timezone
                        TimeZoneData tZone = new TimeZoneData();
                        if (string.IsNullOrEmpty(lvi.SubItems[colTimeZ].Text) == false)
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
                        if (string.IsNullOrEmpty(lvi.SubItems[colPh1].Text) == false)
                        {
                            PhoneData phone1 = new PhoneData
                            {
                                Number = Convert.ToInt64(lvi.SubItems[colPh1].Text.Replace("-", string.Empty)),
                                OptOut = false
                            };

                            if (lvi.SubItems[colPh1Pref].Text == "True")
                            {
                                phone1.PhonePreferred = true;
                            }
                            else
                                phone1.PhonePreferred = false;

                            if (string.IsNullOrEmpty(lvi.SubItems[colPh1Type].Text) == false)
                            {
                                foreach (CommTypeData c in typesLookUp)
                                {
                                    if (lvi.SubItems[colPh1Type].Text == c.Name)
                                    {
                                        phone1.TypeId = c.Id;
                                    }
                                }
                            }
                            else
                                phone1.TypeId = typesLookUp[0].Id;

                            phones.Add(phone1);
                        }

                        if (string.IsNullOrEmpty(lvi.SubItems[colPh2].Text) == false)
                        {
                            PhoneData phone2 = new PhoneData
                            {
                                Number = Convert.ToInt64(lvi.SubItems[colPh2].Text.Replace("-", string.Empty)),
                                OptOut = false
                            };

                            if (lvi.SubItems[colPh2Pref].Text == "True")
                            {
                                phone2.PhonePreferred = true;
                            }
                            else
                                phone2.PhonePreferred = false;

                            if (string.IsNullOrEmpty(lvi.SubItems[colPh2Type].Text) == false)
                            {
                                foreach (CommTypeData c in typesLookUp)
                                {
                                    if (lvi.SubItems[colPh2Type].Text == c.Name)
                                    {
                                        phone2.TypeId = c.Id;
                                    }
                                }
                            }
                            else
                                phone2.TypeId = typesLookUp[0].Id;

                            phones.Add(phone2);
                        }

                        //emails
                        if (string.IsNullOrEmpty(lvi.SubItems[colEm1].Text) == false)
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

                            if (string.IsNullOrEmpty(lvi.SubItems[colEm1Type].Text) == false)
                            {
                                foreach (CommTypeData c in typesLookUp)
                                {
                                    if (lvi.SubItems[colEm1Type].Text == c.Name)
                                    {
                                        email1.TypeId = c.Id;
                                    }
                                }
                            }
                            else
                                email1.TypeId = typesLookUp[0].Id;

                            emails.Add(email1);
                        }

                        if (string.IsNullOrEmpty(lvi.SubItems[colEm2].Text) == false)
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

                            if (string.IsNullOrEmpty(lvi.SubItems[colEm2Type].Text) == false)
                            {
                                foreach (CommTypeData c in typesLookUp)
                                {
                                    if (lvi.SubItems[colEm2Type].Text == c.Name)
                                    {
                                        email2.TypeId = c.Id;
                                    }
                                }
                            }
                            else
                                email2.TypeId = typesLookUp[0].Id;

                            emails.Add(email2);
                        }

                        //addresses
                        if ((string.IsNullOrEmpty(lvi.SubItems[colAdd1L1].Text) == false)
                            || (string.IsNullOrEmpty(lvi.SubItems[colAdd1L2].Text) == false)
                            || (string.IsNullOrEmpty(lvi.SubItems[colAdd1L3].Text) == false)
                            || (string.IsNullOrEmpty(lvi.SubItems[colAdd1City].Text) == false)
                            || (string.IsNullOrEmpty(lvi.SubItems[colAdd1St].Text) == false)
                            || (string.IsNullOrEmpty(lvi.SubItems[colAdd1Zip].Text) == false))
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
                                if ((st.Name == lvi.SubItems[colAdd1St].Text)
                                    || (st.Code == lvi.SubItems[colAdd1St].Text))
                                {
                                    add1.StateId = st.Id;
                                }
                            }

                            if (string.IsNullOrEmpty(lvi.SubItems[colAdd1Type].Text) == false)
                            {
                                foreach (CommTypeData c in typesLookUp)
                                {
                                    if (lvi.SubItems[colAdd1Type].Text == c.Name)
                                    {
                                        add1.TypeId = c.Id;
                                    }
                                }
                            }
                            else
                                add1.TypeId = typesLookUp[0].Id;

                            addresses.Add(add1);
                        }

                        if ((string.IsNullOrEmpty(lvi.SubItems[colAdd2L1].Text) == false)
                            || (string.IsNullOrEmpty(lvi.SubItems[colAdd2L2].Text) == false)
                            || (string.IsNullOrEmpty(lvi.SubItems[colAdd2L3].Text) == false)
                            || (string.IsNullOrEmpty(lvi.SubItems[colAdd2City].Text) == false)
                            || (string.IsNullOrEmpty(lvi.SubItems[colAdd2St].Text) == false)
                            || (string.IsNullOrEmpty(lvi.SubItems[colAdd2Zip].Text) == false))
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
                                if ((st.Name == lvi.SubItems[colAdd2St].Text)
                                    || (st.Code == lvi.SubItems[colAdd2St].Text))
                                {
                                    add2.StateId = st.Id;
                                }
                            }

                            if (string.IsNullOrEmpty(lvi.SubItems[colAdd2Type].Text) == false)
                            {
                                foreach (CommTypeData c in typesLookUp)
                                {
                                    if (lvi.SubItems[colAdd2Type].Text == c.Name)
                                    {
                                        add2.TypeId = c.Id;
                                    }
                                }
                            }
                            else
                                add2.TypeId = typesLookUp[0].Id;

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
                        if (responseContact.ContactId == null)
                        {
                            throw new Exception("Contact card import request failed.");
                        }

                        if (string.IsNullOrEmpty(lvi.SubItems[colCMan].Text) == false)
                        {
                            Guid userIdResponse = getUserId(lvi.SubItems[colCMan].Text);
                            if (userIdResponse != Guid.Empty)
                            {
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
                                    CareMember = careMember
                                };
                                PutCareMemberDataResponse responseCareMember = putCareMemberServiceCall(careMemberRequest, responsePatient.Id.ToString());
                                if (responseCareMember.Id == null)
                                {
                                    throw new Exception("Care Member import request failed.");
                                }
                                UpdateCohortPatientView(responsePatient.Id.ToString(), contactByUserIdResponse.Contact.ContactId);
                            }
                        }

                        if (responsePatient.Id != null && responsePatient.Status == null)
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

                    string listSucceed = string.Empty;

                    if (dictionarySucceed.Count < 10)
                    {
                        foreach (var pairSucceed in dictionarySucceed)
                        {
                            listSucceed += pairSucceed + "\n";
                        }
                    }
                    string listFail = null;
                    foreach (var pairFail in dictionaryFail)
                    {
                        listFail += pairFail + "\n";
                    }

                    MessageBox.Show(dictionarySucceed.Count() + " patient file(s) successfully imported:\n" + listSucceed + "\n"
                                        + dictionaryFail.Count() + " patient file(s) failed to import: \n" + listFail);
                }
                else
                    MessageBox.Show("Invalid 'Admin User Name'!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadLookUps()
        {
            //modes
            string modesUrl = string.Format("{0}/LookUp/{1}/{2}/{3}/commmodes",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text);

            Uri modesUri = null;
            HttpClient modesClient = GetHttpClient(modesUrl, out modesUri);

            GetAllCommModesDataRequest modesRequest = new GetAllCommModesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = txtContract.Text
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
            string typesUrl = string.Format("{0}/LookUp/{1}/{2}/{3}/commtypes",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text);

            Uri typesUri = null;
            HttpClient typesClient = GetHttpClient(typesUrl, out typesUri);

            GetAllCommTypesDataRequest typesRequest = new GetAllCommTypesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = txtContract.Text
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
            string statesURL = (string.Format("{0}/LookUp/{1}/{2}/{3}/states",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text));

            Uri statesUri = null;
            HttpClient statesClient = GetHttpClient(statesURL, out statesUri);

            GetAllStatesDataRequest statesRequest = new GetAllStatesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = txtContract.Text
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
            string zonesURL = (string.Format("{0}/LookUp/{1}/{2}/{3}/timeZones",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text));

            Uri zonesUri = null;
            HttpClient zonesClient = GetHttpClient(zonesURL, out zonesUri);

            GetAllTimeZonesDataRequest zonesRequest = new GetAllTimeZonesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = txtContract.Text
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
            string zoneURL = (string.Format("{0}/LookUp/{1}/{2}/{3}/TimeZone/Default",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text));

            Uri zoneUri = null;
            HttpClient zoneClient = GetHttpClient(zoneURL, out zoneUri);

            GetTimeZoneDataRequest zoneRequest = new GetTimeZoneDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = txtContract.Text
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

            //Care Member
            ///{Context}/{Version}/{txtContract.Text}/Type/{Name}
            string cmURL = (string.Format("{0}/LookUp/{1}/{2}/{3}/Type/CareMemberType",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text));
            Uri careMemberUri = null;
            HttpClient careMemberClient = GetHttpClient(cmURL, out careMemberUri);

            GetLookUpsDataRequest careMemberRequest = new GetLookUpsDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = txtContract.Text
            };

            DataContractJsonSerializer careMemberJsonSer = new DataContractJsonSerializer(typeof(GetLookUpsDataRequest));
            MemoryStream careMemberMs = new MemoryStream();
            careMemberJsonSer.WriteObject(careMemberMs, careMemberRequest);
            careMemberMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader careMemberSr = new StreamReader(careMemberMs);
            StringContent careMemberContent = new StringContent(careMemberSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var careMemberResponse = careMemberClient.GetStringAsync(careMemberUri);
            var careMemberResponseContent = careMemberResponse.Result;

            string careMemberResponseString = careMemberResponseContent;
            GetLookUpsDataResponse responsecareMember = null;

            using (var careMemberMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(careMemberResponseString)))
            {
                var careMemberSerializer = new DataContractJsonSerializer(typeof(GetLookUpsDataResponse));
                responsecareMember = (GetLookUpsDataResponse)careMemberSerializer.ReadObject(careMemberMsResponse);
            }
            careMemberLookUp = responsecareMember.LookUpsData;

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
            try
            {
                if (openFileDialog1.CheckFileExists)
                {
                    filename = openFileDialog1.FileName;
                    textBox1.Text = filename;
                    string[] attributes;
                    string[] filelines = File.ReadAllLines(filename);
                    foreach (string line in filelines)
                    {
                        attributes = line.Split(",".ToCharArray());
                        ListViewItem lvi = new ListViewItem(attributes[colFirstN].Trim());
                        for (int i = 1; i < attributes.Count(); i++)
                        {
                            lvi.SubItems.Add(attributes[i].Trim());
                        }
                        //Check for required fields
                        if (lvi.SubItems[colFirstN].Text == "" || lvi.SubItems[colLastN].Text == ""
                            || lvi.SubItems[colGen].Text == "" || lvi.SubItems[colDB].Text == "")
                        {
                            throw new Exception("Required Patient data not found. Check patient: " + line);
                        }
                        else
                        {
                            listView1.Items.Add(lvi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                chkSelectAll.Text = string.Format("Select All ({0} Patients)", listView1.Items.Count);
                lblStatus.Text = "Select Individuals to Import...";
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

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (chkSelectAll.Checked)
                {
                    listView1.Items[i].Checked = true;
                }
                else
                    listView1.Items[i].Checked = false;
            }

        }

        private Guid getUserId(string userName)
        {
            try
            {
                Guid returnId = Guid.Empty;

                string sql = string.Format("Select userId from [User] where UserName = '{0}'",
                                            userName);
                DataSet ds = Phytel.Services.SQLDataService.Instance.ExecuteSQLDirect(txtSQLConn.Text, sql, 30);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    returnId = Guid.Parse(ds.Tables[0].Rows[0]["userId"].ToString());
                }
                return returnId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private PutPatientDataResponse putPatientServiceCall(PutPatientDataRequest putPatientRequest)
        {
            //Patient
            string theURL = (string.Format("{0}/Patient/{1}/{2}/{3}/patient",
                                                 txtURL.Text,
                                                 context,
                                                 version,
                                                 txtContract.Text));

            Uri theUri = null;
            HttpClient client = GetHttpClient(theURL, out theUri);

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
            string theURL = (string.Format("{0}/PatientSystem/{1}/{2}/{3}/patientsystem",
                                                   txtURL.Text,
                                                   context,
                                                   version,
                                                   txtContract.Text));
            Uri theUriPS = null;
            HttpClient clientPS = GetHttpClient(theURL, out theUriPS);

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
            string theURL = (string.Format("{0}/Patient/{1}/{2}/{3}/patient/{4}",
                                                        txtURL.Text,
                                                        context,
                                                        version,
                                                        txtContract.Text,
                                                        patientId));
            Uri updateUri = null;
            HttpClient updateClient = GetHttpClient(theURL, out updateUri);

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
            string theURL = (string.Format("{0}/Contact/{1}/{2}/{3}/patient/contact/{4}",
                                            txtURL.Text,
                                            context,
                                            version,
                                            txtContract.Text,
                                            patientId));
            Uri contactUri = null;
            HttpClient contactClient = GetHttpClient(theURL, out contactUri);

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
            string theURL = (string.Format("{0}/CareMember/{1}/{2}/{3}/Patient/{4}/CareMember/Insert",
                                                 txtURL.Text,
                                                 context,
                                                 version,
                                                 txtContract.Text,
                                                 patientId));
            Uri careMemberUri = null;
            HttpClient client = GetHttpClient(theURL, out careMemberUri);
            
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
            string theURL = (string.Format("{0}/Contact/{1}/{2}/{3}/Contact/User/{4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text,
                                                    userId));
            Uri getContactUri = null;
            HttpClient getContactClient = GetHttpClient(theURL, out getContactUri);

            GetContactByUserIdDataRequest getContactRequest = new GetContactByUserIdDataRequest
            {
                SQLUserId = userId,
                Context = context,
                Version = version,
                ContractNumber = txtContract.Text
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

        private void UpdateCohortPatientView(string patientId, string careMemberContactId)
        {
            GetCohortPatientViewResponse getResponse = getCohortPatientViewServiceCall(patientId);

            if (getResponse != null && getResponse.CohortPatientView != null)
            {
                CohortPatientViewData cpvd = getResponse.CohortPatientView;
                // check to see if primary care manager's contactId exists in the searchfield
                if (!cpvd.SearchFields.Exists(sf => sf.FieldName == Constants.PCM))
                {
                    cpvd.SearchFields.Add(new SearchFieldData
                    {
                        Value = careMemberContactId,
                        Active = true,
                        FieldName = Constants.PCM
                    });
                }
                else
                {
                    cpvd.SearchFields.ForEach(sf =>
                    {
                        if (sf.FieldName == Constants.PCM)
                        {
                            sf.Value = careMemberContactId;
                            sf.Active = true;
                        }
                    });
                }

                PutUpdateCohortPatientViewRequest request = new PutUpdateCohortPatientViewRequest
                    {
                        CohortPatientView = cpvd,
                        ContractNumber = txtContract.Text,
                        PatientID = patientId
                    };

                PutUpdateCohortPatientViewResponse response = putCohortPatientViewServiceCall(request, patientId);
                if (string.IsNullOrEmpty(response.CohortPatientViewId))
                    throw new Exception("Unable to update Cohort Patient View");
            }
        }

        private GetCohortPatientViewResponse getCohortPatientViewServiceCall(string patientId)
        {
            string theURL = (string.Format("{0}/Patient/{1}/{2}/{3}/patient/{4}/cohortpatientview/",
                                                        txtURL.Text,
                                                        context,
                                                        version,
                                                        txtContract.Text,
                                                        patientId));

            Uri getCohortUri = null;
            HttpClient getCohortClient = GetHttpClient(theURL, out getCohortUri);

            var getCohortResponse = getCohortClient.GetStringAsync(getCohortUri);
            var getCohortResponseContent = getCohortResponse.Result;

            string getCohortResponseString = getCohortResponseContent;
            GetCohortPatientViewResponse responseContact = null;

            using (var getCohortMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(getCohortResponseString)))
            {
                var getContactSerializer = new DataContractJsonSerializer(typeof(GetCohortPatientViewResponse));
                responseContact = (GetCohortPatientViewResponse)getContactSerializer.ReadObject(getCohortMsResponse);
            }

            return responseContact;
        }

        private PutUpdateCohortPatientViewResponse putCohortPatientViewServiceCall(PutUpdateCohortPatientViewRequest request, string patientId)
        {
            string theURL = (string.Format("{0}/Patient/{1}/{2}/{3}/patient/{4}/cohortpatientview/update",
                                                 txtURL.Text,
                                                 context,
                                                 version,
                                                 txtContract.Text,
                                                 patientId));
            Uri cohortPatientUri = null;
            HttpClient client = GetHttpClient(theURL, out cohortPatientUri);

            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(PutUpdateCohortPatientViewRequest));

            // use the serializer to write the object to a MemoryStream 
            MemoryStream ms = new MemoryStream();
            jsonSer.WriteObject(ms, request);
            ms.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader sr = new StreamReader(ms);

            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var response = client.PutAsync(cohortPatientUri, theContent);
            var responseContent = response.Result.Content;

            string responseString = responseContent.ReadAsStringAsync().Result;
            PutUpdateCohortPatientViewResponse responseCohortPatientView = null;

            using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
            {
                var serializer = new DataContractJsonSerializer(typeof(PutUpdateCohortPatientViewResponse));
                responseCohortPatientView = (PutUpdateCohortPatientViewResponse)serializer.ReadObject(msResponse);
            }

            return responseCohortPatientView;
        }

        private HttpClient GetHttpClient(string url, out Uri newURI)
        {
            HttpClient client = new HttpClient();

            string userId = (_headerUserId != string.Empty ? _headerUserId : "000000000000000000000000");

            string newURL = url;
            if (url.Contains("?"))
                newURL = string.Format("{0}&UserId={1}", url, userId);
            else
                newURL = string.Format("{0}?UserId={1}", url, userId);

            newURI = new Uri(newURL);

            return client;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtContract.Text = ConfigurationManager.AppSettings.Get("contractNumber");
            txtURL.Text = ConfigurationManager.AppSettings.Get("DataDomainURL");
            txtSQLConn.Text = Phytel.Services.SQLDataService.Instance.GetConnectionString(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), false);
        }

    }
}
