using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Phytel.Framework.SQL.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Phytel.Framework.SQL.Data
{
    /// <summary>
    /// Implementation of a data context for MSSQL.  This class relies on the Microsoft 
    /// Enterprise Data Application Block.
    /// </summary>
    public class SqlDataContext : IDataContext
    {
    	private readonly string _dbName;
        private readonly string _connectionString;
        private readonly Database _db;
        private int _queryLimit;
        private bool _useQueryLimit;
        private int _sqlTimeout;

        public SqlDataContext(string connectionString, string database)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                _connectionString = Phytel.Services.SQLDataService.Instance.GetConnectionString(database, false);
            }
            else
            {
                _connectionString = connectionString;
            }

            _dbName = database;
            _db = new SqlDatabase(_connectionString);

            _sqlTimeout = ConfigurationUtils.GetConfigInt("framework.sql.timeout", 30);
            _queryLimit = ConfigurationUtils.GetConfigInt("framework.sql.queryLimit");
        }
        public string DbName
    	{
    		get { return _dbName; }
    	}

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public bool UseQueryLimit
        {
            get { return _useQueryLimit; }
            set { _useQueryLimit = value; }
        }

    	public int SqlTimeout
    	{
    		get { return _sqlTimeout; }
    		set { _sqlTimeout = value; }
    	}

		/// <summary>
		/// Executes a reader, optionally applying a query limit if the context
		/// has one defined.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
    	public IDataReader ExecuteReader(IDbCommand command)
        {
            if( _useQueryLimit )
            {
                _db.AddInParameter((DbCommand)command, "@queryLimit", DbType.Int32, _queryLimit );
            }

            return _db.ExecuteReader((DbCommand)command);
        }

		public IDbCommand CreateQuery( string query )
		{
			IDbCommand command = _db.GetSqlStringCommand( query );
			command.CommandTimeout = _sqlTimeout;

			return command;
		}

        public IDbCommand CreateCommand( string sproc )
        {
            IDbCommand command = _db.GetStoredProcCommand(sproc);
            command.CommandTimeout = _sqlTimeout;

            return command;
        }

        public IDbCommand CreateCommand(string sproc, params object[] args)
        {
            IDbCommand command = _db.GetStoredProcCommand(sproc, args);
            command.CommandTimeout = _sqlTimeout;

            return command;
        }

        public void AddInParameter(IDbCommand command, string name, DbType dbType, object value)
        {
            _db.AddInParameter((DbCommand)command, name, dbType, value);
        }

        public void AddOutParameter(IDbCommand command, string name, DbType dbType, int size)
        {
            _db.AddOutParameter((DbCommand)command, name, dbType, size);
        }

		public object GetParameter( IDbCommand command, string name )
		{
			return _db.GetParameterValue( (DbCommand) command, name );
		}

        public DataSet ExecuteDataSet(IDbCommand command)
        {
            return _db.ExecuteDataSet((DbCommand) command);
        }

        public object ExecuteScalar(IDbCommand command)
        {
            return _db.ExecuteScalar((DbCommand) command);
        }

        public int ExecuteNonQuery(IDbCommand command)
        {
            return _db.ExecuteNonQuery((DbCommand) command);
        }

		/// <summary>
		/// Executes all the given commands in a single connection-scoped transaction.
		/// </summary>
		/// <param name="commands"></param>
        public void ExecuteNonQuery(List<IDbCommand> commands)
        {
            using( TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew) )
            {
                foreach( IDbCommand command in commands )
                {
                    _db.ExecuteNonQuery((DbCommand) command);
                }

                scope.Complete();
            }
        }
    }
}
