using System;
using MongoDB.Driver;
using Phytel.Services;

namespace Phytel.Services.Mongo.Linq
{
    /// <summary>
    /// Mongo Context class similar to entityframework.
    /// </summary>
    public abstract class MongoContext : IDisposable
    {
        private static string _defaultSystemType = "Contract";

        private WriteConcern _writeConcern;
        private string _databaseName;

        private MongoUrlBuilder _connectionStringBuilder;
        private MongoClient _client;

        #region Custom Supplied Connection Strings
        public MongoContext(string connectionString) : this(connectionString, null)
        {
        }

        public MongoContext(string connectionString, WriteConcern writeConcern)
        {
            this._writeConcern = writeConcern;

            object[] mongoDbAttr = this.GetType().GetCustomAttributes(typeof(MongoDatabaseAttribute), false);

            if (!mongoDbAttr.HasItems())
            {
                throw new ArgumentException("Classes derived from MongoContext must be decorated with the MongoDatabase attribute");
            }

            _databaseName = ((MongoDatabaseAttribute)mongoDbAttr[0]).DatabaseName;

            if (string.IsNullOrEmpty(_databaseName))
            {
                throw new ArgumentException("DatabaseName cannot be null or empty");
            }
            
            _connectionStringBuilder = new MongoUrlBuilder(connectionString);
            _connectionStringBuilder.DatabaseName = _databaseName;
            _client = new MongoClient(_connectionStringBuilder.ToMongoUrl());
        }

        public MongoContext(string connectionString, string databaseName, WriteConcern writeConcern)
        {
            this._writeConcern = writeConcern;

            _databaseName = databaseName;

            if (string.IsNullOrEmpty(_databaseName))
            {
                throw new ArgumentException("DatabaseName cannot be null or empty");
            }

            _connectionStringBuilder = new MongoUrlBuilder(connectionString);
            _connectionStringBuilder.DatabaseName = _databaseName;
            _client = new MongoClient(_connectionStringBuilder.ToMongoUrl());
        }
        #endregion

        #region Phytel Services Connection Strings
        #region Using PhytelServices Connection from Configuration "PhytelServicesConnName" setting
        public MongoContext(string database, bool isContract, string systemType) : this(database, isContract, false, systemType)
        {
        }

        public MongoContext(string database, bool isContract) : this(database, isContract, false, _defaultSystemType)
        {
        }

        public MongoContext(string database, bool isContract, bool asAdmin) : this(database, isContract, asAdmin, _defaultSystemType)
        {
        }

        public MongoContext(string database, bool isContract, bool asAdmin, string systemType)
        {
            _connectionStringBuilder = new MongoUrlBuilder(MongoService.Instance.GetConnectionString(database, isContract, asAdmin, systemType));
            _databaseName = _connectionStringBuilder.DatabaseName;
            _client = new MongoClient(_connectionStringBuilder.ToMongoUrl());
        }
        #endregion

        #region Using custom dbName for PhytelServices Connection Name
        public MongoContext(string dbConnName, string database, bool isContract, string systemType) : this(dbConnName, database, isContract, false, systemType)
        {
        }

        public MongoContext(string dbConnName, string database, bool isContract) : this(dbConnName, database, isContract, false, _defaultSystemType)
        {
        }

        public MongoContext(string dbConnName, string database, bool isContract, bool asAdmin) : this(dbConnName, database, isContract, asAdmin, _defaultSystemType)
        {
        }

        public MongoContext(string dbConnName, string database, bool isContract, bool asAdmin, string systemType)
        {
            _connectionStringBuilder = new MongoUrlBuilder(MongoService.Instance.GetConnectionString(dbConnName, database, isContract, asAdmin, systemType));
            _databaseName = _connectionStringBuilder.DatabaseName;
            _client = new MongoClient(_connectionStringBuilder.ToMongoUrl());
        }
        #endregion
        #endregion

        public MongoContext(MongoUrlBuilder connectionProperties)
        {
            _databaseName = connectionProperties.DatabaseName;
            _client = new MongoClient(connectionProperties.ToMongoUrl());
        }

        #region IDisposable methods

        public void Dispose()
        {
            _connectionStringBuilder = null;
            _writeConcern = null;
            _databaseName = null;
        }

        #endregion

        public WriteConcern WriteConcern
        {
            get 
            {
                return _writeConcern;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentException("WriteConcern cannot be set to null");
                }

                _writeConcern = value;
            }
        }

        public MongoDatabase GetDatabase()
        {
            return _client.GetServer().GetDatabase(_databaseName);
        }
    }
}