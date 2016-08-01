using NGDataImport;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.Remoting.Contexts;

namespace ExplorysImport
{
    public class ExplorysImport
    {
        Importer import;

        public ExplorysImport(string txtURL, string context, double version, string contract, string _headerUserId)
        {
            import = new Importer(txtURL, context, version, contract, _headerUserId);
        }

        public void ImportFile(string excelFile)
        {
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFile + @";Extended Properties=""Excel 8.0;HDR=YES;""";

            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");

            using (DbConnection connection = factory.CreateConnection())
            {
                connection.ConnectionString = connectionString;

                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Address, [Birth Date], Gender, [Home Phone],  Name, [Work Phone] FROM [Patient Data$]";

                    connection.Open();

                    using (DbDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            #region Parse all needed data
                            //PatientData
                            string[] fullname = dr["Name"].ToString().Split(", ".ToCharArray());

                            string dob = dr["Birth Date"].ToString();
                            string firstname = fullname[2].ToString();
                            string lastname = fullname[0].ToString();
                            string gender = dr["Gender"].ToString();

                            //Contact Info
                            string newadd = dr["Address"].ToString().Replace("<br/>", "-");
                            string[] splitaddress = newadd.Split("-".ToCharArray());
                            string[] cityStateZip = splitaddress[1].Split(",".ToCharArray());
                            string[] stateZip = cityStateZip[1].Split(" ".ToCharArray());
                            
                            string homePhone = dr["Home Phone"].ToString().Replace("-", string.Empty);
                            string workPhone = dr["Work Phone"].ToString().Replace("-", string.Empty);
                            string line1 = splitaddress[0];
                            string city = cityStateZip[0].Trim();
                            string state = stateZip[1];
                            string zip = stateZip[2];
                            #endregion

                            
                            #region Insert Patient
                            //DOB = Birth Date
                            //FirstName = second part of Name(after , )
                            //Gender = Gender
                            //LastName = first part of Name (before ,)
                            PatientData patient = new PatientData
                            {
                                DOB = dob,
                                FirstName = firstname,
                                Gender = gender,
                                LastName = lastname
                            };
                            PutPatientDataRequest patientRequest = new PutPatientDataRequest
                            {
                                Patient = patient
                            };

                            PutPatientDataResponse responsePatient = import.InsertPatient(patientRequest);
                            #endregion

                            
                            #region Add Contact Info (home phone, work phone, address)
                            if (responsePatient.Id != null)
                            {
                                //timezone defaulting to central
                                TimeZoneData tZone = import.GetDefaultTimeZone();

                                List<IdNamePair> modesLookUp = import.GetModes();
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
                                if (!string.IsNullOrEmpty(homePhone))
                                {
                                    PhoneData home = new PhoneData
                                    {
                                        Number = Convert.ToInt64(homePhone),
                                        OptOut = false,
                                        PhonePreferred = true,
                                        TypeId = import.GetType("Home")
                                    };
                                    phones.Add(home);
                                }

                                if (!string.IsNullOrEmpty(workPhone))
                                {
                                    PhoneData work = new PhoneData
                                    {
                                        Number = Convert.ToInt64(workPhone),
                                        OptOut = false,
                                        PhonePreferred = true,
                                        TypeId = import.GetType("Work")
                                    };
                                    phones.Add(work);
                                }

                                //address
                                if (!string.IsNullOrEmpty(newadd))
                                {
                                    AddressData add1 = new AddressData
                                    {
                                        Line1 = line1,
                                        City = city,
                                        PostalCode = zip,
                                        OptOut = false,
                                        Preferred = true,
                                        StateId = import.GetState(state),
                                        TypeId = import.GetFirstTypeLookUp()
                                    };

                                    addresses.Add(add1);
                                }

                                //Contact
                                ContactData data = new ContactData {
                                    PatientId = responsePatient.Id,
                                    Modes = modes,
                                    TimeZoneId = tZone.Id,
                                    Phones = phones,
                                    Emails = emails,
                                    Addresses = addresses,
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
                            }
                            #endregion
                        }
                    }
                }
            }
        }
    }
}
