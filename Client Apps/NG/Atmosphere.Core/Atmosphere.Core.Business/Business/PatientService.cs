using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;

using C3.Data;
using Phytel.Framework.SQL.Data;
using C3.Business.Interfaces;

namespace C3.Business
{
    public class PatientService : SqlDataAccessor, IPatientService
    {
        #region Private Variables

        private static volatile PatientService _svc = null;
        private static object syncRoot = new Object();

        private string _dbConnName = "Phytel";

        public PatientService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        #endregion

        #region Public Methods

        public static PatientService Instance
        {
            get
            {
                if (_svc == null)
                {
                    lock (syncRoot)
                    {
                        if (_svc == null)
                            _svc = new PatientService();
                    }
                }

                return _svc;
            }
        }

        #endregion

        #region IPatientService Members

        public IDictionary<string, DataTable> SearchPatients(Contract contract, string firstName, string lastName, string phone, AuditData auditLog)
        {
            if (auditLog != null)
            {
                Dictionary<string, DataTable> results = new Dictionary<string, DataTable>();

                IDbCommand cmd = GetCommand(contract.ConnectionString, contract.Database, StoredProcedure.SearchPatientsDetails, null);
                SqlDataContext ctx = new SqlDataContext(contract.ConnectionString, contract.Database);
                ctx.AddInParameter(cmd, "@FirstName", DbType.String, firstName);
                ctx.AddInParameter(cmd, "@LastName", DbType.String, lastName);
                ctx.AddInParameter(cmd, "@Phone", DbType.String, phone);
                ctx.AddOutParameter(cmd, "@Message", DbType.String, 7);

                DataSet ds = ctx.ExecuteDataSet(cmd);
                string output = ((DbParameter)cmd.Parameters["@Message"]).Value.ToString();

                results.Add(output, ds.Tables[0]);

                auditLog.Type = "ViewPatientListPatientSearch";
                List<int> patients = new List<int>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    patients.Add(Convert.ToInt32(row["PatientId"]));
                }

                auditLog.Patients = patients;
                AuditService.Instance.LogEvent(auditLog);

                return results;
            }
            else
                return null;
        }

        public Patient GetPatient(Contract contract, int patientId, AuditData auditLog)
        {
            DataSet patientData = new DataSet();
            Patient patient = new Patient();
            DataSet responseCodes = new DataSet();
            DataSet responseORCodes = new DataSet();
            responseCodes = GetAppointmentResponses();
            responseORCodes = GetOutreachResponses();

            if (auditLog != null)
            {
                patientData = QueryDataSet(contract.ConnectionString, contract.Database, StoredProcedure.GetPatientByPatientId, patientId, int.Parse(ApplicationSettingService.Instance.GetSetting("COMM_LOOKBACK_DAYS").Value));

                if (patientData.Tables[0].Rows.Count > 0)
                {
                    patient.PatientId = Convert.ToInt32(patientData.Tables[0].Rows[0]["PatientId"].ToString());
                    patient.DateOfBirth = patientData.Tables[0].Rows[0]["BirthDate"].ToString();
                    patient.Email = patientData.Tables[0].Rows[0]["Email"].ToString();
                    patient.Gender = patientData.Tables[0].Rows[0]["Gender"].ToString();
                    patient.Name = patientData.Tables[0].Rows[0]["Name"].ToString();
                    patient.Phone = patientData.Tables[0].Rows[0]["PhoneNumber"].ToString();
                    patient.ClientPatientId = patientData.Tables[0].Rows[0]["ClientPatientId"].ToString();

                    patient.CommunicationActivities = patientData.Tables[1];

                    //Set Primary Key
                    DataColumn responseColumn = new DataColumn();
                    responseColumn.ColumnName = "ResponseName";
                    responseColumn.DataType = typeof(string);
                    patientData.Tables[2].Columns.Add(responseColumn);

                    foreach (DataRow row in patientData.Tables[2].Rows)
                    {
                        DataRow parent;
                        try
                        {
                            if (row["CommunicationType"].ToString() == "Appointments")
                                parent = responseCodes.Tables[1].Select(string.Format("NotificationCode = '{0}'", row["ResponseCode"]))[0].GetParentRow(responseCodes.Relations[0].RelationName);
                            else if (row["CommunicationType"].ToString() == "Outreach")
                                parent = responseORCodes.Tables[1].Select(string.Format("NotificationCode = '{0}'", row["ResponseCode"]))[0].GetParentRow(responseORCodes.Relations[0].RelationName);
                            else
                                parent = null;
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                            parent = null;
                        }
                        row.SetField("ResponseName", (parent != null ? parent["Description"].ToString() : String.Empty));
                    }

                    patient.CommunicationAttempts = patientData.Tables[2];
                    patient.OptOutData = patientData.Tables[3];

                    auditLog.Type = "ViewPatientListPatientSummary";
                    auditLog.Patients = new List<int> { patient.PatientId };
                    AuditService.Instance.LogEvent(auditLog);
                }
            }
            return patient;
        }

        public DataSet GetOutreachResponses()
        {
            DataSet ds = QueryDataSet(null, _dbConnName, StoredProcedure.GetORResponses);
            DataRelation relation = new DataRelation("ORResponse_ResponseMapping", ds.Tables[0].Columns["ResponseCode"], ds.Tables[1].Columns["ResponseCode"]);
            ds.Relations.Add(relation);
            return ds;
        }

        public DataSet GetAppointmentResponses()
        {

            DataSet ds = QueryDataSet(null, _dbConnName, StoredProcedure.GetResponses);
            DataRelation relation = new DataRelation("Response_ResponseMapping", ds.Tables[0].Columns["ResponseCode"], ds.Tables[1].Columns["ResponseCode"]);
            ds.Relations.Add(relation);
            return ds;
        }

        #endregion
    }
}
