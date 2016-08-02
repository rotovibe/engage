using MongoDB.Bson;
using Phytel.API.DataDomain.Scheduling.DTO;
using Phytel.Services.Mongo.Linq;

namespace Phytel.API.DataDomain.Scheduling
{
	public class SchedulingMongoContext : MongoContext
	{
		private static string COLL_ToDoS = "ToDo";
		private static string COLL_ScheduleS = "Schedule";

		public SchedulingMongoContext(string contractDBName)
			: base(contractDBName, true)
		{
			ToDos = new MongoSet<METoDo, ObjectId>(this, COLL_ToDoS);
			Schedules = new MongoSet<MESchedule, ObjectId>(this, COLL_ScheduleS);
		}

		public MongoSet<METoDo, ObjectId> ToDos { get; private set; }
		public MongoSet<MESchedule, ObjectId> Schedules { get; private set; }
	}
}
