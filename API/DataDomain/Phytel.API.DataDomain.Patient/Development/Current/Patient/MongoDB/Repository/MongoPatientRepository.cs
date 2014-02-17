using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MB = MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Phytel.API.Common.Format;
using MongoDB.Driver.Builders;
using Phytel.API.DataDomain.PatientSystem.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System.Configuration;
using Phytel.API.DataDomain.Patient.MongoDB.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.LookUp;
using Phytel.API.Common.CustomObjects;

namespace Phytel.API.DataDomain.Patient
{
    public class MongoPatientRepository<T> : IPatientRepository<T>
    {
        private string _dbName = string.Empty;
        private List<IdNamePair> modesLookUp = new List<IdNamePair>();
        private List<CommTypeData> typesLookUp = new List<CommTypeData>();
        private List<LookUp.DTO.LanguageData> langLookUp = new List<LookUp.DTO.LanguageData>();
        private List<StateData> stateLookUp = new List<StateData>();
        private List<IdNamePair> timesofDays = new List<IdNamePair>();
        private List<TimeZoneData> timeZones = new List<TimeZoneData>();
        private TimeZoneData timeZone = new TimeZoneData();
        
        #region endpoint addresses
        protected static readonly string DDPatientSystemUrl = ConfigurationManager.AppSettings["DDPatientSystemServiceUrl"];
        protected static readonly string DDContactUrl = ConfigurationManager.AppSettings["DDContactServiceUrl"];
        protected static readonly string DDLookUpUrl = ConfigurationManager.AppSettings["DDLookUpServiceUrl"];
        #endregion

        public MongoPatientRepository(string contractDBName)
        {
            _dbName = contractDBName;

            IRestClient modesClient = new JsonServiceClient();
            GetAllCommModesDataRequest modeRequest = new GetAllCommModesDataRequest();
            GetAllCommModesDataResponse modeResponse = modesClient.Get<GetAllCommModesDataResponse>(string.Format("{0}/{1}/{2}/{3}/commmodes",
                                                                                            DDLookUpUrl,
                                                                                            "NG",
                                                                                            "v1",
                                                                                            "InHealth001"));
            modesLookUp = modeResponse.CommModes;

            IRestClient typeClient = new JsonServiceClient();
            GetAllCommTypesDataRequest typeRequest = new GetAllCommTypesDataRequest();
            GetAllCommTypesDataResponse typeResponse = typeClient.Get<GetAllCommTypesDataResponse>(string.Format("{0}/{1}/{2}/{3}/commtypes",
                                                                                            DDLookUpUrl,
                                                                                            "NG",
                                                                                            "v1",
                                                                                            "InHealth001"));
            typesLookUp = typeResponse.CommTypes;

            IRestClient langClient = new JsonServiceClient();
            GetAllLanguagesDataRequest langRequest = new GetAllLanguagesDataRequest();
            GetAllLanguagesDataResponse langResponse = typeClient.Get<GetAllLanguagesDataResponse>(string.Format("{0}/{1}/{2}/{3}/languages",
                                                                                            DDLookUpUrl,                                                                      
                                                                                            "NG",
                                                                                            "v1",
                                                                                            "InHealth001"));
            langLookUp = langResponse.Languages;

            IRestClient stateClient = new JsonServiceClient();
            GetAllStatesDataRequest stateRequest = new GetAllStatesDataRequest();
            GetAllStatesDataResponse stateResponse = stateClient.Get<GetAllStatesDataResponse>(string.Format("{0}/{1}/{2}/{3}/states",
                                                                                            DDLookUpUrl,
                                                                                            "NG",
                                                                                            "v1",
                                                                                            "InHealth001"));
            stateLookUp = stateResponse.States;

            IRestClient daysClient = new JsonServiceClient();
            GetAllTimesOfDaysDataRequest daysRequest = new GetAllTimesOfDaysDataRequest();
            GetAllTimesOfDaysDataResponse daysResponse = daysClient.Get<GetAllTimesOfDaysDataResponse>(string.Format("{0}/{1}/{2}/{3}/timesOfDays",
                                                                                            DDLookUpUrl,
                                                                                            "NG",
                                                                                            "v1",
                                                                                            "InHealth001"));
            timesofDays = daysResponse.TimesOfDays;

            IRestClient zonesClient = new JsonServiceClient();
            GetAllTimeZonesDataRequest zonesRequest = new GetAllTimeZonesDataRequest();
            GetAllTimeZonesDataResponse zonesResponse = zonesClient.Get<GetAllTimeZonesDataResponse>(string.Format("{0}/{1}/{2}/{3}/timeZones",
                                                                                            DDLookUpUrl,
                                                                                            "NG",
                                                                                            "v1",
                                                                                            "InHealth001"));
            timeZones = zonesResponse.TimeZones;

            IRestClient zoneClient = new JsonServiceClient();
            GetTimeZoneDataRequest zoneRequest = new GetTimeZoneDataRequest();
            GetTimeZoneDataResponse zoneResponse = zoneClient.Get<GetTimeZoneDataResponse>(string.Format("{0}/{1}/{2}/{3}/TimeZone/Default",
                                                                                            DDLookUpUrl,
                                                                                            "NG",
                                                                                            "v1",
                                                                                            "InHealth001"));
            timeZone = zoneResponse.TimeZone;
        }

