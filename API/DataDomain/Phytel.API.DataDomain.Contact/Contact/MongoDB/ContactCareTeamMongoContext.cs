using System.Configuration;
using MongoDB.Bson;
using Phytel.API.DataDomain.Contact.MongoDB.DTO;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Contact.MongoDB
{
    public class ContactCareTeamMongoContext : MongoContext
    {
        private static string CollectionName = "CareTeam";

        public ContactCareTeamMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
        {
            CareTeam = new MongoSet<MEContactCareTeam, ObjectId>(this, CollectionName);
        }

        public MongoSet<MEContactCareTeam, ObjectId> CareTeam { get; private set; }
    }
}

