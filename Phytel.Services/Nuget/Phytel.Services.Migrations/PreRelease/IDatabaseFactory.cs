namespace Phytel.Services.Migrations
{
    public interface IDatabaseFactory
    {
        Database Create(Connection connection);
    }
}