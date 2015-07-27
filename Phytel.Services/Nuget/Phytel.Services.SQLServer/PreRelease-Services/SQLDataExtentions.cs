using System;
using System.Data.SqlClient;

namespace Phytel.Services.SQLServer
{
    public static class SqlDataReaderExtensions
    {
        #region Standard Extensions
        public static int SafeGetInt32(this SqlDataReader reader, string columnName)
        {
            return reader.SafeGetInt32(columnName, true);
        }

        public static int SafeGetByte(this SqlDataReader reader, string columnName)
        {
            return reader.SafeGetByte(columnName, true);
        }

        public static Decimal SafeGetDecimal(this SqlDataReader reader, string columnName)
        {
            return reader.SafeGetDecimal(columnName, true);
        }

        public static string SafeGetString(this SqlDataReader reader, string columnName)
        {
            return reader.SafeGetString(columnName, true);
        }

        public static DateTime SafeGetDate(this SqlDataReader reader, string columnName)
        {
            return reader.SafeGetDate(columnName, true);
        }

        public static bool SafeGetBool(this SqlDataReader reader, string columnName)
        {
            return reader.SafeGetBool(columnName, true);
        }
        #endregion

        #region Custom Extensions (No Error)
        public static int SafeGetInt32(this SqlDataReader reader, string columnName, bool throwError)
        {
            try
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
            catch
            {
                if (throwError)
                    throw;
                else
                    return 0;
            }
        }

        public static int SafeGetByte(this SqlDataReader reader, string columnName, bool throwError)
        {
            try
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
            catch
            {
                if (throwError)
                    throw;
                else
                    return 0;
            }
        }

        public static Decimal SafeGetDecimal(this SqlDataReader reader, string columnName, bool throwError)
        {
            try
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
            catch
            {
                if (throwError)
                    throw;
                else
                    return 0;
            }
        }

        public static string SafeGetString(this SqlDataReader reader, string columnName, bool throwError)
        {
            try
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
            catch
            {
                if (throwError)
                    throw;
                else
                    return string.Empty;
            }
        }

        public static DateTime SafeGetDate(this SqlDataReader reader, string columnName, bool throwError)
        {
            try
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
            catch
            {
                if (throwError)
                    throw;
                else
                    return DateTime.Parse("01/01/1900");
            }
        }

        public static bool SafeGetBool(this SqlDataReader reader, string columnName, bool throwError)
        {
            try
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
            catch
            {
                if (throwError)
                    throw;
                else
                    return false;
            }
        }
        #endregion
    }
}
