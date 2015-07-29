using MongoDB.Bson.Serialization.Attributes;

namespace Phytel.Services.Migrations
{
    public class Error
    {
        [BsonElement(FieldNames.InnerException)]
        public Error InnerException { get; set; }

        [BsonElement(FieldNames.Message)]
        public string Message { get; set; }

        [BsonElement(FieldNames.StackTrace)]
        public string StackTrace { get; set; }

        public static class FieldNames
        {
            public const string InnerException = "err";
            public const string Message = "msg";
            public const string StackTrace = "stack";
        }
    }
}