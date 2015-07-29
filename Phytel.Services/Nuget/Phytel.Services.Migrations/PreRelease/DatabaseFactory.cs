namespace Phytel.Services.Migrations
{
    public class DatabaseFactory : IDatabaseFactory
    {
        public Database Create(Connection connection)
        {
            Database rvalue = null;
            IDatabaseProvider provider = null;

            switch (connection.DatabaseType)
            {
                case DatabaseTypes.Mongo:
                    provider = new MongoDatabaseProvider();
                    break;

                case DatabaseTypes.SQLServer:
                    provider = new SQLServerDatabaseProvider();
                    break;
            }

            rvalue = provider.Get(connection);

            return rvalue;
        }
    }
}