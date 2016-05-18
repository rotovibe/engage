using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Phytel.API.DataDomain.PatientSystem.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using Status = Phytel.API.DataDomain.Contact.DTO.Status;

namespace NGTestData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sqlPatientQuery = string.Format("Select Top {0} ID, FirstName, LastName, CONVERT(VARCHAR, BirthDate, 101) as BirthDate, MiddleInitial, Gender, Suffix From ContactEntities where CategoryCode = 'PT' and DeleteFlag = 0", numPatients.Value.ToString());

            string sqlConn = txtSQLConn.Text;

            List<MEPatient> patients = new List<MEPatient>();
            List<MEPatientProblem> patientProblems = new List<MEPatientProblem>();
            List<MECohortPatientView> cohortPatients = new List<MECohortPatientView>();
            List<MEPatientSystem> patientSystems = new List<MEPatientSystem>();
            List<MEContact> patientContacts = new List<MEContact>();

            List<Problem> problems = null;
            List<State> states = null;

            string mongoConnString = txtMongoConn.Text;

            MongoDB.Driver.MongoDatabase mongoDB = Phytel.Services.MongoService.Instance.GetDatabase(mongoConnString);

            mongoDB.GetCollection("Patient").RemoveAll();
            mongoDB.GetCollection("PatientProblem").RemoveAll();
            mongoDB.GetCollection("CohortPatientView").RemoveAll();
            mongoDB.GetCollection("PatientSystem").RemoveAll();
            mongoDB.GetCollection("PatientUser").RemoveAll();
            mongoDB.GetCollection("Contact").RemoveAll();

            //additional collections to clear out since we are reloading patients
            mongoDB.GetCollection("PatientProgram").RemoveAll();
            mongoDB.GetCollection("PatientProgramAttribute").RemoveAll();
            mongoDB.GetCollection("PatientProgramResponse").RemoveAll();

            mongoDB.GetCollection("PatientBarrier").RemoveAll();
            mongoDB.GetCollection("PatientGoal").RemoveAll();
            mongoDB.GetCollection("PatientIntervention").RemoveAll();
            mongoDB.GetCollection("PatientNote").RemoveAll();
            mongoDB.GetCollection("PatientObservation").RemoveAll();
            mongoDB.GetCollection("PatientTask").RemoveAll();
            mongoDB.GetCollection("PatientProgram").RemoveAll();

            IMongoQuery q = Query.EQ("type", 1);

            problems = GetAllProblems(mongoDB.GetCollection("LookUp"));
            states = GetAllStates(mongoDB.GetCollection("LookUp"));

            System.Random rnd = new Random();
            int maxNum = problems.Count() - 1;

            DataSet dsPatients = Phytel.Services.SQLDataService.Instance.ExecuteSQLDirect(sqlConn, sqlPatientQuery, 0);

            int counter = 0;
            foreach (DataRow dr in dsPatients.Tables[0].Rows)
            {
                counter++;
                MECohortPatientView currentPatientView = new MECohortPatientView(txtUserID.Text);

                string patientSystemID = dr["ID"].ToString();

                MEPatient patient = new MEPatient(txtUserID.Text, null)
                    {
                        DisplayPatientSystemId = null,
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Gender = dr["Gender"].ToString().ToUpper(),
                        DOB = dr["BirthDate"].ToString(),
                        MiddleName = dr["MiddleInitial"].ToString(),
                        Suffix = dr["Suffix"].ToString(),
                        PreferredName = dr["FirstName"].ToString() + "o",
                        DeleteFlag = false,
                        TTLDate = null,
                        Version = 1,
                        LastUpdatedOn = DateTime.UtcNow
                    };

                List<Address> addresses = new List<Address>();
                List<Email> emails = new List<Email>();
                List<Phone> phones = new List<Phone>();
                List<Phytel.API.DataDomain.Contact.DTO.CommMode> modes = new List<Phytel.API.DataDomain.Contact.DTO.CommMode>();

                string sqlAddressQuery = string.Format("select top 1 Address1, Address2, City, [State], ZipCode from Address Where OwnerID = {0}", patientSystemID);
                string sqlEmailQuery = string.Format("select top 1 Address from Email Where OwnerID = {0}", patientSystemID);
                string sqlPhoneQuery = string.Format("select top 1 DialString from Phone Where OwnerID = {0}", patientSystemID);

                DataSet dsAddress = Phytel.Services.SQLDataService.Instance.ExecuteSQLDirect(sqlConn, sqlAddressQuery, 0);
                DataSet dsEmail = Phytel.Services.SQLDataService.Instance.ExecuteSQLDirect(sqlConn, sqlEmailQuery, 0);
                DataSet dsPhone = Phytel.Services.SQLDataService.Instance.ExecuteSQLDirect(sqlConn, sqlPhoneQuery, 0);

                if (dsAddress.Tables[0].Rows.Count > 0)
                {
                    ObjectId stateID = states.Where(x => x.Code == dsAddress.Tables[0].Rows[0]["State"].ToString()).Select(y => y.DataId).FirstOrDefault();

                    addresses.Add(new Address
                    {
                        Id = ObjectId.GenerateNewId(),
                        Line1 = dsAddress.Tables[0].Rows[0]["Address1"].ToString(),
                        Line2 = dsAddress.Tables[0].Rows[0]["Address2"].ToString(),
                        City = dsAddress.Tables[0].Rows[0]["City"].ToString(),
                        StateId = stateID,
                        PostalCode = dsAddress.Tables[0].Rows[0]["ZipCode"].ToString(),
                        TypeId = ObjectId.Parse("52e18c2ed433232028e9e3a6")
                    });
                }

                if (dsEmail.Tables[0].Rows.Count > 0)
                    emails.Add(new Email { TypeId = ObjectId.Parse("52e18c2ed433232028e9e3a6"), Id = ObjectId.GenerateNewId(), Preferred = true, Text = dsEmail.Tables[0].Rows[0]["Address"].ToString() });

                if (dsPhone.Tables[0].Rows.Count > 0)
                    phones.Add(new Phone { Id = ObjectId.GenerateNewId(), IsText = true, Number = long.Parse(dsPhone.Tables[0].Rows[0]["DialString"].ToString()), PreferredPhone = true, PreferredText = false, TypeId = ObjectId.Parse("52e18c2ed433232028e9e3a6") });

                modes.Add(new Phytel.API.DataDomain.Contact.DTO.CommMode { ModeId = ObjectId.Parse("52e17cc2d433232028e9e38f"), OptOut = false, Preferred = false });
                modes.Add(new Phytel.API.DataDomain.Contact.DTO.CommMode { ModeId = ObjectId.Parse("52e17ce6d433232028e9e390"), OptOut = false, Preferred = false });
                modes.Add(new Phytel.API.DataDomain.Contact.DTO.CommMode { ModeId = ObjectId.Parse("52e17d08d433232028e9e391"), OptOut = false, Preferred = false });
                modes.Add(new Phytel.API.DataDomain.Contact.DTO.CommMode { ModeId = ObjectId.Parse("52e17d10d433232028e9e392"), OptOut = false, Preferred = false });

                MEContact patContact = new MEContact(txtUserID.Text, null)
                    {
                        Addresses = addresses,
                        Emails = emails,
                        FirstName = string.Empty,
                        Gender = string.Empty,
                        LastName = string.Empty,
                        MiddleName = string.Empty,
                        PatientId = patient.Id,
                        Phones = phones,
                        PreferredName = string.Empty,
                        TimeZoneId = ObjectId.Parse("52e1817dd433232028e9e39e"),
                        Modes = modes,
                        Version = 1.0
                    };

                MEPatientSystem patSystem = new MEPatientSystem(txtUserID.Text, null)
                    {
                        PatientId = patient.Id,
                        OldSystemId = patientSystemID,
                        SystemName = "Atmosphere",
                        DeleteFlag = false,
                        TTLDate = null,
                        Version = 1.0,
                        DisplayLabel = "ID",
                        LastUpdatedOn = DateTime.UtcNow
                    };

                patient.DisplayPatientSystemId = patSystem.Id;
                
                patients.Add(patient);
                patientSystems.Add(patSystem);
                patientContacts.Add(patContact);

                currentPatientView.PatientID = patient.Id;
                currentPatientView.LastName = patient.LastName;
                currentPatientView.Version = 1.0;
                currentPatientView.SearchFields = new List<SearchField>();
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "FN", Value = patient.FirstName });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "LN", Value = patient.LastName });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "G", Value = patient.Gender.ToUpper() });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "DOB", Value = patient.DOB });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "MN", Value = patient.MiddleName });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "SFX", Value = patient.Suffix });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "PN", Value = patient.PreferredName });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "PCM" });

                List<int> prodIds = new List<int>();
                for(int i = 0; i < numProblems.Value; i++)
                {
                    int probID = rnd.Next(maxNum);
                    while (prodIds.Contains(probID))
                    {
                        probID = rnd.Next(maxNum);
                    }

                    prodIds.Add(probID);
                    patientProblems.Add(new MEPatientProblem(txtUserID.Text)
                        {
                            PatientID = patient.Id,
                            Active = true,
                            DeleteFlag = false,
                            EndDate = null,
                            Featured = true,
                            LastUpdatedOn = DateTime.UtcNow,
                            Level = 1,
                            ProblemID = problems[probID].DataId,
                            StartDate = null,
                            TTLDate = null,
                            Version = 1.0
                        });
                    currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "Problem", Value = problems[probID].DataId.ToString() });
                }

                cohortPatients.Add(currentPatientView);
                
                if(counter == 1000)
                {
                    mongoDB.GetCollection("Patient").InsertBatch(patients);
                    mongoDB.GetCollection("PatientProblem").InsertBatch(patientProblems);
                    mongoDB.GetCollection("CohortPatientView").InsertBatch(cohortPatients);
                    mongoDB.GetCollection("PatientSystem").InsertBatch(patientSystems);
                    mongoDB.GetCollection("Contact").InsertBatch(patientContacts);

                    counter = 0;

                    patients = new List<MEPatient>();
                    patientProblems = new List<MEPatientProblem>();
                    cohortPatients = new List<MECohortPatientView>();
                    patientSystems = new List<MEPatientSystem>();
                    patientContacts = new List<MEContact>();
                }
            }
            if (patients.Count > 0)
            {
                mongoDB.GetCollection("Patient").InsertBatch(patients);
                mongoDB.GetCollection("PatientProblem").InsertBatch(patientProblems);
                mongoDB.GetCollection("CohortPatientView").InsertBatch(cohortPatients);
                mongoDB.GetCollection("PatientSystem").InsertBatch(patientSystems);
                mongoDB.GetCollection("Contact").InsertBatch(patientContacts);
            }
            
        }

        public List<Problem> GetAllProblems(MongoCollection lookupColl)
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(LookUpBase)) == false)
                BsonClassMap.RegisterClassMap<LookUpBase>();

            if (BsonClassMap.IsClassMapRegistered(typeof(MELookup)) == false)
                BsonClassMap.RegisterClassMap<MELookup>();

            if (BsonClassMap.IsClassMapRegistered(typeof(Problem)) == false)
                BsonClassMap.RegisterClassMap<Problem>();

            List<Problem> problems = new List<Problem>();
            List<IMongoQuery> queries = new List<IMongoQuery>();
            queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.Problem));
            queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
            IMongoQuery mQuery = Query.And(queries);
            MELookup meLookup = lookupColl.FindAs<MELookup>(mQuery).FirstOrDefault();
            if (meLookup != null)
            {
                if (meLookup.Data != null)
                {
                    foreach (Problem m in meLookup.Data)
                        problems.Add(m);
                }

            }
            return problems;
        }

        public List<State> GetAllStates(MongoCollection lookupColl)
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(LookUpBase)) == false)
                BsonClassMap.RegisterClassMap<LookUpBase>();

            if (BsonClassMap.IsClassMapRegistered(typeof(MELookup)) == false)
                BsonClassMap.RegisterClassMap<MELookup>();

            if (BsonClassMap.IsClassMapRegistered(typeof(State)) == false)
                BsonClassMap.RegisterClassMap<State>();

            List<State> states = new List<State>();
            List<IMongoQuery> queries = new List<IMongoQuery>();
            queries.Add(Query.EQ(MELookup.TypeProperty, LookUpType.State));
            queries.Add(Query.EQ(MELookup.DeleteFlagProperty, false));
            IMongoQuery mQuery = Query.And(queries);
            MELookup meLookup = lookupColl.FindAs<MELookup>(mQuery).FirstOrDefault();
            if (meLookup != null)
            {
                if (meLookup.Data != null)
                {
                    foreach (State s in meLookup.Data)
                        states.Add(s);
                }
            }
            return states;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string userSql = string.Format("Select u.UserID, u.FirstName, u.LastName, u.DisplayName From [User] u " +
                                "inner join usercontract uc on u.userid = uc.userid " +
                                "inner join contract c on uc.contractid = c.contractid " +
                                "where c.number = '{0}'", txtContract.Text);

            string sqlConn = txtSQLNGConn.Text;

            DataSet users = Phytel.Services.SQLDataService.Instance.ExecuteSQLDirect(sqlConn, userSql, 0);
            foreach (DataRow dr in users.Tables[0].Rows)
            {
                string mongoConnString = txtMongoConn.Text;

                MongoDB.Driver.MongoDatabase mongoDB = Phytel.Services.MongoService.Instance.GetDatabase(mongoConnString);

                IMongoQuery query = Query.EQ(MEContact.ResourceIdProperty, dr["UserID"].ToString());

                MEContact existsC = mongoDB.GetCollection("Contact").FindOneAs<MEContact>(query);

                if (existsC != null) continue;

                List<Phytel.API.DataDomain.Contact.DTO.CommMode> modes = new List<Phytel.API.DataDomain.Contact.DTO.CommMode>();

                modes.Add(new Phytel.API.DataDomain.Contact.DTO.CommMode { ModeId = ObjectId.Parse("52e17cc2d433232028e9e38f"), OptOut = false, Preferred = false });
                modes.Add(new Phytel.API.DataDomain.Contact.DTO.CommMode { ModeId = ObjectId.Parse("52e17ce6d433232028e9e390"), OptOut = false, Preferred = false });
                modes.Add(new Phytel.API.DataDomain.Contact.DTO.CommMode { ModeId = ObjectId.Parse("52e17d08d433232028e9e391"), OptOut = false, Preferred = false });
                modes.Add(new Phytel.API.DataDomain.Contact.DTO.CommMode { ModeId = ObjectId.Parse("52e17d10d433232028e9e392"), OptOut = false, Preferred = false });

                MEContact newC = new MEContact("5368ff2ad4332316288f3e3e", null)
                {
                    FirstName = dr["FirstName"].ToString(),
                    LoweredFirstName = dr["FirstName"].ToString().ToLower(),
                    LastName = dr["LastName"].ToString(),
                    LoweredLastName = dr["LastName"].ToString().ToLower(),
                    PreferredName = dr["DisplayName"].ToString(),
                    ResourceId = dr["UserID"].ToString(),
                    Modes = modes,
                    Status = Status.Active,
                    DataSource = "Engage",
                    ContactTypeId = ObjectId.Parse("56f1a1ad078e10eb86038519"),
                    Version = 1.0
                };
                mongoDB.GetCollection("Contact").Insert(newC);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtContract.Text = ConfigurationManager.AppSettings.Get("Contract").ToString();
            txtMongoConn.Text = Phytel.Services.MongoService.Instance.GetConnectionString("Phytel", txtContract.Text, true);
            txtSQLNGConn.Text = Phytel.Services.SQLDataService.Instance.GetConnectionString("Phytel", false);
        }
    }
}
