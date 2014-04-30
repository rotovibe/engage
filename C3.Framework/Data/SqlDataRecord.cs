using System.Collections.Generic;
using System.Data;

namespace C3.Framework.Data
{
	/// <summary>
	/// Summary description for SqlDataRecord.
	/// </summary>
	/// <author>Fred Fulcher</author>
	public abstract class SqlDataRecord : SqlDataExecutor
	{
		protected Dictionary<string, bool> _dirtyTable;
		protected bool _trackChanges;

        //public abstract List<IDbCommand> SaveCommands { get; }
        //public abstract List<IDbCommand> RemoveCommands { get; }

		/// <summary>
		/// Determines if any member of this listing has changed.
		/// </summary>
		/// <returns></returns>
		public bool IsChanged()
		{
			if( _dirtyTable != null )
			{
				foreach( KeyValuePair<string, bool> kvp in _dirtyTable )
				{
					if( kvp.Value )
					{
						return true;
					}
				}				
			}

			return false;
		}

		public void IsDirty( string column )
		{
			_dirtyTable[column] = true;
		}

		public void SetChanged( string column )
		{
			if( _dirtyTable == null )
			{
				_dirtyTable = new Dictionary<string, bool>();
			}

			_dirtyTable[column] = true;
		}

		public bool IsChanged( string field )
		{
			return _dirtyTable != null && _dirtyTable.ContainsKey( field ) && _dirtyTable[field];
		}

		public string[] DirtyFields()
		{
			List<string> fields = new List<string>();

			if( _dirtyTable != null )
			{
				foreach( KeyValuePair<string, bool> kvp in _dirtyTable )
				{
					if( kvp.Value )
					{
						fields.Add( kvp.Key );
					}
				}
			}

			return fields.ToArray();
		}

		public bool TrackChanges
		{
			get { return _trackChanges; }
			set 
			{ 
				if( ! _trackChanges && value )
				{
					_dirtyTable = new Dictionary<string, bool>();
				}
				
				_trackChanges = value; 
			}
		}

		public void CommitChanges()
		{
			_dirtyTable.Clear();
		}

        //public void Save()
        //{
        //    Execute(SaveCommands);
        //}

        //public void Remove()
        //{
        //    Execute(RemoveCommands);
        //}

        protected static List<IDbCommand> CommandList(IDbCommand command)
        {
            List<IDbCommand> commandList = new List<IDbCommand>();
            commandList.Add(command);

            return commandList;
        }
	}
}
