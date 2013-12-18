using System.Data;

namespace Phytel.Framework.SQL.Data
{
	public delegate T Convert<T>( object scalar );

	public interface IDataModifier
	{
		T Execute<T>( IDbCommand executor, Convert<T> converter );
	}
}
