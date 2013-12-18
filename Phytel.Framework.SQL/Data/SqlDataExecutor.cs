using System.Collections.Generic;
using System.Data;

namespace Phytel.Framework.SQL.Data
{
	/// <summary>
	/// Summary description for SqlDataExecutor.
	/// </summary>
	/// <author>Fred Fulcher</author>
	public class SqlDataExecutor : SqlDataAccessor, IDataModifier
	{
		/// <summary>
		/// Executes a command and applies the given converter to its result.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="executor"></param>
		/// <param name="converter"></param>
		/// <returns></returns>
		public T Execute<T>( IDbCommand executor, Convert<T> converter )
		{
			return converter( Context.ExecuteScalar( executor ) );
		}

		/// <summary>
		/// Executes a stored procedure against a given database, applying
		/// a converter to the scalar result.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="db"></param>
		/// <param name="sproc"></param>
		/// <param name="converter"></param>
		/// <param name="args"></param>
		/// <returns></returns>
        public T Execute<T>(string connectionString, string db, string sproc, Convert<T> converter, params object[] args)
		{
            return Execute(GetCommand(connectionString, db, sproc, args), converter);
		}

		/// <summary>
		/// Executes a stored procedure command against a given database without
		/// processing its result.
		/// </summary>
		/// <param name="db"></param>
		/// <param name="sproc"></param>
		/// <param name="args"></param>
        public void Execute(string connectionString, string db, string sproc, params object[] args)
		{
            SqlContext(connectionString, db).ExecuteNonQuery(GetCommand(connectionString, db, sproc, args));
		}

        public void Execute(string db, string sproc, params object[] args)
        {
            SqlContext(null, db).ExecuteNonQuery(GetCommand(null, db, sproc, args));
        }
        
        /// <summary>
		/// Executes an arbitrary command without processing its result.
		/// </summary>
		/// <param name="executor"></param>
		public void Execute( IDbCommand executor )
		{
			Context.ExecuteNonQuery( executor );
		}

		/// <summary>
		/// Executes a set of arbitrary commands in a single transaction
		/// </summary>
		public void Execute( List<IDbCommand> commands )
		{
			if( commands != null && commands.Count > 0 )
			{
				Context.ExecuteNonQuery( commands );
			}
		}
	}
}
