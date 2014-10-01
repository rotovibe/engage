using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace Phytel.Services
{
    public sealed class SQLDataService
    {
        private static volatile SQLDataService instance;
        private static object syncRoot = new Object();
        private static string _dbConnName = string.Empty;
        private static string _defaultSystemType = "Contract";

        #region Instance Methods
        private SQLDataService() { }

        public static SQLDataService Instance
        {
            get 
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SQLDataService();
                            _dbConnName = ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        public SqlTransaction GetTransaction(string connString)
        {
            SqlConnection sqlConnection = new SqlConnection(connString);

            sqlConnection.Open();

            SqlTransaction trans = sqlConnection.BeginTransaction();

            return trans;
        }

        #region Database Name Methods (using configuration file for phytel services conn name)
        
        public DataSet GetDataSet(string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams)
        {
            return GetDataSet(_dbConnName, databaseName, isContract, procedureName, procedureParams, 3600);
        }

        public DataSet GetDataSet(string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return GetDataSetDirect(GetConnectionString(_dbConnName, databaseName, isContract, _defaultSystemType), procedureName, procedureParams, cmdTimeout);
        }

        public void ExecuteProcedure(string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams)
        {
            ExecuteProcedure(_dbConnName, databaseName, isContract, procedureName, procedureParams, 3600);
        }

        public void ExecuteProcedure(string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            ExecuteProcedureDirect(GetConnectionString(_dbConnName, databaseName, isContract, _defaultSystemType), procedureName, procedureParams, cmdTimeout);
        }

        public void ExecuteProcedure(string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, SqlTransaction sqlTrans)
        {
            ExecuteProcedure(_dbConnName, databaseName, isContract, procedureName, procedureParams, sqlTrans, 3600);
        }

        public void ExecuteProcedure(string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, SqlTransaction sqlTrans, int cmdTimeout)
        {
            ExecuteProcedureDirect(GetConnectionString(_dbConnName, databaseName, isContract, _defaultSystemType), procedureName, procedureParams, sqlTrans, cmdTimeout);
        }

        public object ExecuteScalar(string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams)
        {
            return ExecuteScalar(_dbConnName, databaseName, isContract, procedureName, procedureParams, 3600);
        }

        public object ExecuteScalar(string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return ExecuteScalarDirect(GetConnectionString(_dbConnName, databaseName, isContract, _defaultSystemType), procedureName, procedureParams, null, cmdTimeout);
        }

        public DataSet ExecuteSQL(string databaseName, bool isContract, string queryText)
        {
            return ExecuteSQL(_dbConnName, databaseName, isContract, _defaultSystemType, queryText, null, 3600);
        }

        public DataSet ExecuteSQL(string databaseName, bool isContract, string queryText, int cmdTimeout)
        {
            return ExecuteSQL(_dbConnName, databaseName, isContract, _defaultSystemType, queryText, null, cmdTimeout);
        }

        public DataSet ExecuteSQL(string databaseName, bool isContract, string queryText, SqlTransaction sqlTrans)
        {
            return ExecuteSQL(_dbConnName, databaseName, isContract, _defaultSystemType, queryText, null, 3600);
        }

        public DataSet ExecuteSQL(string databaseName, bool isContract, string queryText, SqlTransaction sqlTrans, int cmdTimeout)
        {
            return ExecuteSQLDirect(GetConnectionString(_dbConnName, databaseName, isContract, _defaultSystemType), queryText, sqlTrans, cmdTimeout);
        }

        public SqlDataReader GetReader(string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams)
        {
            return GetReader(_dbConnName, databaseName, isContract, procedureName, procedureParams, 3600);
        }

        public SqlDataReader GetReader(string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return GetReaderDirect(GetConnectionString(_dbConnName, databaseName, isContract, _defaultSystemType), procedureName, procedureParams, cmdTimeout);
        }

        public DataSet GetDataSet(string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams)
        {
            return GetDataSet(_dbConnName, databaseName, isContract, systemType, procedureName, procedureParams, 3600);
        }

        public DataSet GetDataSet(string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return GetDataSetDirect(GetConnectionString(_dbConnName, databaseName, isContract, systemType), procedureName, procedureParams, cmdTimeout);
        }

        public void ExecuteProcedure(string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams)
        {
            ExecuteProcedure(_dbConnName, databaseName, isContract, systemType, procedureName, procedureParams, 3600);
        }

        public void ExecuteProcedure(string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            ExecuteProcedureDirect(GetConnectionString(_dbConnName, databaseName, isContract, systemType), procedureName, procedureParams, cmdTimeout);
        }

        public void ExecuteProcedure(string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, SqlTransaction sqlTrans)
        {
            ExecuteProcedure(_dbConnName, databaseName, isContract, systemType, procedureName, procedureParams, sqlTrans, 3600);
        }

        public void ExecuteProcedure(string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, SqlTransaction sqlTrans, int cmdTimeout)
        {
            ExecuteProcedureDirect(GetConnectionString(_dbConnName, databaseName, isContract, systemType), procedureName, procedureParams, sqlTrans, cmdTimeout);
        }

        public object ExecuteScalar(string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams)
        {
            return ExecuteScalar(_dbConnName, databaseName, isContract, systemType, procedureName, procedureParams, 3600);
        }

        public object ExecuteScalar(string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return ExecuteScalarDirect(GetConnectionString(_dbConnName, databaseName, isContract, systemType), procedureName, procedureParams, null, cmdTimeout);
        }

        public DataSet ExecuteSQL(string databaseName, bool isContract, string systemType, string queryText)
        {
            return ExecuteSQL(_dbConnName, databaseName, isContract, systemType, queryText, null, 3600);
        }

        public DataSet ExecuteSQL(string databaseName, bool isContract, string systemType, string queryText, int cmdTimeout)
        {
            return ExecuteSQL(_dbConnName, databaseName, isContract, systemType, queryText, null, cmdTimeout);
        }

        public DataSet ExecuteSQL(string databaseName, bool isContract, string systemType, string queryText, SqlTransaction sqlTrans)
        {
            return ExecuteSQL(_dbConnName, databaseName, isContract, systemType, queryText, null, 3600);
        }

        public DataSet ExecuteSQL(string databaseName, bool isContract, string systemType, string queryText, SqlTransaction sqlTrans, int cmdTimeout)
        {
            return ExecuteSQLDirect(GetConnectionString(_dbConnName, databaseName, isContract, systemType), queryText, sqlTrans, cmdTimeout);
        }

        public SqlDataReader GetReader(string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams)
        {
            return GetReader(_dbConnName, databaseName, isContract, systemType, procedureName, procedureParams, 3600);
        }

        public SqlDataReader GetReader(string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return GetReaderDirect(GetConnectionString(_dbConnName, databaseName, isContract, systemType), procedureName, procedureParams, cmdTimeout);
        }

        #endregion

        #region Database Name Methods (using custom value for phytel services conn name)

        public DataSet GetDataSet(string dbConnName, string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams)
        {
            return GetDataSet(dbConnName, databaseName, isContract, procedureName, procedureParams, 3600);
        }

        public DataSet GetDataSet(string dbConnName, string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return GetDataSetDirect(GetConnectionString(dbConnName, databaseName, isContract, _defaultSystemType), procedureName, procedureParams, cmdTimeout);
        }

        public void ExecuteProcedure(string dbConnName, string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams)
        {
            ExecuteProcedure(dbConnName, databaseName, isContract, procedureName, procedureParams, 3600);
        }

        public void ExecuteProcedure(string dbConnName, string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            ExecuteProcedureDirect(GetConnectionString(dbConnName, databaseName, isContract, _defaultSystemType), procedureName, procedureParams, cmdTimeout);
        }

        public void ExecuteProcedure(string dbConnName, string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, SqlTransaction sqlTrans)
        {
            ExecuteProcedure(dbConnName, databaseName, isContract, procedureName, procedureParams, sqlTrans, 3600);
        }

        public void ExecuteProcedure(string dbConnName, string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, SqlTransaction sqlTrans, int cmdTimeout)
        {
            ExecuteProcedureDirect(GetConnectionString(dbConnName, databaseName, isContract, _defaultSystemType), procedureName, procedureParams, sqlTrans, cmdTimeout);
        }

        public object ExecuteScalar(string dbConnName, string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams)
        {
            return ExecuteScalar(dbConnName, databaseName, isContract, procedureName, procedureParams, 3600);
        }

        public object ExecuteScalar(string dbConnName, string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return ExecuteScalarDirect(GetConnectionString(dbConnName, databaseName, isContract, _defaultSystemType), procedureName, procedureParams, null, cmdTimeout);
        }

        public DataSet ExecuteSQL(string dbConnName, string databaseName, bool isContract, string queryText)
        {
            return ExecuteSQL(dbConnName, databaseName, isContract, _defaultSystemType, queryText, null, 3600);
        }

        public DataSet ExecuteSQL(string dbConnName, string databaseName, bool isContract, string queryText, int cmdTimeout)
        {
            return ExecuteSQL(dbConnName, databaseName, isContract, _defaultSystemType, queryText, null, cmdTimeout);
        }

        public DataSet ExecuteSQL(string dbConnName, string databaseName, bool isContract, string queryText, SqlTransaction sqlTrans)
        {
            return ExecuteSQL(dbConnName, databaseName, isContract, _defaultSystemType, queryText, null, 3600);
        }

        public DataSet ExecuteSQL(string dbConnName, string databaseName, bool isContract, string queryText, SqlTransaction sqlTrans, int cmdTimeout)
        {
            return ExecuteSQLDirect(GetConnectionString(dbConnName, databaseName, isContract, _defaultSystemType), queryText, sqlTrans, cmdTimeout);
        }

        public SqlDataReader GetReader(string dbConnName, string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams)
        {
            return GetReader(dbConnName, databaseName, isContract, procedureName, procedureParams, 3600);
        }

        public SqlDataReader GetReader(string dbConnName, string databaseName, bool isContract, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return GetReaderDirect(GetConnectionString(dbConnName, databaseName, isContract, _defaultSystemType), procedureName, procedureParams, cmdTimeout);
        }

        public DataSet GetDataSet(string dbConnName, string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams)
        {
            return GetDataSet(dbConnName, databaseName, isContract, systemType, procedureName, procedureParams, 3600);
        }

        public DataSet GetDataSet(string dbConnName, string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return GetDataSetDirect(GetConnectionString(dbConnName, databaseName, isContract, systemType), procedureName, procedureParams, cmdTimeout);
        }

        public void ExecuteProcedure(string dbConnName, string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams)
        {
            ExecuteProcedure(dbConnName, databaseName, isContract, systemType, procedureName, procedureParams, 3600);
        }

        public void ExecuteProcedure(string dbConnName, string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            ExecuteProcedureDirect(GetConnectionString(dbConnName, databaseName, isContract, systemType), procedureName, procedureParams, cmdTimeout);
        }

        public void ExecuteProcedure(string dbConnName, string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, SqlTransaction sqlTrans)
        {
            ExecuteProcedure(dbConnName, databaseName, isContract, systemType, procedureName, procedureParams, sqlTrans, 3600);
        }

        public void ExecuteProcedure(string dbConnName, string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, SqlTransaction sqlTrans, int cmdTimeout)
        {
            ExecuteProcedureDirect(GetConnectionString(dbConnName, databaseName, isContract, systemType), procedureName, procedureParams, sqlTrans, cmdTimeout);
        }

        public object ExecuteScalar(string dbConnName, string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams)
        {
            return ExecuteScalar(dbConnName, databaseName, isContract, systemType, procedureName, procedureParams, 3600);
        }

        public object ExecuteScalar(string dbConnName, string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return ExecuteScalarDirect(GetConnectionString(dbConnName, databaseName, isContract, systemType), procedureName, procedureParams, null, cmdTimeout);
        }

        public DataSet ExecuteSQL(string dbConnName, string databaseName, bool isContract, string systemType, string queryText)
        {
            return ExecuteSQL(dbConnName, databaseName, isContract, systemType, queryText, null, 3600);
        }

        public DataSet ExecuteSQL(string dbConnName, string databaseName, bool isContract, string systemType, string queryText, int cmdTimeout)
        {
            return ExecuteSQL(dbConnName, databaseName, isContract, systemType, queryText, null, cmdTimeout);
        }

        public DataSet ExecuteSQL(string dbConnName, string databaseName, bool isContract, string systemType, string queryText, SqlTransaction sqlTrans)
        {
            return ExecuteSQL(dbConnName, databaseName, isContract, systemType, queryText, null, 3600);
        }

        public DataSet ExecuteSQL(string dbConnName, string databaseName, bool isContract, string systemType, string queryText, SqlTransaction sqlTrans, int cmdTimeout)
        {
            return ExecuteSQLDirect(GetConnectionString(dbConnName, databaseName, isContract, systemType), queryText, sqlTrans, cmdTimeout);
        }

        public SqlDataReader GetReader(string dbConnName, string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams)
        {
            return GetReader(dbConnName, databaseName, isContract, systemType, procedureName, procedureParams, 3600);
        }

        public SqlDataReader GetReader(string dbConnName, string databaseName, bool isContract, string systemType, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return GetReaderDirect(GetConnectionString(dbConnName, databaseName, isContract, systemType), procedureName, procedureParams, cmdTimeout);
        }

        #endregion

        #region Direct Connection Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataSet GetDataSetDirect(string connectionString, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            SqlConnection sqlConnection = null;
            SqlCommand sqlCommand;
            SqlParameter sqlParam;
            SqlDataAdapter sqlAdapter;
            DataSet returnDataSet;

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                sqlCommand = new SqlCommand(procedureName, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = cmdTimeout;

                foreach (Parameter param in procedureParams)
                {
                    sqlParam = new SqlParameter(param.Name, param.Value);
                    sqlParam.SqlDbType = param.Type;
                    sqlParam.Direction = param.Direction;
                    sqlParam.Size = param.Size;

                    sqlCommand.Parameters.Add(sqlParam);
                }

                //Now execute and return the results
                sqlAdapter = new SqlDataAdapter(sqlCommand);
                returnDataSet = new DataSet();
                sqlAdapter.Fill(returnDataSet);
            }

            finally
            {
                sqlCommand = null;
                sqlParam = null;
                sqlAdapter = null;

                if (sqlConnection != null)
                    sqlConnection.Close();
                sqlConnection = null;
            }

            return returnDataSet;
        }

        public object ExecuteScalarDirect(string connectionString, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            return ExecuteScalarDirect(connectionString, procedureName, procedureParams, null, cmdTimeout);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public object ExecuteScalarDirect(string connectionString, string procedureName, ParameterCollection procedureParams, SqlTransaction sqlTrans, int cmdTimeout)
        {
            SqlConnection sqlConnection = null;
            SqlCommand sqlCommand;
            SqlParameter sqlParam;

            object retVal = null;

            try
            {
                if (sqlTrans == null)
                {
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                }
                else
                {
                    sqlConnection = sqlTrans.Connection;
                }

                sqlCommand = new SqlCommand(procedureName, sqlConnection, sqlTrans);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = cmdTimeout;

                foreach (Parameter param in procedureParams)
                {
                    sqlParam = new SqlParameter(param.Name, param.Value);
                    sqlParam.SqlDbType = param.Type;
                    sqlParam.Direction = param.Direction;
                    sqlParam.Size = param.Size;

                    sqlCommand.Parameters.Add(sqlParam);
                }
                retVal = sqlCommand.ExecuteScalar();
            }

            finally
            {
                sqlCommand = null;
                sqlParam = null;

                if (sqlConnection != null && sqlTrans == null)
                    sqlConnection.Close();
                sqlConnection = null;
            }

            return retVal;
        }

        public void ExecuteProcedureDirect(string connectionString, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            ExecuteProcedureDirect(connectionString, procedureName, procedureParams, null, cmdTimeout);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void ExecuteProcedureDirect(string connectionString, string procedureName, ParameterCollection procedureParams, SqlTransaction sqlTrans, int cmdTimeout)
        {
            SqlConnection sqlConnection = null;
            SqlCommand sqlCommand;
            SqlParameter sqlParam;

            try
            {
                if (sqlTrans == null)
                {
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                }
                else
                {
                    sqlConnection = sqlTrans.Connection;
                }

                sqlCommand = new SqlCommand(procedureName, sqlConnection, sqlTrans);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = cmdTimeout;

                foreach (Parameter param in procedureParams)
                {
                    sqlParam = new SqlParameter(param.Name, param.Value);
                    sqlParam.SqlDbType = param.Type;
                    sqlParam.Direction = param.Direction;
                    sqlParam.Size = param.Size;

                    sqlCommand.Parameters.Add(sqlParam);
                }

                sqlCommand.ExecuteNonQuery();
            }

            finally
            {
                sqlCommand = null;
                sqlParam = null;

                if (sqlConnection != null && sqlTrans == null)
                    sqlConnection.Close();
                sqlConnection = null;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void ExecuteCommand(string connectionString, string commandSql, int cmdTimeout)
        {
            SqlConnection sqlConnection = null;
            SqlCommand sqlCommand;

            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                sqlCommand = new SqlCommand(commandSql, sqlConnection);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandTimeout = cmdTimeout;
                sqlCommand.ExecuteNonQuery();
            }

            finally
            {
                sqlCommand = null;

                if (sqlConnection != null)
                    sqlConnection.Close();
                sqlConnection = null;
            }
        }

        public DataSet ExecuteSQLDirect(string connectionString, string queryText, int cmdTimeout)
        {
            return ExecuteSQLDirect(connectionString, queryText, null, cmdTimeout);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataSet ExecuteSQLDirect(string connectionString, string queryText, SqlTransaction sqlTrans, int cmdTimeout)
        {
            SqlConnection sqlConnection = null;
            SqlCommand sqlCommand;
            SqlDataAdapter sqlAdapter;
            DataSet returnDataSet;

            try
            {
                if (sqlTrans == null)
                {
                    sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                }
                else
                {
                    sqlConnection = sqlTrans.Connection;
                }

                sqlCommand = new SqlCommand(queryText, sqlConnection, sqlTrans);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandTimeout = cmdTimeout;

                //Now execute and return the results
                sqlAdapter = new SqlDataAdapter(sqlCommand);
                returnDataSet = new DataSet();
                sqlAdapter.Fill(returnDataSet);
            }

            finally
            {
                sqlCommand = null;
                sqlAdapter = null;

                if (sqlConnection != null && sqlTrans == null)
                    sqlConnection.Close();

                sqlConnection = null;
            }

            return returnDataSet;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public SqlDataReader GetReaderDirect(string connectionString, string procedureName, ParameterCollection procedureParams, int cmdTimeout)
        {
            SqlConnection sqlConnection = null;
            SqlCommand sqlCommand;
            SqlParameter sqlParam;
            SqlDataReader sqlReader;

            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            sqlCommand = new SqlCommand(procedureName, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = cmdTimeout;

            foreach (Parameter param in procedureParams)
            {
                sqlParam = new SqlParameter(param.Name, param.Value);
                sqlParam.SqlDbType = param.Type;
                sqlParam.Direction = param.Direction;
                sqlParam.Size = param.Size;

                sqlCommand.Parameters.Add(sqlParam);
            }

            sqlReader = sqlCommand.ExecuteReader();

            return sqlReader;
        }

        #endregion

        #region Connection String Methods
        #region Using configuration file for phytel services conn name
        public string GetServer(string ContractID)
        {
            return GetServer(_dbConnName, ContractID, _defaultSystemType);
        }

        public string GetServer(string ContractID, string systemType)
        {
            return GetServer(_dbConnName, ContractID, systemType);
        }

        public string GetConnectionString(string databaseName, bool isContract)
        {
            return GetConnectionString(_dbConnName, databaseName, isContract, _defaultSystemType);
        }
        #endregion

        #region Using configuration file for phytel services conn name

        public string GetServer(string dbConnName, string ContractID, string systemType)
        {
            string retVal = string.Empty;

            try
            {
                string conn = GetConnectionString(dbConnName, ContractID, true, systemType).ToUpper();

                string[] tokens = conn.Split(';');
                foreach (string item in tokens)
                {
                    if (item.Contains("SERVER"))
                    {
                        retVal = item.Substring(item.IndexOf("=") + 1);
                        break;
                    }
                }
            }

            catch
            {
                retVal = string.Empty;
            }

            return retVal;
        }

        public string GetConnectionString(string dbConnName, string databaseName, bool isContract)
        {
            return GetConnectionString(_dbConnName, databaseName, isContract, _defaultSystemType);
        }
        #endregion

        public string GetConnectionString(string dbConnName, string databaseName, bool isContract, string systemType)
        {
            try
            {
                string connectString = string.Empty;

                if (isContract)
                {
                    //we are going to get a contract connection string
                    ParameterCollection parms = new ParameterCollection();
                    parms.Add(new Parameter("@ContractId", DBNull.Value, SqlDbType.Int, ParameterDirection.Input, 8));
                    parms.Add(new Parameter("@ContractNumber", databaseName, SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@DatabaseType", "SQL", SqlDbType.VarChar, ParameterDirection.Input, 10));
                    parms.Add(new Parameter("@SystemType", systemType, SqlDbType.VarChar, ParameterDirection.Input, 10));

                    DataSet ds = SQLDataService.Instance.GetDataSet(dbConnName, false, "spPhy_GetContractDBConnection", parms, 30);

                    string additionalParams = GetContractParams(databaseName);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataProtector dp = new DataProtector(DataProtector.Store.USE_SIMPLE_STORE);

                        connectString = string.Format("server={0};database={1};user id={2};password={3};{4}",
                                            ds.Tables[0].Rows[0]["Server"].ToString(),
                                            ds.Tables[0].Rows[0]["Database"].ToString(),
                                            ds.Tables[0].Rows[0]["UserName"].ToString(),
                                            dp.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString()),
                                            additionalParams);
                    }
                    else
                        connectString = connectString = string.Format("Server={0};Database={1};Trusted_Connection=True;{2}",
                                                  "localhost", databaseName, additionalParams);
                }
                else
                    connectString = GetDBString(databaseName);

                return connectString;
            }
            catch (Exception ex)
            {
                string msg = ex.Message + System.Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name + System.Environment.NewLine;
                throw new Exception(msg + ex.Source);
            }
        } 

        private string GetDBString(string databaseName)
        {
            try
            {
                string connectString = string.Empty;
                XmlNode xmlNode = null;
                XmlDocument doc = new XmlDocument();
                DataProtector dp = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);

                // Load the Phytel Database Security File
                doc.Load(DataProtector.GetConfigurationPathAndFile());

                xmlNode = doc.SelectSingleNode(string.Format("/configuration/database[@type='SQL'][@name='{0}']", databaseName));

                if (xmlNode == null)
                    throw new Exception(string.Format("Unable to find SQL Database '{0}' in configuration file!", databaseName));

                XmlNode trusted = xmlNode.SelectSingleNode("trusted");
                XmlNode options = xmlNode.SelectSingleNode("options");

                string connOptions = string.Empty;
                bool isTrusted = false;

                if (trusted != null)
                    isTrusted = Convert.ToBoolean(trusted.InnerText);

                if (options != null)
                    connOptions = xmlNode.SelectSingleNode("options").InnerText;

                if (isTrusted)
                {
                    connectString = string.Format("Server={0};Database={1};Trusted_Connection=True;{2}",
                                                  xmlNode.SelectSingleNode("server").InnerText,
                                                  xmlNode.SelectSingleNode("databasename").InnerText,
                                                  connOptions);
                }
                else
                {
                    connectString = string.Format("server={0};database={1};user id={2};password={3};{4}",
                        xmlNode.SelectSingleNode("server").InnerText,
                        xmlNode.SelectSingleNode("databasename").InnerText,
                        xmlNode.SelectSingleNode("username").InnerText,
                        dp.Decrypt(xmlNode.SelectSingleNode("password").InnerText),
                        connOptions);
                }

                return connectString;
            }
            catch (Exception ex)
            {
                string msg = ex.Message + System.Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name + System.Environment.NewLine;
                throw new Exception(msg + ex.Source);
            }            
        }

        private string GetContractParams(string databaseName)
        {
            try
            {
                string connectParams = string.Empty;
                XmlNode xmlNode = null;
                XmlDocument doc = new XmlDocument();

                // Load the Phytel Database Security File
                doc.Load(DataProtector.GetConfigurationPathAndFile());

                XmlNodeList nodeList = doc.SelectNodes("/configuration/database[@type='SQL']");

                foreach (XmlNode x in nodeList)
                {
                    if (x.Attributes.GetNamedItem("name").Value.ToUpper() == databaseName.ToUpper())
                    {
                        xmlNode = x;
                        break;
                    }
                }
                if(xmlNode == null)
                    xmlNode = doc.SelectSingleNode("/configuration/database[@type='SQL'][@name='Contract']");

                if (xmlNode == null)
                    connectParams = string.Empty;
                else
                {
                    if(xmlNode.SelectSingleNode("options") != null)
                        connectParams = xmlNode.SelectSingleNode("options").InnerText;
                }
                return connectParams;
            }
            catch (Exception ex)
            {
                string msg = ex.Message + System.Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name + System.Environment.NewLine;
                throw new Exception(msg + ex.Source);
            }
        }
        #endregion
    }
}
