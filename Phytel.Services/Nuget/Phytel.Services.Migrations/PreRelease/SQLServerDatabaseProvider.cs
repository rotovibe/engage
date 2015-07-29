using System.Data.SqlClient;

namespace Phytel.Services.Migrations
{
    public class SQLServerDatabaseProvider : IDatabaseProvider
    {
        public Database Get(Connection connection)
        {
            Database rvalue = new Database();
            rvalue.Type = DatabaseTypes.SQLServer;
            string connectionString = SQLServer.SQLDataService.Instance.GetConnectionString(connection.Name, connection.IsContract);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                rvalue.Server = conn.DataSource;
                rvalue.Name = conn.Database;
            }

            return rvalue;
        }
    }
}