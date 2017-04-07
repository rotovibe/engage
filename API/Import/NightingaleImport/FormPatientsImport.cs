using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
using Phytel.API.Common.Extensions;
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
        private static int colActivateDeactivate = 41;

        private List<IdNamePair> modesLookUp = new List<IdNamePair>();
        private List<CommTypeData> typesLookUp = new List<CommTypeData>();
        private List<StateData> statesLookUp = new List<StateData>();
        private List<TimeZoneData> zonesLookUp = new List<TimeZoneData>();
        private TimeZoneData zoneDefault = new TimeZoneData();
        private List<IdNamePair> careMemberLookUp = new List<IdNamePair>();
        private List<SystemData> systemsData = new List<SystemData>();
        private ImportFile _importFile;
        private List<ImportData> listOfPatientData;
        public string elapsedTime { get; set; }

        private readonly double version = ImportToolConfigurations.version;
        private readonly string context = ImportToolConfigurations.context;
        private string contractNumber = null;

        public const string EngageSystemProperty = "Engage";
        public const string DataSourceProperty = "Import";
        public const string PCMRoleIdProperty = "56f169f8078e10eb86038514";
        string _headerUserId = "000000000000000000000000";
        private string _newdateofbirth;
        private int counter = 0;

        private int _numberofrows = 0;
        private int _numberofcolumns = 0;
        private string[,] _csv_values;
        private string _firstname;
        private string _lastname;
        private string _dob;

        public int NumberOfRows
        {
            get { return _numberofrows; }
            set { _numberofrows = value; }
        }
        public int NumberOfColumns
        {
            get { return _numberofcolumns; }
            set { _numberofcolumns = value; }
        }
        public string[,] Csv_Values
        {
            get { return _csv_values; }
            set { _csv_values = value; }
        }
        public string Firstname
        {
            get { return _firstname; }
            set { _firstname = value; }
        }
        public string Lastname
        {
            get { return _lastname; }
            set { _lastname = value; }
        }
        public string DOB
        {
            get { return _dob; }
            set { _dob = value; }
        }
        public string TextBoxURL
        {
            get { return txtURL.Text; }
            set { txtURL.Text = value; }
        }
        public string ContractNumber
        {
            get { return contractNumber; }
            set { contractNumber = value; }
        }
        public string TextBoxFileName
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        private int _skippedlvi;
        public int SkippedLvi
        {
            get { return _skippedlvi; }
            set { _skippedlvi = value; }
        }

        public ListView PatientListView
        {
            get { return listView1; }
            set { listView1 = value; }
        }

        public Button ImportButton
        {
            get { return btnImport; }
            set { btnImport = value; }
        }
        public string NewDateofBirth
        {
            get
            {
                DateTime newDOB;
                if (DateTime.TryParse(_newdateofbirth, out newDOB))
                {
                    _newdateofbirth = newDOB.ToString("MM/dd/yyyy");
                }
                else
                {
                    _newdateofbirth = string.Empty;
                }
                return _newdateofbirth;
            }
            set
            {
                DateTime newDob;
                if (DateTime.TryParse(value, out newDob))
                {
                    _newdateofbirth = newDob.ToString("MM/dd/yyyy");
                }
                else
                {
                    _newdateofbirth = string.Empty;
                }
            }
        }
        public Dictionary<string, string> ListView1List { get; set; }
        public List<string> ListView2List { get; set; }
        Dictionary<string, ImportData> patientDictionary = new Dictionary<string, ImportData>(StringComparer.InvariantCultureIgnoreCase);
        List<ImportData> allImportData = new List<ImportData>();

        public FormPatientsImport()
        {
            InitializeComponent();
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            comboBoxContractList.DataSource = ImportToolConfigurations.contracts;
            comboBoxContractList.SelectedText = " ";
        }

        public void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            chkSelectAllAlways();
        }

        private void button1_VisibleChanged(object sender, EventArgs e)
        {
            Browse.Text = "Browse";
        }

        private void progressBarUpdate()
        {
            progressBar1.Increment(1);
            lblProgressValue.Refresh();
            lblProgressValue.Text = string.Format("{0}/{1}", progressBar1.Value, NumberOfRows);
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
                btnViewReport.Enabled = false;
                btnImport.Enabled = false;

                Importer import = new Importer(txtURL.Text, context, version, contractNumber, _headerUserId);
                _importFile = new ImportFile(this);
                Guid sqlUserId = getUserId(txtContactID.Text);
                if (sqlUserId != Guid.Empty)
                {

                    GetContactByUserIdDataResponse contactUserResp = import.GetContactByUserId(sqlUserId.ToString());

                    if (contactUserResp.Contact == null)
                    {
                        throw new Exception("Invalid 'Admin User'");
                    }

                    _headerUserId = contactUserResp.Contact.Id;
                    if (string.IsNullOrEmpty(_headerUserId))
                        throw new Exception("Invalid 'Admin User'");
                    import.HeaderUserId = _headerUserId;

                    Task.Factory.StartNew(() => { LoadLookUps(); });
                    Task.Factory.StartNew(() => { LoadSystems(); });

                    #region foreach
                    int num_rows = listOfPatientData.Count;
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    foreach (var datarow in listOfPatientData)
                    {
                        try
                        {
                            GetContactByUserIdDataResponse contactByUserIdResponse = null;
                            if (!string.IsNullOrEmpty(datarow.patientData.CMan))
                            {
                                Guid userIdResponse = getUserId(datarow.patientData.CMan);

                                if (userIdResponse == Guid.Empty)
                                {
                                    datarow.importOperation = ImportOperation.LOOKUP_USER_CONTACT;
                                    datarow.failedMessage = string.Format("Unable to locate the UserId : {0}", datarow.patientData.CMan);
                                    continue;
                                }
                                contactByUserIdResponse = import.GetContactByUserId(userIdResponse.ToString());
                                if (contactByUserIdResponse.Contact == null)
                                {
                                    datarow.importOperation = ImportOperation.LOOKUP_USER_CONTACT;
                                    datarow.failedMessage = string.Format("Unable to locate contact for the UserId: {0}, rid: {1}", datarow.patientData.CMan, userIdResponse.ToString());
                                    continue;
                                }
                            }
                            GetPatientDataResponse existingPatientResponse = null;
                            try
                            {
                                existingPatientResponse = GetExistingPatientData(datarow.patientData, import);
                            }
                            catch (Exception ex)
                            {
                                datarow.importOperation = ImportOperation.SKIPPED;
                                datarow.skipped = true;
                                datarow.failedMessage = string.Format("Exception trying to locate patient. Message: {0}", ex.Message);
                                progressBarUpdate();
                                continue;
                            }

                            if (datarow.importOperation == ImportOperation.SKIPPED) continue;

                            #region insert/update
                            if (existingPatientResponse.Patient == null)
                            {
                                #region INSERT

                                datarow.importOperation = ImportOperation.INSERT;
                                progressBarUpdate();
                                try
                                {
                                    PutPatientDataRequest patientRequest = new PutPatientDataRequest
                                    {
                                        Patient = datarow.patientData,
                                        Context = context,
                                        ContractNumber = contractNumber,
                                        Version = version
                                    };
                                    PutPatientDataResponse responsePatient = import.InsertPatient(patientRequest);

                                    if (responsePatient.Id == null)
                                    {
                                        datarow.failed = true;
                                        datarow.failedMessage = string.Format("Insert failed.");
                                        continue;
                                    }
                                    else
                                    {
                                        #region Insert the patient system record provided in the import file.

                                        if (!String.IsNullOrEmpty(datarow.patientData.SysId) &&
                                            !String.IsNullOrEmpty(datarow.patientData.sysName))
                                        {
                                            var system =
                                                systemsData.Where(
                                                        s => s.Name.ToLower() == datarow.patientData.sysName.Trim().ToLower())
                                                    .FirstOrDefault();
                                            if (system != null)
                                            {
                                                PatientSystemData psData = new PatientSystemData
                                                {
                                                    PatientId = responsePatient.Id,
                                                    Primary =
                                                        (String.IsNullOrEmpty(datarow.patientData.SysPri))
                                                            ? false
                                                            : Boolean.Parse(datarow.patientData.SysPri.Trim()),
                                                    StatusId = (int)Phytel.API.DataDomain.PatientSystem.DTO.Status.Active,
                                                    SystemId = system.Id,
                                                    DataSource = DataSourceProperty,
                                                    Value =
                                                        (String.IsNullOrEmpty(datarow.patientData.SysId))
                                                            ? null
                                                            : datarow.patientData.SysId.Trim(),
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
                                                InsertPatientSystemDataResponse responsePatientPS =
                                                    import.InsertPatientSystem(psRequest);
                                                if (responsePatientPS.PatientSystemData == null)
                                                {
                                                    datarow.failed = true;
                                                    datarow.failedMessage = "Failed to import the PatientSystem Id provided in the file.";
                                                    continue;
                                                }
                                                else
                                                {
                                                    // If imported patientsystem's primary is set to true, override the EngagePatientSystem's primary field.
                                                    if (psData.Primary)
                                                    {
                                                        GetPatientSystemDataResponse engagePatientSystemResponse =
                                                            import.GetPatientSystem(new GetPatientSystemDataRequest
                                                            {
                                                                Context = context,
                                                                ContractNumber = contractNumber,
                                                                Version = version,
                                                                Id = responsePatient.EngagePatientSystemId
                                                            });
                                                        if (engagePatientSystemResponse != null &&
                                                            engagePatientSystemResponse.PatientSystemData != null)
                                                        {
                                                            engagePatientSystemResponse.PatientSystemData.Primary = false;
                                                            UpdatePatientSystemDataRequest updatePDRequest = new UpdatePatientSystemDataRequest
                                                            {
                                                                Context = context,
                                                                ContractNumber = contractNumber,
                                                                Version = version,
                                                                Id = engagePatientSystemResponse.PatientSystemData.Id,
                                                                PatientId =
                                                                    engagePatientSystemResponse.PatientSystemData.PatientId,
                                                                PatientSystemsData =
                                                                    engagePatientSystemResponse.PatientSystemData
                                                            };
                                                            import.UpdatePatientSystem(updatePDRequest);
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                        #endregion

                                        #region Insert Contact record.

                                        //Contact
                                        ContactData data = GetContactData(datarow);
                                        data.PatientId = responsePatient.Id;

                                        InsertContactDataRequest contactRequest = new InsertContactDataRequest
                                        {
                                            ContactData = data,
                                            Version = patientRequest.Version,
                                            Context = patientRequest.Context,
                                            ContractNumber = patientRequest.ContractNumber
                                        };

                                        InsertContactDataResponse responseContact =
                                            import.InsertContactForAPatient(contactRequest, responsePatient.Id.ToString());
                                        if (responseContact.Id == null)
                                        {
                                            datarow.failed = true;
                                            datarow.failedMessage = "Contact card import request failed.";
                                            continue;
                                        }

                                        #endregion

                                        #region CareMember

                                        if (contactByUserIdResponse != null)
                                        {
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
                                            SaveCareTeamDataResponse saveCareTeamDataResponse =
                                                import.InsertCareTeam(saveCareTeamDataRequest);
                                            if (saveCareTeamDataResponse == null)
                                            {
                                                datarow.failed = true;
                                                datarow.failedMessage = "Care Team import request failed.";
                                                continue;
                                            }
                                            import.UpdateCohortPatientView(responsePatient.Id.ToString(),
                                                contactByUserIdResponse.Contact.Id);

                                        }

                                        #endregion
                                    }
                                }
                                catch (Exception ex)
                                {
                                    datarow.failed = true;
                                    datarow.failedMessage = string.Format("Insert Failed - {0}", ex);
                                    continue;
                                }
                                #endregion
                            }
                            else
                            {
                                #region UPDATE
                                datarow.importOperation = ImportOperation.UPDATE;
                                progressBarUpdate();
                                try
                                {
                                    if (!ImportToolConfigurations.enhancedFeaturesContracts.Contains(contractNumber))
                                    {
                                        throw new Exception("This contract is not configured for updates.");
                                    }

                                    bool individualStatus = false;
                                    bool validIndividualStatusValue = false;
                                    int statusBackup = datarow.patientData.StatusId;
                                    if (!string.IsNullOrEmpty(datarow.patientData.ActivateDeactivate))
                                    {
                                        validIndividualStatusValue = bool.TryParse(datarow.patientData.ActivateDeactivate, out individualStatus);
                                    }
                                    if (validIndividualStatusValue)
                                    {
                                        datarow.patientData.StatusId = individualStatus ? (int)Phytel.API.DataDomain.Patient.DTO.Status.Active : (int)Phytel.API.DataDomain.Patient.DTO.Status.Inactive;
                                    }
                                    else
                                    {
                                        datarow.patientData.StatusId = existingPatientResponse.Patient.StatusId;
                                    }

                                    PutUpdatePatientDataRequest updatePatientRequest = new PutUpdatePatientDataRequest
                                    {
                                        PatientData = datarow.patientData,
                                        Context = context,
                                        ContractNumber = contractNumber,
                                        Version = version,
                                        Insert = false
                                    };
                                    updatePatientRequest.PatientData.Id = existingPatientResponse.Patient.Id;
                                    PutUpdatePatientDataResponse updatePatientResponse = import.UpsertPatient(updatePatientRequest, null);

                                    if (string.IsNullOrEmpty(updatePatientResponse.Id))
                                    {
                                        datarow.failed = true;
                                        datarow.failedMessage = ("Failed to update patient.");
                                        continue;
                                    }

                                    #region UPDATE CONTACT
                                    //Contact   
                                    GetContactByPatientIdDataResponse existingContactResponse = import.GetContactByPatientId(existingPatientResponse.Patient.Id);

                                    if (existingContactResponse.Contact == null)
                                    {
                                        datarow.failed = true;
                                        datarow.failedMessage = ("Update Failed. Cannot get contact by patient ID");
                                        continue;
                                    }
                                    ContactData data = GetContactData(datarow);
                                    data.PatientId = existingPatientResponse.Patient.Id;
                                    data.Id = existingContactResponse.Contact.Id;
                                    UpdateContactDataRequest updateContactRequest = new UpdateContactDataRequest()
                                    {
                                        ContactData = data,
                                        Context = context,
                                        ContractNumber = contractNumber,
                                        Version = version,
                                        Id = data.Id,
                                        UserId = data.UserId
                                    };
                                    var updateContactResponse = import.UpdateContactForAPatient(updateContactRequest, data.PatientId);
                                    if (updateContactResponse.SuccessData == false)
                                    {
                                        datarow.failed = true;
                                        datarow.failedMessage = ("Update Failed. Cannot update contact for patient");
                                        continue;
                                    }
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    datarow.failed = true;
                                    datarow.failedMessage = ("Update Failed.");
                                    continue;
                                }
                                #endregion
                                #endregion
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            datarow.skipped = true;
                            datarow.skippedMessage = string.Format("Row Skipped due to error : {0}", ex);
                        }
                    }
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);

                    btnViewReport.Enabled = true;
                    btnImport.Enabled = false;
                    btnImport.Text = elapsedTime;
                    Browse.Enabled = false;
                }
                else
                    MessageBox.Show("Invalid 'Admin User Name'!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Message:{0}, StackTrace:{1}", ex.Message, ex.StackTrace));
            }
            chkSelectAll.Visible = false;
        }

        private void SetPatientDictionaryFailed(string dictionaryKey, string failedMessage)
        {
            patientDictionary[dictionaryKey].failed = true;
            patientDictionary[dictionaryKey].failedMessage = failedMessage;

        }

        private void SetPatientDictionarySkipped(string dictionaryKey, string skippedMessage)
        {
            patientDictionary[dictionaryKey].skipped = true;
            patientDictionary[dictionaryKey].failedMessage = skippedMessage;
            patientDictionary[dictionaryKey].rowSkipped++;
        }

        public void SetListViewItemSkipped(ListViewItem lvi)
        {
            int n = listView1.CheckedItems.IndexOf(lvi);
            PatientListView.CheckedItems[n].BackColor = Color.Red;
            PatientListView.CheckedItems[n].Checked = false;
            _skippedlvi++;
        }

       public void SetListViewItemFailed(ListViewItem lvi)
        {
            int n = listView1.CheckedItems.IndexOf(lvi);
            listView1.CheckedItems[n].BackColor = Color.Red;
            listView1.CheckedItems[n].Checked = false;
        }

        private GetPatientDataResponse GetExistingPatientData(PatientData pdata, Importer import)
        {
            GetPatientDataByNameDOBRequest patientDataRequest = new GetPatientDataByNameDOBRequest
            {
                FirstName = pdata.FirstName,
                LastName = pdata.LastName,
                DOB = pdata.DOB,
                Context = context,
                ContractNumber = contractNumber,
                Version = version
            };
            GetPatientDataResponse patientDataResponse = import.GetPatientData(patientDataRequest);
            return patientDataResponse;
        }

        private PatientData GetPatientData(ListViewItem lvi)
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
                DOB = NewDateofBirth.Trim(),
                DataSource = EngageSystemProperty,
                StatusDataSource = EngageSystemProperty,
                Background = (String.IsNullOrEmpty(lvi.SubItems[colBkgrnd].Text)) ? null : lvi.SubItems[colBkgrnd].Text.Trim(),

            };
            return pdata;
        }

        private PatientData GetPatientData2(string[,] val, int row)
        {
            PatientData pdata = new PatientData
            {
                #region Sync up properties in Contact
                FirstName = val[row, colFirstN].Trim(),
                LastName = val[row, colLastN].Trim(),
                MiddleName = (String.IsNullOrEmpty(val[row, colMiddleN])) ? null : val[row, colMiddleN].Trim(),
                PreferredName = (String.IsNullOrEmpty(val[row, colPrefN])) ? null : val[row, colPrefN].Trim(),
                Gender = val[row, colGen].Trim(),
                Suffix = (String.IsNullOrEmpty(val[row, colSuff])) ? null : val[row, colSuff].Trim(),
                StatusId = (int)Phytel.API.DataDomain.Patient.DTO.Status.Active,
                #endregion
                DOB = NewDateofBirth.Trim(),
                DataSource = EngageSystemProperty,
                StatusDataSource = EngageSystemProperty,
                Background = (String.IsNullOrEmpty(val[row, colBkgrnd])) ? null : val[row, colBkgrnd].Trim(),

            };
            return pdata;
        }

        private ContactData GetContactData(ImportData importdata)
        {
            #region Communication
            //timezone
            TimeZoneData tZone = null;
            if (string.IsNullOrEmpty(importdata.patientData.TimeZ) == false)
            {
                tZone = new TimeZoneData();
                foreach (TimeZoneData t in zonesLookUp)
                {
                    string[] zones = t.Name.Split(" ".ToCharArray());
                    if (importdata.patientData.TimeZ.Trim() == zones[0])
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
            if (string.IsNullOrEmpty(importdata.patientData.Ph1) == false)
            {
                PhoneData phone1 = new PhoneData
                {
                    Number = Convert.ToInt64(importdata.patientData.Ph1.Replace("-", string.Empty)),
                    OptOut = false,
                    DataSource = DataSourceProperty
                };

                if (String.Compare(importdata.patientData.Ph1Pref.Trim(), "true", true) == 0)
                {
                    phone1.PhonePreferred = true;
                }
                else
                    phone1.PhonePreferred = false;

                if (string.IsNullOrEmpty(importdata.patientData.Ph1Type) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(importdata.patientData.Ph1Type.Trim(), c.Name, true) == 0)
                        {
                            phone1.TypeId = c.Id;
                            break;
                        }
                    }
                }
                else
                    phone1.TypeId = typesLookUp[0].Id;

                phones.Add(phone1);
            }

            if (string.IsNullOrEmpty(importdata.patientData.Ph2) == false)
            {
                PhoneData phone2 = new PhoneData
                {
                    Number = Convert.ToInt64(importdata.patientData.Ph2.Replace("-", string.Empty)),
                    OptOut = false,
                    DataSource = DataSourceProperty
                };

                if (String.Compare(importdata.patientData.Ph2Pref.Trim(), "true", true) == 0)
                {
                    phone2.PhonePreferred = true;
                }
                else
                    phone2.PhonePreferred = false;

                if (string.IsNullOrEmpty(importdata.patientData.Ph2Type) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(importdata.patientData.Ph2Type.Trim(), c.Name, true) == 0)
                        {
                            phone2.TypeId = c.Id;
                            break;
                        }
                    }
                }
                else
                    phone2.TypeId = typesLookUp[0].Id;

                phones.Add(phone2);
            }

            //emails
            if (string.IsNullOrEmpty(importdata.patientData.Em1) == false)
            {
                EmailData email1 = new EmailData
                {
                    Text = importdata.patientData.Em1.Trim(),
                    OptOut = false,
                };

                if (String.Compare(importdata.patientData.Em1Pref.Trim(), "true", true) == 0)
                {
                    email1.Preferred = true;
                }
                else
                    email1.Preferred = false;

                if (string.IsNullOrEmpty(importdata.patientData.Em1Pref) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(importdata.patientData.Em1Type.Trim(), c.Name, true) == 0)
                        {
                            email1.TypeId = c.Id;
                            break;
                        }
                    }
                }
                else
                    email1.TypeId = typesLookUp[0].Id;

                emails.Add(email1);
            }

            if (string.IsNullOrEmpty(importdata.patientData.Em2) == false)
            {
                EmailData email2 = new EmailData
                {
                    Text = importdata.patientData.Em2.Trim(),
                    OptOut = false,
                };

                if (String.Compare(importdata.patientData.Em2Pref.Trim(), "true", true) == 0)
                {
                    email2.Preferred = true;
                }
                else
                    email2.Preferred = false;

                if (string.IsNullOrEmpty(importdata.patientData.Em2Type) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(importdata.patientData.Em2Type.Trim(), c.Name, true) == 0)
                        {
                            email2.TypeId = c.Id;
                            break;
                        }
                    }
                }
                else
                    email2.TypeId = typesLookUp[0].Id;

                emails.Add(email2);
            }

            //addresses
            if ((string.IsNullOrEmpty(importdata.patientData.Add1L1) == false)
                || (string.IsNullOrEmpty(importdata.patientData.Add1L2) == false)
                || (string.IsNullOrEmpty(importdata.patientData.Add1L3) == false)
                || (string.IsNullOrEmpty(importdata.patientData.Add1City) == false)
                || (string.IsNullOrEmpty(importdata.patientData.Add1St) == false)
                || (string.IsNullOrEmpty(importdata.patientData.Add1Zip) == false))
            {
                AddressData add1 = new AddressData
                {
                    Line1 = (string.IsNullOrEmpty(importdata.patientData.Add1L1)) ? null : importdata.patientData.Add1L1.Trim(),
                    Line2 = (string.IsNullOrEmpty(importdata.patientData.Add1L2)) ? null : importdata.patientData.Add1L2.Trim(),
                    Line3 = (string.IsNullOrEmpty(importdata.patientData.Add1L3)) ? null : importdata.patientData.Add1L3.Trim(),
                    City = (string.IsNullOrEmpty(importdata.patientData.Add1City)) ? null : importdata.patientData.Add1City.Trim(),
                    PostalCode = (string.IsNullOrEmpty(importdata.patientData.Add1Zip)) ? null : importdata.patientData.Add1Zip.Trim(),
                    OptOut = false
                };

                if (String.Compare(importdata.patientData.Add1Pref.Trim(), "true", true) == 0)
                {
                    add1.Preferred = true;
                }
                else
                    add1.Preferred = false;

                string stateTrim = (string.IsNullOrEmpty(importdata.patientData.Add1St)) ? null : importdata.patientData.Add1St.Trim();
                foreach (StateData st in statesLookUp)
                {
                    if ((st.Name == stateTrim)
                        || (st.Code == stateTrim))
                    {
                        add1.StateId = st.Id;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(importdata.patientData.Add1Type) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(importdata.patientData.Add1Type.Trim(), c.Name, true) == 0)
                        {
                            add1.TypeId = c.Id;
                            break;
                        }
                    }
                }
                else
                    add1.TypeId = typesLookUp[0].Id;

                addresses.Add(add1);
            }

            if ((string.IsNullOrEmpty(importdata.patientData.Add2L1) == false)
                || (string.IsNullOrEmpty(importdata.patientData.Add2L2) == false)
                || (string.IsNullOrEmpty(importdata.patientData.Add2L3) == false)
                || (string.IsNullOrEmpty(importdata.patientData.Add2City) == false)
                || (string.IsNullOrEmpty(importdata.patientData.Add2St) == false)
                || (string.IsNullOrEmpty(importdata.patientData.Add2Zip) == false))
            {
                AddressData add2 = new AddressData
                {
                    Line1 = (string.IsNullOrEmpty(importdata.patientData.Add2L1)) ? null : importdata.patientData.Add2L1.Trim(),
                    Line2 = (string.IsNullOrEmpty(importdata.patientData.Add2L2)) ? null : importdata.patientData.Add2L2.Trim(),
                    Line3 = (string.IsNullOrEmpty(importdata.patientData.Add2L3)) ? null : importdata.patientData.Add2L3.Trim(),
                    City = (string.IsNullOrEmpty(importdata.patientData.Add2City)) ? null : importdata.patientData.Add2City.Trim(),
                    PostalCode = (string.IsNullOrEmpty(importdata.patientData.Add2Zip)) ? null : importdata.patientData.Add2Zip.Trim(),
                    OptOut = false
                };

                if (String.Compare(importdata.patientData.Add2Pref.Trim(), "true", true) == 0)
                {
                    add2.Preferred = true;
                }
                else
                    add2.Preferred = false;

                string stateTrim = (string.IsNullOrEmpty(importdata.patientData.Add2St)) ? null : importdata.patientData.Add2St.Trim();
                foreach (StateData st in statesLookUp)
                {
                    if ((st.Name == stateTrim)
                        || (st.Code == stateTrim))
                    {
                        add2.StateId = st.Id;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(importdata.patientData.Add2Type) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(importdata.patientData.Add2Type.Trim(), c.Name, true) == 0)
                        {
                            add2.TypeId = c.Id;
                            break;
                        }
                    }
                }
                else
                    add2.TypeId = typesLookUp[0].Id;

                addresses.Add(add2);
            }
            #endregion

            //Contact
            ContactData data = new ContactData
            {
                //PatientId = responsePatient.Id,
                ContactTypeId = Phytel.API.DataDomain.Contact.DTO.Constants.PersonContactTypeId,
                #region Sync up properties in Contact
                FirstName = importdata.patientData.FirstName,
                LastName = importdata.patientData.LastName,
                MiddleName = importdata.patientData.MiddleName,
                PreferredName = importdata.patientData.PreferredName,
                Gender = importdata.patientData.Gender,
                Suffix = importdata.patientData.Suffix,
                StatusId = importdata.patientData.StatusId,
                #endregion
                DataSource = EngageSystemProperty,
                Modes = modes,
                TimeZoneId = tZone == null ? null : tZone.Id,
                Phones = phones,
                Emails = emails,
                Addresses = addresses
            };
            return data;
        }

        private ContactData GetContactData(ListViewItem lvi, PatientData pdata)
        {
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
                            break;
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
                            break;
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
                            break;
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
                            break;
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
                        break;
                    }
                }

                if (string.IsNullOrEmpty(lvi.SubItems[colAdd1Type].Text) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(lvi.SubItems[colAdd1Type].Text.Trim(), c.Name, true) == 0)
                        {
                            add1.TypeId = c.Id;
                            break;
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
                        break;
                    }
                }

                if (string.IsNullOrEmpty(lvi.SubItems[colAdd2Type].Text) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(lvi.SubItems[colAdd2Type].Text.Trim(), c.Name, true) == 0)
                        {
                            add2.TypeId = c.Id;
                            break;
                        }
                    }
                }
                else
                    add2.TypeId = typesLookUp[0].Id;

                addresses.Add(add2);
            }
            #endregion

            //Contact
            ContactData data = new ContactData
            {
                //PatientId = responsePatient.Id,
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
            return data;
        }
        private void LoadLookUps()
        {
            //modes
            Uri modesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/commmodes?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    _headerUserId));
            HttpClient modesClient = GetHttpClient(modesUri);

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
            Uri typesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/commtypes?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    _headerUserId));
            HttpClient typesClient = GetHttpClient(typesUri);

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
            Uri statesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/states?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    _headerUserId));
            HttpClient statesClient = GetHttpClient(statesUri);

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
            statesMs.Dispose();
            statesSr.Dispose();

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
                                                    contractNumber,
                                                    _headerUserId));
            HttpClient zonesClient = GetHttpClient(zonesUri);

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
            Uri zoneUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/TimeZone/Default?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    _headerUserId));
            HttpClient zoneClient = GetHttpClient(zoneUri);

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
            zoneMs.Dispose();
            zoneSr.Dispose();

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
            ///{Context}/{Version}/{contractNumber}/Type/{Name}
            Uri careMemberUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/Type/CareMemberType?UserId={4}",
                                                    txtURL.Text,
                                                    context,
                                                    version,
                                                    contractNumber,
                                                    _headerUserId));
            HttpClient careMemberClient = GetHttpClient(careMemberUri);

            GetLookUpsDataRequest careMemberRequest = new GetLookUpsDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = contractNumber
            };

            DataContractJsonSerializer careMemberJsonSer = new DataContractJsonSerializer(typeof(GetLookUpsDataRequest));
            MemoryStream careMemberMs = new MemoryStream();
            careMemberJsonSer.WriteObject(careMemberMs, careMemberRequest);
            careMemberMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader careMemberSr = new StreamReader(careMemberMs);
            StringContent careMemberContent = new StringContent(careMemberSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            careMemberMs.Dispose();
            careMemberSr.Dispose();

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
                                                    contractNumber,
                                                    _headerUserId));
            HttpClient client = GetHttpClient(patientsystemDDUri);

            GetSystemsDataRequest systemDataRequest = new GetSystemsDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = contractNumber
            };

            DataContractJsonSerializer modesJsonSer = new DataContractJsonSerializer(typeof(GetSystemsDataRequest));
            MemoryStream ms = new MemoryStream();
            modesJsonSer.WriteObject(ms, systemDataRequest);
            ms.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader modesSr = new StreamReader(ms);
            StringContent modesContent = new StringContent(modesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            ms.Dispose();
            modesSr.Dispose();

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
            btnImport.Text = "Import";
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


        private string[,] LoadCsv(string filename)
        {
            string _file = System.IO.File.ReadAllText(filename);
            _file = _file.Replace('\n', '\r');
            string[] lines = _file.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int row_length = lines.Length;
            int col_length = lines[0].Split(',').Length;
            string[,] csvValues = new string[row_length, col_length];
            for (int row = 0; row < row_length; row++)
            {
                string[] csvLine = lines[row].Split(',');
                for (int col = 0; col < col_length; col++)
                {
                    csvValues[row, col] = csvLine[col];
                }
            }
            return csvValues;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            lblStatus.Visible = false;
            chkSelectAll.Visible = false;
            filename = openFileDialog1.FileName;
            textBox1.Text = filename;
            int counter = 0;
            _csv_values = LoadCsv(filename);
            _numberofrows = File.ReadAllLines(filename).Length;
            try
            {
                lblProgressValue.Visible = true;
                progressBar1.Visible = true;
                progressBar1.Maximum = _numberofrows;
                progressBar1.Value = 0;

                if (openFileDialog1.CheckFileExists)
                {
                    filename = openFileDialog1.FileName;
                    textBox1.Text = filename;
                    listOfPatientData = File.ReadAllLines(filename)
                                          .Select(v => ImportData.FromCsv(v))
                                          .ToList();
                    try
                    {
                        foreach (
                        var listdata in
                        listOfPatientData.Where(x =>x.patientData!=null && (string.IsNullOrEmpty(x.patientData.FirstName) ||string.IsNullOrEmpty(x.patientData.LastName) ||string.IsNullOrEmpty(x.patientData.DOB) ||(x.patientData.FirstName.ToLower() == "firstname") ||(x.patientData.LastName.ToLower() == "lastname")))
                    )
                        {
                            listdata.importOperation = ImportOperation.SKIPPED;
                            listdata.skipped = true;
                            listdata.failedMessage = "Row skipped because of invalid firstname or lastname or dob";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    counter = listOfPatientData.Count(x => x.importOperation == ImportOperation.SKIPPED);
                    lblProgressValue.Text = string.Format("{0}/{1}", counter, progressBar1.Maximum);
                    lblProgressValue.Refresh();
                    progressBar1.Increment(counter);
                }
                
                var numofrows = _csv_values.GetLength(0);
                var numofcols = _csv_values.GetLength(1);
                listView1.Items.Clear();
                #region display 10 rows in Listview
                if (numofrows > 10)
                {
                    for (int row = 0; row < 10; row++)
                    {
                        ListViewItem new_item = new ListViewItem();
                        for (int column = 0; column < numofcols; column++)
                        {
                            if (column == 0)
                            {
                                new_item.Text = _csv_values[row, 0]; //First item is not a "subitem".
                            }
                            else
                            {
                                new_item.SubItems.Add(_csv_values[row, column]);
                            }
                        }
                        listView1.Items.Add(new_item);
                    }
                }
                else
                {
                    for (int row = 0; row < numofrows; row++)
                    {
                        ListViewItem new_item = new ListViewItem();
                        for (int column = 0; column < numofcols; column++)
                        {
                            if (column == 0)
                            {
                                new_item.Text = _csv_values[row, 0];
                            }
                            else
                            {
                                new_item.SubItems.Add(_csv_values[row, column]);
                            }
                        }
                        listView1.Items.Add(new_item);
                    }
                }
                #endregion
                btnImport.Enabled = numofrows > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                lblTotalValue.Text = counter + " / " + _numberofrows + " will be skipped.";
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
            if (chkSelectAll.Checked)
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    if (listView1.Items[i].BackColor != Color.Red)
                        listView1.Items[i].Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].Checked = false;
                }
            }

        }

        private void chkSelectAllAlways()
        {

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].BackColor != Color.Red)
                    listView1.Items[i].Checked = true;
            }
            chkSelectAll.Checked = true;
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
        //                                             contractNumber,
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
        //                                           contractNumber,
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
        //                                           contractNumber,
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
        //                                                contractNumber,
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
        //                                                contractNumber,
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
        //                                    contractNumber,
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
        //                                         contractNumber,
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
        //                                            contractNumber,
        //                                            userId,
        //                                            _headerUserId));
        //    HttpClient getContactClient = GetHttpClient(getContactUri);

        //    GetContactByUserIdDataRequest getContactRequest = new GetContactByUserIdDataRequest
        //    {
        //        SQLUserId = userId,
        //        Context = context,
        //        Version = version,
        //        ContractNumber = contractNumber
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
        //                ContractNumber = contractNumber,
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
        //                                                contractNumber,
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
        //                                         contractNumber,
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
            txtURL.Text = ConfigurationManager.AppSettings.Get("DataDomainURL");
            txtSQLConn.Text = Phytel.Services.SQLDataService.Instance.GetConnectionString(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), false);
            lblStatus.Visible = false;
            chkSelectAll.Visible = false;
        }

        private void comboBoxContractList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            contractNumber = comboBoxContractList.SelectedValue.ToString();
            Browse.Enabled = !contractNumber.Trim().IsNullOrEmpty();
            this.Text = string.Format("{0}: Nightingale Import Utility", contractNumber);

        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ListView lv = sender as ListView;
            if (lv.Items[e.Index].BackColor == Color.Red)
            {
                e.NewValue = CheckState.Unchecked;
            }
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            FormImportReport frmImportReport = new FormImportReport(listOfPatientData);

            frmImportReport.Show();

        }

    }
}