        public object Insert(object newEntity)
        {
            try
            {
                //Patient
                PutPatientDataRequest request = newEntity as PutPatientDataRequest;
                MEPatient patient = null;
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    //Does the patient exist?
                    IMongoQuery query = Query.And(
                                    Query.EQ(MEPatient.FirstNameProperty, request.FirstName),
                                    Query.EQ(MEPatient.LastNameProperty, request.LastName),
                                    //Query.EQ(MEPatient.SSNProperty, request.SSN),
                                    Query.EQ(MEPatient.DOBProperty, request.DOB));

                    patient = ctx.Patients.Collection.FindOneAs<MEPatient>(query);
                    MongoCohortPatientViewRepository<T> repo = new MongoCohortPatientViewRepository<T>(_dbName);
                    if (patient != null)
                    {
                        //Got one, update it and save it here..
                        //Patient
                        patient.FirstName = request.FirstName;
                        patient.LastName = request.LastName;
                        patient.MiddleName = request.MiddleName;
                        patient.Suffix = request.Suffix;
                        patient.PreferredName = request.PreferredName;
                        patient.Gender = request.Gender;
                        patient.DOB = request.DOB;
                        //patient.SSN = request.SSN;
                        patient.Version = request.Version;
                        //UpdatedBy = security token user id;
                        patient.LastUpdatedOn = System.DateTime.Now;
                        ctx.Patients.Collection.Save(patient);

                        //CohortPatientView
                        IRestClient updateCohortClient = new JsonServiceClient();
                        GetCohortPatientViewRequest updateCohortRequest = new GetCohortPatientViewRequest
                        {
                            PatientID = patient.Id.ToString(),
                            Context = request.Context,
                            Version = request.Version,
                            ContractNumber = request.ContractNumber
                        };
                        GetCohortPatientViewResponse updateCohortResponse = updateCohortClient.Get<GetCohortPatientViewResponse>(string.Format("{0}/{1}/{2}/{3}/patient/{4}/cohortpatientview/",
                                                                                                                            "http://localhost:8888/Patient",
                                                                                                                            updateCohortRequest.Context,
                                                                                                                            updateCohortRequest.Version,
                                                                                                                            updateCohortRequest.ContractNumber,
                                                                                                                            updateCohortRequest.PatientID));
                        List<SearchFieldData> updateData = new List<SearchFieldData>();
                        updateData.Add(new SearchFieldData { Active = true, FieldName = "FN", Value = request.FirstName });
                        updateData.Add(new SearchFieldData { Active = true, FieldName = "LN", Value = request.LastName });
                        updateData.Add(new SearchFieldData { Active = true, FieldName = "G", Value = request.Gender.ToUpper() });
                        updateData.Add(new SearchFieldData { Active = true, FieldName = "DOB", Value = request.DOB });
                        updateData.Add(new SearchFieldData { Active = true, FieldName = "MN", Value = request.MiddleName });
                        updateData.Add(new SearchFieldData { Active = true, FieldName = "SFX", Value = request.Suffix });
                        updateData.Add(new SearchFieldData { Active = true, FieldName = "PN", Value = request.PreferredName });
                        //updateData.Add(new SearchFieldData {Active = true, FieldName = "SSN", Value = patient.SSN });

                        updateCohortResponse.CohortPatientView.LastName = request.LastName;
                        updateCohortResponse.CohortPatientView.SearchFields = updateData;
                        ctx.CohortPatientViews.Collection.Save(updateCohortResponse.CohortPatientView);

                        //PatientSystem

                        //Contact

                        //timezone
                        TimeZoneData updateZone = new TimeZoneData();
                        if (request.TimeZone != null)
                        {
                            foreach (TimeZoneData t in timeZones)
                            {
                                string[] zones = t.Name.Split(" ".ToCharArray());
                                if (request.TimeZone == zones[0])
                                {
                                    updateZone.Id = t.Id;
                                }
                            }
                        }
                        else
                        {
                            updateZone.Id = timeZone.Id;
                        }

                        List<CommModeData> updateModes = new List<CommModeData>();
                        List<PhoneData> updatePhones = new List<PhoneData>();
                        List<AddressData> updateAddresses = new List<AddressData>();
                        List<EmailData> updateEmails = new List<EmailData>();

                        //modes
                        if (modesLookUp != null && modesLookUp.Count > 0)
                        {
                            foreach (IdNamePair l in modesLookUp)
                            {
                                updateModes.Add(new CommModeData { ModeId = l.Id, OptOut = false, Preferred = false });
                            }
                        }


                        //phones
                        if (request.Phone1 != "")
                        {
                            PhoneData phone1 = new PhoneData
                            {
                                Number = Convert.ToInt64(request.Phone1),
                                PhonePreferred = request.Phone1Preferred,
                                OptOut = false
                            };
                            foreach (CommTypeData c in typesLookUp)
                            {
                                if (request.Phone1Type == c.Name)
                                {
                                    phone1.TypeId = c.Id;
                                }
                            }
                            updatePhones.Add(phone1);
                        }

                        if (request.Phone2 != "")
                        {
                            PhoneData phone2 = new PhoneData
                            {
                                Number = Convert.ToInt64(request.Phone2),
                                PhonePreferred = request.Phone2Preferred,
                                OptOut = false
                            };
                            foreach (CommTypeData c in typesLookUp)
                            {
                                if (request.Phone2Type == c.Name)
                                {
                                    phone2.TypeId = c.Id;
                                }
                            }
                            updatePhones.Add(phone2);
                        }

                        //emails
                        if (request.Email1 != "")
                        {
                            EmailData email1 = new EmailData
                            {
                                Text = request.Email1,
                                Preferred = request.Email1Preferred,
                                OptOut = false,
                            };
                            foreach (CommTypeData c in typesLookUp)
                            {
                                if (request.Email1Type == c.Name)
                                {
                                    email1.TypeId = c.Id;
                                }
                            }
                            updateEmails.Add(email1);
                        }

                        if (request.Email2 != "")
                        {
                            EmailData email2 = new EmailData
                            {
                                Text = request.Email2,
                                Preferred = request.Email2Preferred,
                                OptOut = false,
                            };
                            foreach (CommTypeData c in typesLookUp)
                            {
                                if (request.Email2Type == c.Name)
                                {
                                    email2.TypeId = c.Id;
                                }
                            }
                            updateEmails.Add(email2);
                        }

                        //addresses
                        if (request.Address1Line1 != "")
                        {
                            AddressData add1 = new AddressData
                            {
                                Line1 = request.Address1Line1,
                                Line2 = request.Address1Line2,
                                Line3 = request.Address1Line3,
                                City = request.Address1City,
                                PostalCode = request.Address1Zip,
                                Preferred = request.Address1Preferred,
                                OptOut = false
                            };
                            foreach (StateData st in stateLookUp)
                            {
                                if (st.Name == request.Address1State)
                                {
                                    add1.StateId = st.Id;
                                }
                            }
                            foreach (CommTypeData c in typesLookUp)
                            {
                                if (request.Address1Type == c.Name)
                                {
                                    add1.TypeId = c.Id;
                                }
                            }
                            updateAddresses.Add(add1);
                        }

                        if (request.Address2Line1 != "")
                        {
                            AddressData add2 = new AddressData
                            {
                                Line1 = request.Address2Line1,
                                Line2 = request.Address2Line2,
                                Line3 = request.Address2Line3,
                                City = request.Address2City,
                                PostalCode = request.Address2Zip,
                                Preferred = request.Address2Preferred,
                                OptOut = false
                            };
                            foreach (StateData st in stateLookUp)
                            {
                                if (st.Name == request.Address1State)
                                {
                                    add2.StateId = st.Id;
                                }
                            }
                            foreach (CommTypeData c in typesLookUp)
                            {
                                if (request.Address2Type == c.Name)
                                {
                                    add2.TypeId = c.Id;
                                }
                            }
                            updateAddresses.Add(add2);
                        }

                        //Test user ID
                        int updateUser = 123456789;
                        IRestClient updateClient = new JsonServiceClient();
                        GetContactDataRequest getContactRequest = new GetContactDataRequest();
                        GetContactDataResponse getContactResponse = updateClient.Get<GetContactDataResponse>(string.Format("{0}/{1}/{2}/{3}/Patient/{4}/Contact",
                                                                                                        DDContactUrl,
                                                                                                        "NG",
                                                                                                        "v1",
                                                                                                        "InHealth001",
                                                                                                        patient.Id.ToString()));

                        PutUpdateContactDataRequest updateRequest = new PutUpdateContactDataRequest
                        {
                            ContactId = getContactResponse.Contact.ContactId,
                            Modes = updateModes,
                            Phones = updatePhones,
                            Emails = updateEmails,
                            Addresses = updateAddresses,
                            TimeZoneId = updateZone.Id.ToString(),
                            UserId = updateUser.ToString(),
                            Context = request.Context,
                            ContractNumber = request.ContractNumber,
                            Version = request.Version
                        };

                        PutUpdateContactDataResponse updateResponse = updateClient.Put<PutUpdateContactDataResponse>(string.Format("{0}/{1}/{2}/{3}/Patient/Contact",
                                                                                                                DDContactUrl,
                                                                                                                updateRequest.Context,
                                                                                                                updateRequest.Version,
                                                                                                                updateRequest.ContractNumber), updateRequest);

                    }
                    else
                    {
                        patient = new MEPatient
                        {
                            Id = ObjectId.GenerateNewId(),
                            FirstName = request.FirstName,
                            LastName = request.LastName,
                            MiddleName = request.MiddleName,
                            Suffix = request.Suffix,
                            PreferredName = request.PreferredName,
                            Gender = request.Gender,
                            DOB = request.DOB,
                            //SSN = request.SSN,
                            Version = request.Version,
                            //UpdatedBy = security token user id,
                            TTLDate = null,
                            DeleteFlag = false,
                            LastUpdatedOn = System.DateTime.Now
                        };

                        ctx.Patients.Collection.Insert(patient);

                        List<SearchFieldData> data = new List<SearchFieldData>();
                        data.Add(new SearchFieldData { Active = true, FieldName = "FN", Value = patient.FirstName });
                        data.Add(new SearchFieldData { Active = true, FieldName = "LN", Value = patient.LastName });
                        data.Add(new SearchFieldData { Active = true, FieldName = "G", Value = patient.Gender.ToUpper() });
                        data.Add(new SearchFieldData { Active = true, FieldName = "DOB", Value = patient.DOB });
                        data.Add(new SearchFieldData { Active = true, FieldName = "MN", Value = patient.MiddleName });
                        data.Add(new SearchFieldData { Active = true, FieldName = "SFX", Value = patient.Suffix });
                        data.Add(new SearchFieldData { Active = true, FieldName = "PN", Value = patient.PreferredName });
                        //data.Add(new SearchFieldData {Active = true, FieldName = "SSN", Value = patient.SSN });

                        PutCohortPatientViewDataRequest cohortPatientRequest = new PutCohortPatientViewDataRequest
                        {
                            PatientID = patient.Id.ToString(),
                            LastName = patient.LastName,
                            SearchFields = data,
                            Version = patient.Version,
                            Context = request.Context,
                            ContractNumber = request.ContractNumber
                        };

                        
                        repo.Insert(cohortPatientRequest);

                        //PatientSystem
                        if (string.IsNullOrEmpty(request.SystemID) == false)
                        {
                            PutPatientSystemDataRequest systemRequest = new PutPatientSystemDataRequest
                            {
                                SystemID = request.SystemID,
                                SystemName = request.SystemName,
                                PatientID = patient.Id.ToString()
                            };



                            IRestClient sysClient = new JsonServiceClient();
                            PutPatientSystemDataResponse sysResponse = sysClient.Put<PutPatientSystemDataResponse>(string.Format("{0}/{1}/{2}/{3}/PatientSystem",
                                                                                                    DDPatientSystemUrl,
                                                                                                    "NG",
                                                                                                    request.Version,
                                                                                                    request.ContractNumber), systemRequest);


                            patient.DisplayPatientSystemID = ObjectId.Parse(sysResponse.PatientSystemId);

                            ctx.Patients.Collection.Save(patient);
                            
                        }
                    }

                    //Contact

                    //timezone
                    TimeZoneData tZone = new TimeZoneData();
                    if (request.TimeZone != null)
                    {
                        foreach (TimeZoneData t in timeZones)
                        {
                            string[] zones = t.Name.Split(" ".ToCharArray());
                            if (request.TimeZone == zones[0])
                            {
                                tZone.Id = t.Id;
                            }
                        }
                    }
                    else
                    {
                        tZone.Id = timeZone.Id;
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
                    if (request.Phone1 != "")
                    {
                        PhoneData phone1 = new PhoneData
                        {
                            Number = Convert.ToInt64(request.Phone1),
                            PhonePreferred = request.Phone1Preferred,
                            OptOut = false
                        };
                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (request.Phone1Type == c.Name)
                            {
                                phone1.TypeId = c.Id;
                            }
                        }
                        phones.Add(phone1);
                    }

                    if (request.Phone2 != "")
                    {
                        PhoneData phone2 = new PhoneData
                        {
                            Number = Convert.ToInt64(request.Phone2),
                            PhonePreferred = request.Phone2Preferred,
                            OptOut = false
                        };
                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (request.Phone2Type == c.Name)
                            {
                                phone2.TypeId = c.Id;
                            }
                        }
                        phones.Add(phone2);
                    }

