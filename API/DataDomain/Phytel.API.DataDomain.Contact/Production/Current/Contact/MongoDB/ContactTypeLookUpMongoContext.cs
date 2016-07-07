using System.Configuration;
using MongoDB.Bson;
using Phytel.API.DataDomain.Contact.MongoDB.DTO;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Contact.MongoDB
{
    public class ContactTypeLookUpMongoContext : MongoContext
    {
        private static string CollectionName = "ContactTypeLookUp";

        public ContactTypeLookUpMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            ContactTypeLookUps = new MongoSet<MEContactTypeLookup, ObjectId>(this, CollectionName);
		}

        public MongoSet<MEContactTypeLookup, ObjectId> ContactTypeLookUps { get; private set; }
    }
}
