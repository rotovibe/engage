namespace Phytel.Engage.Integrations.Repo.Connections
{
    public interface ISQLConnectionProvider
    {
        string GetConnectionStringEF(string context);
        string GetConnectionString(string context);
    }
}