                    //emails
                    if (request.Email1 != "")
                    {
                        EmailData email1 = new EmailData
                        {
                            Text = request.Email1,
                            Preferred = request.Email1Preferred,
                            OptOut = false,
                        };
                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (request.Email1Type == c.Name)
                            {
                                email1.TypeId = c.Id;
                            }
                        }
                        emails.Add(email1);
                    }

                    if (request.Email2 != "")
                    {
                        EmailData email2 = new EmailData
                        {
                            Text = request.Email2,
                            Preferred = request.Email2Preferred,
                            OptOut = false,
                        };
                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (request.Email2Type == c.Name)
                            {
                                email2.TypeId = c.Id;
                            }
                        }
                        emails.Add(email2);
                    }

                    //addresses
                    if (request.Address1Line1 != "")
                    {
                        AddressData add1 = new AddressData
                        {
                            Line1 = request.Address1Line1,
                            Line2 = request.Address1Line2,
                            Line3 = request.Address1Line3,
                            City = request.Address1City,
                            PostalCode = request.Address1Zip,
                            Preferred = request.Address1Preferred,
                            OptOut = false
                        };
                        foreach (StateData st in stateLookUp)
                        {
                            if (st.Name == request.Address1State)
                            {
                                add1.StateId = st.Id;
                            }
                        }
                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (request.Address1Type == c.Name)
                            {
                                add1.TypeId = c.Id;
                            }
                        }
                        addresses.Add(add1);
                    }

                    if (request.Address2Line1 != "")
                    {
                        AddressData add2 = new AddressData
                        {
                            Line1 = request.Address2Line1,
                            Line2 = request.Address2Line2,
                            Line3 = request.Address2Line3,
                            City = request.Address2City,
                            PostalCode = request.Address2Zip,
                            Preferred = request.Address2Preferred,
                            OptOut = false
                        };
                        foreach (StateData st in stateLookUp)
                        {
                            if (st.Name == request.Address1State)
                            {
                                add2.StateId = st.Id;
                            }
                        }
                        foreach (CommTypeData c in typesLookUp)
                        {
                            if (request.Address2Type == c.Name)
                            {
                                add2.TypeId = c.Id;
                            }
                        }
                        addresses.Add(add2);
                    }

                    //Test user ID
                    int testUser = 123456789;

                    PutContactDataRequest contactRequest = new PutContactDataRequest
                    {
                        PatientId = patient.Id.ToString(),
                        Modes = modes,
                        TimeZoneId = tZone.Id,
                        Phones = phones,
                        Emails = emails,
                        Addresses = addresses,
                        Version = patient.Version,
                        Context = request.Context,
                        ContractNumber = request.ContractNumber,
                        UserId = testUser.ToString()
                    };

                    IRestClient contactClient = new JsonServiceClient();
                    PutContactDataResponse contactResponse = contactClient.Put<PutContactDataResponse>(string.Format("{0}/{1}/{2}/{3}/Patient/Contact/{4}",
                                                                                            DDContactUrl,
                                                                                            "NG",
                                                                                            contactRequest.Version,
                                                                                            contactRequest.ContractNumber,
                                                                                            contactRequest.PatientId), contactRequest);



                    return new PutPatientDataResponse
                    {
                        Id = patient.Id.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            DTO.PatientData patient = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                patient = (from p in ctx.Patients
                           where p.Id == ObjectId.Parse(entityID)
                           select new DTO.PatientData
                            {
                                ID = p.Id.ToString(),
                                DOB = CommonFormatter.FormatDateOfBirth(p.DOB),
                                FirstName = p.FirstName,
                                Gender = p.Gender,
                                LastName = p.LastName,
                                PreferredName = p.PreferredName,
                                MiddleName = p.MiddleName,
                                Suffix = p.Suffix,
                                PriorityData = (DTO.PriorityData)((int)p.Priority),
                                DisplayPatientSystemID = p.DisplayPatientSystemID.ToString(),
                                CareTeamData = getCareTeam(p.CareTeam)
                            }).FirstOrDefault();
            }
            return patient;
        }

        private List<CareTeamMemberData> getCareTeam(List<CareTeamMember> list)
        {
            List<CareTeamMemberData> careTeam = null;
            if(list != null)
            {
                careTeam = new List<CareTeamMemberData>();
                foreach(CareTeamMember meCtm in list)
                {
                    CareTeamMemberData ctm = new CareTeamMemberData
                    { 
                        ContactId = meCtm.ContactId.ToString(),
                        Primary  = meCtm.Primary,
                        Type = DTOUtils.ToFriendlyString(meCtm.Type)
                    };
                    careTeam.Add(ctm);
                }
            }
            return careTeam;
        }

        public object FindByID(string entityId, string userId)
        {
            DTO.PatientData patient = null;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                patient = (from p in ctx.Patients
                           where p.Id == ObjectId.Parse(entityId)
                           select new DTO.PatientData
                           {
                               ID = p.Id.ToString(),
                               DOB = CommonFormatter.FormatDateOfBirth(p.DOB),
                               FirstName = p.FirstName,
                               Gender = p.Gender,
                               LastName = p.LastName,
                               PreferredName = p.PreferredName,
                               MiddleName = p.MiddleName,
                               Suffix = p.Suffix,
                               PriorityData = (DTO.PriorityData)((int)p.Priority),
                               DisplayPatientSystemID = p.DisplayPatientSystemID.ToString(),
                               Flagged = GetFlaggedStatus(entityId, userId),
                               CareTeamData = getCareTeam(p.CareTeam)
                           }).FirstOrDefault();
            }
            return patient;
        }

        private bool GetFlaggedStatus(string entityId, string userId)
        {
            bool result = false;
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                var patientUsr = FindPatientUser(entityId, userId, ctx);

                if (patientUsr != null)
                {
                    result = patientUsr.Flagged;
                }
            }
            return result;
        }

        private static MEPatientUser FindPatientUser(string entityId, string userId, PatientMongoContext ctx)
        {
            var findQ = MB.Query.And(
                MB.Query<MEPatientUser>.EQ(b => b.PatientId, ObjectId.Parse(entityId)),
                MB.Query<MEPatientUser>.EQ(b => b.UserId, userId)
            );

            var patientUsr = ctx.PatientUsers.Collection.Find(findQ).FirstOrDefault();
            return patientUsr;
        }

        public List<PatientData> Select(string query, string[] filterData, string querySort, int skip, int take)
        {
            throw new NotImplementedException();
            ///* Query without filter:
            // *  { sf: { $elemMatch : {'val':'528a66f4d4332317acc5095f', 'fldn':'Problem', 'act':true}}}
            // * 
            // * Query with single field filter:
            // *  { $and : [ 
            // *      { sf: { $elemMatch : {'val':'528a66f4d4332317acc5095f', 'fldn':'Problem', 'act':true}}}, 
            // *      { $or : [ 
            // *          { sf: { $elemMatch : {'val':/^<Single Field Text Here>/i, 'fldn':'FN'}}}, 
            // *          { sf: { $elemMatch : {'val':/^<Single Field Text Here>/i, 'fldn':'LN'}}}, 
            // *          { sf: { $elemMatch : {'val':/^<Single Field Text Here>/i, 'fldn':'PN'}}} 
            // *          ] 
            // *      } 
            // *   ] 
            // * }
            // * 
            // * Query with double field filter:
            // *  { $and : [ 
            // *      { sf: { $elemMatch : {'val':'528a66f4d4332317acc5095f', 'fldn':'Problem', 'act':true}}}, 
            // *      { $and : [ 
            // *          { $or : [ 
            // *              { sf : { $elemMatch: {'val':/^<First Field Text Here>/i, 'fldn':'FN'}}}, 
            // *              { sf : { $elemMatch: {'val':/^<First Field Text Here>/i, 'fldn':'PN'}}}
            // *            ]
            // *          },	
            // *          { sf: { $elemMatch : {'val':/^<Second Field Text Here>/i, 'fldn':'LN'}}}
            // *        ] 
            // *      } 
            // *    ] 
            // *  }
            // * 
            //*/

            //try
            //{
            //    string jsonQuery = string.Empty;
            //    string queryName = "TBD"; //Pass this into the method call
            //    string redisKey = string.Empty;

            //    if (filterData[0].Trim() == string.Empty)
            //    {
            //        jsonQuery = query;
            //        redisKey = string.Format("{0}{1}{2}", queryName, skip, take);
            //    }
            //    else
            //    {
            //        if (filterData[1].Trim() == string.Empty)
            //        {
            //            redisKey = string.Format("{0}{1}{2}{3}", queryName, filterData[0].Trim(), skip, take);

            //            jsonQuery = "{ $and : [ ";
            //            jsonQuery += string.Format("{0},  ", query);
            //            jsonQuery += "{ $or : [  ";
            //            jsonQuery += "{ sf: { $elemMatch : {'val':/^";
            //            jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
            //            jsonQuery += "'" + SearchField.FieldNameProperty + "':'FN'}}},  ";
            //            jsonQuery += "{ sf: { $elemMatch : {'" + SearchField.ValueProperty + "':/^";
            //            jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
            //            jsonQuery += "'" + SearchField.FieldNameProperty + "':'LN'}}},  ";
            //            jsonQuery += "{ sf: { $elemMatch : {'" + SearchField.ValueProperty + "':/^";
            //            jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
            //            jsonQuery += "'" + SearchField.FieldNameProperty + "':'PN'}}}]}]}";
            //        }
            //        else
            //        {
            //            redisKey = string.Format("{0}{1}{2}{3}{4}", queryName, filterData[0].Trim(), filterData[1].Trim(), skip, take);

            //            jsonQuery = "{ $and : [ ";
            //            jsonQuery += string.Format("{0},  ", query);
            //            jsonQuery += "{ $and : [  ";
            //            jsonQuery += "{ $or : [  ";
            //            jsonQuery += "{ sf : { $elemMatch: {'" + SearchField.ValueProperty + "':/^";
            //            jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
            //            jsonQuery += "'" + SearchField.FieldNameProperty + "':'FN'}}},  ";
            //            jsonQuery += "{ sf : { $elemMatch: {'" + SearchField.ValueProperty + "':/^";
            //            jsonQuery += string.Format("{0}/i, ", filterData[0].Trim());
            //            jsonQuery += "'" + SearchField.FieldNameProperty + "':'PN'}}} ";
            //            jsonQuery += "]}, ";
            //            jsonQuery += "{ sf: { $elemMatch : {'" + SearchField.ValueProperty + "':/^";
            //            jsonQuery += string.Format("{0}/i, ", filterData[1].Trim());
            //            jsonQuery += "'" + SearchField.FieldNameProperty + "':'LN'}}}]}]}}";
            //        }
            //    }

            //    string redisClientIPAddress = System.Configuration.ConfigurationManager.AppSettings.Get("RedisClientIPAddress");

            //    List<PatientData> cohortPatientList = new List<PatientData>();
            //    ServiceStack.Redis.RedisClient client = null;

            //    //TODO: Uncomment the following 2 lines to turn Redis cache on
            //    //if(string.IsNullOrEmpty(redisClientIPAddress) == false)
            //    //    client = new ServiceStack.Redis.RedisClient(redisClientIPAddress);

            //    //If the redisKey is already in Cache (REDIS) get it from there, else re-query
            //    if (client != null && client.ContainsKey(redisKey))
            //    {
            //        //go get cohortPatientList from Redis using the redisKey now
            //        cohortPatientList = client.Get<List<PatientData>>(redisKey);
            //    }
            //    else
            //    {
            //        using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            //        {
            //            BsonDocument searchQuery = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(jsonQuery);
            //            QueryDocument queryDoc = new QueryDocument(searchQuery);
            //            SortByBuilder builder = PatientsUtils.BuildSortByBuilder(querySort);

            //            List<MECohortPatientView> meCohortPatients = ctx.CohortPatientViews.Collection.Find(queryDoc)
            //                .SetSortOrder(builder).SetSkip(skip).SetLimit(take).Distinct().ToList();

            //            if (meCohortPatients != null && meCohortPatients.Count > 0)
            //            {
            //                meCohortPatients.ForEach(delegate(MECohortPatientView pat)
            //                {
            //                    PatientData cohortPatient = new PatientData();
            //                    cohortPatient.ID = pat.PatientID.ToString();

            //                    foreach (SearchField sf in pat.SearchFields)
            //                    {
            //                        cohortPatient.FirstName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "FN").FirstOrDefault()).Value;
            //                        cohortPatient.LastName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "LN").FirstOrDefault()).Value;
            //                        cohortPatient.Gender = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "G").FirstOrDefault()).Value;
            //                        cohortPatient.DOB = CommonFormatter.FormatDateOfBirth(((SearchField)pat.SearchFields.Where(x => x.FieldName == "DOB").FirstOrDefault()).Value);
            //                        cohortPatient.MiddleName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "MN").FirstOrDefault()).Value;
            //                        cohortPatient.Suffix = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "SFX").FirstOrDefault()).Value;
            //                        cohortPatient.PreferredName = ((SearchField)pat.SearchFields.Where(x => x.FieldName == "PN").FirstOrDefault()).Value;
            //                    }
            //                    cohortPatientList.Add(cohortPatient);
            //                });
            //            }
            //        }
            //        //put cohortPatientList into cache using redisKey now
            //        if (client != null)
            //            client.Set<List<PatientData>>(redisKey, cohortPatientList);
            //    }
            //    return cohortPatientList;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
            //List<IMongoQuery> queries = new List<IMongoQuery>();

            //queries.Add(Query.EQ(MEPatient.FirstNameProperty, "Greg"));
            //queries.Add(Query.EQ(MEPatient.LastNameProperty, "Tony"));

            //IMongoQuery query2 = Query.And(queries);

            //IMongoQuery query = Query.Or(
            //    Query.EQ(MEPatient.FirstNameProperty, "Greg"),
            //    Query.EQ(MEPatient.LastNameProperty, "Tony"));
        }

        public GetPatientsDataResponse Select(string[] patientIds)
        {
            BsonValue[] bsv = new BsonValue[patientIds.Length];
            for (int i = 0; i < patientIds.Length; i++)
            {
                bsv[i] = ObjectId.Parse(patientIds[i]);
            }

            IMongoQuery query = MB.Query.In("_id", bsv);
            List<MEPatient> pr = null;
            List<DTO.PatientData> pResp = new List<DTO.PatientData>();
            using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
            {
                pr = ctx.Patients.Collection.Find(query).ToList();
                // convert to a PatientDetailsResponse
                foreach (MEPatient mp in pr)
                {
                    pResp.Add(new DTO.PatientData
                    {
                        ID = mp.Id.ToString(),
                        PreferredName = mp.PreferredName,
                        DOB = mp.DOB,
                        FirstName = mp.FirstName,
                        Gender = mp.Gender,
                        LastName = mp.LastName,
                        MiddleName = mp.MiddleName,
                        Suffix = mp.Suffix,
                        Version = mp.Version,
                        PriorityData = (PriorityData)((int)mp.Priority),
                        DisplayPatientSystemID = mp.DisplayPatientSystemID.ToString(),
                        CareTeamData = getCareTeam(mp.CareTeam)
                    });
                }
            }

            GetPatientsDataResponse pdResponse = new GetPatientsDataResponse();
            pdResponse.Patients = pResp;

            return pdResponse;
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public PutPatientPriorityResponse UpdatePriority(PutPatientPriorityRequest request)
        {
            PutPatientPriorityResponse response = new PutPatientPriorityResponse();
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    FindAndModifyResult result = ctx.Patients.Collection.FindAndModify(MB.Query.EQ(MEPatient.IdProperty, ObjectId.Parse(request.PatientId)), MB.SortBy.Null,
                                                new MB.UpdateBuilder().Set(MEPatient.PriorityProperty, (PriorityData)request.Priority).Set(MEPatient.UpdatedByProperty, request.UserId));
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PutPatientFlaggedResponse UpdateFlagged(PutPatientFlaggedRequest request)
        {
            PutPatientFlaggedResponse response = new PutPatientFlaggedResponse();
            try
            {
                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {

                    var patientUsr = FindPatientUser(request.PatientId, request.UserId, ctx);

                    if (patientUsr == null)
                    {
                        ctx.PatientUsers.Collection.Insert(new MEPatientUser
                        {
                            PatientId = ObjectId.Parse(request.PatientId),
                            UserId = request.UserId,
                            Flagged = Convert.ToBoolean(request.Flagged),
                            Version = "v1",
                            LastUpdatedOn = System.DateTime.UtcNow,
                            DeleteFlag = false,
                             UpdatedBy = request.UserId
                        });
                        response.flagged = Convert.ToBoolean(request.Flagged);
                    }
                    else
                    {

                        var pUQuery = new QueryDocument(MEPatientUser.IdProperty, patientUsr.Id);
                        var sortBy = new MB.SortByBuilder().Ascending("_id");
                        MB.UpdateBuilder updt = new MB.UpdateBuilder().Set(MEPatientUser.FlaggedProperty, Convert.ToBoolean(request.Flagged))
                            .Set(MEPatientUser.UpdatedByProperty, request.UserId);
                        var pt = ctx.PatientUsers.Collection.FindAndModify(pUQuery, sortBy, updt, true);
                        response.flagged = Convert.ToBoolean(request.Flagged);
                    }
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object Update(PutUpdatePatientDataRequest request)
        {
            PutUpdatePatientDataResponse response = new PutUpdatePatientDataResponse();
            try
            {
                if (request.UserId == null)
                    throw new ArgumentException("UserId is missing from the DataDomain request.");

                if (request.Priority == null)
                    throw new ArgumentException("Priority is missing from the DataDomain request.");

                using (PatientMongoContext ctx = new PatientMongoContext(_dbName))
                {
                    var pUQuery = new QueryDocument(MEPatient.IdProperty, ObjectId.Parse(request.Id));

                    MB.UpdateBuilder updt = new MB.UpdateBuilder();
                    if (request.FirstName != null)
                    {
                        if (request.FirstName == "\"\"" || (request.FirstName == "\'\'"))
                            updt.Set(MEPatient.FirstNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.FirstNameProperty, request.FirstName);
                    }
                    if (request.LastName != null)
                    {
                        if (request.LastName == "\"\"" || (request.LastName == "\'\'"))
                            updt.Set(MEPatient.LastNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.LastNameProperty, request.LastName);
                    }
                    if (request.MiddleName != null)
                    {
                        if (request.MiddleName == "\"\"" || (request.MiddleName == "\'\'"))
                            updt.Set(MEPatient.MiddleNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.MiddleNameProperty, request.MiddleName);
                    }
                    if (request.Suffix != null)
                    {
                        if (request.Suffix == "\"\"" || (request.Suffix == "\'\'"))
                            updt.Set(MEPatient.SuffixProperty, string.Empty);
                        else
                            updt.Set(MEPatient.SuffixProperty, request.Suffix);
                    }
                    if (request.PreferredName != null)
                    {
                        if (request.PreferredName == "\"\"" || (request.PreferredName == "\'\'"))
                            updt.Set(MEPatient.PreferredNameProperty, string.Empty);
                        else
                            updt.Set(MEPatient.PreferredNameProperty, request.PreferredName);
                    }
                    if (request.Gender != null)
                    {
                        if (request.Gender == "\"\"" || (request.Gender == "\'\'"))
                            updt.Set(MEPatient.GenderProperty, string.Empty);
                        else
                            updt.Set(MEPatient.GenderProperty, request.Gender);
                    }
                    if (request.DOB != null)
                    {
                        if (request.DOB == "\"\"" || (request.DOB == "\'\'"))
                            updt.Set(MEPatient.DOBProperty, string.Empty);
                        else
                            updt.Set(MEPatient.DOBProperty, request.DOB);
                    }
                    if (request.Version != null)
                    {
                        if ((request.Version == "\"\"") || (request.Version == "\'\'"))
                            updt.Set(MEPatient.VersionProperty, string.Empty);
                        else
                            updt.Set(MEPatient.VersionProperty, request.Version);
                    }
                    updt.Set("uon", System.DateTime.UtcNow);
                    updt.Set("pri", request.Priority);
                    updt.Set("uby", request.UserId);

                    var sortBy = new MB.SortByBuilder().Ascending("_id");
                    var pt = ctx.Patients.Collection.FindAndModify(pUQuery, sortBy, updt, true);

                    response.Id = request.Id;

                    // save to cohortuser collection
                    var findQ = MB.Query<MECohortPatientView>.EQ(b => b.PatientID, ObjectId.Parse(request.Id));
                    MECohortPatientView cPV = ctx.CohortPatientViews.Collection.Find(findQ).FirstOrDefault();
                    cPV.SearchFields.ForEach(s => UpdateProperty(request, s));
                    List<SearchField> sfs = cPV.SearchFields.ToList<SearchField>();

                    ctx.CohortPatientViews.Collection.Update(findQ, MB.Update.SetWrapped<List<SearchField>>("sf", sfs).Set(MECohortPatientView.LastNameProperty, request.LastName));
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateProperty(PutUpdatePatientDataRequest request, SearchField s)
        {
            if (s.FieldName.Equals("PN"))
            {
                if (request.PreferredName != null)
                {
                    if (request.PreferredName == "\"\"" || (request.PreferredName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.PreferredName;
                }
            }

            if (s.FieldName.Equals("SFX"))
            {
                if (request.Suffix != null)
                {
                    if (request.Suffix == "\"\"" || (request.Suffix == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.Suffix;
                }
            }

            if (s.FieldName.Equals("MN"))
            {
                if (request.MiddleName != null)
                {
                    if (request.MiddleName == "\"\"" || (request.MiddleName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.MiddleName;
                }
            }

            if (s.FieldName.Equals("DOB"))
            {
                if (request.DOB != null)
                {
                    if (request.DOB == "\"\"" || (request.DOB == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.DOB;
                }
            }

            if (s.FieldName.Equals("G"))
            {
                if (request.Gender != null)
                {
                    if (request.Gender == "\"\"" || (request.Gender == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.Gender;
                }
            }

            if (s.FieldName.Equals("LN"))
            {
                if (request.LastName != null)
                {
                    if (request.LastName == "\"\"" || (request.LastName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.LastName;
                }
            }
            if (s.FieldName.Equals("FN"))
            {
                if (request.FirstName != null)
                {
                    if (request.FirstName == "\"\"" || (request.FirstName == "\'\'"))
                        s.Value = string.Empty;
                    else
                        s.Value = request.FirstName;
                }
            }
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }


        public object Update(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
