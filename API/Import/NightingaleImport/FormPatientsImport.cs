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
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Microsoft.VisualBasic.FileIO;
using NGDataImport;
using NightingaleImport.Configuration;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace NightingaleImport
{

    public partial class FormPatientsImport : Form
    {
        string filename;
        static int colFirstN = 0;
        static int colLastN = 1;
        static int colMiddleN = 2;
        static int colSuff = 3;
        static int colPrefN = 4;
        static int colGen = 5;
        static int colDB = 6;
        static int colSysID = 7;
        static int colBkgrnd = 8;
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
        static int colSysNm = 39;
        static int colSysPrim = 40;
        
        private List<IdNamePair> modesLookUp = new List<IdNamePair>();
        private List<CommTypeData> typesLookUp = new List<CommTypeData>();
        private List<StateData> statesLookUp = new List<StateData>();
        private List<TimeZoneData> zonesLookUp = new List<TimeZoneData>();
        private TimeZoneData zoneDefault = new TimeZoneData();
        private List<IdNamePair> careMemberLookUp = new List<IdNamePair>();
        private List<SystemData> systemsData = new List<SystemData>();

        private readonly double version = Configurations.version;
        private readonly string context = Configurations.context;
        private readonly string contractNumber = Configurations.contractNumber;

        public const string EngageSystemProperty = "Engage";
        public const string DataSourceProperty = "Import";
        public const string PCMRoleIdProperty = "56f169f8078e10eb86038514";
        string _headerUserId = "000000000000000000000000";

        public FormPatientsImport()
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
                Importer import = new Importer(txtURL.Text, context, version, txtContract.Text, _headerUserId);
                Guid sqlUserId = getUserId(txtContactID.Text);
                if (sqlUserId != Guid.Empty)
                {
                    GetContactByUserIdDataResponse contactUserResp = import.GetContactByUserId(sqlUserId.ToString());

                    _headerUserId = contactUserResp.Contact.Id;
                    if (string.IsNullOrEmpty(_headerUserId))
                        throw new Exception("Invalid 'Admin User'");
                    import.HeaderUserId = _headerUserId;
                    
                    Dictionary<string, string> dictionarySucceed = new Dictionary<string, string>();
                    Dictionary<string, string> dictionaryFail = new Dictionary<string, string>();

                    LoadLookUps();
                    LoadSystems();

                    foreach (ListViewItem lvi in listView1.CheckedItems)
                    {

                        PatientData pdata = new PatientData
                        {
                            #region Sync up properties in Contact
                            FirstName = lvi.SubItems[colFirstN].Text.Trim(),
                            LastName = lvi.SubItems[colLastN].Text.Trim(),
                            MiddleName = (String.IsNullOrEmpty(lvi.SubItems[colMiddleN].Text)) ? null : lvi.SubItems[colMiddleN].Text.Trim(),
                            PreferredName = (String.IsNullOrEmpty(lvi.SubItems[colPrefN].Text)) ? null : lvi.SubItems[colPrefN].Text.Trim(),
                            Gender = lvi.SubItems[colGen].Text.Trim(),
                            Suffix = (String.IsNullOrEmpty(lvi.SubItems[colSuff].Text)) ? null : lvi.SubItems[colSuff].Text.Trim(),
                            StatusId = (int)Phytel.API.DataDomain.Patient.DTO.Status.Active, 
                            #endregion
                            DOB = lvi.SubItems[colDB].Text.Trim(),
                            DataSource = EngageSystemProperty,
                            StatusDataSource = EngageSystemProperty,
                            Background = (String.IsNullOrEmpty(lvi.SubItems[colBkgrnd].Text)) ? null : lvi.SubItems[colBkgrnd].Text.Trim(),

                        };
                        PutPatientDataRequest patientRequest = new PutPatientDataRequest
                        {
                            Patient = pdata,
                            Context = context,
                            ContractNumber = txtContract.Text,
                            Version = version
                        };

                        lblStatus.Text = string.Format("Importing '{0} {1}'...", pdata.FirstName, pdata.LastName);
                        lblStatus.Refresh();

                        PutPatientDataResponse responsePatient = import.InsertPatient(patientRequest);
                        if (responsePatient.Id == null)
                        {
                            dictionaryFail.Add(pdata.FirstName + " " + pdata.LastName, string.Format("Message: {0} StackTrace: {1}", responsePatient.Status.Message, responsePatient.Status.StackTrace));
                            int n = listView1.CheckedItems.IndexOf(lvi);
                            listView1.CheckedItems[n].BackColor = Color.Red;
                            listView1.CheckedItems[n].Checked = false;
                        }
                        else
                        {
                            #region Insert the patient system record provided in the import file.
                            if (!String.IsNullOrEmpty(lvi.SubItems[colSysID].Text) && !String.IsNullOrEmpty(lvi.SubItems[colSysNm].Text))
                            {
                                var system = systemsData.Where(s => s.Name.ToLower() == lvi.SubItems[colSysNm].Text.Trim().ToLower()).FirstOrDefault();
                                if(system != null)
                                {
                                    PatientSystemData psData = new PatientSystemData 
                                    { 
                                        PatientId = responsePatient.Id, 
                                        Primary = (String.IsNullOrEmpty(lvi.SubItems[colSysPrim].Text)) ? false : Boolean.Parse(lvi.SubItems[colSysPrim].Text.Trim()),
                                        StatusId = (int)Phytel.API.DataDomain.PatientSystem.DTO.Status.Active,
                                        SystemId = system.Id,
                                        DataSource = DataSourceProperty,
                                        Value = (String.IsNullOrEmpty(lvi.SubItems[colSysID].Text)) ? null : lvi.SubItems[colSysID].Text.Trim(),
                                    };
                                    InsertPatientSystemDataRequest psRequest = new InsertPatientSystemDataRequest
                                    {
                                        PatientId = responsePatient.Id,
                                        IsEngageSystem = false,
                                        PatientSystemsData = psData,
                                        Version = responsePatient.Version,
                                        Context = patientRequest.Context,
                                        ContractNumber = patientRequest.ContractNumber
                                    };
                                    InsertPatientSystemDataResponse responsePatientPS = import.InsertPatientSystem(psRequest);
                                    if (responsePatientPS.PatientSystemData == null)
                                    {
                                        throw new Exception("Failed to import the PatientSystem Id provided in the file.");
                                    }
                                    else
                                    {
                                        // If imported patientsystem's primary is set to true, override the EngagePatientSystem's primary field.
                                        if (psData.Primary)
                                        {
                                            GetPatientSystemDataResponse engagePatientSystemResponse = import.GetPatientSystem(new GetPatientSystemDataRequest 
                                            { 
                                                Context = context,
                                                ContractNumber = txtContract.Text,
                                                Version = version,
                                                Id = responsePatient.EngagePatientSystemId
                                            });
                                            if (engagePatientSystemResponse != null && engagePatientSystemResponse.PatientSystemData != null)
                                            {
                                                engagePatientSystemResponse.PatientSystemData.Primary = false;
                                                UpdatePatientSystemDataRequest updatePDRequest = new UpdatePatientSystemDataRequest 
                                                {
                                                    Context = context,
                                                    ContractNumber = txtContract.Text,
                                                    Version = version,
                                                    Id = engagePatientSystemResponse.PatientSystemData.Id,
                                                    PatientId = engagePatientSystemResponse.PatientSystemData.PatientId,
                                                    PatientSystemsData = engagePatientSystemResponse.PatientSystemData
                                                };
                                                import.UpdatePatientSystem(updatePDRequest);
                                            }
                                        }
                                    }

                                }
                            }
                            #endregion

                            #region Insert Contact record.

                            #region Communication
                            //timezone
                            TimeZoneData tZone = null;
                            if (string.IsNullOrEmpty(lvi.SubItems[colTimeZ].Text) == false)
                            {
                                tZone = new TimeZoneData();
                                foreach (TimeZoneData t in zonesLookUp)
                                {
                                    string[] zones = t.Name.Split(" ".ToCharArray());
                                    if (lvi.SubItems[colTimeZ].Text.Trim() == zones[0])
                                    {
                                        tZone.Id = t.Id;
                                    }
                                }
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
                                    OptOut = false,
                                    DataSource = DataSourceProperty
                                };

                                if (String.Compare(lvi.SubItems[colPh1Pref].Text.Trim(), "true", true) == 0)
                                {
                                    phone1.PhonePreferred = true;
                                }
                                else
                                    phone1.PhonePreferred = false;

                                if (string.IsNullOrEmpty(lvi.SubItems[colPh1Type].Text) == false)
                                {
                                    foreach (CommTypeData c in typesLookUp)
                                    {
                                        if (String.Compare(lvi.SubItems[colPh1Type].Text.Trim(), c.Name, true) == 0)
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
                                    OptOut = false,
                                    DataSource = DataSourceProperty
                                };

                                if (String.Compare(lvi.SubItems[colPh2Pref].Text.Trim(), "true", true) == 0)
                                {
                                    phone2.PhonePreferred = true;
                                }
                                else
                                    phone2.PhonePreferred = false;

                                if (string.IsNullOrEmpty(lvi.SubItems[colPh2Type].Text) == false)
                                {
                                    foreach (CommTypeData c in typesLookUp)
                                    {
                                        if (String.Compare(lvi.SubItems[colPh2Type].Text.Trim(), c.Name, true) == 0)
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
                                    Text = lvi.SubItems[colEm1].Text.Trim(),
                                    OptOut = false,
                                };

                                if (String.Compare(lvi.SubItems[colEm1Pref].Text.Trim(), "true", true) == 0)
                                {
                                    email1.Preferred = true;
                                }
                                else
                                    email1.Preferred = false;

                                if (string.IsNullOrEmpty(lvi.SubItems[colEm1Type].Text) == false)
                                {
                                    foreach (CommTypeData c in typesLookUp)
                                    {
                                        if (String.Compare(lvi.SubItems[colEm1Type].Text.Trim(), c.Name, true) == 0)
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
                                    Text = lvi.SubItems[colEm2].Text.Trim(),
                                    OptOut = false,
                                };

                                if (String.Compare(lvi.SubItems[colEm2Pref].Text.Trim(), "true", true) == 0)
                                {
                                    email2.Preferred = true;
                                }
                                else
                                    email2.Preferred = false;

                                if (string.IsNullOrEmpty(lvi.SubItems[colEm2Type].Text) == false)
                                {
                                    foreach (CommTypeData c in typesLookUp)
                                    {
                                        if (String.Compare(lvi.SubItems[colEm2Type].Text.Trim(), c.Name, true) == 0)
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
                                    Line1 = (string.IsNullOrEmpty(lvi.SubItems[colAdd1L1].Text)) ? null : lvi.SubItems[colAdd1L1].Text.Trim(),
                                    Line2 = (string.IsNullOrEmpty(lvi.SubItems[colAdd1L2].Text)) ? null : lvi.SubItems[colAdd1L2].Text.Trim(),
                                    Line3 = (string.IsNullOrEmpty(lvi.SubItems[colAdd1L3].Text)) ? null : lvi.SubItems[colAdd1L3].Text.Trim(),
                                    City = (string.IsNullOrEmpty(lvi.SubItems[colAdd1City].Text)) ? null : lvi.SubItems[colAdd1City].Text.Trim(),
                                    PostalCode = (string.IsNullOrEmpty(lvi.SubItems[colAdd1Zip].Text)) ? null : lvi.SubItems[colAdd1Zip].Text.Trim(),
                                    OptOut = false
                                };

                                if (String.Compare(lvi.SubItems[colAdd1Pref].Text.Trim(), "true", true) == 0)
                                {
                                    add1.Preferred = true;
                                }
                                else
                                    add1.Preferred = false;

                                string stateTrim = (string.IsNullOrEmpty(lvi.SubItems[colAdd1St].Text)) ? null : lvi.SubItems[colAdd1St].Text.Trim();
                                foreach (StateData st in statesLookUp)
                                {
                                    if ((st.Name == stateTrim)
                                        || (st.Code == stateTrim))
                                    {
                                        add1.StateId = st.Id;
                                    }
                                }

                                if (string.IsNullOrEmpty(lvi.SubItems[colAdd1Type].Text) == false)
                                {
                                    foreach (CommTypeData c in typesLookUp)
                                    {
                                        if (String.Compare(lvi.SubItems[colAdd1Type].Text.Trim(), c.Name, true) == 0)
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
                                    Line1 = (string.IsNullOrEmpty(lvi.SubItems[colAdd2L1].Text)) ? null : lvi.SubItems[colAdd2L1].Text.Trim(),
                                    Line2 = (string.IsNullOrEmpty(lvi.SubItems[colAdd2L2].Text)) ? null : lvi.SubItems[colAdd2L2].Text.Trim(),
                                    Line3 = (string.IsNullOrEmpty(lvi.SubItems[colAdd2L3].Text)) ? null : lvi.SubItems[colAdd2L3].Text.Trim(),
                                    City = (string.IsNullOrEmpty(lvi.SubItems[colAdd2City].Text)) ? null : lvi.SubItems[colAdd2City].Text.Trim(),
                                    PostalCode = (string.IsNullOrEmpty(lvi.SubItems[colAdd2Zip].Text)) ? null : lvi.SubItems[colAdd2Zip].Text.Trim(),
                                    OptOut = false
                                };

                                if (String.Compare(lvi.SubItems[colAdd2Pref].Text.Trim(), "true", true) == 0)
                                {
                                    add2.Preferred = true;
                                }
                                else
                                    add2.Preferred = false;

                                string stateTrim = (string.IsNullOrEmpty(lvi.SubItems[colAdd2St].Text)) ? null : lvi.SubItems[colAdd2St].Text.Trim();
                                foreach (StateData st in statesLookUp)
                                {
                                    if ((st.Name == stateTrim)
                                        || (st.Code == stateTrim))
                                    {
                                        add2.StateId = st.Id;
                                    }
                                }

                                if (string.IsNullOrEmpty(lvi.SubItems[colAdd2Type].Text) == false)
                                {
                                    foreach (CommTypeData c in typesLookUp)
                                    {
                                        if (String.Compare(lvi.SubItems[colAdd2Type].Text.Trim(), c.Name, true) == 0)
                                        {
                                            add2.TypeId = c.Id;
                                        }
                                    }
                                }
                                else
                                    add2.TypeId = typesLookUp[0].Id;

                                addresses.Add(add2);
                            } 
                            #endregion

                            //Contact
                            ContactData data = new ContactData {
                                PatientId = responsePatient.Id,
                                ContactTypeId = Phytel.API.DataDomain.Contact.DTO.Constants.PersonContactTypeId,
                                #region Sync up properties in Contact
                                FirstName = pdata.FirstName,
                                LastName = pdata.LastName,
                                MiddleName = pdata.MiddleName,
                                PreferredName = pdata.PreferredName,
                                Gender = pdata.Gender,
                                Suffix = pdata.Suffix,
                                StatusId = pdata.StatusId,
                                #endregion
                                DataSource = EngageSystemProperty,
                                Modes = modes,
                                TimeZoneId = tZone == null ? null : tZone.Id,
                                Phones = phones,
                                Emails = emails,
                                Addresses = addresses
                            };
                            InsertContactDataRequest contactRequest = new InsertContactDataRequest
                            {
                                ContactData = data,
                                Version = patientRequest.Version,
                                Context = patientRequest.Context,
                                ContractNumber = patientRequest.ContractNumber
                            };

                            InsertContactDataResponse responseContact = import.InsertContactForAPatient(contactRequest, responsePatient.Id.ToString());
                            if (responseContact.Id == null)
                            {
                                throw new Exception("Contact card import request failed.");
                            }

                            #endregion

                            #region CareMember
                            if (string.IsNullOrEmpty(lvi.SubItems[colCMan].Text) == false)
                            {
                                Guid userIdResponse = getUserId(lvi.SubItems[colCMan].Text);
                                if (userIdResponse != Guid.Empty)
                                {
                                    GetContactByUserIdDataResponse contactByUserIdResponse = import.GetContactByUserId(userIdResponse.ToString());

                                    CareTeamMemberData member = new CareTeamMemberData
                                    {
                                        ContactId = contactByUserIdResponse.Contact.Id,
                                        RoleId = PCMRoleIdProperty,
                                        Core = true,
                                        DataSource = "Engage",
                                        DistanceUnit = "mi",
                                        StatusId = (int)CareTeamMemberStatus.Active,
                                    };
                                    List<CareTeamMemberData> memberList = new List<CareTeamMemberData>();
                                    memberList.Add(member);

                                    CareTeamData careTeamData = new CareTeamData
                                    {
                                          ContactId = responseContact.Id,
                                          Members = memberList
                                    };
                                    SaveCareTeamDataRequest saveCareTeamDataRequest = new SaveCareTeamDataRequest
                                    {
                                        CareTeamData = careTeamData,
                                        ContactId = responseContact.Id,
                                    };
                                    SaveCareTeamDataResponse saveCareTeamDataResponse = import.InsertCareTeam(saveCareTeamDataRequest);
                                    if (saveCareTeamDataResponse == null)
                                    {
                                        throw new Exception("Care Team import request failed.");
                                    }
                                    import.UpdateCohortPatientView(responsePatient.Id.ToString(), contactByUserIdResponse.Contact.Id);
                                }
                            }

                            #endregion

                            if (responsePatient.Id != null && responsePatient.Status == null)
                            {
                                dictionarySucceed.Add(pdata.FirstName + " " + pdata.LastName, responsePatient.Id);
                                int n = listView1.CheckedItems.IndexOf(lvi);
                                listView1.CheckedItems[n].Remove();
                            }
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
            Uri modesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/commmodes?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text,
                                                    _headerUserId));
            HttpClient modesClient = GetHttpClient(modesUri);

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
            Uri typesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/commtypes?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text,
                                                    _headerUserId));
            HttpClient typesClient = GetHttpClient(typesUri);

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
            Uri statesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/states?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text,
                                                    _headerUserId));
            HttpClient statesClient = GetHttpClient(statesUri);

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
            Uri zonesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/timeZones?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text,
                                                    _headerUserId));
            HttpClient zonesClient = GetHttpClient(zonesUri);

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
            Uri zoneUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/TimeZone/Default?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text,
                                                    _headerUserId));
            HttpClient zoneClient = GetHttpClient(zoneUri);

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
            Uri careMemberUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/Type/CareMemberType?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text,
                                                    _headerUserId));
            HttpClient careMemberClient = GetHttpClient(careMemberUri);

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

        private void LoadSystems()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/System", "GET")]
            Uri patientsystemDDUri = new Uri(string.Format("{0}/PatientSystem/{1}/{2}/{3}/System?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    txtContract.Text,
                                                    _headerUserId));
            HttpClient client = GetHttpClient(patientsystemDDUri);

            GetSystemsDataRequest systemDataRequest = new GetSystemsDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = txtContract.Text
            };

            DataContractJsonSerializer modesJsonSer = new DataContractJsonSerializer(typeof(GetSystemsDataRequest));
            MemoryStream ms = new MemoryStream();
            modesJsonSer.WriteObject(ms, systemDataRequest);
            ms.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader modesSr = new StreamReader(ms);
            StringContent modesContent = new StringContent(modesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var response = client.GetStringAsync(patientsystemDDUri);
            var responseContent = response.Result;

            string modesResponseString = responseContent;
            GetSystemsDataResponse getSystemsDataResponse = null;

            using (var memStream = new MemoryStream(Encoding.Unicode.GetBytes(modesResponseString)))
            {
                var modesSerializer = new DataContractJsonSerializer(typeof(GetSystemsDataResponse));
                getSystemsDataResponse = (GetSystemsDataResponse)modesSerializer.ReadObject(memStream);
            }
            systemsData = getSystemsDataResponse.SystemsData;
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
                    using (TextFieldParser parser = new TextFieldParser(filename))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        while (!parser.EndOfData)
                        {
                            string[] line = parser.ReadFields();
                            ListViewItem lvi = new ListViewItem(line[colFirstN].Trim());
                            for (int i = 1; i < line.Count(); i++)
                            {
                                lvi.SubItems.Add(line[i].Trim());
                            }
                            //Check for required fields
                            if (lvi.SubItems[colFirstN].Text == "" || lvi.SubItems[colLastN].Text == ""
                                || lvi.SubItems[colGen].Text == "" || lvi.SubItems[colDB].Text == "")
                            {
                                throw new Exception("Required Patient data not found. Check patient: " + String.Join(",", line));
                            }
                            else
                            {
                                listView1.Items.Add(lvi);
                            }
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

        #region putPatientServiceCall
        //private PutPatientDataResponse putPatientServiceCall(PutPatientDataRequest putPatientRequest)
        //{
        //    try
        //    {
        //        //Patient
        //        Uri theUri = new Uri(string.Format("{0}/Patient/{1}/{2}/{3}/Patient/Insert?UserId={4}",
        //                                             txtURL.Text,
        //                                             context,
        //                                             version,
        //                                             txtContract.Text,
        //                                             _headerUserId));

        //        HttpClient client = GetHttpClient(theUri);

        //        DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(PutPatientDataRequest));

        //        // use the serializer to write the object to a MemoryStream 
        //        MemoryStream ms = new MemoryStream();
        //        jsonSer.WriteObject(ms, putPatientRequest);
        //        ms.Position = 0;


        //        //use a Stream reader to construct the StringContent (Json) 
        //        StreamReader sr = new StreamReader(ms);

        //        StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

        //        //Post the data 
        //        var response = client.PutAsync(theUri, theContent);
        //        var responseContent = response.Result.Content;

        //        string responseString = responseContent.ReadAsStringAsync().Result;
        //        PutPatientDataResponse responsePatient = null;

        //        using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
        //        {
        //            var serializer = new DataContractJsonSerializer(typeof(PutPatientDataResponse));
        //            responsePatient = (PutPatientDataResponse)serializer.ReadObject(msResponse);
        //        }

        //        return responsePatient;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #region insertPatientSystem
        //private InsertPatientSystemDataResponse insertPatientSystem(InsertPatientSystemDataRequest request)
        //{
        //    //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystem", "POST")]
        //    Uri theUriPS = new Uri(string.Format("{0}/PatientSystem/{1}/{2}/{3}/Patient/{4}/PatientSystem?UserId={5}",
        //                                           txtURL.Text,
        //                                           context,
        //                                           version,
        //                                           txtContract.Text,
        //                                           request.PatientId,
        //                                           _headerUserId));
        //    HttpClient clientPS = GetHttpClient(theUriPS);

        //    DataContractJsonSerializer jsonSerPS = new DataContractJsonSerializer(typeof(InsertPatientSystemDataRequest));

        //    // use the serializer to write the object to a MemoryStream 
        //    MemoryStream msPS = new MemoryStream();
        //    jsonSerPS.WriteObject(msPS, request);
        //    msPS.Position = 0;


        //    //use a Stream reader to construct the StringContent (Json) 
        //    StreamReader srPS = new StreamReader(msPS);

        //    StringContent theContentPS = new StringContent(srPS.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

        //    //Post the data 
        //    var responsePS = clientPS.PostAsync(theUriPS, theContentPS);
        //    var responseContentPS = responsePS.Result.Content;

        //    string responseStringPS = responseContentPS.ReadAsStringAsync().Result;
        //    InsertPatientSystemDataResponse responsePatientPS = null;

        //    using (var msResponsePS = new MemoryStream(Encoding.Unicode.GetBytes(responseStringPS)))
        //    {
        //        var serializerPS = new DataContractJsonSerializer(typeof(InsertPatientSystemDataResponse));
        //        responsePatientPS = (InsertPatientSystemDataResponse)serializerPS.ReadObject(msResponsePS);
        //    }

        //    return responsePatientPS;
        //}
        #endregion

        #region getPatientSystem
        //private GetPatientSystemDataResponse getPatientSystem(GetPatientSystemDataRequest request)
        //{
        //    //[Route("/{Context}/{Version}/{ContractNumber}/PatientSystem/{Id}", "GET")]
        //    Uri theUriPS = new Uri(string.Format("{0}/PatientSystem/{1}/{2}/{3}/PatientSystem/{4}?UserId={5}",
        //                                           txtURL.Text,
        //                                           context,
        //                                           version,
        //                                           txtContract.Text,
        //                                           request.Id,
        //                                           _headerUserId));
        //    HttpClient client = GetHttpClient(theUriPS);

        //    DataContractJsonSerializer modesJsonSer = new DataContractJsonSerializer(typeof(GetPatientSystemDataRequest));
        //    MemoryStream ms = new MemoryStream();
        //    modesJsonSer.WriteObject(ms, request);
        //    ms.Position = 0;

        //    //use a Stream reader to construct the StringContent (Json) 
        //    StreamReader modesSr = new StreamReader(ms);
        //    StringContent modesContent = new StringContent(modesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

        //    //Post the data 
        //    var response = client.GetStringAsync(theUriPS);
        //    var responseContent = response.Result;

        //    string modesResponseString = responseContent;
        //    GetPatientSystemDataResponse getPatientSystemDataResponse = null;

        //    using (var memStream = new MemoryStream(Encoding.Unicode.GetBytes(modesResponseString)))
        //    {
        //        var modesSerializer = new DataContractJsonSerializer(typeof(GetPatientSystemDataResponse));
        //        getPatientSystemDataResponse = (GetPatientSystemDataResponse)modesSerializer.ReadObject(memStream);
        //    }
        //    return getPatientSystemDataResponse;
        //}
        #endregion

        #region updatedPatientSystem
        //private UpdatePatientSystemDataResponse updatePatientSystem(UpdatePatientSystemDataRequest request)
        //{
        //    //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystem/{Id}", "PUT")]
        //    Uri updateUri = new Uri(string.Format("{0}/PatientSystem/{1}/{2}/{3}/Patient/{4}/PatientSystem/{5}?UserId={6}",
        //                                                txtURL.Text,
        //                                                context,
        //                                                version,
        //                                                txtContract.Text,
        //                                                request.PatientId,
        //                                                request.Id,
        //                                                _headerUserId));
        //    HttpClient updateClient = GetHttpClient(updateUri);

        //    UpdatePatientSystemDataResponse response = null;

        //    DataContractJsonSerializer updateJsonSer = new DataContractJsonSerializer(typeof(UpdatePatientSystemDataRequest));

        //    // use the serializer to write the object to a MemoryStream 
        //    MemoryStream updateMs = new MemoryStream();
        //    updateJsonSer.WriteObject(updateMs, request);
        //    updateMs.Position = 0;


        //    //use a Stream reader to construct the StringContent (Json) 
        //    StreamReader updateSr = new StreamReader(updateMs);

        //    StringContent updateContent = new StringContent(updateSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

        //    //Post the data 
        //    var updateResponse = updateClient.PutAsync(updateUri, updateContent);
        //    var updateResponseContent = updateResponse.Result.Content;

        //    string updateResponseString = updateResponseContent.ReadAsStringAsync().Result;


        //    using (var updateMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(updateResponseString)))
        //    {
        //        var updateSerializer = new DataContractJsonSerializer(typeof(UpdatePatientSystemDataResponse));
        //        response = (UpdatePatientSystemDataResponse)updateSerializer.ReadObject(updateMsResponse);
        //    }

        //    return response;
        //}
        #endregion

        #region putUpdatePatientServiceCall
        //private PutUpdatePatientDataResponse putUpdatePatientServiceCall(PutUpdatePatientDataRequest putUpdatePatient, string patientId)
        //{
        //    Uri updateUri = new Uri(string.Format("{0}/Patient/{1}/{2}/{3}/patient?UserId={4}",
        //                                                txtURL.Text,
        //                                                context,
        //                                                version,
        //                                                txtContract.Text,
        //                                                _headerUserId));
        //    HttpClient updateClient = GetHttpClient(updateUri);

        //    PutUpdatePatientDataResponse updateResponsePatient = null;

        //    DataContractJsonSerializer updateJsonSer = new DataContractJsonSerializer(typeof(PutUpdatePatientDataRequest));

        //    // use the serializer to write the object to a MemoryStream 
        //    MemoryStream updateMs = new MemoryStream();
        //    updateJsonSer.WriteObject(updateMs, putUpdatePatient);
        //    updateMs.Position = 0;


        //    //use a Stream reader to construct the StringContent (Json) 
        //    StreamReader updateSr = new StreamReader(updateMs);

        //    StringContent updateContent = new StringContent(updateSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

        //    //Post the data 
        //    var updateResponse = updateClient.PutAsync(updateUri, updateContent);
        //    var updateResponseContent = updateResponse.Result.Content;

        //    string updateResponseString = updateResponseContent.ReadAsStringAsync().Result;


        //    using (var updateMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(updateResponseString)))
        //    {
        //        var updateSerializer = new DataContractJsonSerializer(typeof(PutUpdatePatientDataResponse));
        //        updateResponsePatient = (PutUpdatePatientDataResponse)updateSerializer.ReadObject(updateMsResponse);
        //    }

        //    return updateResponsePatient;
        //}
        #endregion

        #region putContactServiceCall
        //private PutContactDataResponse putContactServiceCall(PutContactDataRequest putContactRequest, string patientId)
        //{
        //    Uri contactUri = new Uri(string.Format("{0}/Contact/{1}/{2}/{3}/patient/contact/{4}?UserId={5}",
        //                                    txtURL.Text,
        //                                    context,
        //                                    version,
        //                                    txtContract.Text,
        //                                    patientId,
        //                                    _headerUserId));
        //    HttpClient contactClient = GetHttpClient(contactUri);

        //    DataContractJsonSerializer contactJsonSer = new DataContractJsonSerializer(typeof(PutContactDataRequest));
        //    MemoryStream contactMs = new MemoryStream();
        //    contactJsonSer.WriteObject(contactMs, putContactRequest);
        //    contactMs.Position = 0;

        //    //use a Stream reader to construct the StringContent (Json) 
        //    StreamReader contactSr = new StreamReader(contactMs);
        //    StringContent contactContent = new StringContent(contactSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

        //    //Post the data 
        //    var contactResponse = contactClient.PutAsync(contactUri, contactContent);
        //    var contactResponseContent = contactResponse.Result.Content;

        //    string contactResponseString = contactResponseContent.ReadAsStringAsync().Result;
        //    PutContactDataResponse responseContact = null;

        //    using (var contactMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(contactResponseString)))
        //    {
        //        var contactSerializer = new DataContractJsonSerializer(typeof(PutContactDataResponse));
        //        responseContact = (PutContactDataResponse)contactSerializer.ReadObject(contactMsResponse);
        //    }

        //    return responseContact;
        //}
        #endregion

        #region putCareMemberServiceCall
        //private PutCareMemberDataResponse putCareMemberServiceCall(PutCareMemberDataRequest putCareMemberRequest, string patientId)
        //{
        //    //Patient
        //    Uri careMemberUri = new Uri(string.Format("{0}/CareMember/{1}/{2}/{3}/Patient/{4}/CareMember/Insert?UserId={5}",
        //                                         txtURL.Text,
        //                                         context,
        //                                         version,
        //                                         txtContract.Text,
        //                                         patientId,
        //                                         _headerUserId));
        //    HttpClient client = GetHttpClient(careMemberUri);
            
        //    DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(PutCareMemberDataRequest));

        //    // use the serializer to write the object to a MemoryStream 
        //    MemoryStream ms = new MemoryStream();
        //    jsonSer.WriteObject(ms, putCareMemberRequest);
        //    ms.Position = 0;


        //    //use a Stream reader to construct the StringContent (Json) 
        //    StreamReader sr = new StreamReader(ms);

        //    StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

        //    //Post the data 
        //    var response = client.PutAsync(careMemberUri, theContent);
        //    var responseContent = response.Result.Content;

        //    string responseString = responseContent.ReadAsStringAsync().Result;
        //    PutCareMemberDataResponse responseCareMember = null;

        //    using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
        //    {
        //        var serializer = new DataContractJsonSerializer(typeof(PutCareMemberDataResponse));
        //        responseCareMember = (PutCareMemberDataResponse)serializer.ReadObject(msResponse);
        //    }

        //    return responseCareMember;
        //}
        #endregion

        #region getContractByUserId
        //private GetContactByUserIdDataResponse getContactByUserIdServiceCall(string userId)
        //{
        //    Uri getContactUri = new Uri(string.Format("{0}/Contact/{1}/{2}/{3}/Contact/User/{4}?UserId={5}",
        //                                            txtURL.Text,
        //                                            context,
        //                                            version,
        //                                            txtContract.Text,
        //                                            userId,
        //                                            _headerUserId));
        //    HttpClient getContactClient = GetHttpClient(getContactUri);

        //    GetContactByUserIdDataRequest getContactRequest = new GetContactByUserIdDataRequest
        //    {
        //        SQLUserId = userId,
        //        Context = context,
        //        Version = version,
        //        ContractNumber = txtContract.Text
        //    };

        //    DataContractJsonSerializer getContactJsonSer = new DataContractJsonSerializer(typeof(GetContactByUserIdDataRequest));
        //    MemoryStream getContactMs = new MemoryStream();
        //    getContactJsonSer.WriteObject(getContactMs, getContactRequest);
        //    getContactMs.Position = 0;

        //    //use a Stream reader to construct the StringContent (Json) 
        //    StreamReader getContactSr = new StreamReader(getContactMs);
        //    StringContent getContactContent = new StringContent(getContactSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

        //    //Post the data 
        //    var getContactResponse = getContactClient.GetStringAsync(getContactUri);
        //    var getContactResponseContent = getContactResponse.Result;

        //    string getContactResponseString = getContactResponseContent;
        //    GetContactByUserIdDataResponse responseContact = null;

        //    using (var getContactMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(getContactResponseString)))
        //    {
        //        var getContactSerializer = new DataContractJsonSerializer(typeof(GetContactByUserIdDataResponse));
        //        responseContact = (GetContactByUserIdDataResponse)getContactSerializer.ReadObject(getContactMsResponse);
        //    }
        //    return responseContact;
        //}
        #endregion

        #region UpdatedCohortPatientView
        //private void UpdateCohortPatientView(string patientId, string careMemberContactId)
        //{
        //    GetCohortPatientViewResponse getResponse = getCohortPatientViewServiceCall(patientId);

        //    if (getResponse != null && getResponse.CohortPatientView != null)
        //    {
        //        CohortPatientViewData cpvd = getResponse.CohortPatientView;
        //        // check to see if primary care manager's contactId exists in the searchfield
        //        if (!cpvd.SearchFields.Exists(sf => sf.FieldName == "PCM"))
        //        {
        //            cpvd.SearchFields.Add(new SearchFieldData
        //            {
        //                Value = careMemberContactId,
        //                Active = true,
        //                FieldName = "PCM"
        //            });
        //        }
        //        else
        //        {
        //            cpvd.SearchFields.ForEach(sf =>
        //            {
        //                if (sf.FieldName == "PCM")
        //                {
        //                    sf.Value = careMemberContactId;
        //                    sf.Active = true;
        //                }
        //            });
        //        }

        //        PutUpdateCohortPatientViewRequest request = new PutUpdateCohortPatientViewRequest
        //            {
        //                CohortPatientView = cpvd,
        //                ContractNumber = txtContract.Text,
        //                PatientID = patientId
        //            };

        //        PutUpdateCohortPatientViewResponse response = putCohortPatientViewServiceCall(request, patientId);
        //        if (string.IsNullOrEmpty(response.CohortPatientViewId))
        //            throw new Exception("Unable to update Cohort Patient View");
        //    }
        //}
        #endregion

        #region getCohortPatientView
        //private GetCohortPatientViewResponse getCohortPatientViewServiceCall(string patientId)
        //{
        //    Uri getCohortUri = new Uri(string.Format("{0}/Patient/{1}/{2}/{3}/patient/{4}/cohortpatientview?UserId={5}",
        //                                                txtURL.Text,
        //                                                context,
        //                                                version,
        //                                                txtContract.Text,
        //                                                patientId,
        //                                                _headerUserId));

        //    HttpClient getCohortClient = GetHttpClient(getCohortUri);

        //    var getCohortResponse = getCohortClient.GetStringAsync(getCohortUri);
        //    var getCohortResponseContent = getCohortResponse.Result;

        //    string getCohortResponseString = getCohortResponseContent;
        //    GetCohortPatientViewResponse responseContact = null;

        //    using (var getCohortMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(getCohortResponseString)))
        //    {
        //        var getContactSerializer = new DataContractJsonSerializer(typeof(GetCohortPatientViewResponse));
        //        responseContact = (GetCohortPatientViewResponse)getContactSerializer.ReadObject(getCohortMsResponse);
        //    }

        //    return responseContact;
        //}
        #endregion

        #region putCohortPatientView
        //private PutUpdateCohortPatientViewResponse putCohortPatientViewServiceCall(PutUpdateCohortPatientViewRequest request, string patientId)
        //{
        //    Uri cohortPatientUri = new Uri(string.Format("{0}/Patient/{1}/{2}/{3}/patient/{4}/cohortpatientview/update?UserId={5}",
        //                                         txtURL.Text,
        //                                         context,
        //                                         version,
        //                                         txtContract.Text,
        //                                         patientId,
        //                                         _headerUserId));
        //    HttpClient client = GetHttpClient(cohortPatientUri);

        //    DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(PutUpdateCohortPatientViewRequest));

        //    // use the serializer to write the object to a MemoryStream 
        //    MemoryStream ms = new MemoryStream();
        //    jsonSer.WriteObject(ms, request);
        //    ms.Position = 0;

        //    //use a Stream reader to construct the StringContent (Json) 
        //    StreamReader sr = new StreamReader(ms);

        //    StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

        //    //Post the data 
        //    var response = client.PutAsync(cohortPatientUri, theContent);
        //    var responseContent = response.Result.Content;

        //    string responseString = responseContent.ReadAsStringAsync().Result;
        //    PutUpdateCohortPatientViewResponse responseCohortPatientView = null;

        //    using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
        //    {
        //        var serializer = new DataContractJsonSerializer(typeof(PutUpdateCohortPatientViewResponse));
        //        responseCohortPatientView = (PutUpdateCohortPatientViewResponse)serializer.ReadObject(msResponse);
        //    }

        //    return responseCohortPatientView;
        //}
        #endregion

        private HttpClient GetHttpClient(Uri uri)
        {
            HttpClient client = new HttpClient();

            string userId = (_headerUserId != string.Empty ? _headerUserId : "000000000000000000000000");

            client.DefaultRequestHeaders.Host = uri.Host;
            client.DefaultRequestHeaders.Add("x-Phytel-UserID", userId);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private void FormPatientsImport_Load(object sender, EventArgs e)
        {
            txtContract.Text = ConfigurationManager.AppSettings.Get("contractNumber");
            txtURL.Text = ConfigurationManager.AppSettings.Get("DataDomainURL");
            txtSQLConn.Text = Phytel.Services.SQLDataService.Instance.GetConnectionString(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), false);
            this.Text = string.Format("{0}: {1}", contractNumber, this.Text);
        }

    }
}
