using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using NGDataImport;
using NightingaleImport.Configuration;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;

namespace NightingaleImport
{
    public class ImportFile : FormImportReport
    {
        private ImportFileModel _ImportModel;
        private List<IdNamePair> modesLookUp = new List<IdNamePair>();
        private List<CommTypeData> typesLookUp = new List<CommTypeData>();
        private List<StateData> statesLookUp = new List<StateData>();
        private List<TimeZoneData> zonesLookUp = new List<TimeZoneData>();
        private TimeZoneData zoneDefault = new TimeZoneData();
        private List<IdNamePair> careMemberLookUp = new List<IdNamePair>();
        private List<SystemData> systemsData = new List<SystemData>();

        private ImportFile _importFile;
        FormPatientsImport _formPatientsImport;

        private readonly double version = ImportToolConfigurations.version;
        private readonly string context = ImportToolConfigurations.context;
        private string contractNumber = null;
        //private string txtURL = FormPatientsImport._FormPatientsImport.TextBoxURL;

        public const string EngageSystemProperty = "Engage";
        public const string DataSourceProperty = "Import";
        public const string PCMRoleIdProperty = "56f169f8078e10eb86038514";
        string _headerUserId = "000000000000000000000000";
        private string _newdateofbirth;
        GetContactByUserIdDataResponse contactByUserIdResponse = null;
        Dictionary<string, ImportData> patientDictionary = new Dictionary<string, ImportData>(StringComparer.InvariantCultureIgnoreCase);

        public ImportFile(FormPatientsImport _formPatientsImport)
        {
            _ImportModel = new ImportFileModel();
            this._formPatientsImport = _formPatientsImport;
        }

        #region NewDateofBirth
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
        #endregion

        #region GetPatientData

        public PatientData GetPatientData(string[,] val, int row)
        {
            PatientData pdata = new PatientData
            {
                #region Sync up properties in Contact
                FirstName = val[row, _ImportModel.colFirstN].Trim(),
                LastName = val[row, _ImportModel.colLastN].Trim(),
                MiddleName = (String.IsNullOrEmpty(val[row, _ImportModel.colMiddleN])) ? null : val[row, _ImportModel.colMiddleN].Trim(),
                PreferredName = (String.IsNullOrEmpty(val[row, _ImportModel.colPrefN])) ? null : val[row, _ImportModel.colPrefN].Trim(),
                Gender = val[row, _ImportModel.colGen].Trim(),
                Suffix = (String.IsNullOrEmpty(val[row, _ImportModel.colSuff])) ? null : val[row, _ImportModel.colSuff].Trim(),
                StatusId = (int)Phytel.API.DataDomain.Patient.DTO.Status.Active,
                #endregion
                DOB = NewDateofBirth.Trim(),
                DataSource = EngageSystemProperty,
                StatusDataSource = EngageSystemProperty,
                Background = (String.IsNullOrEmpty(val[row, _ImportModel.colBkgrnd])) ? null : val[row, _ImportModel.colBkgrnd].Trim(),

            };
            return pdata;
        }

        public PatientData GetAllPatientData(ImportData  val)
        {
            PatientData pdata = new PatientData
            {
                #region Sync up properties in Contact
                FirstName = val.patientData.FirstName.Trim(),
                LastName = val.patientData.LastName.Trim(),
                MiddleName = (String.IsNullOrEmpty(val.patientData.MiddleName)) ? null : val.patientData.MiddleName.Trim(),
                PreferredName = (String.IsNullOrEmpty(val.patientData.PreferredName) ? null : val.patientData.PreferredName.Trim()),
                Gender = val.patientData.Gender.Trim(),
                Suffix = (String.IsNullOrEmpty(val.patientData.Suffix)) ? null : val.patientData.Suffix.Trim(),
                StatusId = (int)Phytel.API.DataDomain.Patient.DTO.Status.Active,
                #endregion
                DOB = NewDateofBirth.Trim(),
                DataSource = EngageSystemProperty,
                StatusDataSource = EngageSystemProperty,
                Background = (String.IsNullOrEmpty(val.patientData.Background)) ? null : val.patientData.Background.Trim(),
            };
            return pdata;

        }

