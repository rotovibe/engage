namespace Phytel.Mongo.Linq
{
    public interface IMongoConnectionProperties
    {
        string Server { get; }

        string DatabaseName { get; }

        string UserName { get; }

        string Password { get; }
    }
}
