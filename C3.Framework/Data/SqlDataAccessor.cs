using System;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace C3.Framework.Data
{
	/// <summary>
	/// Implementation of a data accessor for MSSQL, with conveniences for the
	/// client database structure.
	/// </summary>
	/// <author>Fred Fulcher</author>
	public class SqlDataAccessor : DataAccessor
	{
		// Define these one per database
        //public static readonly string C3DB = "PortalSQLServer";		

        public ITypeReader QueryReader(String connectionString, String database, String procedure, params Object[] args)
		{
            IDbCommand command = GetCommand(connectionString, database, procedure, args);

			return FindReader(command);
		}

        public DataSet QueryDataSet(string connectionString, string database, string procedure, params object[] args)
        {
            IDbCommand command = GetCommand(connectionString, database, procedure, args);
            return FindDataSet(command);

        }

        public DataTable Query(string connectionString, string database, string procedure, params object[] args)
        {
            IDbCommand command = GetCommand(connectionString, database, procedure, args);
            return Find(command);
        }

        /// <summary>
		/// Returns a single item of a given type as a result of the invocation of a
		/// stored procedure on the specified database.  Accepts 0 or more parameters.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="database"></param>
		/// <param name="procedure"></param>
		/// <param name="builder"></param>
		/// <param name="args"></param>
		/// <returns></returns>
        public T Query<T>(string connectionString, string database, string procedure, Build<T> builder, params object[] args)
		{
            IDbCommand command = GetCommand(connectionString, database, procedure, args);

			return Find( command, builder );
		}

        /// <summary>
		/// Same as Query, but caches the result.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="database"></param>
		/// <param name="procedure"></param>
		/// <param name="builder"></param>
		/// <param name="cacher"></param>
		/// <param name="args"></param>
		/// <returns></returns>
        public T CachedQuery<T>(string connectionString, string database, string procedure, Build<T> builder, CacheAccessor cacher, params object[] args)
		{
			if( cacher.HasCacheResult() )
			{
				return cacher.GetCacheResult<T>();
			}

            IDbCommand command = GetCommand(connectionString, database, procedure, args);

			return Find( command, builder, cacher );
		}

		/// <summary>
		/// Returns a list of items of the given type as a result of the invocation
		/// of a stored procedure on the specified database.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="database"></param>
		/// <param name="procedure"></param>
		/// <param name="builder"></param>
		/// <param name="args"></param>
		/// <returns></returns>
        public List<T> QueryAll<T>(string connectionString, string database, string procedure, Build<T> builder, params object[] args)
		{
            IDbCommand command = null;
            if (args.Length == 0)
            {
                command = GetCommand(connectionString, database, procedure);
            }
            else
            {
                command = GetCommand(connectionString, database, procedure, args);
            }

			return FindAll( command, builder );
		}

        /// <summary>
		/// Same as QueryAll but cached.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="database"></param>
		/// <param name="procedure"></param>
		/// <param name="builder"></param>
		/// <param name="cacher"></param>
		/// <param name="args"></param>
		/// <returns></returns>
        public List<T> CachedQueryAll<T>(string connectionString, string database, string procedure, Build<T> builder, CacheAccessor cacher, params object[] args)
		{
			if( cacher.HasCacheResult() )
			{
				return cacher.GetCacheListResult<T>();
			}

            IDbCommand command = GetCommand(connectionString, database, procedure, args);

			return FindAll( command, builder, cacher );
		}

		/// <summary>
		/// Executes a stored procedure that returns multiple result sets, applying 
		/// the builders positionally (one builder per result set).
		/// </summary>
		/// <param name="database"></param>
		/// <param name="procedure"></param>
		/// <param name="builders"></param>
		/// <param name="args"></param>
		/// <returns></returns>
        public IList QueryAll(string connectionString, string database, string procedure, List<Build> builders, params object[] args)
		{
            IDbCommand command = GetCommand(connectionString, database, procedure, args);

			return FindAll( command, builders );
		}

        public IList CachedQueryAll(string connectionString, string database, string procedure, List<Build> builders, CacheAccessor cacher, params object[] args)
		{
			if( cacher.HasCacheResult() )
			{
				return cacher.GetCacheResult<IList>();
			}

            IDbCommand command = GetCommand(connectionString, database, procedure, args);

			return FindAll( command, builders, cacher );
		}

        public DataTable CachedQuery(string connectionString, string database, string procedure, CacheAccessor cacher, params object[] args)
        {
            if (cacher.HasCacheResult())
            {
                return cacher.GetCacheResult<DataTable>();
            }

            IDbCommand command = GetCommand(connectionString, database, procedure, args);

            return Find(command, cacher);
        }

		/// <summary>
		/// Returns a dictionary based on the invocation of a stored procedure.
		/// </summary>
		/// <typeparam name="K"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="database"></param>
		/// <param name="procedure"></param>
		/// <param name="builder"></param>
		/// <param name="keyBuilder"></param>
		/// <param name="args"></param>
		/// <returns></returns>
        public Dictionary<K, V> QueryAll<K, V>(string connectionString, string database, string procedure, Build<V> builder, Build<K> keyBuilder, params object[] args)
		{
            IDbCommand command = GetCommand(connectionString, database, procedure, args);

			return FindAll( command, builder, keyBuilder );
		}

        public Dictionary<K, V> CachedQueryAll<K, V>(string connectionString, string database, string procedure, Build<V> builder, Build<K> keyBuilder, CacheAccessor cacher, params object[] args)
		{
			if( cacher.HasCacheResult() )
			{
				return cacher.GetCacheDictionaryResult<K,V>();
			}

            IDbCommand command = GetCommand(connectionString, database, procedure, args);

			return FindAll( command, builder, keyBuilder, cacher );
		}

		/// <summary>
		/// Retrieves the appropriate SQL data context for the given database.
		/// </summary>
		/// <param name="database"></param>
		/// <returns></returns>
        protected SqlDataContext SqlContext(string connectionString, string database)
        {
            SqlDataContext context = Context as SqlDataContext;

            if (context == null || context.DbName != database || context.ConnectionString != connectionString)
            {
                Context = context = new SqlDataContext(connectionString, database);
            }

            return context;
        }

        /// <summary>
		/// Initializes a new dynamic SQL command against the given database.
		/// </summary>
		/// <param name="database"></param>
		/// <param name="sql"></param>
		/// <returns></returns>
        protected IDbCommand GetCommand(string connectionString, string database, string sql)
		{
			return TimedCommand( SqlContext(connectionString, database).CreateQuery( sql ) );
		}

		/// <summary>
		/// Initializes a new stored procedure command against the given database.
		/// </summary>
		/// <param name="database"></param>
		/// <param name="procedure"></param>
		/// <param name="args"></param>
		/// <returns></returns>
        protected IDbCommand GetCommand(string connectionString, string database, string procedure, params object[] args)
		{
            if ( args == null)
                return TimedCommand(SqlContext(connectionString, database).CreateCommand(procedure));
            else
                return TimedCommand(SqlContext(connectionString, database).CreateCommand(procedure, args));
		}

		protected IDbCommand TimedCommand( IDbCommand command )
		{
			if( Timeout > 0 )
			{
				command.CommandTimeout = Timeout;
			}

			return command;
		}

       

	}
}
