namespace Phytel.Services.Journal
{
    public class ActionIdAsMongoObjectIdProvider : IActionIdProvider
    {
        public string New()
        {
            return MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        }
    }
}