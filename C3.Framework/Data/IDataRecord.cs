using System.Collections.Generic;
using System.Data;

namespace C3.Framework.Data
{
    public interface IDataRecord
    {
        void Save();
        void Remove();
        List<IDbCommand> SaveCommands { get; }
        List<IDbCommand> RemoveCommands { get; }
    }
}