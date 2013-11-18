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
            List<MECondition> conditions = null;

            MongoDB.Driver.MongoDatabase mongoDB = Phytel.Services.MongoService.Instance.GetDatabase("InHealth001", true);

            conditions = mongoDB.GetCollection("ProblemLookUp").FindAllAs<MECondition>().ToList();

            DataSet dsPatients = Phytel.Services.SQLDataService.Instance.ExecuteSQLDirect(sqlConn, sqlPatientQuery, 0);

            foreach (DataRow dr in dsPatients.Tables[0].Rows)
            {
                Phytel.API.DataDomain.Patient.DTO.MEPatient patient = new Phytel.API.DataDomain.Patient.DTO.MEPatient
                    {
                        DisplayPatientSystemID = null,
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Gender = dr["Gender"].ToString(),
                        DOB = dr["BirthDate"].ToString(),
                        MiddleName = dr["MiddleInitial"].ToString(),
                        Suffix = dr["Suffix"].ToString(),
                        PreferredName = null,
                        UpdatedBy = null,
                        DeleteFlag = false,
                        TTLDate = null,
                        Version = "v1",
                        LastUpdatedOn = DateTime.Now
                    };

                patients.Add(patient);

            }
            mongoDB.GetCollection("Patient").RemoveAll();
            mongoDB.GetCollection("Patient").InsertBatch(patients);
        }
    }
}
