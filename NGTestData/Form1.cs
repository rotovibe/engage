using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.PatientProblem.DTO;
using Phytel.API.DataDomain.CohortPatient.DTO;

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
            string sqlPatientQuery = string.Format("Select Top {0} FirstName, LastName, CONVERT(VARCHAR, BirthDate, 101) as BirthDate, MiddleInitial, Gender, Suffix From ContactEntities where CategoryCode = 'PT' and DeleteFlag = 0", numPatients.Value.ToString());

            string sqlConn = "server=10.90.1.10;database=JCMR001;user id=jcmrtestuser;password=testuser;";

            List<MEPatient> patients = new List<MEPatient>();
            List<MEPatientProblem> patientProblems = new List<MEPatientProblem>();
            List<MECohortPatientView> cohortPatients = new List<MECohortPatientView>();

            List<MEProblem> problems = null;

            MongoDB.Driver.MongoDatabase mongoDB = Phytel.Services.MongoService.Instance.GetDatabase("InHealth001", true);

            mongoDB.GetCollection("Patient").RemoveAll();
            mongoDB.GetCollection("PatientProblem").RemoveAll();
            mongoDB.GetCollection("CohortPatientView").RemoveAll();

            problems = mongoDB.GetCollection("ProblemLookUp").FindAllAs<MEProblem>().ToList();

            System.Random rnd = new Random();
            int maxNum = problems.Count() - 1;

            DataSet dsPatients = Phytel.Services.SQLDataService.Instance.ExecuteSQLDirect(sqlConn, sqlPatientQuery, 0);

            int counter = 0;
            foreach (DataRow dr in dsPatients.Tables[0].Rows)
            {
                counter++;
                MECohortPatientView currentPatientView = new MECohortPatientView();

                //Phytel.API.DataDomain.Cohort.DTO.
                Phytel.API.DataDomain.Patient.DTO.MEPatient patient = new Phytel.API.DataDomain.Patient.DTO.MEPatient
                    {
                        DisplayPatientSystemID = null,
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Gender = dr["Gender"].ToString().ToUpper(),
                        DOB = dr["BirthDate"].ToString(),
                        MiddleName = dr["MiddleInitial"].ToString(),
                        Suffix = dr["Suffix"].ToString(),
                        PreferredName = dr["FirstName"].ToString() + "o",
                        UpdatedBy = null,
                        DeleteFlag = false,
                        TTLDate = null,
                        Version = "v1",
                        LastUpdatedOn = DateTime.Now
                    };

                patients.Add(patient);

                currentPatientView.PatientID = patient.Id;
                currentPatientView.LastName = patient.LastName;
                currentPatientView.SearchFields = new List<SearchField>();
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "FN", Value = patient.FirstName });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "LN", Value = patient.LastName });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "G", Value = patient.Gender.ToUpper() });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "DOB", Value = patient.DOB });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "MN", Value = patient.MiddleName });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "SFX", Value = patient.Suffix });
                currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "PN", Value = patient.PreferredName });
                
                for(int i = 0; i < numProblems.Value; i++)
                {
                    int probID = rnd.Next(maxNum);

                    patientProblems.Add(new MEPatientProblem
                        {
                            PatientID = patient.Id,
                            Active = true,
                            DeleteFlag = false,
                            EndDate = null,
                            Featured = true,
                            LastUpdatedOn = DateTime.Now,
                            Level = 1,
                            ProblemID = problems[probID].Id,
                            StartDate = null,
                            TTLDate = null,
                            Version = "v1"
                        });
                    currentPatientView.SearchFields.Add(new SearchField { Active = true, FieldName = "Problem", Value = problems[probID].Id.ToString() });
                }

                cohortPatients.Add(currentPatientView);
                
                if(counter == 1000)
                {
                    mongoDB.GetCollection("Patient").InsertBatch(patients);
                    mongoDB.GetCollection("PatientProblem").InsertBatch(patientProblems);
                    mongoDB.GetCollection("CohortPatientView").InsertBatch(cohortPatients);
                    counter = 0;

                    patients = new List<MEPatient>();
                    patientProblems = new List<MEPatientProblem>();
                    cohortPatients = new List<MECohortPatientView>();
                }
            }
            if (patients.Count > 0)
            {
                mongoDB.GetCollection("Patient").InsertBatch(patients);
                mongoDB.GetCollection("PatientProblem").InsertBatch(patientProblems);
                mongoDB.GetCollection("CohortPatientView").InsertBatch(cohortPatients);
            }
            
        }
    }
}
