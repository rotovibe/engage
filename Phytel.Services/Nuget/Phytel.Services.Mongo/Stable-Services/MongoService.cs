﻿using MongoDB.Driver;
using Phytel.Services.Security;
using Phytel.Services.SQLServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml;

namespace Phytel.Services.Mongo
{
    public sealed class MongoService
    {
        private static volatile MongoService instance;
        private static object syncRoot = new Object();
        private static string _dbConnName = string.Empty;
        private static string _defaultSystemType = "Contract";

        #region Instance Methods
        private MongoService() {}

        public static MongoService Instance
        {
            get 
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new MongoService();
                            _dbConnName = ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
                        }
                    }
                }
                if (string.IsNullOrEmpty(_dbConnName))
                    _dbConnName = "Phytel";

                return instance;
            }
        }
        #endregion

        #region Mongo Objects

        public MongoDatabase GetDatabase(string connectionString)
        {
            MongoUrlBuilder urlBuilder = new MongoUrlBuilder(connectionString);
            string databaseName = urlBuilder.DatabaseName;

            MongoClient client = new MongoClient(urlBuilder.ToMongoUrl());
            return client.GetServer().GetDatabase(databaseName);
        }

        #region Using Configuration Setting "PhytelServicesConnName"
        public MongoDatabase GetDatabase(string databaseName, bool isContract)
        {
            return GetDatabase(_dbConnName, databaseName, isContract, false, _defaultSystemType);
        }

        public MongoDatabase GetDatabase(string databaseName, bool isContract, bool asAdmin)
        {
            string connectionString = GetConnectionString(_dbConnName, databaseName, isContract, asAdmin, _defaultSystemType);
            return GetDatabase(connectionString);
        }

        public MongoDatabase GetDatabase(string databaseName, bool isContract, string systemType)
        {
            string connectionString = GetConnectionString(_dbConnName, databaseName, isContract, false, systemType);
            return GetDatabase(connectionString);
        }

        public MongoDatabase GetDatabase(string databaseName, bool isContract, bool asAdmin, string systemType)
        {
            string connectionString = GetConnectionString(_dbConnName, databaseName, isContract, asAdmin, systemType);
            return GetDatabase(connectionString);
        }

        public string GetConnectionString(string databaseName, bool isContract)
        {
            return GetConnectionString(_dbConnName, databaseName, isContract, false, _defaultSystemType);
        }
        
        public string GetConnectionString(string databaseName, bool isContract, bool asAdmin)
        {
            return GetConnectionString(_dbConnName, databaseName, isContract, asAdmin, _defaultSystemType);
        }

        public string GetConnectionString(string databaseName, bool isContract, bool asAdmin, string systemType)
        {
            return GetConnectionString(_dbConnName, databaseName, isContract, asAdmin, systemType);
        }
        #endregion

        #region Using Passed In DB Connection Name
        public MongoDatabase GetDatabase(string dbConnName, string databaseName, bool isContract)
        {
            return GetDatabase(dbConnName, databaseName, isContract, false, _defaultSystemType);
        }

        public MongoDatabase GetDatabase(string dbConnName, string databaseName, bool isContract, bool asAdmin)
        {
            string connectionString = GetConnectionString(dbConnName, databaseName, isContract, asAdmin, _defaultSystemType);
            return GetDatabase(connectionString);
        }

        public MongoDatabase GetDatabase(string dbConnName, string databaseName, bool isContract, string systemType)
        {
            string connectionString = GetConnectionString(dbConnName, databaseName, isContract, false, systemType);
            return GetDatabase(connectionString);
        }

        public MongoDatabase GetDatabase(string dbConnName, string databaseName, bool isContract, bool asAdmin, string systemType)
        {
            string connectionString = GetConnectionString(dbConnName, databaseName, isContract, asAdmin, systemType);
            return GetDatabase(connectionString);
        }

        public string GetConnectionString(string dbConnName, string databaseName, bool isContract)
        {
            return GetConnectionString(dbConnName, databaseName, isContract, false, _defaultSystemType);
        }

        public string GetConnectionString(string dbConnName, string databaseName, bool isContract, bool asAdmin)
        {
            return GetConnectionString(dbConnName, databaseName, isContract, asAdmin, _defaultSystemType);
        }
        #endregion

        public string GetConnectionString(string dbConnName, string databaseName, bool isContract, bool asAdmin, string systemType)
        {
            try
            {
                string connectString = string.Empty;
                string userName = string.Empty;
                string password = string.Empty;
                string replicationSetName = string.Empty;
                string dbName = string.Empty;

                if (asAdmin)
                {
                    //go get the "MongoAdmin" credentials from the services file
                    XmlNode adminNode = GetConnectionNode("MongoAdmin");
                    if (adminNode != null)
                    {
                        DataProtector adminDP = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);
                        userName = adminNode.SelectSingleNode("username").InnerText + "(admin)";
                        password = adminDP.Decrypt(adminNode.SelectSingleNode("password").InnerText).Replace("@", "%40");
                    }
                }

                if (isContract)
                {
                    //we are going to get a contract connection string
                    ParameterCollection parms = new ParameterCollection();
                    parms.Add(new Parameter("@ContractId", DBNull.Value, SqlDbType.Int, ParameterDirection.Input, 8));
                    parms.Add(new Parameter("@ContractNumber", databaseName, SqlDbType.VarChar, ParameterDirection.Input, 100));
                    parms.Add(new Parameter("@DatabaseType", "MONGO", SqlDbType.VarChar, ParameterDirection.Input, 10));
                    parms.Add(new Parameter("@SystemType", systemType, SqlDbType.VarChar, ParameterDirection.Input, 10));

                    string connString = SQLDataService.Instance.GetConnectionString(dbConnName, false);

                    using (SqlConnection sqlConnection = new SqlConnection(connString))
                    {
                        sqlConnection.Open();

                        using (SqlCommand sqlCommand = new SqlCommand("spPhy_GetContractDBConnection", sqlConnection))
                        {
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.CommandTimeout = 3600;

                            SqlParameter sqlParam = null;
                            foreach (Parameter param in parms)
                            {
                                sqlParam = new SqlParameter(param.Name, param.Value);
                                sqlParam.SqlDbType = param.Type;
                                sqlParam.Direction = param.Direction;
                                sqlParam.Size = param.Size;

                                sqlCommand.Parameters.Add(sqlParam);
                            }

                            using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                            {
                                List<string> servers = new List<string>();

                                if (dataReader.HasRows)
                                {
                                    DataProtector dp = new DataProtector(DataProtector.Store.USE_SIMPLE_STORE);

                                    dataReader.Read();

                                    replicationSetName = dataReader.SafeGetString("ReplicationSetName");
                                    userName = dataReader.SafeGetString("UserName");
                                    password = dp.Decrypt(dataReader.SafeGetString("Password")).Replace("@", "%40");
                                    dbName = dataReader.SafeGetString("Database");

                                    //Ok, we could get multiple records back, one for each potential server (Primary and Secondary)
                                    servers.Add(dataReader.SafeGetString("Server"));
                                    while (dataReader.Read())
                                        servers.Add(dataReader.SafeGetString("Server"));

                                    connectString = GetContractDBString(servers, dbName, replicationSetName, userName, password);
                                }
                                else
                                {
                                    servers.Add("localhost");
                                    connectString = GetContractDBString(servers, databaseName, string.Empty, userName, password);
                                }
                            }
                        }
                    }
                }
                else
                    connectString = GetDBString(databaseName, userName, password);
                
                return connectString;
            }
            catch (Exception ex)
            {
                string msg = ex.Message + System.Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name + System.Environment.NewLine;
                throw new Exception(msg + ex.Source);
            }
        }

        private string GetDBString(string databaseName, string userName, string password)
        {
            //mongodb://[username:password@]host1[:port1][,host2[:port2],...[,hostN[:portN]]][/[database][?options]]

            try
            {
                string connectString = string.Empty;
                string serverList = string.Empty;
                string domainprefix = string.Empty;
                string options = "?";

                XmlNode xmlNode = GetConnectionNode(databaseName);

                if (xmlNode == null)
                    throw new Exception(string.Format("Unable to find MongoDB '{0}' in configuration file!", databaseName));

                serverList = xmlNode.SelectSingleNode("server").InnerText.Trim();

                XmlNode trusted = xmlNode.SelectSingleNode("trusted");
                bool isTrusted = false;

                if (trusted != null)
                    isTrusted = Convert.ToBoolean(trusted.InnerText);

                options += xmlNode.SelectSingleNode("options").InnerText;

                XmlNode replSetNode = xmlNode.SelectSingleNode("replicationsetname");
                if(replSetNode != null && replSetNode.InnerText.Trim() != string.Empty)
                    options += string.Format(";replicaSet={0}", replSetNode.InnerText.Trim());

                if (isTrusted)
                {
                    XmlNode prefixNode = xmlNode.SelectSingleNode("domainprefix");
                    if (prefixNode != null)
                        domainprefix = "." + prefixNode.InnerText.Trim();

                    string ntUsername = string.Format("{0}%40{1}{2}", Environment.UserName, Environment.UserDomainName, domainprefix);
                    options += ";authMechanism=GSSAPI";

                    connectString = string.Format("mongodb://{0}@{1}/{2}{3}",
                        ntUsername, serverList, xmlNode.SelectSingleNode("databasename").InnerText, options);
                }
                else
                {
                    DataProtector dp = new DataProtector(DataProtector.Store.USE_MACHINE_STORE);

                    connectString = string.Format("mongodb://{0}:{1}@{2}/{3}{4}",
                        (userName == string.Empty ? xmlNode.SelectSingleNode("username").InnerText : userName),
                        (password == string.Empty ? dp.Decrypt(xmlNode.SelectSingleNode("password").InnerText).Replace("@", "%40") : password.Replace("@", "%40")),
                        serverList, xmlNode.SelectSingleNode("databasename").InnerText, options);
                }
                return connectString.Replace("?;", "?");
            }
            catch (Exception ex)
            {
                string msg = ex.Message + System.Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name + System.Environment.NewLine;
                throw new Exception(msg + ex.Source);
            }            
        }

        private string GetContractDBString(List<string> servers, string databaseName, string replicationSetName, string userName, string password)
        {
            //mongodb://[username:password@]host1[:port1][,host2[:port2],...[,hostN[:portN]]][/[database][?options]]

            try
            {
                string connectString = string.Empty;
                bool isTrusted = false;
                string options = "?";
                string serverList = string.Empty;
                string domainprefix = string.Empty;

                //First try to get a connection node specific to this contract, if it doesn't exist, get it from the "Contract" node
                XmlNode xmlNode = GetConnectionNode(databaseName);
                if (xmlNode == null)
                    xmlNode = GetConnectionNode("Contract");

                if (xmlNode != null)
                {
                    XmlNode trusted = xmlNode.SelectSingleNode("trusted");

                    if (trusted != null)
                        isTrusted = Convert.ToBoolean(trusted.InnerText);

                    options += xmlNode.SelectSingleNode("options").InnerText;
                }

                serverList = servers.Aggregate((i, j) => i + "," + j);

                if (replicationSetName.Trim() != string.Empty)
                    options += string.Format(";replicaSet={0};readPreference=primaryPreferred", replicationSetName);

                if (isTrusted)
                {
                    XmlNode prefixNode = xmlNode.SelectSingleNode("domainprefix");
                    if (prefixNode != null)
                        domainprefix = "." + prefixNode.InnerText.Trim();

                    string ntUsername = string.Format("{0}%40{1}{2}", Environment.UserName, Environment.UserDomainName, domainprefix);
                    options += ";authMechanism=GSSAPI";

                    connectString = string.Format("mongodb://{0}@{1}/{2}{3}",
                        ntUsername, serverList, databaseName, options);
                }
                else
                {
                    //mongodb://[username:password@]host1[:port1][,host2[:port2],...[,hostN[:portN]]][/[database][?options]]
                    connectString = string.Format("mongodb://{0}:{1}@{2}/{3}{4}",
                        userName, password, serverList, databaseName, options);
                }
                return connectString.Replace("?;", "?");
            }
            catch (Exception ex)
            {
                string msg = ex.Message + System.Environment.NewLine + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name + System.Environment.NewLine;
                throw new Exception(msg + ex.Source);
            }
        }
        
        private XmlNode GetConnectionNode(string databaseName)
        {
            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(DataProtector.GetConfigurationPathAndFile());

            XmlNode returnNode = null;
            XmlNodeList nodeList = configDoc.SelectNodes("/configuration/database[@type='Mongo']");

            foreach (XmlNode x in nodeList)
            {
                if (x.Attributes.GetNamedItem("name").Value.ToUpper() == databaseName.ToUpper())
                {
                    returnNode = x;
                    break;
                }
            }
            return returnNode;
        }
        
        #endregion
    }
}
