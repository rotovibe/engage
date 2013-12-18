using System.Collections.Generic;
using System.Data;

namespace Phytel.Framework.SQL.Data
{
    public interface IDataRecord
    {
        void Save();
        void Remove();
        List<IDbCommand> SaveCommands { get; }
        List<IDbCommand> RemoveCommands { get; }
    }
}