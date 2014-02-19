using MongoDB.Bson;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.Mongo.Linq;

namespace Phytel.API.DataDomain.Contact
{
    public class ContactMongoContext : MongoContext
    {
        private static string COLL_ContactS = "Contact";

        public ContactMongoContext(string contractDBName)
            : base(contractDBName, true)
		{
            Contacts = new MongoSet<MEContact, ObjectId>(this, COLL_ContactS);
		}

        public MongoSet<MEContact, ObjectId> Contacts { get; private set; }
    }
}
