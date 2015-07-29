namespace Phytel.Services.Migrations
{
    public interface IDatabaseProvider
    {
        Database Get(Connection connection);
    }
}