using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.Services.Mongo;
using Phytel.Services.Utility;
using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace Phytel.Services.Web
{
    /// <summary>
    /// Custom ASP.NET Session State Provider using MongoDB as the state store.
    /// For reference on this implementation see MSDN ref:
    ///     - http://msdn.microsoft.com/en-us/library/ms178587.aspx
    ///     - http://msdn.microsoft.com/en-us/library/ms178588.aspx - this sample provider was used as the basis for this
    ///       provider, with MongoDB-specific implementation swapped in, plus cosmetic changes like naming conventions.
    /// 
    /// Session state is stored in a "Sessions" collection within a "SessionState" database. Example session document:
    /// {
    ///    "_id" : "bh54lskss4ycwpreet21dr1h",
    ///    "ApplicationName" : "/",
    ///    "Created" : ISODate("2011-04-29T21:41:41.953Z"),
    ///    "Expires" : ISODate("2011-04-29T22:01:41.953Z"),
    ///    "LockDate" : ISODate("2011-04-29T21:42:02.016Z"),
    ///    "LockId" : 1,
    ///    "Timeout" : 20,
    ///    "Locked" : true,
    ///    "SessionItems" : "AQAAAP////8EVGVzdAgAAAABBkFkcmlhbg==",
    ///    "Flags" : 0
    /// }
    /// 
    /// Inline with the above MSDN reference:
    /// If the provider encounters an exception when working with the data source, it writes the details of the exception 
    /// to the Application Event Log instead of returning the exception to the ASP.NET application. This is done as a security 
    /// measure to avoid private information about the data source from being exposed in the ASP.NET application.
    /// The sample provider specifies an event Source property value of "MongoSessionStateStore." Before your ASP.NET 
    /// application will be able to write to the Application Event Log successfully, you will need to create the following registry key:
    ///     HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Eventlog\Application\MongoSessionStateStore
    /// If you do not want the sample provider to write exceptions to the event log, then you can set the custom writeExceptionsToEventLog 
    /// attribute to false in the Web.config file.
    ///
    /// The session-state store provider does not provide support for the Session_OnEnd event, it does not automatically clean up expired session-item data. 
    /// You should have a job to periodically delete expired session information from the data store where Expires date is in the past, i.e.:
    ///     db.Sessions.remove({"Expires" : {$lt : new Date() }})
    /// 
    /// Example web.config settings:
    ///  
    ///  <connectionStrings>
    ///     <add name="MongoSessionServices"
    ///        connectionString="mongodb://localhost" />
    ///  </connectionStrings>
    ///  <system.web>
    ///     <sessionState
    ///         mode="Custom"
    ///         customProvider="MongoSessionStateProvider">
    ///             <providers>
    ///                 <add name="MongoSessionStateProvider"
    ///                     type="MongoSessionStateStore.MongoSessionStateStore"
    ///                     connectionStringName="MongoSessionServices"
    ///                     writeExceptionsToEventLog="false"
    ///                     fsync="false"
    ///                     replicasToWrite="0" />
    ///             </providers>
    ///     </sessionState>
    ///     ...
    /// </system.web>
    /// </summary>
    public sealed class MongoSessionStateStore : SessionStateStoreProviderBase
    {
        // collection property constants
        public const string _IdProperty = "_id";
        public const string ApplicationNameProperty = "ApplicationName";
        public const string CreatedProperty = "Created";
        public const string ExpiresProperty = "Expires";
        public const string LockDateProperty = "LockDate";
        public const string LockIdProperty = "LockId";
        public const string TimeoutProperty = "Timeout";
        public const string LockedProperty = "Locked";
        public const string SessionItemsProperty = "SessionItems";
        public const string FlagsProperty = "Flags";

        private static SessionStateSection _config = null;
        private static string _connectionString;

        private string _applicationName;

        private bool _writeExceptionsToEventLog;
        private const string _exceptionMessage = "An exception occurred. Please contact your administrator.";
        private const string _eventSource = "MongoSessionStateStore";
        private const string _eventLog = "Application";
        private WriteConcern _writeConcern = null;

        /// <summary>
        /// Do the overall database setup on first touch of the class. This will ensure that index checks etc only happens once per process
        /// </summary>
        static MongoSessionStateStore()
        {
            _config = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
            _connectionString = MongoService.Instance.GetConnectionString("ASPSessionState", false);
        }

        /// <summary>
        /// The ApplicationName property is used to differentiate sessions
        /// in the data source by application.
        ///</summary>
        public string ApplicationName
        {
            get { return _applicationName; }
        }

        /// <summary>
        /// If false, exceptions are thrown to the caller. If true,
        /// exceptions are written to the event log. 
        /// </summary>
        public bool WriteExceptionsToEventLog
        {
            get { return _writeExceptionsToEventLog; }
            set { _writeExceptionsToEventLog = value; }
        }

        /// <summary>
        /// Initialise the session state store.
        /// </summary>
        /// <param name="name">session state store name. Defaults to "MongoSessionStateStore" if not supplied</param>
        /// <param name="config">configuration settings</param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            
            // Initialize values from web.config.
            if (config == null)
            {
                System.Diagnostics.Debug.WriteLine("Config is null");
                throw new ArgumentNullException("config");
            }

            if (name == null || name.Length == 0)
            {
                name = "MongoSessionStateStore";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "MongoDB Session State Store provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            // Initialize the ApplicationName property.
            _applicationName = "/"; //System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;

            // Initialize WriteExceptionsToEventLog
            _writeExceptionsToEventLog = false;

            if (config["writeExceptionsToEventLog"] != null)
            {
                if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
                    _writeExceptionsToEventLog = true;
            }

            // Initialise safe mode options. Defaults to Safe Mode=true, fsynch=false, w=0 (replicas to write to before returning)
            bool safeModeEnabled = true;

            bool fsync = false;
            if (config["fsync"] != null)
            {
                if (config["fsync"].ToUpper() == "TRUE")
                    fsync = true;
            }

            int replicasToWrite = 0;
            if (config["replicasToWrite"] != null)
            {
                if (!int.TryParse(config["replicasToWrite"], out replicasToWrite))
                    throw new ProviderException("Replicas To Write must be a valid integer");
            }

            if (safeModeEnabled)
            {
                _writeConcern = WriteConcern.Acknowledged;
                //_writeConcern.FSync = fsync;
            }
            else
                _writeConcern = WriteConcern.Unacknowledged;
        }

        public override SessionStateStoreData CreateNewStoreData(System.Web.HttpContext context, int timeout)
        {
            return new SessionStateStoreData(new SessionStateItemCollection(),
                SessionStateUtility.GetSessionStaticObjects(context),
                timeout);
        }

        /// <summary>
        /// SessionStateProviderBase.SetItemExpireCallback
        /// </summary>
        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            return false;
        }

        /// <summary>
        /// Serialize is called by the SetAndReleaseItemExclusive method to 
        /// convert the SessionStateItemCollection into a Base64 string to    
        /// be stored in MongoDB.
        /// </summary>
        private string Serialize(SessionStateItemCollection items)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                if (items != null)
                    items.Serialize(writer);

                writer.Close();

                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// SessionStateProviderBase.SetAndReleaseItemExclusive
        /// </summary>
        public override void SetAndReleaseItemExclusive(HttpContext context,
          string id,
          SessionStateStoreData item,
          object lockId,
          bool newItem)
        {
            // Serialize the SessionStateItemCollection as a string.
            string sessItems = Serialize((SessionStateItemCollection)item.Items);

            using (var dbWrapper = GetDatabaseWrapper())
            {
                BsonDocument insertDoc = null;

                try
                {
                    if (newItem)
                    {
                        insertDoc = new BsonDocument();
                        insertDoc.Add(_IdProperty, id);
                        insertDoc.Add(ApplicationNameProperty, ApplicationName);
                        insertDoc.Add(CreatedProperty, DateTime.Now.ToUniversalTime());
                        insertDoc.Add(ExpiresProperty, DateTime.Now.AddMinutes((Double)item.Timeout).ToUniversalTime());
                        insertDoc.Add(LockDateProperty, DateTime.Now.ToUniversalTime());
                        insertDoc.Add(LockIdProperty, 0);
                        insertDoc.Add(TimeoutProperty, item.Timeout);
                        insertDoc.Add(LockedProperty, false);
                        insertDoc.Add(SessionItemsProperty, sessItems);
                        insertDoc.Add(FlagsProperty, 0);

                        var query = Query.And(Query.EQ(_IdProperty, id), Query.EQ(ApplicationNameProperty, ApplicationName), Query.LT(ExpiresProperty, DateTime.Now.ToUniversalTime()));
                        RetryHelper.DoWithRetry(() => dbWrapper.SessionCollection.Remove(query, _writeConcern), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);
                        RetryHelper.DoWithRetry(() => dbWrapper.SessionCollection.Insert(insertDoc, _writeConcern), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);
                    }
                    else
                    {
                        var query = Query.And(Query.EQ(_IdProperty, id), Query.EQ(ApplicationNameProperty, ApplicationName), Query.EQ(LockIdProperty, (Int32)lockId));
                        var update = Update.Set(ExpiresProperty, DateTime.Now.AddMinutes((Double)item.Timeout).ToUniversalTime());
                        update.Set(SessionItemsProperty, sessItems);
                        update.Set(LockedProperty, false);
                        RetryHelper.DoWithRetry(() => dbWrapper.SessionCollection.Update(query, update, _writeConcern), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);
                    }
                }
                catch (Exception e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "SetAndReleaseItemExclusive");
                        throw new ProviderException(_exceptionMessage);
                    }
                    else
                        throw;
                }
            }
        }

        /// <summary>
        /// SessionStateProviderBase.GetItem
        /// </summary>
        public override SessionStateStoreData GetItem(HttpContext context,
          string id,
          out bool locked,
          out TimeSpan lockAge,
          out object lockId,
          out SessionStateActions actionFlags)
        {
            return GetSessionStoreItem(false, context, id, out locked,
              out lockAge, out lockId, out actionFlags);
        }

        /// <summary>
        /// SessionStateProviderBase.GetItemExclusive
        /// </summary>
        public override SessionStateStoreData GetItemExclusive(HttpContext context,
          string id,
          out bool locked,
          out TimeSpan lockAge,
          out object lockId,
          out SessionStateActions actionFlags)
        {
            return GetSessionStoreItem(true, context, id, out locked,
              out lockAge, out lockId, out actionFlags);
        }

        /// <summary>
        /// GetSessionStoreItem is called by both the GetItem and 
        /// GetItemExclusive methods. GetSessionStoreItem retrieves the 
        /// session data from the data source. If the lockRecord parameter
        /// is true (in the case of GetItemExclusive), then GetSessionStoreItem
        /// locks the record and sets a new LockId and LockDate.
        /// </summary>
        private SessionStateStoreData GetSessionStoreItem(bool lockRecord,
          HttpContext context,
          string id,
          out bool locked,
          out TimeSpan lockAge,
          out object lockId,
          out SessionStateActions actionFlags)
        {
            // Initial values for return value and out parameters.
            SessionStateStoreData item = null;
            lockAge = TimeSpan.Zero;
            lockId = null;
            locked = false;
            actionFlags = 0;

            using (var dbWrapper = GetDatabaseWrapper())
            {
                // DateTime to check if current session item is expired.
                DateTime expires;
                // String to hold serialized SessionStateItemCollection.
                string serializedItems = "";
                // True if a record is found in the database.
                bool foundRecord = false;
                // True if the returned session item is expired and needs to be deleted.
                bool deleteData = false;
                // Timeout value from the data store.
                int timeout = 0;
                IMongoQuery query;
                try
                {
                    // lockRecord is true when called from GetItemExclusive and
                    // false when called from GetItem.
                    // Obtain a lock if possible. Ignore the record if it is expired.
                    if (lockRecord)
                    {
                        query = Query.And(
                                    Query.EQ(_IdProperty, id),
                                    Query.EQ(ApplicationNameProperty, ApplicationName),
                                    Query.EQ(LockedProperty, false),
                                    Query.GT(ExpiresProperty, DateTime.Now.ToUniversalTime()));
                        var update = Update.Set(LockedProperty, true);
                        update.Set(LockDateProperty, DateTime.Now.ToUniversalTime());
                        var result = RetryHelper.DoWithRetry(() => dbWrapper.SessionCollection.Update(query, update, _writeConcern), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);

                        if (result.DocumentsAffected == 0)
                        {
                            // No record was updated because the record was locked or not found.
                            locked = true;
                        }
                        else
                        {
                            // The record was updated.
                            locked = false;
                        }
                    }

                    // Retrieve the current session item information.
                    query = Query.And(Query.EQ(_IdProperty, id), Query.EQ(ApplicationNameProperty, ApplicationName));
                    var results = RetryHelper.DoWithRetry<BsonDocument>(() => dbWrapper.SessionCollection.FindOneAs<BsonDocument>(query), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);

                    if (results != null)
                    {
                        expires = results[ExpiresProperty].ToUniversalTime();

                        if (expires < DateTime.Now.ToUniversalTime())
                        {
                            // The record was expired. Mark it as not locked.
                            locked = false;
                            // The session was expired. Mark the data for deletion.
                            deleteData = true;
                        }
                        else
                            foundRecord = true;

                        serializedItems = results[SessionItemsProperty].AsString;
                        lockId = results[LockIdProperty].AsInt32;
                        lockAge = DateTime.Now.ToUniversalTime().Subtract(results[LockDateProperty].ToUniversalTime());
                        actionFlags = (SessionStateActions)results[FlagsProperty].AsInt32;
                        timeout = results[TimeoutProperty].AsInt32;
                    }

                    // If the returned session item is expired, 
                    // delete the record from the data source.
                    if (deleteData)
                    {
                        query = Query.And(Query.EQ(_IdProperty, id), Query.EQ(ApplicationNameProperty, ApplicationName));
                        RetryHelper.DoWithRetry(() => dbWrapper.SessionCollection.Remove(query, _writeConcern), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);
                    }

                    // The record was not found. Ensure that locked is false.
                    if (!foundRecord)
                        locked = false;

                    // If the record was found and you obtained a lock, then set 
                    // the lockId, clear the actionFlags,
                    // and create the SessionStateStoreItem to return.
                    if (foundRecord && !locked)
                    {
                        lockId = (int)lockId + 1;

                        query = Query.And(Query.EQ(_IdProperty, id), Query.EQ(ApplicationNameProperty, ApplicationName));
                        var update = Update.Set(LockIdProperty, (int)lockId);
                        update.Set("Flags", 0);
                        RetryHelper.DoWithRetry(() => dbWrapper.SessionCollection.Update(query, update, _writeConcern), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);

                        // If the actionFlags parameter is not InitializeItem, 
                        // deserialize the stored SessionStateItemCollection.
                        if (actionFlags == SessionStateActions.InitializeItem)
                            item = CreateNewStoreData(context, (int)_config.Timeout.TotalMinutes);
                        else
                            item = Deserialize(context, serializedItems, timeout);
                    }
                }
                catch (Exception e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "GetSessionStoreItem");
                        throw new ProviderException(_exceptionMessage);
                    }
                    else
                        throw;
                }
            }

            return item;
        }

        private SessionStateStoreData Deserialize(HttpContext context,
         string serializedItems, int timeout)
        {
            using (MemoryStream ms =
              new MemoryStream(Convert.FromBase64String(serializedItems)))
            {
                SessionStateItemCollection sessionItems =
                  new SessionStateItemCollection();

                if (ms.Length > 0)
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        sessionItems = SessionStateItemCollection.Deserialize(reader);
                    }
                }

                return new SessionStateStoreData(sessionItems,
                  SessionStateUtility.GetSessionStaticObjects(context),
                  timeout);
            }
        }

        public override void CreateUninitializedItem(System.Web.HttpContext context, string id, int timeout)
        {
            using (var dbWrapper = GetDatabaseWrapper())
            {
                BsonDocument doc = new BsonDocument();
                doc.Add(_IdProperty, id);
                doc.Add(ApplicationNameProperty, ApplicationName);
                doc.Add(CreatedProperty, DateTime.Now.ToUniversalTime());
                doc.Add(ExpiresProperty, DateTime.Now.AddMinutes((Double)timeout).ToUniversalTime());
                doc.Add(LockDateProperty, DateTime.Now.ToUniversalTime());
                doc.Add(LockIdProperty, 0);
                doc.Add(TimeoutProperty, timeout);
                doc.Add(LockedProperty, false);
                doc.Add(SessionItemsProperty, "");
                doc.Add(FlagsProperty, 1);

                try
                {
                    var result = RetryHelper.DoWithRetry<WriteConcernResult>(() => dbWrapper.SessionCollection.Save(doc, _writeConcern), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);
                    if (!result.Ok)
                    {
                        throw new Exception(result.ErrorMessage);
                    }
                }
                catch (Exception e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "CreateUninitializedItem");
                        throw new ProviderException(_exceptionMessage);
                    }
                    else
                        throw;
                }
            }
        }

        /// <summary>
        /// This is a helper function that writes exception detail to the 
        /// event log. Exceptions are written to the event log as a security
        /// measure to ensure private database details are not returned to 
        /// browser. If a method does not return a status or Boolean
        /// indicating the action succeeded or failed, the caller also 
        /// throws a generic exception.
        /// </summary>
        private void WriteToEventLog(Exception e, string action)
        {
            using (EventLog log = new EventLog())
            {
                log.Source = _eventSource;
                log.Log = _eventLog;

                string message =
                  String.Format("An exception occurred communicating with the data source.\n\nAction: {0}\n\nException: {1}",
                  action, e.ToString());

                log.WriteEntry(message);
            }
        }

        public override void Dispose()
        {
        }

        public override void EndRequest(System.Web.HttpContext context)
        {

        }

        public override void InitializeRequest(System.Web.HttpContext context)
        {

        }

        public override void ReleaseItemExclusive(System.Web.HttpContext context, string id, object lockId)
        {
            using (var dbWrapper = GetDatabaseWrapper())
            {
                var query = Query.And(Query.EQ(_IdProperty, id), Query.EQ(ApplicationNameProperty, ApplicationName), Query.EQ(LockIdProperty, (Int32)lockId));
                var update = Update.Set("Locked", false);
                update.Set("Expires", DateTime.Now.AddMinutes(_config.Timeout.TotalMinutes).ToUniversalTime());

                try
                {
                    RetryHelper.DoWithRetry(() => dbWrapper.SessionCollection.Update(query, update, _writeConcern), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);
                }
                catch (Exception e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "ReleaseItemExclusive");
                        throw new ProviderException(_exceptionMessage);
                    }
                    else
                        throw;
                }
            }
        }

        public override void RemoveItem(System.Web.HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            using (var dbWrapper = GetDatabaseWrapper())
            {
                var query = Query.And(Query.EQ(_IdProperty, id), Query.EQ(ApplicationNameProperty, ApplicationName), Query.EQ(LockIdProperty, (Int32)lockId));

                try
                {
                    RetryHelper.DoWithRetry(() => dbWrapper.SessionCollection.Remove(query, _writeConcern), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);
                }
                catch (Exception e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "RemoveItem");
                        throw new ProviderException(_exceptionMessage);
                    }
                    else
                        throw;
                }
            }
        }

        public override void ResetItemTimeout(System.Web.HttpContext context, string id)
        {
            var query = Query.And(Query.EQ(_IdProperty, id), Query.EQ(ApplicationNameProperty, ApplicationName));
            var update = Update.Set(ExpiresProperty, DateTime.Now.AddMinutes(_config.Timeout.TotalMinutes).ToUniversalTime());

            using (var dbWrapper = GetDatabaseWrapper())
            {
                try
                {
                    RetryHelper.DoWithRetry(() => dbWrapper.SessionCollection.Update(query, update, _writeConcern), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);
                }
                catch (Exception e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "ResetItemTimeout");
                        throw new ProviderException(_exceptionMessage);
                    }
                    else
                        throw;
                }
            }
        }

        private static MongoSessionStateDatabaseWrapper GetDatabaseWrapper()
        {
            return new MongoSessionStateDatabaseWrapper(_connectionString);
        }
    }

    public class MongoSessionStateDatabaseWrapper : IDisposable
    {
        private const string DATABASENAME = "ASPSessionState";
        private const string SESSIONCOLLECTION = "Sessions";
        private const string CACHECOLLECTION = "Cache";

        public MongoSessionStateDatabaseWrapper(string connectionString)
        {
            RetryHelper.DoWithRetry(() =>
            {
                _connection = new MongoClient(connectionString).GetServer();
                _database = _connection.GetDatabase(DATABASENAME);
                _sessionCollection = _database.GetCollection(SESSIONCOLLECTION);
            },
                    RetryHelper.RETRIES,
                    RetryHelper.RETRYDELAY);

            EnsureIndex();
        }

        private MongoServer _connection;

        public MongoServer Connection
        {
            get
            {
                return _connection;
            }

            set
            {
                _connection = value;
            }
        }

        private MongoDatabase _database;

        public MongoDatabase Database
        {
            get
            {
                return _database;
            }

            set
            {
                _database = value;
            }
        }

        private MongoCollection<BsonDocument> _sessionCollection;

        public MongoCollection<BsonDocument> SessionCollection
        {
            get
            {
                return _sessionCollection;
            }

            set
            {
                _sessionCollection = value;
            }
        }

        public void Dispose()
        {
        //    // TJD: DO NOT CALL DISCONNECT ON SERVER OBJECT (PER MONGODB TEAM) AS THIS DESTROYS ALL CONNECTION INFORMATION AND LOSES CONNECTION POOLING INFORMATION

        //    //if (_connection.State == MongoServerState.Connected ||
        //    //    _connection.State == MongoServerState.ConnectedToSubset ||
        //    //    _connection.State == MongoServerState.Connecting)
        //    //{
        //    //    RetryHelper.DoWithRetry(() => _connection.Disconnect(), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);
        //    //}
        }

        private void EnsureIndex()
        {
            IndexKeysBuilder keyBuilder = new IndexKeysBuilder();
            keyBuilder.Ascending(new string[] { MongoSessionStateStore.ExpiresProperty });

            IndexOptionsBuilder optionsBuilder = new IndexOptionsBuilder();
            optionsBuilder.SetTimeToLive(new TimeSpan());

            // Add TTL index
            RetryHelper.DoWithRetry(() => SessionCollection.EnsureIndex(keyBuilder, optionsBuilder), RetryHelper.RETRIES, RetryHelper.RETRYDELAY);
        }
    }
}