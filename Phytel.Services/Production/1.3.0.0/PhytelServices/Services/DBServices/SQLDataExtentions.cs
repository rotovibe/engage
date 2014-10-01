using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Phytel.Services
{
    public static class SqlDataReaderExtensions
    {
        public static int SafeGetInt32(this SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetInt32(ordinal);
            }
            else
            {
                return 0;
            }
        }

        public static int SafeGetByte(this SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetByte(ordinal);
            }
            else
            {
                return 0;
            }
        }

        public static Decimal SafeGetDecimal(this SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetDecimal(ordinal);
            }
            else
            {
                return 0;
            }
        }

        public static string SafeGetString(this SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetString(ordinal);
            }
            else
            {
                return string.Empty;
            }
        }

        public static DateTime SafeGetDate(this SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetDateTime(ordinal);
            }
            else
            {
                return DateTime.Parse("01/01/1900");
            }
        }

        public static bool SafeGetBool(this SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            if (!reader.IsDBNull(ordinal))
            {
                return reader.GetBoolean(ordinal);
            }
            else
            {
                return false;
            }
        }

    }
}
