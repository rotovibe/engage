using MongoDB.Bson;
using Phytel.API.DataDomain.Cohort.DTO;
using Phytel.Services.Mongo.Linq;
using System.Configuration;

namespace Phytel.API.DataDomain.Cohort
{
    public class CohortMongoContext : MongoContext
    {
        private static string COLL_CohortS = "Cohort";

        public CohortMongoContext(string contractDBName)
            : base(ConfigurationManager.AppSettings.Get("PhytelServicesConnName"), contractDBName, true)
		{
            Cohorts = new MongoSet<MECohort, ObjectId>(this, COLL_CohortS);
		}

        public MongoSet<MECohort, ObjectId> Cohorts { get; private set; }
    }
}
