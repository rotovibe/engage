using MongoDB.Bson;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.Contact
{
    public class ContactMongoContext : MongoContext
    {
        private static string COLL_ContactS = "Contact";

        public ContactMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            Contacts = new MongoSet<MEContact, ObjectId>(this, COLL_ContactS);
		}

        public MongoSet<MEContact, ObjectId> Contacts { get; private set; }
    }
}
