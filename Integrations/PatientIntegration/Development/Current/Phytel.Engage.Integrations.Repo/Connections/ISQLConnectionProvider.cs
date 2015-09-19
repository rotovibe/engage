namespace Phytel.Engage.Integrations.Repo.Connections
{
    public interface ISQLConnectionProvider
    {
        string GetConnectionString(string context);
    }
}