        public PatientData GetPatientData1(ListViewItem lvi)
        {
            _ImportModel = new ImportFileModel();
            PatientData pdata = new PatientData
            {
                #region Sync up properties in Contact
                FirstName = lvi.SubItems[_ImportModel.colFirstN].Text.Trim(),
                LastName = lvi.SubItems[_ImportModel.colLastN].Text.Trim(),
                MiddleName = (String.IsNullOrEmpty(lvi.SubItems[_ImportModel.colMiddleN].Text)) ? null : lvi.SubItems[_ImportModel.colMiddleN].Text.Trim(),
                PreferredName = (String.IsNullOrEmpty(lvi.SubItems[_ImportModel.colPrefN].Text)) ? null : lvi.SubItems[_ImportModel.colPrefN].Text.Trim(),
                Gender = lvi.SubItems[_ImportModel.colGen].Text.Trim(),
                Suffix = (String.IsNullOrEmpty(lvi.SubItems[_ImportModel.colSuff].Text)) ? null : lvi.SubItems[_ImportModel.colSuff].Text.Trim(),
                StatusId = (int)Phytel.API.DataDomain.Patient.DTO.Status.Active,
                #endregion
                DOB = _ImportModel.colDB.ToString().Trim(),
                DataSource = EngageSystemProperty,
                StatusDataSource = EngageSystemProperty,
                Background = (String.IsNullOrEmpty(lvi.SubItems[_ImportModel.colBkgrnd].Text)) ? null : lvi.SubItems[_ImportModel.colBkgrnd].Text.Trim(),
            };
            return pdata;
        }
        #endregion

