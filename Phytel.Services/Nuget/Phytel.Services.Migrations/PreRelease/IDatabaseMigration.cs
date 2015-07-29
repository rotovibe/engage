namespace Phytel.Services.Migrations
{
    public interface IDatabaseMigration
    {
        string Product { get; }

        Phytel.Services.Versions.SemanticVersion Version { get; }

        void Down(Connection database);

        void Up(Connection database);
    }
}