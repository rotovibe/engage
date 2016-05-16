using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Phytel.Engage.Integrations.Repo
{
    public static class Utils
    {
        public static string FormatError(Exception ex, SqlBulkCopy bcc)
        {
            var strErr = string.Empty;
            if (!ex.Message.Contains("Received an invalid column length from the bcp client for colid")) return strErr;

            const string pattern = @"\d+";
            var match = Regex.Match(ex.Message.ToString(), pattern);
            var index = Convert.ToInt32(match.Value) - 1;

            var fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
            var sortedColumns = fi.GetValue(bcc);
            var info = sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (info == null) return strErr;
            var items = (Object[])info.GetValue(sortedColumns);
            var itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (itemdata == null) return strErr;
            var metadata = itemdata.GetValue(items[index]);

            var fieldInfo = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (fieldInfo == null) return strErr;
            var column = fieldInfo.GetValue(metadata);
            var field = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (field == null) return strErr;
            var length = field.GetValue(metadata);

            strErr = bcc.DestinationTableName + " :SqlBulkCopy process failure: " +
                     ex.Message + string.Format("Column: {0} contains data with a length greater than: {1}", column, length) + " : " + ex.InnerException;
            return strErr;
        }
    }
}