        #region getcontactdata
        public ContactData GetContactData(ListViewItem lvi, PatientData pdata)
        {
            #region Communication
            //timezone
            TimeZoneData tZone = null;
            if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colTimeZ].Text) == false)
            {
                tZone = new TimeZoneData();
                foreach (TimeZoneData t in zonesLookUp)
                {
                    string[] zones = t.Name.Split(" ".ToCharArray());
                    if (lvi.SubItems[_ImportModel.colTimeZ].Text.Trim() == zones[0])
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
            if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colPh1].Text) == false)
            {
                PhoneData phone1 = new PhoneData
                {
                    Number = Convert.ToInt64(lvi.SubItems[_ImportModel.colPh1].Text.Replace("-", string.Empty)),
                    OptOut = false,
                    DataSource = DataSourceProperty
                };

                if (String.Compare(lvi.SubItems[_ImportModel.colPh1Pref].Text.Trim(), "true", true) == 0)
                {
                    phone1.PhonePreferred = true;
                }
                else
                    phone1.PhonePreferred = false;

                if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colPh1Type].Text) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(lvi.SubItems[_ImportModel.colPh1Type].Text.Trim(), c.Name, true) == 0)
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

            if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colPh2].Text) == false)
            {
                PhoneData phone2 = new PhoneData
                {
                    Number = Convert.ToInt64(lvi.SubItems[_ImportModel.colPh2].Text.Replace("-", string.Empty)),
                    OptOut = false,
                    DataSource = DataSourceProperty
                };

                if (String.Compare(lvi.SubItems[_ImportModel.colPh2Pref].Text.Trim(), "true", true) == 0)
                {
                    phone2.PhonePreferred = true;
                }
                else
                    phone2.PhonePreferred = false;

                if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colPh2Type].Text) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(lvi.SubItems[_ImportModel.colPh2Type].Text.Trim(), c.Name, true) == 0)
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
            if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colEm1].Text) == false)
            {
                EmailData email1 = new EmailData
                {
                    Text = lvi.SubItems[_ImportModel.colEm1].Text.Trim(),
                    OptOut = false,
                };

                if (String.Compare(lvi.SubItems[_ImportModel.colEm1Pref].Text.Trim(), "true", true) == 0)
                {
                    email1.Preferred = true;
                }
                else
                    email1.Preferred = false;

                if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colEm1Type].Text) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(lvi.SubItems[_ImportModel.colEm1Type].Text.Trim(), c.Name, true) == 0)
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

            if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colEm2].Text) == false)
            {
                EmailData email2 = new EmailData
                {
                    Text = lvi.SubItems[_ImportModel.colEm2].Text.Trim(),
                    OptOut = false,
                };

                if (String.Compare(lvi.SubItems[_ImportModel.colEm2Pref].Text.Trim(), "true", true) == 0)
                {
                    email2.Preferred = true;
                }
                else
                    email2.Preferred = false;

                if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colEm2Type].Text) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(lvi.SubItems[_ImportModel.colEm2Type].Text.Trim(), c.Name, true) == 0)
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
            if ((string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1L1].Text) == false)
                || (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1L2].Text) == false)
                || (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1L3].Text) == false)
                || (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1City].Text) == false)
                || (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1St].Text) == false)
                || (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1Zip].Text) == false))
            {
                AddressData add1 = new AddressData
                {
                    Line1 = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1L1].Text)) ? null : lvi.SubItems[_ImportModel.colAdd1L1].Text.Trim(),
                    Line2 = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1L2].Text)) ? null : lvi.SubItems[_ImportModel.colAdd1L2].Text.Trim(),
                    Line3 = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1L3].Text)) ? null : lvi.SubItems[_ImportModel.colAdd1L3].Text.Trim(),
                    City = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1City].Text)) ? null : lvi.SubItems[_ImportModel.colAdd1City].Text.Trim(),
                    PostalCode = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1Zip].Text)) ? null : lvi.SubItems[_ImportModel.colAdd1Zip].Text.Trim(),
                    OptOut = false
                };

                if (String.Compare(lvi.SubItems[_ImportModel.colAdd1Pref].Text.Trim(), "true", true) == 0)
                {
                    add1.Preferred = true;
                }
                else
                    add1.Preferred = false;

                string stateTrim = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1St].Text)) ? null : lvi.SubItems[_ImportModel.colAdd1St].Text.Trim();
                foreach (StateData st in statesLookUp)
                {
                    if ((st.Name == stateTrim)
                        || (st.Code == stateTrim))
                    {
                        add1.StateId = st.Id;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd1Type].Text) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(lvi.SubItems[_ImportModel.colAdd1Type].Text.Trim(), c.Name, true) == 0)
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

            if ((string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2L1].Text) == false)
                || (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2L2].Text) == false)
                || (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2L3].Text) == false)
                || (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2City].Text) == false)
                || (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2St].Text) == false)
                || (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2Zip].Text) == false))
            {
                AddressData add2 = new AddressData
                {
                    Line1 = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2L1].Text)) ? null : lvi.SubItems[_ImportModel.colAdd2L1].Text.Trim(),
                    Line2 = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2L2].Text)) ? null : lvi.SubItems[_ImportModel.colAdd2L2].Text.Trim(),
                    Line3 = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2L3].Text)) ? null : lvi.SubItems[_ImportModel.colAdd2L3].Text.Trim(),
                    City = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2City].Text)) ? null : lvi.SubItems[_ImportModel.colAdd2City].Text.Trim(),
                    PostalCode = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2Zip].Text)) ? null : lvi.SubItems[_ImportModel.colAdd2Zip].Text.Trim(),
                    OptOut = false
                };

                if (String.Compare(lvi.SubItems[_ImportModel.colAdd2Pref].Text.Trim(), "true", true) == 0)
                {
                    add2.Preferred = true;
                }
                else
                    add2.Preferred = false;

                string stateTrim = (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2St].Text)) ? null : lvi.SubItems[_ImportModel.colAdd2St].Text.Trim();
                foreach (StateData st in statesLookUp)
                {
                    if ((st.Name == stateTrim)
                        || (st.Code == stateTrim))
                    {
                        add2.StateId = st.Id;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(lvi.SubItems[_ImportModel.colAdd2Type].Text) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(lvi.SubItems[_ImportModel.colAdd2Type].Text.Trim(), c.Name, true) == 0)
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

        public ContactData GetContactData1(string[,] val, PatientData pdata, int row)
        {
            #region Communication
            //timezone
            TimeZoneData tZone = null;
            if (string.IsNullOrEmpty(val[row, _ImportModel.colTimeZ]) == false)
            {
                tZone = new TimeZoneData();
                foreach (TimeZoneData t in zonesLookUp)
                {
                    string[] zones = t.Name.Split(" ".ToCharArray());
                    if (val[row, _ImportModel.colTimeZ].Trim() == zones[0])
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
            if (string.IsNullOrEmpty(val[row, _ImportModel.colPh1]) == false)
            {
                PhoneData phone1 = new PhoneData
                {
                    Number = Convert.ToInt64(val[row, _ImportModel.colPh1].Replace("-", string.Empty)),
                    OptOut = false,
                    DataSource = DataSourceProperty
                };

                if (String.Compare(val[row, _ImportModel.colPh1Pref].Trim(), "true", true) == 0)
                {
                    phone1.PhonePreferred = true;
                }
                else
                    phone1.PhonePreferred = false;

                if (string.IsNullOrEmpty(val[row, _ImportModel.colPh1Type]) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(val[row, _ImportModel.colPh1Type].Trim(), c.Name, StringComparison.OrdinalIgnoreCase) == 0)
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

            if (string.IsNullOrEmpty(val[row, _ImportModel.colPh2]) == false)
            {
                PhoneData phone2 = new PhoneData
                {
                    Number = Convert.ToInt64(val[row, _ImportModel.colPh2].Replace("-", string.Empty)),
                    OptOut = false,
                    DataSource = DataSourceProperty
                };

                if (String.Compare(val[row, _ImportModel.colPh2Pref].Trim(), "true", true) == 0)
                {
                    phone2.PhonePreferred = true;
                }
                else
                    phone2.PhonePreferred = false;

                if (string.IsNullOrEmpty(val[row, _ImportModel.colPh2Type]) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(val[row, _ImportModel.colPh2Type].Trim(), c.Name, true) == 0)
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
            if (string.IsNullOrEmpty(val[row, _ImportModel.colEm1]) == false)
            {
                EmailData email1 = new EmailData
                {
                    Text = val[row, _ImportModel.colEm1].Trim(),
                    OptOut = false,
                };

                if (String.Compare(val[row, _ImportModel.colEm1Pref].Trim(), "true", true) == 0)
                {
                    email1.Preferred = true;
                }
                else
                    email1.Preferred = false;

                if (string.IsNullOrEmpty(val[row, _ImportModel.colEm1Type]) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(val[row, _ImportModel.colEm1Type].Trim(), c.Name, true) == 0)
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

            if (string.IsNullOrEmpty(val[row, _ImportModel.colEm2]) == false)
            {
                EmailData email2 = new EmailData
                {
                    Text = val[row, _ImportModel.colEm2].Trim(),
                    OptOut = false,
                };

                if (String.Compare(val[row, _ImportModel.colEm2Pref].Trim(), "true", true) == 0)
                {
                    email2.Preferred = true;
                }
                else
                    email2.Preferred = false;

                if (string.IsNullOrEmpty(val[row, _ImportModel.colEm2Type]) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(val[row, _ImportModel.colEm2Type].Trim(), c.Name, true) == 0)
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
            if ((string.IsNullOrEmpty(val[row, _ImportModel.colAdd1L1]) == false)
                || (string.IsNullOrEmpty(val[row,_ImportModel.colAdd1L2]) == false)
                || (string.IsNullOrEmpty(val[row,_ImportModel.colAdd1L3]) == false)
                || (string.IsNullOrEmpty(val[row,_ImportModel.colAdd1City]) == false)
                || (string.IsNullOrEmpty(val[row,_ImportModel.colAdd1St]) == false)
                || (string.IsNullOrEmpty(val[row,_ImportModel.colAdd1Zip]) == false))
            {
                AddressData add1 = new AddressData
                {
                    Line1 = (string.IsNullOrEmpty(val[row,_ImportModel.colAdd1L1])) ? null : val[row,_ImportModel.colAdd1L1].Trim(),
                    Line2 = (string.IsNullOrEmpty(val[row,_ImportModel.colAdd1L2])) ? null : val[row,_ImportModel.colAdd1L2].Trim(),
                    Line3 = (string.IsNullOrEmpty(val[row,_ImportModel.colAdd1L3])) ? null : val[row, _ImportModel.colAdd1L3].Trim(),
                    City = (string.IsNullOrEmpty(val[row, _ImportModel.colAdd1City])) ? null : val[row, _ImportModel.colAdd1City].Trim(),
                    PostalCode = (string.IsNullOrEmpty(val[row, _ImportModel.colAdd1Zip])) ? null : val[row, _ImportModel.colAdd1Zip].Trim(),
                    OptOut = false
                };

                if (String.Compare(val[row, _ImportModel.colAdd1Pref].Trim(), "true", true) == 0)
                {
                    add1.Preferred = true;
                }
                else
                    add1.Preferred = false;

                string stateTrim = (string.IsNullOrEmpty(val[row, _ImportModel.colAdd1St])) ? null : val[row, _ImportModel.colAdd1St].Trim();
                foreach (StateData st in statesLookUp)
                {
                    if ((st.Name == stateTrim)
                        || (st.Code == stateTrim))
                    {
                        add1.StateId = st.Id;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(val[row, _ImportModel.colAdd1Type]) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(val[row, _ImportModel.colAdd1Type].Trim(), c.Name, true) == 0)
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

            if ((string.IsNullOrEmpty(val[row, _ImportModel.colAdd2L1]) == false)
                || (string.IsNullOrEmpty(val[row,_ImportModel.colAdd2L2]) == false)
                || (string.IsNullOrEmpty(val[row,_ImportModel.colAdd2L3]) == false)
                || (string.IsNullOrEmpty(val[row,_ImportModel.colAdd2City]) == false)
                || (string.IsNullOrEmpty(val[row,_ImportModel.colAdd2St]) == false)
                || (string.IsNullOrEmpty(val[row, _ImportModel.colAdd2Zip]) == false))
            {
                AddressData add2 = new AddressData
                {
                    Line1 = (string.IsNullOrEmpty(val[row,_ImportModel.colAdd2L1])) ? null : val[row, _ImportModel.colAdd2L1].Trim(),
                    Line2 = (string.IsNullOrEmpty(val[row,_ImportModel.colAdd2L2])) ? null : val[row, _ImportModel.colAdd2L2].Trim(),
                    Line3 = (string.IsNullOrEmpty(val[row, _ImportModel.colAdd2L3])) ? null : val[row, _ImportModel.colAdd2L3].Trim(),
                    City = (string.IsNullOrEmpty(val[row, _ImportModel.colAdd2City])) ? null : val[row, _ImportModel.colAdd2City].Trim(),
                    PostalCode = (string.IsNullOrEmpty(val[row, _ImportModel.colAdd2Zip])) ? null : val[row, _ImportModel.colAdd2Zip].Trim(),
                    OptOut = false
                };

                if (String.Compare(val[row, _ImportModel.colAdd2Pref].Trim(), "true", true) == 0)
                {
                    add2.Preferred = true;
                }
                else
                    add2.Preferred = false;

                string stateTrim = (string.IsNullOrEmpty(val[row, _ImportModel.colAdd2St])) ? null : val[row, _ImportModel.colAdd2St].Trim();
                foreach (StateData st in statesLookUp)
                {
                    if ((st.Name == stateTrim)
                        || (st.Code == stateTrim))
                    {
                        add2.StateId = st.Id;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(val[row, _ImportModel.colAdd2Type]) == false)
                {
                    foreach (CommTypeData c in typesLookUp)
                    {
                        if (String.Compare(val[row, _ImportModel.colAdd2Type].Trim(), c.Name, true) == 0)
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
        #endregion

        public void LoadLookUps()
        {
            //modes
            Uri modesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/commmodes?UserId={4}",
                                                    _formPatientsImport.TextBoxURL,
                                                    context,
                                                    version,
                                                    _formPatientsImport.ContractNumber,
                                                    _headerUserId));
            HttpClient modesClient = GetHttpClient(modesUri);

            GetAllCommModesDataRequest modesRequest = new GetAllCommModesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = _formPatientsImport.ContractNumber
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
                                                    _formPatientsImport.TextBoxURL,
                                                    context,
                                                    version,
                                                    _formPatientsImport.ContractNumber,
                                                    _headerUserId));
            HttpClient typesClient = GetHttpClient(typesUri);

            GetAllCommTypesDataRequest typesRequest = new GetAllCommTypesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = _formPatientsImport.ContractNumber
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
                                                   _formPatientsImport.TextBoxURL,
                                                    context,
                                                    version,
                                                    _formPatientsImport.ContractNumber,
                                                    _headerUserId));
            HttpClient statesClient = GetHttpClient(statesUri);

            GetAllStatesDataRequest statesRequest = new GetAllStatesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = _formPatientsImport.ContractNumber
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
                                                    _formPatientsImport.TextBoxURL,
                                                    context,
                                                    version,
                                                    _formPatientsImport.ContractNumber,
                                                    _headerUserId));
            HttpClient zonesClient = GetHttpClient(zonesUri);

            GetAllTimeZonesDataRequest zonesRequest = new GetAllTimeZonesDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = _formPatientsImport.ContractNumber
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
                                                    _formPatientsImport.TextBoxURL,
                                                    context,
                                                    version,
                                                    _formPatientsImport.ContractNumber,
                                                    _headerUserId));
            HttpClient zoneClient = GetHttpClient(zoneUri);

            GetTimeZoneDataRequest zoneRequest = new GetTimeZoneDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = _formPatientsImport.ContractNumber
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
                                                    _formPatientsImport.TextBoxURL,
                                                    context,
                                                    version,
                                                    _formPatientsImport.ContractNumber,
                                                    _headerUserId));
            HttpClient careMemberClient = GetHttpClient(careMemberUri);

            GetLookUpsDataRequest careMemberRequest = new GetLookUpsDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = _formPatientsImport.ContractNumber
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

        public void LoadSystems()
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/System", "GET")]
            Uri patientsystemDDUri = new Uri(string.Format("{0}/PatientSystem/{1}/{2}/{3}/System?UserId={4}",
                                                    _formPatientsImport.TextBoxURL,
                                                    context,
                                                    version,
                                                    _formPatientsImport.ContractNumber,
                                                    _headerUserId));
            HttpClient client = GetHttpClient(patientsystemDDUri);

            GetSystemsDataRequest systemDataRequest = new GetSystemsDataRequest
            {
                Version = version,
                Context = context,
                ContractNumber = _formPatientsImport.ContractNumber
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

        #region getuserbycontact
        public void GetUserByContactId()
        {
            
        }
        #endregion

        #region importcsvfile

        public void OpenFileDialog(string filename, ProgressBar progressBar1, OpenFileDialog openFileDialog1)
        {
            try
            {
                progressBar1.Value = 0;
                if (openFileDialog1.CheckFileExists)
                {
                    //_ImportModel.filename = openFileDialog1.FileName;
                    //_formPatientsImport.TextBoxFileName = filename;
                    using (TextFieldParser parser = new TextFieldParser(filename))
                    {

                        String[] values = File.ReadAllText(@filename).Split(',');
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        while (!parser.EndOfData)
                        {
                            string[] line = parser.ReadFields();
                            ListViewItem lvi = new ListViewItem(line[_ImportModel.colFirstN].Trim());
                            for (int i = 1; i < line.Count(); i++)
                            {
                                lvi.SubItems.Add(line[i].Trim());
                            }
                            _newdateofbirth = lvi.SubItems[_ImportModel.colDB].Text;

                            //Check for required fields
                            if (lvi.SubItems[_ImportModel.colFirstN].Text == "" || lvi.SubItems[_ImportModel.colLastN].Text == ""
                                || lvi.SubItems[_ImportModel.colGen].Text == "" || NewDateofBirth == "")
                            {
                                _formPatientsImport.SetListViewItemSkipped(lvi);
                            }
                            else
                            {
                                var patientDictKey = string.Format("{0}{1}{2}", lvi.SubItems[_ImportModel.colFirstN].Text,
                                    lvi.SubItems[_ImportModel.colLastN].Text, NewDateofBirth);
                                if (patientDictionary.ContainsKey(patientDictKey))
                                {
                                    patientDictionary[patientDictKey].importOperation = ImportOperation.SKIPPED;
                                    SetPatientDictionarySkipped(patientDictKey,
                                            string.Format("The following Patient data is duplicated. Only one operation is allowed per patient. Check patient: " + String.Join(",", line)));
                                    _formPatientsImport.SetListViewItemSkipped(lvi);
                                    //int n1 = lvi.Index;
                                    //listView1.Items.RemoveAt(n1);
                                    //lvi.ToolTipText = "The following Patient data is duplicated. Only one operation is allowed per patient. Check patient: " + String.Join(",", line);
                                }
                                else
                                {
                                    patientDictionary.Add(patientDictKey, new ImportData());
                                }
                            }
                            _formPatientsImport.PatientListView.Items.Add(lvi);
                        }
                    }
                }

                //ListView1List = listView1.Items.ToDictionary(key => key.Value, item => item.Text);
                //ListView1List = PatientListView.Items.Cast<ListViewItem>().Select(key => key.ToString(), item => item.Text).ToList();
                _formPatientsImport.ImportButton.Enabled = patientDictionary.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //chkSelectAll.Text = string.Format("Select All ({0} Patients)", patientDictionary.Count);
                //lblStatus.Text = "Select Individuals to Import...";
            }
        }
        private void SetPatientDictionarySkipped(string dictionaryKey, string skippedMessage)
        {
            patientDictionary[dictionaryKey].skipped = true;
            patientDictionary[dictionaryKey].failedMessage = skippedMessage;
        }
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

    }
}
